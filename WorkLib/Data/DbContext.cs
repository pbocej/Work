using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using WorkLib.Model;
using System.Configuration;
using System.Reflection;

namespace WorkLib.Data
{
    /// <summary>Database access class</summary>
    public class DbContext : IDisposable
    {
        private IDbConnection dbConnection;
        private static DbProviderFactory factory;
        private static string connectionString;

        #region Initialization 

        /// <summary>Initializes a new instance of the <see cref="DbContext" /> class.</summary>
        /// <param name="name">The connection name in the config file.</param>
        public DbContext(string name = "Data")
        {
            LoadDataConfiguration(name);
            Open();
        }

        /// <summary>Loads the data configuration.</summary>
        /// <param name="name">The connection name.</param>
        /// <exception cref="WorkLib.Model.AppException">Error loading data provider. Provider not found.
        /// or
        /// Error loading configuration.</exception>
        private void LoadDataConfiguration(string name)
        {
            if (factory != null)
                return;
            try
            {
                var providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
                factory = DbProviderFactories.GetFactory(providerName);
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new AppException("Error loading configuration.", ex);
            }
        }

        #endregion

        #region Connection

        /// <summary>Opens this connection.</summary>
        /// <exception cref="WorkLib.Model.AppException">Error opening database.</exception>
        public void Open()
        {
            try
            {
                Close();
                dbConnection = factory.CreateConnection();
                dbConnection.ConnectionString = connectionString;
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                throw new AppException("Error opening database.", ex);
            }
        }

        /// <summary>
        ///   <para>
        /// Closes this database.
        /// </para>
        /// </summary>
        public void Close()
        {
            if (dbConnection != null)
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Close();
            dbConnection = null;
        }

        #endregion

        #region IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

    }
}
