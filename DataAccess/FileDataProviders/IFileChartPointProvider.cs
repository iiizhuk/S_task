using Models;

namespace DataAccess.FileDataProviders;

public interface IFileChartPointProvider
{
    bool TryGetChartPoints(string filePath, out ChartPoint[] chartPoints);
}