using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using ReadLargeFile.Console;
using System.Data;
using System.Diagnostics;
using System.Globalization;

/// <summary>
/// - book2000k-3000k.csv file contains 395_957 rows
///     + Read + Saving data using entity framework took: 2.6251554 + 64.8285084 seconds
///     + Read + Saving data using SqlBulkCopy: 3.8110047 + 5.4166284 seconds
/// - book1-100k.csv file contains 58_292 rows
///     + Read + Saving data using entity framework took: 0.5258479 + 11.5094226 seconds
///     + Read + Saving data using SqlBulkCopy: 0.7612469 + 1.0769108 seconds
/// </summary>

//const string filePath = "E:\\Books\\book1-100k.csv";
const string filePath = "E:\\Books\\book2000k-3000k.csv";
//const string folderPath = "E:\\Books";
CsvConfiguration config = new(CultureInfo.InvariantCulture)
{
    PrepareHeaderForMatch = args => args.Header.ToLower(),
};

await ReadAndSaveUsingEF(filePath, config);
//await ReadAndSaveUsingBulkCopy(filePath, config);

Console.ReadLine();





















static async Task ReadAndSaveUsingEF(string filePath, CsvConfiguration config)
{
    Stopwatch stopwatch = new();
    stopwatch.Start();
    Console.WriteLine("Start reading the file...");
    List<Book> books = ReadCsvBookData(filePath, config);
    stopwatch.Stop();
    Console.WriteLine($"Read data took {stopwatch.Elapsed.TotalSeconds} seconds");

    stopwatch.Restart();
    Console.WriteLine("Start saving the data...");

    using AppDbContext context = new();
    await context.Books.AddRangeAsync(books);
    await context.SaveChangesAsync();
    stopwatch.Stop();
    Console.WriteLine($"Saving data took: {stopwatch.Elapsed.TotalSeconds} seconds");
}

static List<Book> ReadExcelBookData(string filePath, Dictionary<string, int> colIndex)
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    ExcelPackage package = new(new FileInfo(filePath));
    Stopwatch stopwatch = new();
    stopwatch.Start();

    Console.WriteLine("Start reading the file...");
    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
    int rowCount = worksheet.Dimension.Rows;
    Helper.MapColIndex(worksheet, colIndex);

    List<Book> books = new(rowCount - 1);
    for (int row = 2; row <= rowCount; row++)
    {
        Book book = new()
        {
            Id = Ulid.NewUlid().ToGuid(),
            Name = worksheet.Cells[row, colIndex["name"]].Text,
            RatingDistribution = new RatingDistribution
            {
                RatingDist1 = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdist1"]].Text),
                RatingDist2 = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdist2"]].Text),
                RatingDist3 = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdist3"]].Text),
                RatingDist4 = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdist4"]].Text),
                RatingDist5 = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdist5"]].Text),
                RatingDistTotal = Helper.ParseRating(worksheet.Cells[row, colIndex["ratingdisttotal"]].Text)
            },
            PagesNumber = double.Parse(worksheet.Cells[row, colIndex["pagesnumber"]].Text),
            PublishMonth = int.Parse(worksheet.Cells[row, colIndex["publishmonth"]].Text),
            PublishDay = int.Parse(worksheet.Cells[row, colIndex["publishday"]].Text),
            Publisher = worksheet.Cells[row, colIndex["publisher"]].Text,
            CountsOfReview = int.Parse(worksheet.Cells[row, colIndex["countsofreview"]].Text),
            PublishYear = int.Parse(worksheet.Cells[row, colIndex["publishyear"]].Text),
            Language = worksheet.Cells[row, colIndex["language"]].Text,
            Authors = worksheet.Cells[row, colIndex["authors"]].Text,
            Rating = double.Parse(worksheet.Cells[row, colIndex["rating"]].Text),
            ISBN = worksheet.Cells[row, colIndex["isbn"]].Text
        };

        books.Add(book);
    }

    stopwatch.Stop();
    Console.WriteLine($"Read data took {stopwatch.Elapsed.TotalSeconds} seconds");

    return books;
}

static List<Book> ReadCsvBookData(string filePath, CsvConfiguration config)
{
    using StreamReader reader = new(filePath);
    using CsvReader csv = new(reader, config);
    csv.Context.RegisterClassMap<BookMap>();
    return csv.GetRecords<Book>().ToList();
}

static async Task ReadAndSaveUsingBulkCopy(string filePath, CsvConfiguration config)
{
    SqlBulkCopy bulkBook = new(AppDbContext.ConnectionString)
    {
        DestinationTableName = "dbo.Books"
    };
    SqlBulkCopy bulkRating = new(AppDbContext.ConnectionString)
    {
        DestinationTableName = "dbo.RatingDistributions"
    };

    bulkBook.ColumnMappings.Add(nameof(Book.Id), "Id");
    bulkBook.ColumnMappings.Add(nameof(Book.Name), "Name");
    bulkBook.ColumnMappings.Add(nameof(Book.PagesNumber), "PagesNumber");
    bulkBook.ColumnMappings.Add(nameof(Book.PublishMonth), "PublishMonth");
    bulkBook.ColumnMappings.Add(nameof(Book.PublishDay), "PublishDay");
    bulkBook.ColumnMappings.Add(nameof(Book.Publisher), "Publisher");
    bulkBook.ColumnMappings.Add(nameof(Book.CountsOfReview), "CountsOfReview");
    bulkBook.ColumnMappings.Add(nameof(Book.PublishYear), "PublishYear");
    bulkBook.ColumnMappings.Add(nameof(Book.Language), "Language");
    bulkBook.ColumnMappings.Add(nameof(Book.Authors), "Authors");
    bulkBook.ColumnMappings.Add(nameof(Book.Rating), "Rating");
    bulkBook.ColumnMappings.Add(nameof(Book.ISBN), "ISBN");
    bulkBook.ColumnMappings.Add(nameof(Book.RatingDistributionId), "RatingDistributionId");

    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.Id), "Id");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDist1), "RatingDist1");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDist2), "RatingDist2");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDist3), "RatingDist3");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDist4), "RatingDist4");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDist5), "RatingDist5");
    bulkRating.ColumnMappings.Add(nameof(RatingDistribution.RatingDistTotal), "RatingDistTotal");

    var (bookTable, ratingTable) = ReadExcelBookDatatable(filePath, config);

    Stopwatch stopwatch = new();
    stopwatch.Start();
    Console.WriteLine("Start saving the data...");
    await bulkBook.WriteToServerAsync(bookTable);
    await bulkRating.WriteToServerAsync(ratingTable);

    stopwatch.Stop();
    Console.WriteLine($"Saving data took: {stopwatch.Elapsed.TotalSeconds} seconds");
}

static (DataTable, DataTable) ReadExcelBookDatatable(string filePath, CsvConfiguration config)
{
    Stopwatch stopwatch = new();
    stopwatch.Start();

    Console.WriteLine("Start reading the file...");

    DataTable bookTable = new();
    DataTable bookRating = new();

    bookTable.Columns.Add(nameof(Book.Id), typeof(Guid));
    bookTable.Columns.Add(nameof(Book.RatingDistributionId), typeof(Guid));
    bookTable.Columns.Add(nameof(Book.Name), typeof(string));
    bookTable.Columns.Add(nameof(Book.PagesNumber), typeof(double));
    bookTable.Columns.Add(nameof(Book.PublishMonth), typeof(int));
    bookTable.Columns.Add(nameof(Book.PublishDay), typeof(int));
    bookTable.Columns.Add(nameof(Book.Publisher), typeof(string));
    bookTable.Columns.Add(nameof(Book.CountsOfReview), typeof(int));
    bookTable.Columns.Add(nameof(Book.PublishYear), typeof(int));
    bookTable.Columns.Add(nameof(Book.Language), typeof(string));
    bookTable.Columns.Add(nameof(Book.Authors), typeof(string));
    bookTable.Columns.Add(nameof(Book.Rating), typeof(double));
    bookTable.Columns.Add(nameof(Book.ISBN), typeof(string));

    bookRating.Columns.Add(nameof(RatingDistribution.Id), typeof(Guid));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDist1), typeof(int));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDist2), typeof(int));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDist3), typeof(int));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDist4), typeof(int));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDist5), typeof(int));
    bookRating.Columns.Add(nameof(RatingDistribution.RatingDistTotal), typeof(int));

    using StreamReader reader = new(filePath);
    using CsvReader csv = new(reader, config);
    csv.Context.RegisterClassMap<BookMap>();
    IEnumerable<Book> books = csv.GetRecords<Book>();
    foreach (Book item in books)
    {
        Guid bookId = Ulid.NewUlid().ToGuid();
        Guid ratingId = Ulid.NewUlid().ToGuid();

        bookTable.Rows.Add(
            bookId,
            ratingId,
            item.Name,
            item.PagesNumber,
            item.PublishMonth,
            item.PublishDay,
            item.Publisher,
            item.CountsOfReview,
            item.PublishYear,
            item.Language,
            item.Authors,
            item.Rating,
            item.ISBN
        );

        bookRating.Rows.Add(
            ratingId,
            item.RatingDistribution.RatingDist1,
            item.RatingDistribution.RatingDist2,
            item.RatingDistribution.RatingDist3,
            item.RatingDistribution.RatingDist4,
            item.RatingDistribution.RatingDist5,
            item.RatingDistribution.RatingDistTotal
        );
    }

    stopwatch.Stop();
    Console.WriteLine($"Read data took {stopwatch.Elapsed.TotalSeconds} seconds");

    return (bookTable, bookRating);
}
