using System.Linq.Expressions;
using Models;

namespace FunctionalFitting;

public interface IFittingModelDescription
{
    FittingMode RegressionMode { get; }

    string FormulaDescription { get; } //TODO: most probably it's redundant. Func/Expression should be enough

    bool IsValid { get; }

    //just as a comment.
    //this approach will help to have more complex function (not only A,B dependent)
    //for instance, f(x) = a * (x ^ b) + C
    Func<double, double>? ModelFunction { get; }
}