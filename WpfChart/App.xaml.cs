using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DataAccess;
using NLog;
using WpfChart.GUI.ViewModels;
using WpfChart.GUI.Views;
using ProjectResources = WpfChart.GUI.Properties.Resources;

namespace WpfChart.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IContainer? _rootScope;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupExceptionHandling();
            WriteStartupLogMessage();

            var builder = new ContainerBuilder();
            builder.Register(c => _logger).As<ILogger>().SingleInstance();
            builder.RegisterModule(new MainModule());
            builder.RegisterModule(new FunctionalFitting.ProjectModule());
            builder.RegisterModule(new DataAccess.ProjectModule());
            _rootScope = builder.Build();

            Current.MainWindow = new MainWindow
            {
                DataContext = _rootScope.Resolve<MainWindowViewModel>()
            };
            Current.MainWindow.Closed += HandleClosed;
            Current.MainWindow.Show();
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            WriteExitLogMessage();
            LogManager.Flush();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (_, e) => LogUnhandledException((Exception)e.ExceptionObject, nameof(AppDomain.CurrentDomain.UnhandledException));

            DispatcherUnhandledException += (_, e) =>
            {
                LogUnhandledException(e.Exception, nameof(Application.Current.DispatcherUnhandledException));
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                LogUnhandledException(e.Exception, nameof(TaskScheduler.UnobservedTaskException));
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            var message = $"Unhandled exception ({source})";
            try
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = $"Unhandled exception in {assemblyName.Name} version {assemblyName.Version}";
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Exception while {nameof(LogUnhandledException)}");
            }
            finally
            {
                _logger.Error(exception, message);
                _logger.Error(exception.StackTrace);

                MessageBox.Show(ProjectResources.FatalErrorMessage,
                    ProjectResources.FatalErrorMessageCaption,
                    MessageBoxButton.OK);

                Shutdown();
            }
        }

        private void HandleClosed(object? sender, EventArgs e)
        {
            _rootScope?.Dispose();
        }

        private void WriteStartupLogMessage() => _logger.Info(ProjectResources.StartupMessage);

        private void WriteExitLogMessage() => _logger.Info(ProjectResources.ExitMessage);
    }
}
