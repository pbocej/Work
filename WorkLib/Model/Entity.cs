using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLib.Model
{
    /// <summary>Base class for entity.</summary>
    /// <typeparam name="T">IEntity class.</typeparam>
    public class Entity<T> : IEntity where T : IEntity
    {
        /// <summary>Initializes a new instance of the <see cref="Entity{T}" /> class.</summary>
        /// <param name="data">The data.</param>
        public Entity(object data = null)
        {
            if (data == null) return;

            if (data is IDataReader reader)
                Load(reader);
            if (data is DataRow row)
                Load(row);
        }
        /// <summary>
        /// Loads the specified entity from data reader.
        /// </summary>
        /// <param name="reader">
        /// The data reader.
        /// </param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Load(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the specified entity from data row.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Load(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
