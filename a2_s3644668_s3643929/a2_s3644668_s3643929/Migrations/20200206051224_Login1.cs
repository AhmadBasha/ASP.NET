using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace a2_s3644668_s3643929.Migrations
{
    public partial class Login1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(maxLength: 50, nullable: false),
                    TFN = table.Column<string>(maxLength: 11, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Payees",
                columns: table => new
                {
                    PayeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayeeName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payees", x => x.PayeeID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType = table.Column<int>(maxLength: 1, nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    transactCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    FailedCtr = table.Column<int>(nullable: true),
                    LockoutEnd = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Logins_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPays",
                columns: table => new
                {
                    BillPayID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    PayeeID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    Period = table.Column<int>(maxLength: 1, nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Blocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPays", x => x.BillPayID);
                    table.ForeignKey(
                        name: "FK_BillPays_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPays_Payees_PayeeID",
                        column: x => x.PayeeID,
                        principalTable: "Payees",
                        principalColumn: "PayeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<int>(maxLength: 1, nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    DestAccount = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(maxLength: 255, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_DestAccount",
                        column: x => x.DestAccount,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "City", "CustomerName", "Phone", "PostCode", "State", "TFN" },
                values: new object[] { 2100, "123 Fake Street", "Melbourne", "Matthew Bolger", "(61)- 1234 5678", "3000", null, null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "City", "CustomerName", "Phone", "PostCode", "State", "TFN" },
                values: new object[] { 2200, "456 Real Road", "Melbourne", "Rodney Cocker", "(61)- 1234 5679", "3005", null, null });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "City", "CustomerName", "Phone", "PostCode", "State", "TFN" },
                values: new object[] { 2300, null, null, "Shekhar Kalra", "(61)- 1234 5680", null, null, null });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountNumber", "AccountType", "Balance", "CustomerID", "ModifyDate", "transactCount" },
                values: new object[,]
                {
                    { 4100, 83, 100.0, 2100, new DateTime(2019, 12, 19, 20, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4101, 67, 500.0, 2100, new DateTime(2019, 12, 19, 20, 30, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4200, 83, 500.94999999999999, 2200, new DateTime(2019, 12, 19, 21, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4300, 67, 1250.5, 2300, new DateTime(2019, 12, 19, 22, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "UserID", "CustomerID", "FailedCtr", "LockoutEnd", "ModifyDate", "Password" },
                values: new object[,]
                {
                    { 12345678, 2100, null, null, new DateTime(2020, 2, 6, 5, 12, 23, 999, DateTimeKind.Utc).AddTicks(6180), "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2" },
                    { 38074569, 2200, null, null, new DateTime(2020, 2, 6, 5, 12, 23, 999, DateTimeKind.Utc).AddTicks(6590), "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04" },
                    { 17963428, 2300, null, null, new DateTime(2020, 2, 6, 5, 12, 23, 999, DateTimeKind.Utc).AddTicks(6600), "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionID", "AccountNumber", "Amount", "Comment", "DestAccount", "ModifyDate", "TransactionType" },
                values: new object[,]
                {
                    { 1, 4100, 100.0, "Acc Creation", null, new DateTime(2019, 12, 19, 20, 0, 0, 0, DateTimeKind.Unspecified), 68 },
                    { 2, 4101, 500.0, "Acc Creation", null, new DateTime(2019, 12, 19, 20, 30, 0, 0, DateTimeKind.Unspecified), 68 },
                    { 3, 4200, 500.94999999999999, "Acc Creation", null, new DateTime(2019, 12, 19, 21, 0, 0, 0, DateTimeKind.Unspecified), 68 },
                    { 4, 4300, 1250.5, "Acc Creation", null, new DateTime(2019, 12, 19, 22, 0, 0, 0, DateTimeKind.Unspecified), 68 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerID",
                table: "Accounts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_BillPays_AccountNumber",
                table: "BillPays",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillPays_PayeeID",
                table: "BillPays",
                column: "PayeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_CustomerID",
                table: "Logins",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountNumber",
                table: "Transactions",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestAccount",
                table: "Transactions",
                column: "DestAccount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPays");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Payees");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
