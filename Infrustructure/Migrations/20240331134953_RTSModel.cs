using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrustructure.Migrations
{
    public partial class RTSModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RTS_InDependentCreditNote",
                columns: table => new
                {
                    CreditNumber = table.Column<long>(type: "bigint", maxLength: 10, nullable: false),
                    ExternalCreditNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreditStatus = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTS_InDependentCreditNote", x => x.CreditNumber);
                });

            migrationBuilder.CreateTable(
                name: "RTS_InvoiceDocument",
                columns: table => new
                {
                    InvoiceNumber = table.Column<long>(type: "bigint", maxLength: 10, nullable: false),
                    ExternalInvoiceNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTS_InvoiceDocument", x => x.InvoiceNumber);
                });

            migrationBuilder.CreateTable(
                name: "RTS_DependentCreditNote",
                columns: table => new
                {
                    CreditNumber = table.Column<long>(type: "bigint", maxLength: 10, nullable: false),
                    ExternalCreditNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreditStatus = table.Column<int>(type: "int", nullable: false),
                    ParentInvoiceNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceDocumentInvoiceNumber = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTS_DependentCreditNote", x => x.CreditNumber);
                    table.ForeignKey(
                        name: "FK_RTS_DependentCreditNote_RTS_InvoiceDocument_InvoiceDocumentInvoiceNumber",
                        column: x => x.InvoiceDocumentInvoiceNumber,
                        principalTable: "RTS_InvoiceDocument",
                        principalColumn: "InvoiceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RTS_DependentCreditNote_InvoiceDocumentInvoiceNumber",
                table: "RTS_DependentCreditNote",
                column: "InvoiceDocumentInvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "RTSCreditNumber_IDX01",
                table: "RTS_DependentCreditNote",
                column: "CreditNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RTS_InDependentCreditNote_IDX01",
                table: "RTS_InDependentCreditNote",
                column: "CreditNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RTS_InvoiceDocument_IDX1",
                table: "RTS_InvoiceDocument",
                column: "InvoiceNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RTS_DependentCreditNote");

            migrationBuilder.DropTable(
                name: "RTS_InDependentCreditNote");

            migrationBuilder.DropTable(
                name: "RTS_InvoiceDocument");
        }
    }
}
