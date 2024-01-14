using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationApi.Migrations
{
    /// <inheritdoc />
    public partial class unique_index_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Index_OrganizationId",
                table: "Organizations",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_OrganizationId",
                table: "Organizations");
        }
    }
}
