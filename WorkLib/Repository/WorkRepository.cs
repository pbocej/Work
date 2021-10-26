﻿using System;
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


        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="context">The data context.</param>
        /// <returns></returns>
        /// <exception cref="WorkLib.Model.AppException">
        /// Invalid password.
        /// or
        /// User not found.
        /// or
        /// Login failed.
        /// </exception>
        public static User Login(string userName, string password, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    return Login(userName, password, c);
            try
            {
                using (var cmd = context.CreateCommand("select * from Users where UserName=@username"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("username", userName));
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var user = new User(r);
                            if (user.Password == HashPassword(password))
                                return user;
                            else throw new AppException("Invalid password.");
                        }
                        else throw new AppException("User not found.");
                    }
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException("Login failed.", ex);
            }
        }

        private static string HashPassword(string password)
        {
            return password;
        }

        #endregion

        #region Projects

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="context">The data context.</param>
        /// <returns>Project</returns>
        public static Project GetProject(int projectId, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    return GetProject(projectId, c);
            try
            {
                using (var cmd = context.CreateCommand("select * from Projects where ProjectId=@id"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("id", projectId, DbType.Int32));
                    using (var reader = cmd.ExecuteReader())
                        if (reader.Read())
                            return new Project(reader);
                }
                return null;
            }
            catch (AppException)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the project users.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="DbContext"></exception>
        public static ICollection<User> GetProjectUsers(int projectId, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    return GetProjectUsers(projectId, c);
                }
            try
            {
                var list = new List<User>();
                using (var cmd = context.CreateCommand("select UserId, UserName, FirstName, LastName from v_UserProjects where ProjectId=@id"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("id", projectId, DbType.Int32));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(new User(reader));
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new AppException("Error loading project users.", ex);
            }
        }
        #endregion

        #region Work

        #endregion
    }
}
