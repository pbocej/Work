using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
