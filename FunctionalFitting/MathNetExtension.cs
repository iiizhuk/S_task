namespace FunctionalFitting;

internal static class MathNetExtension
{
    public static bool RegressionParametersValidation(params double[] parameters) => parameters.All(it => !double.IsNaN(it));
}