using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TODOListDomainModel.Migrations
{
    public partial class initial : Migration
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
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Remind = table.Column<bool>(type: "bit", nullable: false),
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
                columns: new[] { "Id", "CreatedAt", "Deadline", "Description", "Priority", "Remind", "Status", "Title", "ToDoListID" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 23, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(4791), new DateTime(2023, 5, 26, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(5198), "Milk, bread, eggs, cheese", 2, true, 0, "Buy groceries", 1 },
                    { 6, new DateTime(2023, 5, 22, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6549), new DateTime(2023, 5, 25, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6550), "Vacuum the floors and dust the surfaces", 2, false, 0, "Clean the house", 1 },
                    { 2, new DateTime(2023, 5, 21, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6486), new DateTime(2023, 5, 25, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6497), "Write up the findings from the experiments", 0, false, 1, "Finish project report", 2 },
                    { 5, new DateTime(2023, 5, 20, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6546), new DateTime(2023, 5, 26, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6547), "Complete the coding challenge for the job application", 0, false, 1, "Finish coding challenge", 2 },
                    { 9, new DateTime(2023, 5, 21, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6556), new DateTime(2023, 5, 24, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6556), "Submit the expense report for reimbursement", 2, false, 1, "Submit expense report", 2 },
                    { 11, new DateTime(2023, 5, 23, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6560), new DateTime(2023, 5, 24, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6561), "Review the agenda and prepare notes for the meeting", 0, true, 1, "Prepare for meeting", 2 },
                    { 13, new DateTime(2023, 5, 23, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6565), new DateTime(2023, 5, 25, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6565), "Respond to the email from your coworker", 0, false, 0, "Reply to email", 2 },
                    { 3, new DateTime(2023, 5, 17, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6541), new DateTime(2023, 5, 22, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6542), "Run for 30 minutes around the park", 1, false, 2, "Go for a run", 3 },
                    { 4, new DateTime(2023, 5, 22, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6544), new DateTime(2023, 5, 27, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6545), "Check in with her and see how she's doing", 0, false, 0, "Call mom", 3 },
                    { 7, new DateTime(2023, 5, 19, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6551), new DateTime(2023, 5, 25, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6552), "Mail the birthday card to your friend", 0, true, 2, "Send birthday card", 3 },
                    { 8, new DateTime(2023, 5, 23, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6553), new DateTime(2023, 5, 31, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6554), "Make an appointment for a teeth cleaning", 1, false, 0, "Schedule dentist appointment", 3 },
                    { 10, new DateTime(2023, 5, 18, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6558), new DateTime(2023, 5, 27, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6558), "Read the next chapter in the book club book", 0, false, 0, "Read book", 3 },
                    { 12, new DateTime(2023, 5, 19, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6562), new DateTime(2023, 5, 26, 15, 4, 27, 528, DateTimeKind.Utc).AddTicks(6563), "Choose and purchase a gift for the upcoming birthday", 0, false, 0, "Buy birthday gift", 3 }
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
