namespace ReadLargeFile.Console;

public class RatingDistribution
{
    public Guid Id { get; set; }
    public int RatingDist1 { get; set; } // 1-star ratings
    public int RatingDist2 { get; set; } // 2-star ratings
    public int RatingDist3 { get; set; } // 3-star ratings
    public int RatingDist4 { get; set; } // 4-star ratings
    public int RatingDist5 { get; set; } // 5-star ratings
    public int RatingDistTotal { get; set; } // Total number of ratings
}
