using Models;

namespace FunctionalFitting;

internal interface IFittingModelAlgorithm
{
    bool CanAccept(FittingMode model);
    IFittingModelDescription Calculate(double[] xValues, double[] yValues);
}