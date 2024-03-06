using Common;
using MathNet.Numerics;
using Models;

namespace FunctionalFitting;

internal class ExponentialFittingModelAlgorithm : IFittingModelAlgorithm
{
    public bool CanAccept(FittingMode model) => model == FittingMode.Exponential;

    public IFittingModelDescription Calculate(double[] xValues, double[] yValues)
    {
        var (a, r) = Fit.Exponential(xValues, yValues);
        
        return (new FittingModelDescription(FittingMode.Exponential,
            $"f(x) = {CommonHelper.TwoDecimal(a)} * exp ({CommonHelper.TwoDecimal(r)} * x)",
            MathNetExtension.RegressionParametersValidation(a, r),
            x => a * (Math.Exp(x * r))));
    }
}