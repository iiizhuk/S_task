using Models;

namespace DataAccess.FileDataProviders;

internal interface IFileChartPointReader
{
    ChartPoint[] GetChartPoints(string filePath);
}