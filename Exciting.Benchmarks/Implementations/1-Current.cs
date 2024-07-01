using SkiaSharp;

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
