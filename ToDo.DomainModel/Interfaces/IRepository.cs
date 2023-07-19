using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.DomainModel.Models;

namespace ToDo.DomainModel.Interfaces
{
    /// <summary>
    /// Generic interface for repositories.
    /// </summary>
    /// <typeparam name="TEntity">Class of objects that will be stored in database.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// InsertAsync object to database.
        /// </summary>
        /// <param name="t">Object to be InsertAsynced into dataabse.</param>
        /// <returns>Task becuase method is async.</returns>
        public Task<TEntity> InsertAsync(TEntity t);

        /// <summary>
        /// Update obejct in databse.
        /// </summary>
        /// <param name="t">Object to be updated in database.</param>
        public void Update(TEntity t);

        /// <summary>
        /// Gets all objects.
        /// </summary>
        /// <returns>List of objects in database.</returns>
        public IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets object by ID from database.
        /// </summary>
        /// <param name="id">Wanted object's id.</param>
        /// <returns>Found object, null if there is no such.</returns>
        public Task<TEntity?> GetByID(int id);

        /// <summary>
        /// Delete object form database.
        /// </summary>
        /// <param name="id">Id of object to be deleted from database.</param>
        public void Delete(int id);
    }
}
