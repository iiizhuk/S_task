using System.Runtime.InteropServices.ComTypes;
using MathNet.Numerics;
using Models;
using System.Xml.Linq;
using Common;

namespace FunctionalFitting;

internal class LinearFittingModelAlgorithm : IFittingModelAlgorithm
{
    public bool CanAccept(FittingMode model) => model == FittingMode.Linear;

    public IFittingModelDescription Calculate(double[] xValues, double[] yValues)
    {
        var (a, b) = Fit.Line(xValues, yValues);

        return (new FittingModelDescription(FittingMode.Linear, 
            $"f(x) = ({CommonHelper.TwoDecimal(b)} * x) + {CommonHelper.TwoDecimal(a)}",
            MathNetExtension.RegressionParametersValidation(a, b),
            x => b * x + a));
    }
}