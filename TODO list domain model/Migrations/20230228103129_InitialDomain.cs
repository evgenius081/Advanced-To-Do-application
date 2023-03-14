using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TODOListDomainModel.Migrations
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1413:Use trailing comma in multi-line initializers", Justification = "Generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "enerated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Generated code")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Generated code")]
    public partial class InitialDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    Remind = table.Column<bool>(type: "bit", nullable: false),
                    Starred = table.Column<bool>(type: "bit", nullable: false),
                    ToDoListID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ToDoLists_ToDoListID",
                        column: x => x.ToDoListID,
                        principalTable: "ToDoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ToDoLists",
                columns: new[] { "Id", "IsArchived", "Title" },
                values: new object[] { 1, true, "House things" });

            migrationBuilder.InsertData(
                table: "ToDoLists",
                columns: new[] { "Id", "IsArchived", "Title" },
                values: new object[] { 2, false, "Work tasks" });

            migrationBuilder.InsertData(
                table: "ToDoLists",
                columns: new[] { "Id", "IsArchived", "Title" },
                values: new object[] { 3, false, "Being adult" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreatedAt", "Deadline", "Description", "IsHidden", "Remind", "Starred", "Status", "Title", "ToDoListID" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 2, 27, 10, 31, 28, 149, DateTimeKind.Utc).AddTicks(9521), new DateTime(2023, 3, 2, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(689), "Milk, bread, eggs, cheese", false, true, true, 0, "Buy groceries", 1 },
                    { 6, new DateTime(2023, 2, 26, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4725), new DateTime(2023, 3, 1, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4727), "Vacuum the floors and dust the surfaces", false, false, true, 0, "Clean the house", 1 },
                    { 2, new DateTime(2023, 2, 25, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4448), new DateTime(2023, 3, 1, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4491), "Write up the findings from the experiments", false, false, false, 1, "Finish project report", 2 },
                    { 5, new DateTime(2023, 2, 24, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4717), new DateTime(2023, 3, 2, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4721), "Complete the coding challenge for the job application", false, false, false, 1, "Finish coding challenge", 2 },
                    { 9, new DateTime(2023, 2, 25, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4752), new DateTime(2023, 2, 28, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4754), "Submit the expense report for reimbursement", false, false, true, 1, "Submit expense report", 2 },
                    { 11, new DateTime(2023, 2, 27, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4766), new DateTime(2023, 2, 28, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4768), "Review the agenda and prepare notes for the meeting", false, true, false, 1, "Prepare for meeting", 2 },
                    { 13, new DateTime(2023, 2, 27, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4780), new DateTime(2023, 3, 1, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4785), "Respond to the email from your coworker", false, false, false, 0, "Reply to email", 2 },
                    { 3, new DateTime(2023, 2, 21, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4699), new DateTime(2023, 2, 26, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4703), "Run for 30 minutes around the park", true, false, false, 2, "Go for a run", 3 },
                    { 4, new DateTime(2023, 2, 26, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4710), new DateTime(2023, 3, 3, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4712), "Check in with her and see how she's doing", false, false, false, 0, "Call mom", 3 },
                    { 7, new DateTime(2023, 2, 23, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4737), new DateTime(2023, 3, 1, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4742), "Mail the birthday card to your friend", false, true, false, 2, "Send birthday card", 3 },
                    { 8, new DateTime(2023, 2, 27, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4744), new DateTime(2023, 3, 7, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4745), "Make an appointment for a teeth cleaning", true, false, false, 0, "Schedule dentist appointment", 3 },
                    { 10, new DateTime(2023, 2, 22, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4757), new DateTime(2023, 3, 3, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4763), "Read the next chapter in the book club book", false, false, false, 0, "Read book", 3 },
                    { 12, new DateTime(2023, 2, 23, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4775), new DateTime(2023, 3, 2, 10, 31, 28, 150, DateTimeKind.Utc).AddTicks(4777), "Choose and purchase a gift for the upcoming birthday", false, false, false, 0, "Buy birthday gift", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ToDoListID",
                table: "Items",
                column: "ToDoListID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ToDoLists");
        }
    }
}
