using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SkiaSharp;

BenchmarkRunner.Run<Benchmarks>();

[MemoryDiagnoser]
[ShortRunJob]
public class Benchmarks
{
    private readonly byte[] data;

    public Benchmarks()
    {
        data = File.ReadAllBytes("Assets/matthew.jpg");
    }

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

    static class Current
    {
        public static SKColor GetBestColor(byte[] bytes)
        {
            var colors = GetColors(bytes).ToArray();

            var r = colors.Sum(c => c.Red);
            var g = colors.Sum(c => c.Green);
            var b = colors.Sum(c => c.Blue);

            var count = colors.Length;

            var best = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count));

            return best;
        }

        public static IEnumerable<SKColor> GetColors(byte[] bytes)
        {
            using var image = SKImage.FromEncodedData(bytes);
            using var loaded = image.ToRasterImage(true);
            using var pixmap = loaded.PeekPixels();

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = pixmap.GetPixelColor(x, y);
                    yield return color;
                }
            }
        }
    }

    static class NoToArray
    {
        public static SKColor GetBestColor(byte[] bytes)
        {
            var colors = GetColors(bytes);

            var r = colors.Sum(c => c.Red);
            var g = colors.Sum(c => c.Green);
            var b = colors.Sum(c => c.Blue);

            var count = colors.Count();

            var best = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count));

            return best;
        }

        public static IEnumerable<SKColor> GetColors(byte[] bytes)
        {
            using var image = SKImage.FromEncodedData(bytes);
            using var loaded = image.ToRasterImage(true);
            using var pixmap = loaded.PeekPixels();

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = pixmap.GetPixelColor(x, y);
                    yield return color;
                }
            }
        }
    }

    static class SingleLoop
    {
        public static SKColor GetBestColor(byte[] bytes)
        {
            var colors = GetColors(bytes).ToArray();

            int r = 0;
            int g = 0;
            int b = 0;
            var count = colors.Length;
            for (var i = 0; i < count; i++)
            {
                var c = colors[i];
                r += c.Red;
                g += c.Green;
                b += c.Blue;
            }

            var best = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count));

            return best;
        }

        public static IEnumerable<SKColor> GetColors(byte[] bytes)
        {
            using var image = SKImage.FromEncodedData(bytes);
            using var loaded = image.ToRasterImage(true);
            using var pixmap = loaded.PeekPixels();

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = pixmap.GetPixelColor(x, y);
                    yield return color;
                }
            }
        }
    }

    static class SingleLoopNoToArray
    {
        public static SKColor GetBestColor(byte[] bytes)
        {
            var colors = GetColors(bytes);

            int r = 0;
            int g = 0;
            int b = 0;
            var count = 0;
            foreach (var c in colors)
            {
                count++;
                r += c.Red;
                g += c.Green;
                b += c.Blue;
            }

            var best = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count));

            return best;
        }

        public static IEnumerable<SKColor> GetColors(byte[] bytes)
        {
            using var image = SKImage.FromEncodedData(bytes);
            using var loaded = image.ToRasterImage(true);
            using var pixmap = loaded.PeekPixels();

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = pixmap.GetPixelColor(x, y);
                    yield return color;
                }
            }
        }
    }

    static class Best
    {
        public static SKColor GetBestColor(byte[] bytes)
        {
            using var image = SKImage.FromEncodedData(bytes);
            using var loaded = image.ToRasterImage(true);
            using var pixmap = loaded.PeekPixels();

            int r = 0;
            int g = 0;
            int b = 0;

            var w = image.Width;
            var h = image.Height;
            var count = w * h;

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var color = pixmap.GetPixelColor(x, y);
                    r += color.Red;
                    g += color.Green;
                    b += color.Blue;
                }
            }

            var best = new SKColor(
                (byte)(r / count),
                (byte)(g / count),
                (byte)(b / count));

            return best;
        }
    }
}
