using MetadataExtractor;
using MetadataExtractor.Formats.Xmp;
using System;
using System.Linq;

static class MetadataReader
{
    public static Metadata Read(string filePath)
    {
        var directories = ImageMetadataReader.ReadMetadata(filePath);
        var xmpDict = directories
                        .OfType<XmpDirectory>()
                        .SelectMany(d => d.XmpMeta.Properties)
                        .Where(p => p.Path != null)
                        .ToDictionary(p => p.Path, p => p.Value);

        var base64Image = xmpDict["GImage:Data"];
        var rightImageBytes = ConvertBase64ToBytes(base64Image);

        return new Metadata()
        {
            RightImageBytes = rightImageBytes,
            Pitch = float.Parse(xmpDict["GPano:PosePitchDegrees"]),
            Roll = float.Parse(xmpDict["GPano:PoseRollDegrees"]),
        };
    }

    static byte[] ConvertBase64ToBytes(string base64)
    {
        return Convert.FromBase64String(PadBase64String(base64));
    }

    static string PadBase64String(string base64)
    {
        var remainder = (base64.Length % 4);
        if (remainder == 0) return base64;
        var paddingLength = 4 - remainder;
        return base64 + new string('=', paddingLength);
    }
}
