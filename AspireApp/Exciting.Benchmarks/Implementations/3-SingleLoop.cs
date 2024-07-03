using SkiaSharp;

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
