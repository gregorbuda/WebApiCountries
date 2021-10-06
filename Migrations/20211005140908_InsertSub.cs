using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCountries.Migrations
{
    public partial class InsertSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO SubDivision (CodeSubDivision, NameSubDivision,  CountryId) VALUES ('AF-BDS', 'Badakhshān', 1)");
            migrationBuilder.Sql("INSERT INTO SubDivision (CodeSubDivision, NameSubDivision, CountryId) VALUES ('AL-01', 'Berat', 2)");
            migrationBuilder.Sql("INSERT INTO SubDivision (CodeSubDivision, NameSubDivision,  CountryId) VALUES ('DZ-01', 'Adrar', 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
