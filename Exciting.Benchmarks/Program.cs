using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SkiaSharp;

BenchmarkRunner.Run<Benchmarks>();

[MemoryDiagnoser]
[ShortRunJob]
public class Benchmarks
{
    private readonly byte[] data = File.ReadAllBytes("Assets/matthew.jpg");

    // TODO: add benchmarks here
}

/* benchies

[Benchmark(Baseline = true)]
public SKColor CurrentImpl() => Current.GetBestColor(data);

[Benchmark]
public SKColor NoToArrayImpl() => NoToArray.GetBestColor(data);

[Benchmark]
public SKColor SingleLoopImpl() => SingleLoop.GetBestColor(data);

[Benchmark]
public SKColor SingleLoopNoToArrayImpl() => SingleLoopNoToArray.GetBestColor(data);

[Benchmark]
public SKColor BestImpl() => Best.GetBestColor(data);
*/
