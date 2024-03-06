using Models;
using NLog;
using static System.Array;

namespace DataAccess.FileDataProviders
{
    public static class FileChartPointProvider
    {
        private static readonly Dictionary<string, Lazy<IFileChartPointReader>> Map = new();

        static FileChartPointProvider()
        {
            Map[".csv"] = new Lazy<IFileChartPointReader>(() => new CsvFileChartPointReader());
            //Map[".xls"] = new Lazy<IFileChartPointReader>(() => new ExcelFileChartPointReader());
            //...
        }

        public static ChartPoint[] GetChartPoints(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) ||
                !File.Exists(filePath))
            {
                throw new InvalidOperationException($"File {filePath} does not exist.");
            }

            var fileExtension = Path.GetExtension(filePath);
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new InvalidOperationException($"{filePath} is not a file path.");
            }

            if (Map.TryGetValue(fileExtension.ToLower(), out var charPointFileReader))
            {
                return charPointFileReader.Value.GetChartPoints(filePath);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported import file type {fileExtension}");
            }
        }
    }
}