using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ReadLargeFile.Console;

public class RatingDistributionConverter : ITypeConverter
{
    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "";
        }

        return Helper.ParseRating(text);
    }

    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        throw new NotImplementedException();
    }
}
