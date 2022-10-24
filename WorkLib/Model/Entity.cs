using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using WorkLib.Data;

namespace WorkLib.Model
{
    /// <summary>Base class for entity.</summary>
    /// <typeparam name="T">IEntity class.</typeparam>
    public abstract class Entity<T> : IEntity where T : IEntity
    {
        /// <summary>Initializes a new instance of the <see cref="Entity{T}" /> class.</summary>
        /// <param name="data">The data reader or data row.</param>
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
                catch (Exception)
                { } // column does not exists
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

        /// <summary>
        /// Saves current object to data.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract int Save(DbContext context = null);

        /// <summary>
        /// Deletes the current object.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract void Delete(DbContext context = null);
    }
}
