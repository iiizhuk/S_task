using Common;
using MathNet.Numerics;
using Models;

namespace FunctionalFitting;

internal class PowerFittingModelAlgorithm : IFittingModelAlgorithm
{
    public bool CanAccept(FittingMode model) => model == FittingMode.Power;

    public IFittingModelDescription Calculate(double[] xValues, double[] yValues)
    {
        var (a, b) = Fit.Power(xValues, yValues);

        return (new FittingModelDescription(FittingMode.Power,
            $"f(x) = {CommonHelper.TwoDecimal(a)} * (x ^ {CommonHelper.TwoDecimal(b)})",
            MathNetExtension.RegressionParametersValidation(a, b),
            x => a * (Math.Pow(x, b))));
    }
}