using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODOListDomainModel.Classes;

namespace TODOListDomainModel.Interfaces
{
    /// <summary>
    /// Generic interface for repositories.
    /// </summary>
    /// <typeparam name="TEntity">Class of objects that will be stored in database.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Insert ToDo listobject to database.
        /// </summary>
        /// <param name="t">Object to be inserted into dataabse.</param>
        /// <returns>Task becuase method is async.</returns>
        public Task<TEntity> Insert(TEntity t);

        /// <summary>
        /// Update ToDo listobejct in databse.
        /// </summary>
        /// <param name="t">Object to be updated in database.</param>
        public void Update(TEntity t);

        /// <summary>
        /// Gets all ToDo lists, marked as not hidden.
        /// </summary>
        /// <returns>List of objects in database.</returns>
        public Task<List<TEntity>> GetAll();

        /// <summary>
        /// Gets object by ID from datab.
        /// </summary>
        /// <param name="id">Wanted ToDo listID.</param>
        /// <returns>Found ToDo list object, null if there is no such.</returns>
        public Task<TEntity> GetByID(int id);

        /// <summary>
        /// Delete ToDo list object form database.
        /// </summary>
        /// <param name="t">Object to be deleted from database.</param>
        public void Delete(TEntity t);
    }
}
