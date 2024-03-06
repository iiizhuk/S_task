using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using DataAccess.FileDataProviders;
using DynamicData.Binding;
using FunctionalFitting;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.Win32;
using Models;
using NLog;
using OxyPlot;
using OxyPlot.Series;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using IOPath = System.IO.Path;

namespace WpfChart.GUI.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {

        private readonly IFittingModelResolver _fittingModelResolver;
        private readonly ILogger _logger;

        public MainWindowViewModel(IFittingModelResolver fittingModelResolver, ILogger logger)
        {
            _fittingModelResolver = fittingModelResolver;
            _logger = logger;

            CurrentFittingMode = FittingMode.None;

            LoadFileCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(LoadFile));

            ClearDataCommand = ReactiveCommand.Create(ClearData,
                this.WhenAnyValue(x => x.FilePath).Select(x => !string.IsNullOrWhiteSpace(FilePath)));

            var calculateCommand = ReactiveCommand.CreateFromTask<(ChartPoint[], FittingMode), PlotModel>(GeneratePlotModelAsync);
            calculateCommand.IsExecuting.Subscribe(x => IsBusy = x);
            calculateCommand
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => this.PlotModel = x);
            calculateCommand.ThrownExceptions.Subscribe(ProcessException);

            this.WhenAnyValue(x => x.CurrentFittingMode,
                    x => x.InitialPoints,
                    (mode, points) => (points, mode))
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .Select(data => data)
                .InvokeCommand(calculateCommand);
        }

        [Reactive] public FittingMode CurrentFittingMode { get; set; }
        [Reactive] public bool IsBusy { get; set; } //TODO: is not updated for now
        [Reactive] public string FilePath { get; set; }
        [Reactive] public PlotModel PlotModel { get; set; }

        public ReactiveCommand<Unit, Unit> LoadFileCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearDataCommand { get; }

        [Reactive] private ChartPoint[] InitialPoints { get; set; } = Array.Empty<ChartPoint>();

        private Task<PlotModel> GeneratePlotModelAsync((ChartPoint[] points, FittingMode fittingMode) data)
        {
            return Task.Run(() =>
            {
                if (data.points.Length <= 0)
                {
                    return new PlotModel { Title = Properties.Resources.EmptyTitlePlot };
                }

                var model = new PlotModel();
                var initialPoints = new LineSeries()
                {
                    Title = Properties.Resources.InitialPointsTitle,
                    ItemsSource = data.points.Select(it => new DataPoint(it.X, it.Y)),
                    LineStyle = LineStyle.None,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 5
                };
                model.Series.Add(initialPoints);

                if (data.fittingMode != FittingMode.None &&
                    TryExecute(() => _fittingModelResolver.Resolve(data.fittingMode, data.points), out var fittingModel))
                {
                    var sortedPoint = data.points.OrderBy(it => it.X).ToArray();
                    var minX = sortedPoint.First().X;
                    var maxX = sortedPoint.Last().X;
                    var step = (maxX - minX) / ViewParameters.ChartPointCount;

                    if (fittingModel.IsValid)
                    {
                        model.Title = fittingModel.FormulaDescription;
                        model.Series.Add(new FunctionSeries(fittingModel.ModelFunction, minX, maxX, step));
                    }
                    else
                    {
                        model.Title = $"Cannot find parameters for {data.fittingMode}";
                    }
                }

                return model;
            });
        }

        private void LoadFile()
        {
            ClearData();

            string currentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string defaultPath = IOPath.GetFullPath(IOPath.Combine(currentDirectory, @"..\...\..\..\TestData"));

            var openFileDialog = new OpenFileDialog() { Multiselect = false, Filter = "CSV Files (*.csv)|*.csv" };

            if (Directory.Exists(defaultPath))
            {
                openFileDialog.InitialDirectory = defaultPath;
            }
            else
            {
                openFileDialog.InitialDirectory = string.Empty;
            }

            if (openFileDialog.ShowDialog() == true)
            {
                Application.Current.Dispatcher.InvokeIfRequired(() => FilePath = openFileDialog.FileName);

                if (TryExecute(() => FileChartPointProvider.GetChartPoints(openFileDialog.FileName),
                        out var result))
                {
                    InitialPoints = result;
                }
            }
        }

        private void ClearData()
        {
            Application.Current.Dispatcher.InvokeIfRequired(() =>
            {
                FilePath = string.Empty;
                CurrentFittingMode = FittingMode.None;
                InitialPoints = Array.Empty<ChartPoint>();
            });
        }

        private bool TryExecute<T>(Func<T> func, out T result)
        {
            result = default;
            try
            {
                result = func();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                return false;
            }

            return true;
        }

        private void ProcessException(Exception exception)
        {
            MessageBox.Show(exception.Message,
                Properties.Resources.ErrorMessageCaption,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            _logger.Error(exception);
            _logger.Error(exception.StackTrace);
        }
    }
}
