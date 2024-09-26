using CsvHelper.Configuration;

namespace ReadLargeFile.Console;

public class BookMap : ClassMap<Book>
{
    public BookMap()
    {
        Map(m => m.Name);
        Map(m => m.PagesNumber);
        Map(m => m.PublishMonth);
        Map(m => m.PublishDay);
        Map(m => m.Publisher);
        Map(m => m.CountsOfReview);
        Map(m => m.PublishYear);
        Map(m => m.Language);
        Map(m => m.Authors);
        Map(m => m.Rating);
        Map(m => m.ISBN);

        Map(m => m.RatingDistribution.RatingDist1).Name("RatingDist1").TypeConverter<RatingDistributionConverter>();
        Map(m => m.RatingDistribution.RatingDist2).Name("RatingDist2").TypeConverter<RatingDistributionConverter>();
        Map(m => m.RatingDistribution.RatingDist3).Name("RatingDist3").TypeConverter<RatingDistributionConverter>();
        Map(m => m.RatingDistribution.RatingDist4).Name("RatingDist4").TypeConverter<RatingDistributionConverter>();
        Map(m => m.RatingDistribution.RatingDist5).Name("RatingDist5").TypeConverter<RatingDistributionConverter>();
        Map(m => m.RatingDistribution.RatingDistTotal).Name("RatingDistTotal").TypeConverter<RatingDistributionConverter>();
    }
}
