using Models;

namespace FunctionalFitting
{
    public interface IFittingModelResolver
    {
        IFittingModelDescription Resolve(FittingMode model, ChartPoint[] initialData);
    }
}
