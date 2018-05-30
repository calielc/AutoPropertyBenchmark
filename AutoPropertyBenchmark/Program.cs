using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoPropertyBenchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            var tests = new(int numberOfCall, int repeatTimes)[]
            {
                (            1, 1_000_000),
                (          100, 1_000_000),
                (       10_000, 97),
                (    1_000_000, 97),
                (   10_000_000, 97),
                (  100_000_000, 37),
                (1_000_000_000, 19),
            };
            foreach (var (numberOfCall, repeatTimes) in tests)
            {
                Run(numberOfCall, repeatTimes);
            }
        }

        private static void Run(long numberOfCalls, int repeatTimes)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"numberOfCalls: {numberOfCalls:#,##0}");

            var results = new[]
            {
                Run(new ClassSettableBackField {X = 10, Y = 20}, numberOfCalls, repeatTimes),
                Run(new ClassSettableAutoProperty {X = 10, Y = 20}, numberOfCalls, repeatTimes),
                Run(new ClassReadonlyBackField(10, 20), numberOfCalls, repeatTimes),
                Run(new ClassReadonlyAutoProperty(10, 20), numberOfCalls, repeatTimes),
            };

            var orderedResult = results.OrderBy(x => x.ElapsedTicks).ThenBy(x => x.AveragedTicks).Select(x => x.Name).ToList();

            foreach (var result in results)
            {
                var prize = orderedResult.IndexOf(result.Name);
                Console.ForegroundColor = GetPrizeColor(prize);

                Console.WriteLine($"{result.Name,-30} | " +
                                  $"total: {result.Elapsed.TotalMilliseconds:#,##0.0000000} ms ({result.ElapsedTicks:0.000} ticks) | " +
                                  $"average: {result.Averaged.TotalMilliseconds:#,##0.0000000} ms ({result.AveragedTicks:0.000} ticks) | " +
                                  $"prize: {prize + 1}");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            ConsoleColor GetPrizeColor(int prize)
            {
                switch (prize)
                {
                    case 0:
                        return ConsoleColor.Green;
                    case 2:
                        return ConsoleColor.Yellow;
                    case 3:
                        return ConsoleColor.Red;
                    default:
                        return ConsoleColor.White;
                }
            }
        }

        private static RunResult Run(IClass instance, long numberOfCalls, int repeatTimes)
        {
            var result = 0;
            var stopwatch = new Stopwatch();

            var repetition = new LinkedList<double>();
            for (var repeatTime = 0; repeatTime < repeatTimes; repeatTime++)
            {
                stopwatch.Restart();
                for (var numberOfCall = 0; numberOfCall < numberOfCalls; numberOfCall++)
                {
                    //result = instance.X;
                    result = instance.Sum();
                }
                stopwatch.Stop();

                repetition.AddLast(stopwatch.ElapsedTicks);
            }

            var oneFiveth = repeatTimes / 5;

            var elapsedTicks = repetition.OrderBy(time => time).Skip(oneFiveth).Take(repeatTimes - 2 * oneFiveth).Average();
            var elapsed = TimeSpan.FromTicks((long)elapsedTicks);

            var averagedTicks = elapsedTicks / numberOfCalls;
            var averaged = TimeSpan.FromTicks((long)averagedTicks);

            return new RunResult
            {
                Name = instance.GetType().Name,
                Elapsed = elapsed,
                ElapsedTicks = elapsedTicks,
                Averaged = averaged,
                AveragedTicks = averagedTicks
            };
        }
    }

    internal class RunResult
    {
        public string Name { get; set; }
        public TimeSpan Elapsed { get; set; }
        public double ElapsedTicks { get; set; }
        public TimeSpan Averaged { get; set; }
        public double AveragedTicks { get; set; }
    }
}
