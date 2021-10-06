using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCountries.Migrations
{
    public partial class Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    CountryId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlphaOneCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlphaTwoCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumericCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Independent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "subDivision",
                columns: table => new
                {
                    SubDivisionId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSubDivision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameSubDivision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<short>(type: "smallint", nullable: false),
                    countriesCountryId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subDivision", x => x.SubDivisionId);
                    table.ForeignKey(
                        name: "FK_subDivision_countries_countriesCountryId",
                        column: x => x.countriesCountryId,
                        principalTable: "countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subDivision_countriesCountryId",
                table: "subDivision",
                column: "countriesCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subDivision");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
