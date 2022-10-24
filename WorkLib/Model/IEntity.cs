using System.Data;
using WorkLib.Data;

namespace WorkLib.Model
{
    /// <summary>
    ///   <para>Interface for base entity class.</para>
    /// </summary>
    public interface IEntity
    {
        /// <summary>Loads the specified entity from data reader.</summary>
        /// <param name="reader">The data reader.</param>
        void Load(IDataReader reader);
        /// <summary>Loads the specified entity from data row.</summary>
        /// <param name="row">The data row.</param>
        void Load(DataRow row);
        /// <summary>
        /// Saves current object to data.
        /// </summary>
        /// <param name="context">The data context.</param>
        int Save(DbContext context = null);
        /// <summary>
        /// Deletes the current object.
        /// </summary>
        /// <param name="context">The data context.</param>
        void Delete(DbContext context = null);
    }
}
