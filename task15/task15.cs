using System.Diagnostics;
using System.Threading;
using ScottPlot;
using task14;

class Analyzer
{
    static void Main()
    {
        double a = -100, b = 100;
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int iterations = 100;
        var stepResults = new List<(double step, double result, double time)>();

        Console.WriteLine("Определение минимального шага:");
        foreach (var step in steps)
        {
            var watch = new Stopwatch();
            watch.Start();
            double result = Math.Abs(DefiniteIntegral.SolveOneThread(a, b, Math.Sin, step));
            watch.Stop();
            double time = watch.Elapsed.TotalMilliseconds;
            stepResults.Add((step, result, time));
            Console.WriteLine($"Шаг: {step} Погрешность: {result} Время: {time}");
        }

        var optimalStep = stepResults.Where(x => x.result <= 1e-4).OrderBy(x => x.time).First().step;
        Console.WriteLine($"Оптимальный шаг: {optimalStep}");

        int maxThreads = 12;
        double[] threadTimes = new double[maxThreads];

        Console.WriteLine($"Измерение времени для разных потоков:");
        for (int threads = 1; threads <= maxThreads; threads++)
        {
            double totalTime = 0;

            for (int i = 0; i < iterations; i++)
            {
                var watch = new Stopwatch();
                watch.Start();
                DefiniteIntegral.Solve(a, b, Math.Sin, optimalStep, threads);
                watch.Stop();
                totalTime += watch.Elapsed.TotalMilliseconds;
            }

            threadTimes[threads - 1] = totalTime / iterations;
            Console.WriteLine($"Количество потоков: {threads} Среднее время: {threadTimes[threads - 1]} мс");
        }

        int optimalThreads = Array.IndexOf(threadTimes, threadTimes.Min()) + 1;
        double parallelTime = threadTimes[optimalThreads - 1];
        double oneThreadTime = 0;

        for (int i = 0; i < iterations; i++)
        {
            var watch = new Stopwatch();
            watch.Start();
            DefiniteIntegral.SolveOneThread(a, b, Math.Sin, optimalStep);
            watch.Stop();
            oneThreadTime += watch.Elapsed.TotalMilliseconds;
        }
        oneThreadTime /= iterations;

        double speedup = (oneThreadTime - parallelTime) / oneThreadTime * 100;

        Console.WriteLine($"Сравнение версий:");
        Console.WriteLine($"Однопоточная: {oneThreadTime} мс");
        Console.WriteLine($"Многопоточная ({optimalThreads} потоков): {parallelTime} мс");
        Console.WriteLine($"Ускорение: {speedup}%");

        ScottPlot.Plot plot = new ();
        double[] graphThreads = Enumerable.Range(1, maxThreads).Select(x => (double)x).ToArray();
        plot.Add.Scatter(threadTimes, graphThreads);
        plot.Title("Время выполнения в зависимости от количества потоков");
        plot.XLabel("Время вычисления функции, мс");
        plot.YLabel("Количество потоков");
        plot.SavePng("result.png", 800, 600);

        using (StreamWriter sw = new StreamWriter("result.txt"))
        {
            sw.WriteLine($"Оптимальный шаг: {optimalStep}");
            sw.WriteLine($"Оптимальное число потоков: {optimalThreads}");
            sw.WriteLine($"Время работы однопоточной версии: {oneThreadTime} мс");
            sw.WriteLine($"Время оптимальной многопоточной версии: {parallelTime} мс");
            sw.WriteLine($"Разница в процентах: {speedup}%");
        }
    }
}
