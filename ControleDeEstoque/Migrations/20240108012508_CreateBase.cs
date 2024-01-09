using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class CreateBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Tipo = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    ValordeCompra = table.Column<decimal>(type: "Decimal(18,2)", nullable: false),
                    ValordeVenda = table.Column<decimal>(type: "Decimal(18,2)", nullable: false),
                    Codigodebarra = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Fornecedor = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Quantidade = table.Column<int>(type: "Int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
