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
    /// Tests for <see cref="ToDoListRepository"/> class.
    /// </summary>
    [TestFixture]
    public class ToDoListRepositoryTests : IDisposable
    {
        private FakeApplicationContext context;
        private bool disposedValue;

        /// <summary>
        /// Tests correct behaviour of ctor.
        /// </summary>
        [Test]
        public void CtorCorrect()
        {
            // Assert
            Assert.DoesNotThrow(() => new ToDoListRepository(this.context));
        }

        /// <summary>
        /// Tests behavoiur of ctor in case <see cref="IApllicationContext"/> object is null.
        /// </summary>
        [Test]
        public void CtorNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListRepository((FakeApplicationContext)null));
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
        /// Tests correct inserting <see cref="ToDoList"/> object to empty databse.
        /// </summary>
        [Test]
        public void InsertCorrectEmptyDatabase()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.DoesNotThrowAsync(() => repo.Insert(todo));
            Assert.IsTrue(this.context.ToDoLists.Contains(todo));
        }

        /// <summary>
        /// Tests correct inserting <see cref="ToDoList"/> object to databse.
        /// </summary>
        [Test]
        public void InsertCorrect()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title 1" };
            var todo2 = new ToDoList { Title = "Title 2" };
            this.context.ToDoLists.Add(todo1);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.DoesNotThrowAsync(() => repo.Insert(todo2));
            Assert.IsTrue(this.context.ToDoLists.Contains(todo2));
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoList"/> object in case tis object is null.
        /// </summary>
        [Test]
        public void InsertNullException()
        {
            // Arrange
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => repo.Insert((ToDoList)null));
        }

        /// <summary>
        /// Tests inserting <see cref="ToDoList"/> object in case this ocject already is in database.
        /// </summary>
        [Test]
        public void InsertExceptionAlreadyInDatabase()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            this.context.ToDoLists.Add(todo);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => repo.Insert(todo));
            Assert.IsTrue(this.context.ToDoLists.FirstOrDefault(t => t.Id == todo.Id) != null);
        }

        /// <summary>
        /// Tests correct updating <see cref="ToDoList"/> object to databse.
        /// </summary>
        [Test]
        public void UpdateCorrect()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            this.context.ToDoLists.Add(todo);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);
            todo.Title = "New title";

            // Assert
            Assert.DoesNotThrow(() => repo.Update(todo));
            Assert.IsTrue(this.context.ToDoLists.FirstOrDefault(t => t.Id == todo.Id).Title == todo.Title);
            Assert.IsTrue(this.context.ToDoLists.Count() == 1);
        }

        /// <summary>
        /// Tests updating <see cref="ToDoList"/> object in case tis object is null.
        /// </summary>
        [Test]
        public void UpdateNullException()
        {
            // Arrange
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentNullException>(() => repo.Update((ToDoList)null));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoList"/> object in case there is no such object in database.
        /// </summary>
        [Test]
        public void UpdateNotFound()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title 1" };
            this.context.ToDoLists.Add(todo1);
            this.context.SaveChanges();
            var todo2 = new ToDoList { Title = "Title 2" };
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(todo2));
        }

        /// <summary>
        /// Tests updating <see cref="ToDoList"/> object in case there is no such object in empty database.
        /// </summary>
        [Test]
        public void UpdateNotFoundEmptyDatabase()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Update(todo));
        }

        /// <summary>
        /// Tests correct deleting <see cref="ToDoList"/> object with no <see cref="ToDoItem"/> objects attached.
        /// </summary>
        [Test]
        public void DeleteNoItemsCorrect()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            this.context.ToDoLists.Add(todo);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.DoesNotThrow(() => repo.Delete(todo));
            Assert.IsTrue(this.context.ToDoLists.ToList().Count == 0);
        }

        /// <summary>
        /// Tests correct deleting <see cref="ToDoList"/> object with no <see cref="ToDoItem"/> objects attached.
        /// </summary>
        [Test]
        public void DeleteWithItemsCorrect()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };
            var item = new ToDoItem { Title = "Item title", Deadline = DateTime.Now.AddDays(-1), CreatedAt = DateTime.Now, TodoList = todo, ToDoListID = todo.Id, Status = Status.NotStarted };
            this.context.ToDoLists.Add(todo);
            this.context.Items.Add(item);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.DoesNotThrow(() => repo.Delete(todo));
            Assert.IsTrue(!this.context.ToDoLists.Any());
            Assert.IsTrue(!this.context.Items.Any());
        }

        /// <summary>
        /// Tests deleteing <see cref="ToDoList"/> object in case it's null.
        /// </summary>
        [Test]
        public void DeleteNullException()
        {
            // Arrange
            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentNullException>(() => repo.Delete((ToDoList)null));
            Assert.IsTrue(!this.context.ToDoLists.Any());
        }

        /// <summary>
        /// Tests deleteing <see cref="ToDoList"/> object in case there is no such entry in empty databse.
        /// </summary>
        [Test]
        public void DeleteNotFoundEmptyDatabase()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };

            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Delete(todo));
            Assert.IsTrue(!this.context.ToDoLists.Any());
        }

        /// <summary>
        /// Tests deleteing <see cref="ToDoList"/> object in case there is no such entry in empty databse.
        /// </summary>
        [Test]
        public void DeleteNotFound()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title 1" };
            this.context.ToDoLists.Add(todo1);
            this.context.SaveChanges();
            var todo2 = new ToDoList { Title = "Title 2" };

            var repo = new ToDoListRepository(this.context);

            // Assert
            Assert.Throws<ArgumentException>(() => repo.Delete(todo2));
            Assert.IsTrue(this.context.ToDoLists.Count() == 1);
        }

        /// <summary>
        /// Tests correct getting <see cref="ToDoList"/> object form database by it's ID.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetByIDCorrect()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title 1" };
            var todo2 = new ToDoList { Title = "Title 2" };
            this.context.ToDoLists.Add(todo1);
            this.context.ToDoLists.Add(todo2);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Act
            var res = await repo.GetByID(todo1.Id).ConfigureAwait(true);

            // Assert
            Assert.IsTrue(res == todo1);
        }

        /// <summary>
        /// Tests getting <see cref="ToDoList"/> object from database in case there is no such object in empty database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetByIdNotFoundEmptyDatabase()
        {
            // Arrange
            var todo = new ToDoList { Title = "Title" };

            var repo = new ToDoListRepository(this.context);

            // Act
            var res = await repo.GetByID(todo.Id).ConfigureAwait(true);

            // Assert
            Assert.IsNull(res);
        }

        /// <summary>
        /// Tests getting <see cref="ToDoList"/> object from database in case there is no such object in database.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetByIdNotFound()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title 1" };
            this.context.ToDoLists.Add(todo1);
            this.context.SaveChanges();
            var todo2 = new ToDoList { Title = "Title 2" };

            var repo = new ToDoListRepository(this.context);

            // Act
            var res = await repo.GetByID(todo2.Id).ConfigureAwait(true);

            // Assert
            Assert.IsNull(res);
        }

        /// <summary>
        /// Tests correct getting list of <see cref="ToDoItem"/> marked as not hidden.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetAllCorrect()
        {
            // Arrange
            var todo1 = new ToDoList { Title = "Title" };
            var todo2 = new ToDoList { Title = "Title" };

            this.context.ToDoLists.Add(todo1);
            this.context.ToDoLists.Add(todo2);
            this.context.SaveChanges();
            var repo = new ToDoListRepository(this.context);

            // Act
            var res = await repo.GetAll().ConfigureAwait(true);

            // Assert
            Assert.IsTrue(res.Count == 2);
            Assert.IsTrue(res.Contains(todo1));
            Assert.IsTrue(res.Contains(todo2));
        }

        /// <summary>
        /// Tests getting list of <see cref="ToDoItem"/> marked as not hidden in case there is no such objects.
        /// </summary>
        /// <returns>Void async function result.</returns>
        [Test]
        public async Task GetAllNotFound()
        {
            // Arrange
            var repo = new ToDoListRepository(this.context);

            // Act
            var res = await repo.GetAll().ConfigureAwait(true);

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
