using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationApi.Migrations
{
    /// <inheritdoc />
    public partial class country_industry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Founded",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CountryId",
                table: "Organizations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IndustryId",
                table: "Organizations",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Countries_CountryId",
                table: "Organizations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Industries_IndustryId",
                table: "Organizations",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Countries_CountryId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Industries_IndustryId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CountryId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_IndustryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Founded",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
