using System;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Models;

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

            if (!context.Lists.Any())
            {
                Seed(context);
            }
        }

        /// <summary>
        /// Method to seed examplary data to database.
        /// </summary>
        /// <param name="context">Database context.</param>
        public static void Seed(ApplicationContext context)
        {
            User user = new User()
            {
                Username = "admin",
                PasswordHash = "0P5Ru8CkrLbSnuC7710rwg==.u2nHQ0zJ2/bOY4s7dtP9KUONEjmDySRiEJU/tN0xr5c=",
                RefreshTokenExpire = DateTime.UtcNow,
            }; // password - P@55w0rd
            context.Users.Add(user);
            context.SaveChanges();

            ToDoList toDoList1 = new ToDoList { Title = "House things", IsArchived = true, User = user, UserID = user.Id };
            ToDoList toDoList2 = new ToDoList { Title = "Work tasks", IsArchived = false, User = user, UserID = user.Id };
            ToDoList toDoList3 = new ToDoList { Title = "Being adult", IsArchived = false, User = user, UserID = user.Id };
            context.Lists.AddRange(toDoList1, toDoList2, toDoList3);
            user.Lists.Add(toDoList1);
            user.Lists.Add(toDoList2);
            user.Lists.Add(toDoList3);
            context.SaveChanges();

            toDoList1.Items = new List<ToDoItem>()
            {
                new ToDoItem
                {
                    Title = "Buy groceries",
                    Description = "Milk, bread, eggs, cheese",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Deadline = DateTime.UtcNow.AddDays(2),
                    ToDoListID = toDoList1.Id,
                    TodoList = toDoList1,
                    Status = ItemStatus.NotStarted,
                    Remind = true,
                    Priority = Priority.Top,
                },
                new ToDoItem
                {
                    Title = "Clean the house",
                    Description = "Vacuum the floors and dust the surfaces",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    Deadline = DateTime.UtcNow.AddDays(1),
                    ToDoListID = toDoList1.Id,
                    TodoList = toDoList1,
                    Status = ItemStatus.NotStarted,
                    Remind = false,
                    Priority = Priority.Top,
                },
            };

            toDoList2.Items = new List<ToDoItem>()
            {
                new ToDoItem { Title = "Finish coding challenge", Description = "Complete the coding challenge for the job application", CreatedAt = DateTime.UtcNow.AddDays(-4), Deadline = DateTime.UtcNow.AddDays(2), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Default },
                new ToDoItem { Title = "Finish project report", Description = "Write up the findings from the experiments", CreatedAt = DateTime.UtcNow.AddDays(-3), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Default },
                new ToDoItem { Title = "Submit expense report", Description = "Submit the expense report for reimbursement", CreatedAt = DateTime.UtcNow.AddDays(-3), Deadline = DateTime.UtcNow, ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = false, Priority = Priority.Top },
                new ToDoItem { Title = "Reply to email", Description = "Respond to the email from your coworker", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Default },
                new ToDoItem { Title = "Prepare for meeting", Description = "Review the agenda and prepare notes for the meeting", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow, ToDoListID = toDoList2.Id, TodoList = toDoList2, Status = ItemStatus.InProcess, Remind = true, Priority = Priority.Default },
            };

            toDoList3.Items = new List<ToDoItem>()
            {
                new ToDoItem { Title = "Go for a run", Description = "Run for 30 minutes around the park", CreatedAt = DateTime.UtcNow.AddDays(-7), Deadline = DateTime.UtcNow.AddDays(-2), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.Completed, Remind = false, Priority = Priority.Low },
                new ToDoItem { Title = "Call mom", Description = "Check in with her and see how she's doing", CreatedAt = DateTime.UtcNow.AddDays(-2), Deadline = DateTime.UtcNow.AddDays(3), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false,  Priority = Priority.Default },
                new ToDoItem { Title = "Send birthday card", Description = "Mail the birthday card to your friend", CreatedAt = DateTime.UtcNow.AddDays(-5), Deadline = DateTime.UtcNow.AddDays(1), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.Completed, Remind = true, Priority = Priority.Default },
                new ToDoItem { Title = "Schedule dentist appointment", Description = "Make an appointment for a teeth cleaning", CreatedAt = DateTime.UtcNow.AddDays(-1), Deadline = DateTime.UtcNow.AddDays(7), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Low },
                new ToDoItem { Title = "Read book", Description = "Read the next chapter in the book club book", CreatedAt = DateTime.UtcNow.AddDays(-6), Deadline = DateTime.UtcNow.AddDays(3), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false,  Priority = Priority.Default },
                new ToDoItem { Title = "Buy birthday gift", Description = "Choose and purchase a gift for the upcoming birthday", CreatedAt = DateTime.UtcNow.AddDays(-5), Deadline = DateTime.UtcNow.AddDays(2), ToDoListID = toDoList3.Id, TodoList = toDoList3, Status = ItemStatus.NotStarted, Remind = false, Priority = Priority.Default },
            };
            context.SaveChanges();
        }
    }
}
