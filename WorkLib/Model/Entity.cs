using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                select p).ToArray();
            foreach (var pi in properties)
            {
                var pos = reader.GetOrdinal(pi.Name);
                if (pos > -1)
                    pi.SetValue(this, reader.GetValue(pos));
            }
        }

        /// <summary>
        /// Loads the specified entity from data row.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Load(DataRow row)
        {
            var properties = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                select p).ToArray();
            foreach (var pi in properties)
            {
                try
                {
                    pi.SetValue(this, row[pi.Name]);
                }
                catch
                { }
            }
        }
    }
}
