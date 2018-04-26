using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pt = new PerformanceTests();
            pt.AddRemoveTestDDS();

            var summary = BenchmarkRunner.Run<PerformanceTests>();
        }
    }
}