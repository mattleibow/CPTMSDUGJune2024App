using SkiaSharp;

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
