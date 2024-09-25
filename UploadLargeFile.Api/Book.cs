namespace UploadLargeFile.Api;

public class Book
{
    public Guid Id { get; set; }
    public Guid RatingDistributionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PagesNumber { get; set; }
    public int PublishMonth { get; set; }
    public int PublishDay { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public int CountsOfReview { get; set; }
    public int PublishYear { get; set; }
    public string Language { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public double Rating { get; set; } // Average rating
    public string? ISBN { get; set; }

    public RatingDistribution RatingDistribution { get; set; } = default!;
}
