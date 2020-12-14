using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biplov.PaymentGateway.Infrastructure.Migrations
{
    public partial class InitialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardToken = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ExpiryMonth = table.Column<int>(type: "int", nullable: false),
                    Cvv = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MaskedCardNumber = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MerchantIdentity = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportedCurrencies = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardToken = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Cvv = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecipientDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecipientAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RecipientFirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RecipientLastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RecipientZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SuccessWebHookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorWebHookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMetaData",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMetaData", x => new { x.PaymentId, x.Id });
                    table.ForeignKey(
                        name: "FK_PaymentMetaData_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardToken",
                table: "Cards",
                column: "CardToken",
                unique: true,
                filter: "[CardToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantIdentity",
                table: "Merchants",
                column: "MerchantIdentity",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantId",
                table: "Payments",
                column: "MerchantId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "PaymentMetaData");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
