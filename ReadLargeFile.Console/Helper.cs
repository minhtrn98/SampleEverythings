using OfficeOpenXml;

namespace ReadLargeFile.Console;

public static class Helper
{
    //public static int ParseRating(string ratingText)
    //{
    //    if (string.IsNullOrWhiteSpace(ratingText))
    //    {
    //        return 0;
    //    }

    //    string[] parts = ratingText.Split(':');
    //    if (parts.Length == 2 && int.TryParse(parts[1], out int result))
    //    {
    //        return result;
    //    }

    //    return 0;
    //}

    public static int ParseRating(ReadOnlySpan<char> ratingText)
    {
        if (ratingText.IsWhiteSpace())
        {
            return 0;
        }

        int colonIndex = ratingText.IndexOf(':');
        if (colonIndex != -1 && int.TryParse(ratingText[(colonIndex + 1)..], out int result))
        {
            return result;
        }

        return 0;
    }

    public static void MapColIndex(ExcelWorksheet worksheet, Dictionary<string, int> colIndex)
    {
        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
        {
            string colName = worksheet.Cells[1, col].Text.ToLower();
            if (colIndex.ContainsKey(colName))
            {
                colIndex[colName] = col;
            }
        }
    }

    public static void MapColIndex(string[] headers, Dictionary<string, int> colIndex)
    {
        for (int col = 0; col < headers.Length; col++)
        {
            string colName = headers[col].ToLower();
            if (colIndex.ContainsKey(colName))
            {
                colIndex[colName] = col;
            }
        }
    }
}
