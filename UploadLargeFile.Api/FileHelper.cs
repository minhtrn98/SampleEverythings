namespace UploadLargeFile.Api;

public sealed class FileInfo
{
    public required string Extension { get; init; }
    public required byte[] Bytes { get; init; }
}

public sealed class FileHelper
{
    public static FileInfo? GetFileInfo(string base64StringWithPrefix)
    {
        if (string.IsNullOrWhiteSpace(base64StringWithPrefix))
        {
            return null;
        }

        string[] parts = base64StringWithPrefix.Split(',');
        if (parts.Length != 2)
        {
            return null;
        }

        string type = parts[0];
        string ext = GetFileExtension(type);

        string base64Data = parts[1];
        byte[] fileBytes = Convert.FromBase64String(base64Data);

        return new FileInfo()
        {
            Extension = ext,
            Bytes = fileBytes,
        };
    }

    public static string GetFileExtension(string base64String)
    {
        int startIndex = base64String.IndexOf(':') + 1;
        int endIndex = base64String.IndexOf(';');
        if (startIndex == 0 || endIndex == -1 || startIndex >= endIndex)
        {
            return string.Empty;
        }
        return base64String[startIndex..endIndex] switch
        {
            "image/jpeg" => ".jpeg",
            "image/png" or "image/x-png" => ".png",
            _ => string.Empty
        };
    }

    public static string ConvertFileToBase64WithPrefix(string filePath)
    {
        string mimeType = GetMimeType(filePath);

        byte[] fileBytes = File.ReadAllBytes(filePath);

        string base64Data = Convert.ToBase64String(fileBytes);

        return $"data:{mimeType};base64,{base64Data}";
    }

    public static string GetMimeType(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }
}
