using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System;
using System.IO;
using System.Linq;

namespace ThorusViewer.Extensions;

public enum GifCompressionLevel
{
    Lowest,
    Medium,
    Highest
}

public static class GifBuilder
{
    public static void CreateGifFromFolder(
        string inputFolder,
        string outputPath,
        int frameDurationMs = 500,
        bool loop = true,
        int pauseBetweenLoopsMs = 0,
        GifCompressionLevel compressionLevel = GifCompressionLevel.Medium)
    {
        if (!Directory.Exists(inputFolder))
            throw new DirectoryNotFoundException(inputFolder);

        var files = Directory.GetFiles(inputFolder, "*.png")
                             .OrderBy(f => f)
                             .ToArray();

        if (files.Length == 0)
            throw new FileNotFoundException("No PNG files found.");

        int frameDelay = Math.Max(1, frameDurationMs / 10); // GIF uses 1/100 sec
        int pauseDelay = Math.Max(1, pauseBetweenLoopsMs / 10);

        using var image = Image.Load<Rgba32>(files[0]);

        // First frame
        image.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = frameDelay;

        // Add remaining frames
        for (int i = 1; i < files.Length; i++)
        {
            var file = files[i];
            using var frameImage = Image.Load<Rgba32>(file);
            var frame = frameImage.Frames.RootFrame;

            frame.Metadata.GetGifMetadata().FrameDelay = (i == (files.Length - 1) && loop && pauseDelay > 0) ? pauseDelay : frameDelay;
            image.Frames.AddFrame(frame);

        }

        // Looping
        image.Metadata.GetGifMetadata().RepeatCount = (ushort)(loop ? 0 : 1);

        // Compression via quantization (important for GIFs)
        int maxColors = compressionLevel switch
        {
            GifCompressionLevel.Lowest => 256,
            GifCompressionLevel.Medium => 128,
            GifCompressionLevel.Highest => 64,
            _ => 128
        };

        image.Mutate(ctx => ctx.Quantize(new OctreeQuantizer(new QuantizerOptions
        {
            MaxColors = maxColors
        })));

        // GIF encoder (2.x supports ColorTableMode)
        var encoder = new GifEncoder
        {
            ColorTableMode = compressionLevel switch
            {
                GifCompressionLevel.Highest => GifColorTableMode.Local,  // best per-frame compression
                _ => GifColorTableMode.Global
            }
        };

        image.Save(outputPath, encoder);
    }
}
