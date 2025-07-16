using System;
using System.Threading;
namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double integral = 0;
        double interval = (b - a) / threadsNumber;
        Barrier barrier = new Barrier(threadsNumber + 1);
        Thread[] threads = new Thread[threadsNumber];

        for (int i = 0; i < threadsNumber; i++)
        {
            double start = a + i * interval;
            double end = (i == threadsNumber - 1) ? b : start + interval;

            threads[i] = new Thread(() =>
            {
                double localSum = Trapezoid(start, end, function, step);
                Interlocked.Exchange(ref integral, integral + localSum);
                barrier.SignalAndWait();
            });
            threads[i].Start();
        }

        barrier.SignalAndWait();
        return integral;
    }

    static double Trapezoid(double start, double end, Func<double, double> function, double step)
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
