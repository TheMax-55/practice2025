using System;
using System.Threading;
namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double interval = (b - a) / threadsNumber;
        double[] localSums = new double[threadsNumber];

        Parallel.For(0, threadsNumber, i =>
        {
            double localSum = 0;
            double start = a + i * interval;
            double end = (i == threadsNumber - 1) ? b : start + interval;
            localSum = Trapezoid(start, end, function, step);
            localSums[i] = localSum;
        });

        return localSums.Sum();
    }

    public static double SolveOneThread(double a, double b, Func<double, double> function, double step)
        => Trapezoid(a, b, function, step);

    public static double Trapezoid(double start, double end, Func<double, double> function, double step)
    {
        double result = 0;
        double current = start;

        while (current < end)
        {
            double next = Math.Min(current + step, end);
            double h = next - current;
            result += (function(current) + function(next)) * h / 2;
            current = next;
        }

        return result;
    }
}
