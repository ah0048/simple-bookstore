using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookstore.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowerBooks",
                table: "BorrowerBooks");

            migrationBuilder.DropIndex(
                name: "IX_BorrowerBooks_BorrowerId",
                table: "BorrowerBooks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BorrowerBooks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowerBooks",
                table: "BorrowerBooks",
                columns: new[] { "BorrowerId", "BookId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowerBooks",
                table: "BorrowerBooks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BorrowerBooks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowerBooks",
                table: "BorrowerBooks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowerBooks_BorrowerId",
                table: "BorrowerBooks",
                column: "BorrowerId");
        }
    }
}
