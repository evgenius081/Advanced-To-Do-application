using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ToDoDomainModel.Tests;
using TODOListDomainModel.Classes;
using TODOListDomainModel.Context;
using TODOListDomainModel.Repositories;

namespace ToDoListApplication.Tests
{
    /// <summary>
    /// Tests for <see cref="ToDoItemRepository"/> class.
    /// </summary>
    [TestFixture]
    public class ToDoItemRepositoryTests : IDisposable
    {
        private FakeApplicationContext context;
        private ToDoList todo;
        private bool disposedValue;

        /// <summary>
        /// Tests correct behaviour of ctor.
        /// </summary>
        [Test]
        public void CtorCorrect()
        {
            // Assert
            Assert.DoesNotThrow(() => new ToDoItemRepository(this.context));
        }

        /// <summary>
        /// Tests behavoiur of ctor in case <see cref="IApllicationContext"/> object is null.
        /// </summary>
        [Test]
        public void CtorNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoItemRepository((FakeApplicationContext)null));
        }

        /// <summary>
        /// Set up for all tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FakeApplicationContext>()
           .UseInMemoryDatabase(databaseName: "MockToDoDatabase")
           .Options;

            this.context = new FakeApplicationContext(options);

            this.todo = new ToDoList { Title = "List title" };
            this.context.ToDoLists.Add(this.todo);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Cleans up database after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.context.ToDoLists.RemoveRange(this.context.ToDoLists);
            this.context.Items.RemoveRange(this.context.Items);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Tests correct inserting <see cref="ToDoItem"/> object to empty databse.
        /// </summary>
        [Test]
        public void InsertCorrectEmptyDatabase()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.DoesNotThrowAsync(() => repo.Insert(item));
            Assert.IsTrue(this.context.Items.FirstOrDefault(e => e.Id == item.Id) != null);
        }

        /// <summary>
        /// Tests correct inserting <see cref="ToDoItem"/> object to databse.
        /// </summary>
        [Test]
        public void InsertCorrectNotEmptyDatabase()
        {
            // Arrange
            var item1 = new ToDoItem { Title = "Item title 1", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            var item2 = new ToDoItem { Title = "Item title 2", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item1);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.DoesNotThrowAsync(() => repo.Insert(item2));
            Assert.IsTrue(this.context.Items.FirstOrDefault(e => e.Id == item2.Id) != null);
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case this object is null.
        /// </summary>
        [Test]
        public void InsertNullException()
        {
            // Arrange
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => repo.Insert((ToDoItem)null));
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case <see cref="ToDoItem.ToDoList"/> does not match <see cref="ToDoItem.ToDoListID"/>.
        /// </summary>
        [Test]
        public void InsertExceptionToDoListIdDoesNotMatch()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id + 1, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(item));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case such <see cref="ToDoList"/> as <see cref="ToDoItem.ToDoList"/> does not exist in empty database.
        /// </summary>
        [Test]
        public void InsertExceptionNonExistingToDoListNoLists()
        {
            // Arrange
            this.context.ToDoLists.RemoveRange(this.context.ToDoLists);
            this.context.SaveChanges();
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(item));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case such <see cref="ToDoList"/> as <see cref="ToDoItem.ToDoList"/> does not exist in database.
        /// </summary>
        [Test]
        public void InsertExceptionNonExistingToDoList()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "List title 1" };
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = todo1, ToDoListID = todo1.Id, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(item));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case this object is already in database.
        /// </summary>
        [Test]
        public void InsertExceptionAlreadyExists()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(item));
            Assert.IsTrue(this.context.Items.Count() == 1);
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoItem"/> object in case it has no <see cref="ToDoList"/> object assigned.
        /// </summary>
        [Test]
        public void InsertExceptionNoToDoListAssigned()
        {
            // Arrange
            var item = new ToDoItem();
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(item));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests correct updating <see cref="ToDoItem"/> object to databse.
        /// </summary>
        [Test]
        public void UpdateCorrect()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);
            item.Title = "New title";

            // Assert
            Assert.DoesNotThrow(() => repo.Update(item));
            Assert.IsTrue(this.context.Items.FirstOrDefault(e => e.Id == item.Id).Title == item.Title);
            Assert.IsTrue(this.context.Items.ToList().Count == 1);
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case this object is null.
        /// </summary>
        [Test]
        public void UpdateNullException()
        {
            // Arrange
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.Throws<ArgumentNullException>(() => repo.Update((ToDoItem)null));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case it has no <see cref="ToDoList"/> object assigned.
        /// </summary>
        [Test]
        public void UpdateExceptionNoToDoListAssigned()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);
            item.TodoList = null;

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(item));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case <see cref="ToDoItem.ToDoList"/> does not match <see cref="ToDoItem.ToDoListID"/>.
        /// </summary>
        [Test]
        public void UpdateExceptionToDoListIdDoesNotMatch()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);
            item.ToDoListID++;

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(item));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case such <see cref="ToDoList"/> as <see cref="ToDoItem.ToDoList"/> does not exist in database.
        /// </summary>
        [Test]
        public void UpdateExceptionNonExistingToDoList()
        {
            // Arrange
            var todo2 = new ToDoList { Title = "List title 2" };
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);
            item.TodoList = todo2;
            item.ToDoListID = todo2.Id;

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(item));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case there is no such object in empty database.
        /// </summary>
        [Test]
        public void UpdateNotFoundEmptyDatabase()
        {
            // Arrange
            var repo = new ToDoItemRepository(this.context);
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            item.Title = "New title";

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(item));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoItem"/> object in case there is no such object in database.
        /// </summary>
        [Test]
        public void UpdateNotFound()
        {
            // Arrange
            var repo = new ToDoItemRepository(this.context);
            var item1 = new ToDoItem { Title = "Item title 1", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item1);
            this.context.SaveChanges();
            var item2 = new ToDoItem { Title = "Item title 2", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            item1.Title = "New title";

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(item2));
        }

        /// <summary>
        /// Tests correct deleting <see cref="ToDoItem"/> object from database.
        /// </summary>
        [Test]
        public void DeleteCorrect()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.DoesNotThrow(() => repo.Delete(item.Id));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests deleteing <see cref="ToDoItem"/> object in case there is no such item in empty databse.
        /// </summary>
        [Test]
        public void DeleteNotFoundEmptyDatabase()
        {
            // Arrange
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };

            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Delete(item.Id));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests deleteing <see cref="ToDoItem"/> object in case there is no such item in databse.
        /// </summary>
        [Test]
        public void DeleteNotFound()
        {
            // Arrange
            var item1 = new ToDoItem { Title = "Item title 1", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item1);
            var item2 = new ToDoItem { Title = "Item title 2", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Delete(item2.Id));
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests correct getting <see cref="ToDoItem"/> object form database by it's ID.
        /// </summary>
        [Test]
        public void GetByIDCorrect()
        {
            // Arrange
            var todo = new ToDoList { Title = "List title" };
            this.context.ToDoLists.Add(todo);
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = todo, ToDoListID = todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoItemRepository(this.context);

            // Act
            var res = repo.GetByID(item.Id);

            // Assert
            Assert.IsTrue(res != null);
        }

        /// <summary>
        /// Tests getting <see cref="ToDoItem"/> object from database in case there is no such object in empty database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetByIdNotFoundEmptyDatabase()
        {
            // Arrange
            var todo = new ToDoList();
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = todo, ToDoListID = todo.Id, Status = ItemStatus.NotStarted };

            var repo = new ToDoItemRepository(this.context);

            // Act
            var res = await repo.GetByID(item.Id).ConfigureAwait(true);

            // Assert
            Assert.IsNull(res);
        }

        /// <summary>
        /// Tests getting <see cref="ToDoItem"/> object from database in case there is no such object in database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetByIdNotFound()
        {
            // Arrange
            var item1 = new ToDoItem { Title = "Item title 1", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item1);
            this.context.SaveChanges();
            var item2 = new ToDoItem { Title = "Item title 2", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = this.todo, ToDoListID = this.todo.Id, Status = ItemStatus.NotStarted };
            var repo = new ToDoItemRepository(this.context);

            // Act
            var res = await repo.GetByID(item2.Id).ConfigureAwait(true);

            // Assert
            Assert.IsNull(res);
        }

        /// <summary>
        /// Tests getting all <see cref="ToDoItem"/> objects from database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public void GetAllCorrect()
        {
            var todo = new ToDoList { Title = "List title" };
            this.context.ToDoLists.Add(todo);
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(1), CreatedAt = DateTime.Now, TodoList = todo, ToDoListID = todo.Id, Status = ItemStatus.NotStarted };
            this.context.Items.Add(item);
            this.context.SaveChanges();

            var repo = new ToDoItemRepository(this.context);

            // Act
            var res = repo.GetAll().ToList();

            // Assert
            Assert.IsTrue(res.Count == 1);
            Assert.IsTrue(res.Contains(item));
        }

        /// <summary>
        /// Tests getting all <see cref="ToDoItem"/> objects from database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public void GetAllEmpty()
        {
            var repo = new ToDoItemRepository(this.context);

            // Act
            var res = repo.GetAll().ToList();

            // Assert
            Assert.IsTrue(res.Count == 0);
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this object depending on state.
        /// </summary>
        /// <param name="disposing">Decides whether to dispose the object or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
