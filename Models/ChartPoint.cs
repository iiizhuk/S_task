using System.Diagnostics;

namespace Models;

[DebuggerDisplay("X = {X}, Y = {Y}")]
public class ChartPoint
{
    public ChartPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }
}