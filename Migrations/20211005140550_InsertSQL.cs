using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCountries.Migrations
{
    public partial class InsertSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Countries (NameCountry, AlphaOneCountry, AlphaTwoCountry, NumericCode, Independent) VALUES ('Afganistan', 'AF', 'AFG', '004', 1)");
            migrationBuilder.Sql("INSERT INTO Countries (NameCountry, AlphaOneCountry, AlphaTwoCountry, NumericCode, Independent) VALUES ('Albania', 'AL', 'ALB', '008', 1)");
            migrationBuilder.Sql("INSERT INTO Countries (NameCountry, AlphaOneCountry, AlphaTwoCountry, NumericCode, Independent) VALUES ('Algeria', 'DZ', 'DZA', '012', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
