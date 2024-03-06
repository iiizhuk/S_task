using Models;
using NLog;

namespace FunctionalFitting;

internal class FittingModelResolver : IFittingModelResolver
{
    private readonly IEnumerable<IFittingModelAlgorithm> _algorithms;
    private readonly ILogger _logger;

    public FittingModelResolver(IEnumerable<IFittingModelAlgorithm> algorithms, ILogger logger)
    {
        _algorithms = algorithms;
        _logger = logger;
    }

    public IFittingModelDescription Resolve(FittingMode model, ChartPoint[] initialData)
    {
        IFittingModelDescription result = new FittingModelDescription(model, string.Empty, false, null);
        if (initialData == null || initialData.Length < 2)
        {
            return result;
        }

        var targetAlgorithm = _algorithms.Where(x => x.CanAccept(model)).ToArray();
        if (targetAlgorithm.Length > 1)
        {
            throw new InvalidOperationException($"More than one processor for {model} found.");
        }
        else if (targetAlgorithm.Length == 0)
        {
            throw new InvalidOperationException($"Processor for {model} is not found.");
        }

        var xValues = initialData.Select(it => it.X).ToArray();
        var yValues = initialData.Select(it => it.Y).ToArray();

        return targetAlgorithm.Single().Calculate(xValues, yValues);
    }
}