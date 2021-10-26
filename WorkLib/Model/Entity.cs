using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WorkLib.Data;
using System.ComponentModel.DataAnnotations;

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
                from p in this.GetType().GetProperties(
                    BindingFlags.Public | 
                    BindingFlags.Instance | 
                    BindingFlags.SetProperty)
                where p.CanWrite
                select p).ToArray();
            foreach (var pi in properties)
            {
                try
                {
                    pi.SetValue(this, DbContext.GetValue(reader, pi.Name));
                }
                catch (Exception ex)
                {
                    throw new AppException($"Column '{pi.Name}' not found.", ex);
                }
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
                    pi.SetValue(this, DbContext.GetValue(row, pi.Name));
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// Compares property with KeyAttribute defined.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType().Name == this.GetType().Name)
            {
                var prop = (
                    from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    where Attribute.IsDefined(p, typeof(KeyAttribute))
                    select p).FirstOrDefault();
                int objId = (int)prop.GetValue(obj);
                int thisId = (int)prop.GetValue(this);
                return thisId.Equals(objId);
            }
            else return false;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var prop = (
                from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                where Attribute.IsDefined(p, typeof(KeyAttribute))
                select p).FirstOrDefault();
            int id = (int)prop.GetValue(this);
            return id.GetHashCode();
        }
    }
}
