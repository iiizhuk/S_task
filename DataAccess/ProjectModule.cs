using Autofac;
using DataAccess.FileDataProviders;

namespace DataAccess
{
    public sealed class ProjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<FileChartPointProvider>().As<IFileChartPointProvider>();

            //builder.RegisterType<CsvFileChartPointReader>().As<IFileChartPointReader>();
            //etc. builder.RegisterType<ExcelChartPointFileReader>().As<IChartPointFileReader>();
        }
    }
}
