using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCityIdByDeliveryInfoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblDeliveryInfos_tblCities_CityId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropIndex(
                name: "IX_tblDeliveryInfos_CityId",
                table: "tblDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "tblDeliveryInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CityId",
                table: "tblDeliveryInfos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveryInfos_CityId",
                table: "tblDeliveryInfos",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblDeliveryInfos_tblCities_CityId",
                table: "tblDeliveryInfos",
                column: "CityId",
                principalTable: "tblCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
