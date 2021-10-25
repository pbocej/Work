using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLib.Data;
using WorkLib.Model;

namespace WorkLib.Repository
{
    /// <summary>
    /// Wrapper for data
    /// </summary>
    public static class WorkRepository
    {
        #region Users

        /// <summary>
        /// Gets the user by Id.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns>User</returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting user by id: {id}</exception>
        public static User GetUser(int id, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    return GetUser(id, context);
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "select * from Users where UserId = @id",
                    CommandType.Text))
                {
                    cmd.Parameters.Add(context.CreateParameter("id", id));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new User(reader);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting user by id: {id}", ex);
            }
        }

        /// <summary>Gets the user by username.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="context">Data context</param>
        /// <returns>User</returns>
        public static User GetUser(string userName, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    return GetUser(userName, c);
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "select * from Users where Username = @Username", 
                    CommandType.Text))
                {
                    cmd.Parameters.Add(context.CreateParameter("Username", userName));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new User(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting user by name: {userName}", ex);
            }
            return null;
        }

        #endregion

        #region Projects
        #endregion

        #region Work

        #endregion
    }
}
