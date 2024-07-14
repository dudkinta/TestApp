using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationContextDb.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "loc");

            migrationBuilder.CreateTable(
                name: "countries",
                schema: "loc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                schema: "loc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_provinces_countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "loc",
                        principalTable: "countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_provinces_CountryId",
                schema: "loc",
                table: "provinces",
                column: "CountryId");


            migrationBuilder.InsertData(
                schema: "loc",
                table: "countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "USA" },
                    { 2, "Canada" }
                });
            migrationBuilder.InsertData(
                schema: "loc",
                table: "provinces",
                columns: new[] { "Id", "Name", "CountryId" },
                values: new object[,]
                {
                    { 1, "California", 1 },
                    { 2, "Texas", 1 },
                    { 3, "Florida", 1 },
                    { 4, "Ontario", 2 },
                    { 5, "Quebec", 2 },
                    { 6, "Alberta", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "provinces",
                schema: "loc");

            migrationBuilder.DropTable(
                name: "countries",
                schema: "loc");
        }
    }
}
