using System.Linq.Expressions;
using Models;

namespace FunctionalFitting;

internal record FittingModelDescription(FittingMode RegressionMode, string FormulaDescription, bool IsValid,
    Func<double, double>? ModelFunction) : IFittingModelDescription;

internal record FittingModelExpressionDescription(bool IsValid, Expression<Func<double, double>>? ModelFunction);