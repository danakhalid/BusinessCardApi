using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardApi.Migrations
{
    /// <inheritdoc />
    public partial class photocolschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "BusinessCards");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BusinessCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoBase64",
                table: "BusinessCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoBase64",
                table: "BusinessCards");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "BusinessCards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "BusinessCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
