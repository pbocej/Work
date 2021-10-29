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
        #region Entity

        #endregion

        #region Users

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <returns>User[]</returns>
        /// <exception cref="WorkLib.Model.AppException">Error getting users</exception>
        public static User[] GetAllUsers(DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    return GetAllUsers(c);
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "select * from Users",
                    CommandType.Text))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var users = new List<User>();
                        while (reader.Read())
                            users.Add(new User(reader));
                        return users.OrderBy(u => u.FullName).ToArray<User>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error getting users", ex);
            }
        }
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
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting user by name: {userName}", ex);
            }
            return null;
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="context">The context.</param>
        /// <exception cref="WorkLib.Model.AppException">Error deleting user {user.FullName}.</exception>
        public static void DeleteUser(User user, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                    DeleteUser(user, c);
            try
            {
                using (var cmd = context.CreateCommand("delete from Users where UserId=id"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("id", user.UserId, DbType.Int32));
                    var ret = cmd.ExecuteNonQuery();
                    if (ret != 1)
                        throw new AppException("Operation failed. No user deleted.");
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error deleting user {user.FullName}.", ex);
            }
        }
        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="DbContext">
        /// </exception>
        public static User SaveUser(User user, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                    return SaveUser(user, c);
            try
            {
                using (var cmd = context.CreateCommand())
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("UserName", user.UserName));
                    cmd.Parameters.Add(
                        context.CreateParameter("Password", user.Password));
                    cmd.Parameters.Add(
                        context.CreateParameter("FirstName", user.FirstName));
                    cmd.Parameters.Add(
                        context.CreateParameter("LastName", user.LastName));
                    cmd.Parameters.Add(
                        context.CreateParameter("Email", user.Email));
                    cmd.Parameters.Add(
                        context.CreateParameter("Phone", user.Phone));
                    cmd.Parameters.Add(
                        context.CreateParameter("UserGroupId", user.UserGroupId, DbType.Int32));
                    if (user.UserId == 0) // insert
                        cmd.CommandText = @"
INSERT INTO [Users]
	([UserName],[Password],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES
	(@UserName,@Password,@FirstName,@LastName,@Email,@Phone,@UserGroupId);
SELECT @@IDENTITY;";
                    else                  // edit
                    {
                        cmd.CommandText = @"
UPDATE [Users]
   SET [UserName] = @UserName,
       [Password] = @Password,
       [FirstName] = @FirstName,
       [LastName] = @LastName,
       [Email] = @Email,
       [Phone] = @Phone,
       [UserGroupId] = @UserGroupId
 WHERE UserId=@UserId";
                        cmd.Parameters.Add(
                            context.CreateParameter("UserId", user.UserId, DbType.Int32));
                    }
                    var userId = cmd.ExecuteNonQuery();
                    if (userId > 0) // OK
                    {
                        cmd.CommandText = "select * from Users where UserId=@UserId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(
                            context.CreateParameter("UserId", userId, DbType.Int32));
                        using (var r = cmd.ExecuteReader())
                            if (r.Read())
                                return new User(r);
                    }
                    throw new AppException("Operation failed. No user saved.");
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error saving user {user.FullName}.", ex);
            }
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

        /// <summary>
        /// Gets the user projects.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="allProjects">if set to <c>true</c> [all projects], otherwise only projects owned by user.</param>
        /// <param name="context">The context.</param>
        /// <returns>UserProjects[]</returns>
        /// <exception cref="WorkLib.Model.AppException">Loading projects for user failed.</exception>
        public static UserProject[] GetUserProjects(int userId, bool allProjects = false, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    return GetUserProjects(userId, allProjects, c);
            try
            {
                using (var cmd = context.CreateCommand())
                {
                    var projects = new List<UserProject>();
                    if (allProjects)
                    {
                        WorkRepository.GetAllProjects(context)
                            .ToList()
                            .ForEach(p => projects.Add(new UserProject(p)));
                        cmd.CommandText = "select * from UserProjects where UserId=@userId";
                        cmd.Parameters.Add(
                            context.CreateParameter("userId", userId, DbType.Int32));
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                var projectId = DbContext.GetValue<int>(r, "ProjectId");
                                projects.Where(p => p.ProjectId == projectId)
                                    .ToList()
                                    .ForEach(p => { p.Owned = true; p.UserId = userId; return; });
                            }
                            return projects.ToArray();
                        }
                    }
                    else
                    {
                        cmd.CommandText = "select * from UserProjects where UserId=@userId";
                        cmd.Parameters.Add(
                            context.CreateParameter("userId", userId, DbType.Int32));
                        using (var r = cmd.ExecuteReader())
                        {
                            var list = new List<UserProject>();
                            while (r.Read())
                                list.Add(new UserProject(r));
                            return list.ToArray();
                        }
                    }
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException("Loading projects for user failed.", ex);
            }

            public static UserGroup[] GetUserGroups(DbContext context1)
            {
                if (context == null)
                    using (var c = new DbContext())
                        return GetUserGroups(c);

            }
        }
        #endregion

        #region Projects

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <returns>Project[]</returns>
        /// <exception cref="WorkLib.Model.AppException">Error loadint projects.</exception>
        public static Project[] GetAllProjects(DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    return GetAllProjects(c);
            try
            {
                using (var cmd = context.CreateCommand(
                    "select * from Projects",
                    CommandType.Text))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var projects = new List<Project>();
                        while (reader.Read())
                            projects.Add(new Project(reader));
                        return projects.OrderBy(p => p.ProjectName).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error loadint projects.", ex);
            }
        }
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

        /// <summary>
        /// Gets all work.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="context">The data context.</param>
        /// <returns>WorkHour[]></returns>
        /// <exception cref="WorkLib.Model.AppException">Error loading work.</exception>
        public static WorkHour[] GetAllWork(int userId, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    return GetAllWork(userId, c);
            try
            {
                using (var cmd = context.CreateCommand(
                    "select * from WorkHours where UserId=@userId",
                    CommandType.Text))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("UserId", userId, DbType.Int32));
                    using (var reader = cmd.ExecuteReader())
                    {
                        var works = new List<WorkHour>();
                        while (reader.Read())
                            works.Add(new WorkHour(reader));
                        return works.OrderBy(w => w.Date).ThenBy(u => u.From).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error Error loading work.", ex);
            }
        }

        #endregion
    }
}
