using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orders_backend.Migrations
{
    /// <inheritdoc />
    public partial class intialState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "AnimalCategories",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAnimalCategories",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    AnimalCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAnimalCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAnimalCategories_AnimalCategories_AnimalCategoryId",
                        column: x => x.AnimalCategoryId,
                        principalSchema: "products",
                        principalTable: "AnimalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAnimalCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "products",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAnimalCategories_AnimalCategoryId",
                schema: "products",
                table: "ProductAnimalCategories",
                column: "AnimalCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAnimalCategories_ProductId",
                schema: "products",
                table: "ProductAnimalCategories",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAnimalCategories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "AnimalCategories",
                schema: "products");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "products");
        }
    }
}
