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
        private IDbConnection _dbConnection;
        private static DbProviderFactory _factory;
        private static string _connectionString;

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
            if (_factory != null)
                return;
            try
            {
                var providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
                _factory = DbProviderFactories.GetFactory(providerName);
                _connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
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
                _dbConnection = _factory.CreateConnection();
                _dbConnection.ConnectionString = _connectionString;
                _dbConnection.Open();
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
            if (_dbConnection != null)
                if (_dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            _dbConnection = null;
        }

        /// <summary>Gets the connection.</summary>
        /// <value>The connection.</value>
        public IDbConnection Connection => _dbConnection;

        #endregion

        #region Command & Parameters

        /// <summary>Creates the database command.</summary>
        /// <param name="sql">The SQL command string</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>IDbCommand</returns>
        /// <exception cref="WorkLib.Model.AppException">Error creating database command.</exception>
        public IDbCommand CreateCommand(string sql = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                IDbCommand cmd = _dbConnection.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandText = sql;
                return cmd;
            }
            catch (Exception ex)
            {
                throw new AppException("Error creating database command.", ex);
            }
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
