using System;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Classes;

namespace ToDo.Infrastructure.Context
{
    /// <summary>
    /// Class seeding data in database.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Applies pending migration to database.
        /// </summary>
        /// <param name="context">The <see cref="GraidexDbContext"/> object used to perform database operations.</param>
        /// <exception cref="ArgumentNullException">The GraidexDbContext object is null.</exception>
        public static void EnsurePopulated(ApplicationContext? context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.ToDoLists.Any())
            {
                Seed(context);
            }
        }

        /// <summary>
        /// Method to seed examplary data to database.
        /// </summary>
        /// <param name="modelBuilder">For extending <see cref="ModelBuilder"/>.</param>
        public static void Seed(ApplicationContext context)
        {
            ToDoList toDoList1 = new ToDoList { Id = 1, Title = "House things", IsArchived = true };
            ToDoList toDoList2 = new ToDoList { Id = 2, Title = "Work tasks", IsArchived = false };
            ToDoList toDoList3 = new ToDoList { Id = 3, Title = "Being adult", IsArchived = false };

            toDoList1.Items = new List<ToDoItem>() {
                new ToDoItem { Id = 1, Title = "Buy groceries", Description = "Milk, bread, eggs, cheese", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow.AddDays(2), ToDoListID = toDoList1.Id, TodoList = toDoList1, Status = ItemStatus.NotStarted, Remind = true, Priority = Priority.Top },
                new ToDoItem { Id = 6, Title = "Clean the house", Description = "Vacuum the floors and dust the surfaces", CreatedAt = DateTime.UtcNow.AddDays(-2), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList1.Id, TodoList = toDoList1, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Top }
                };
            toDoList2.Items = new List<ToDoItem>() {
                new ToDoItem { Id = 5, Title = "Finish coding challenge", Description = "Complete the coding challenge for the job application", CreatedAt = DateTime.UtcNow.AddDays(-4), Deadline = DateTime.UtcNow.AddDays(2), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Default },
                new ToDoItem { Id = 2, Title = "Finish project report", Description = "Write up the findings from the experiments", CreatedAt = DateTime.UtcNow.AddDays(-3), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Default },
                new ToDoItem { Id = 9, Title = "Submit expense report", Description = "Submit the expense report for reimbursement", CreatedAt = DateTime.UtcNow.AddDays(-3), Deadline = DateTime.UtcNow, ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Top },
                new ToDoItem { Id = 13, Title = "Reply to email", Description = "Respond to the email from your coworker", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Default },
                new ToDoItem { Id = 11, Title = "Prepare for meeting", Description = "Review the agenda and prepare notes for the meeting", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow, ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = true, Priority = Priority.Default },
                };
            toDoList3.Items = new List<ToDoItem>() {
                new ToDoItem { Id = 3, Title = "Go for a run", Description = "Run for 30 minutes around the park", CreatedAt = DateTime.UtcNow.AddDays(-7), Deadline = DateTime.UtcNow.AddDays(-2), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.Completed, Remind = false, Priority = Priority.Low },
                new ToDoItem { Id = 4, Title = "Call mom", Description = "Check in with her and see how she's doing", CreatedAt = DateTime.UtcNow.AddDays(-2), Deadline = DateTime.UtcNow.AddDays(3), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false,  Priority = Priority.Default },
                new ToDoItem { Id = 7, Title = "Send birthday card", Description = "Mail the birthday card to your friend", CreatedAt = DateTime.UtcNow.AddDays(-5), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.Completed, Remind = true, Priority = Priority.Default },
                new ToDoItem { Id = 8, Title = "Schedule dentist appointment", Description = "Make an appointment for a teeth cleaning", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow.AddDays(7), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Low },
                new ToDoItem { Id = 10, Title = "Read book", Description = "Read the next chapter in the book club book", CreatedAt = DateTime.UtcNow.AddDays(-6), Deadline = DateTime.UtcNow.AddDays(3), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false,  Priority = Priority.Default },
                new ToDoItem { Id = 11, Title = "Prepare for meeting", Description = "Review the agenda and prepare notes for the meeting", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow, ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = true, Priority = Priority.Default },
                new ToDoItem { Id = 12, Title = "Buy birthday gift", Description = "Choose and purchase a gift for the upcoming birthday", CreatedAt = DateTime.UtcNow.AddDays(-5), Deadline = DateTime.UtcNow.AddDays(2), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Default },
                };


            context.ToDoLists.AddRange(toDoList1, toDoList2, toDoList3);
        }
    }
}
