using SkiaSharp;

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
