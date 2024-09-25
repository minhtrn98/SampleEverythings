using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UploadLargeFile.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RatingDistributions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingDist1 = table.Column<int>(type: "int", nullable: false),
                    RatingDist2 = table.Column<int>(type: "int", nullable: false),
                    RatingDist3 = table.Column<int>(type: "int", nullable: false),
                    RatingDist4 = table.Column<int>(type: "int", nullable: false),
                    RatingDist5 = table.Column<int>(type: "int", nullable: false),
                    RatingDistTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDistributions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingDistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PagesNumber = table.Column<int>(type: "int", nullable: false),
                    PublishMonth = table.Column<int>(type: "int", nullable: false),
                    PublishDay = table.Column<int>(type: "int", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountsOfReview = table.Column<int>(type: "int", nullable: false),
                    PublishYear = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_RatingDistributions_RatingDistributionId",
                        column: x => x.RatingDistributionId,
                        principalTable: "RatingDistributions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_RatingDistributionId",
                table: "Books",
                column: "RatingDistributionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "RatingDistributions");
        }
    }
}
