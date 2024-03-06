using Models;
using NLog;
using Sylvan.Data;
using Sylvan.Data.Csv;

namespace DataAccess.FileDataProviders;

internal class CsvFileChartPointReader : IFileChartPointReader
{
    public ChartPoint[] GetChartPoints (string filePath)
    {
        using var dr = CsvDataReader.Create(filePath);
        var records = dr.GetRecords<CsvPoint>().ToArray();

        if (records == null || !records.Any())
        {
            return Array.Empty<ChartPoint>();
        }

        return records.Select(it => new ChartPoint(it.X, it.Y)).ToArray();
    }

    private class CsvPoint //matches with expected file structure
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

}