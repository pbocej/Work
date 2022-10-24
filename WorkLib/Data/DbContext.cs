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
                _dbConnection.Dispose();
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

        /// <summary>
        /// Creates the data parameter for command.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value value (default: null).</param>
        /// <param name="dbType">Database type (default: string).</param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string name, object value = null, DbType dbType = DbType.String)
        {
            IDataParameter par = _factory.CreateParameter();
            par.ParameterName = name;
            par.DbType = dbType;
            if (value == null)
                par.Value = DBNull.Value;
            else
                par.Value = value;
            return par;
        }

        #endregion

        #region Transaction

        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>IDbTransaction</returns>
        /// <exception cref="WorkLib.Model.AppException">Error creating database transaction.</exception>
        public IDbTransaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            try
            {
                return _dbConnection.BeginTransaction(isolationLevel);
            }
            catch (Exception ex)
            {
                throw new AppException("Error creating database transaction.", ex);
            }
        }

        #endregion

        #region DataSet & DataTable

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="cmd">The db command.</param>
        /// <returns>DataSet</returns>
        /// <exception cref="WorkLib.Model.AppException">Error creating DataSet.</exception>
        public DataSet GetDataSet(IDbCommand cmd)
        {
            try
            {
                var ds = new DataSet();
                var adapter = _factory.CreateDataAdapter();
                adapter.SelectCommand = (DbCommand)cmd;
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new AppException("Error creating DataSet.", ex);
            }
        }

        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <param name="cmd">The db command.</param>
        /// <returns>Datatable</returns>
        /// <exception cref="WorkLib.Model.AppException">Error creating DataSet.</exception>
        public DataTable GetTable(IDbCommand cmd)
        {
            try
            {
                var tbl = new DataTable();
                var adapter = _factory.CreateDataAdapter();
                adapter.SelectCommand = (DbCommand)cmd;
                adapter.Fill(tbl);
                return tbl;
            }
            catch (Exception ex)
            {
                throw new AppException("Error creating DataSet.", ex);
            }
        }

        #endregion

        #region Get values

        /// <summary>
        /// Gets the value from reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="field">The field.</param>
        /// <returns>Field`s value</returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting '{field}' value from reader.</exception>
        public static object GetValue(IDataReader reader, string field)
        {
            try
            {
                var pos = reader.GetOrdinal(field);
                return reader.GetValue(pos);
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting '{field}' value from reader.", ex);
            }
        }
        /// <summary>
        /// Gets the value from reader.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <param name="field">The field.</param>
        /// <returns>Field`s value</returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting '{field}' value from data row.</exception>
        public static object GetValue(DataRow row, string field)
        {
            try
            {
                return row[field];
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting '{field}' value from data row.", ex);
            }
        }

        /// <summary>
        /// Gets the value from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The data reader.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting '{field}' value from reader.</exception>
        public static T GetValue<T>(IDataReader reader, string field)
        {
            try
            {
                var val = GetValue(reader, field);
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting '{field}' value from reader.", ex);
            }
        }
        /// <summary>
        /// Gets the value from data row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">The data row.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting '{field}' value from data row.</exception>
        public static T GetValue<T>(DataRow row, string field)
        {
            try
            {
                var val = GetValue(row, field);
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting '{field}' value from data row.", ex);
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
