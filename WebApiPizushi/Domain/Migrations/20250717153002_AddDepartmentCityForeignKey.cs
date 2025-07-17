using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentCityForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "tblPostDepartments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_tblPostDepartments_CityId",
                table: "tblPostDepartments",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPostDepartments_tblCities_CityId",
                table: "tblPostDepartments",
                column: "CityId",
                principalTable: "tblCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPostDepartments_tblCities_CityId",
                table: "tblPostDepartments");

            migrationBuilder.DropIndex(
                name: "IX_tblPostDepartments_CityId",
                table: "tblPostDepartments");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "tblPostDepartments");
        }
    }
}
