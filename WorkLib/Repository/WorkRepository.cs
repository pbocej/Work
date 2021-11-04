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
                {
                    DeleteUser(user, c);
                    return;
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "delete from UserProjects where UserId=@userId;delete from Users where UserId=@userId;"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("userId", user.UserId, DbType.Int32));
                    var ret = cmd.ExecuteNonQuery();
                    if (ret == 0)
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
	([UserName],[FirstName],[LastName],[Email],[Phone],[UserGroupId])
     VALUES
	(@UserName,@FirstName,@LastName,@Email,@Phone,@UserGroupId);
SELECT CAST(@@IDENTITY AS int);";
                    else                  // edit
                    {
                        cmd.CommandText = @"
UPDATE [Users]
   SET [UserName] = @UserName,
       [FirstName] = @FirstName,
       [LastName] = @LastName,
       [Email] = @Email,
       [Phone] = @Phone,
       [UserGroupId] = @UserGroupId
 WHERE UserId=@UserId;
SELECT @UserId";
                        cmd.Parameters.Add(
                            context.CreateParameter("UserId", user.UserId, DbType.Int32));
                    }
                    var userId = (int)cmd.ExecuteScalar(); // get user id
                    if (userId > 0) // OK
                    {
                        user.UserId = userId; // get id after insert
                        // user projects
                        cmd.CommandText = "delete from UserProjects where UserId = @UserId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(
                            context.CreateParameter("UserId", userId, DbType.Int32));
                        cmd.ExecuteNonQuery();
                        foreach (var up in user.UserProjects.Where(a => a.Owned))
                        {
                            cmd.CommandText = "INSERT INTO UserProjects (UserId, ProjectId) VALUES (@UserId, @ProjectId)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(
                                context.CreateParameter("UserId", userId, DbType.Int32));
                            cmd.Parameters.Add(
                                context.CreateParameter("ProjectId", up.ProjectId, DbType.Int32));
                            if (cmd.ExecuteNonQuery() != 1)
                                throw new AppException("Error saving project for user.");
                        }
                    }
                    return user;
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

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <param name="context">The data context.</param>
        public static void ChangePassword(int userId, string password, DbContext context = null)
        {
            if (context == null)
                using (var c = new DbContext())
                    ChangePassword(userId, password, c);
            try
            {
                using (var cmd = context.CreateCommand("update Users set Password=@password where UserId=@userId"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("password", HashPassword(password)));
                    cmd.Parameters.Add(
                        context.CreateParameter("userId", userId, DbType.Int32));
                    if (cmd.ExecuteNonQuery() != 1)
                        throw new AppException("Password has not been changed.");
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error changing password.", ex);
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
        /// <returns>
        /// UserProjects[]
        /// </returns>
        /// <exception cref="DbContext">
        /// </exception>
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
                        cmd.CommandText = @"select p.ProjectId, p.ProjectName from UserProjects up
inner join Projects p on up.ProjectId = p.ProjectId
where up.UserId = @userId";
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
                        cmd.CommandText = @"select p.ProjectId, p.ProjectName from UserProjects up
inner join Projects p on up.ProjectId = p.ProjectId
where UserId=@userId";
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
        }
        /// <summary>
        /// Gets the user groups.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <returns></returns>
        public static UserGroup[] GetUserGroups(DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                    return GetUserGroups(c);
            try
            {
                using (var cmd = context.CreateCommand("select * from UserGroups order by UserGroupId"))
                {
                    var list = new List<UserGroup>();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            list.Add(new UserGroup(r));
                    return list.ToArray();
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException("Loading user groups failed.", ex);
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
        public static User[] GetProjectUsers(int projectId, DbContext context = null)
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
                return list.ToArray();
            }
            catch (Exception ex)
            {
                throw new AppException("Error loading project users.", ex);
            }
        }

        /// <summary>
        /// Saves the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="context">The data context.</param>
        /// <returns>Project</returns>
        /// <exception cref="WorkLib.Model.AppException">
        /// Error saving project.
        /// </exception>
        public static Project SaveProject(Project project, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                    return SaveProject(project, c);
            try
            {
                using (var cmd = context.CreateCommand())
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("ProjectName", project.ProjectName));
                    cmd.Parameters.Add(
                        context.CreateParameter("ProjectDescription", project.ProjectDescription));
                    if (project.ProjectId == 0) // insert
                        cmd.CommandText = @"
INSERT INTO [Projects]
	([ProjectName],[ProjectDescription])
     VALUES
	(@ProjectName,@ProjectDescription);
SELECT CAST(@@IDENTITY AS int);";
                    else                  // edit
                    {
                        cmd.CommandText = @"
UPDATE [Projects]
   SET [ProjectName] = @ProjectName,
       [ProjectDescription] = @ProjectDescription
 WHERE ProjectId=@ProjectId;
SELECT @ProjectId";
                        cmd.Parameters.Add(
                            context.CreateParameter("ProjectId", project.ProjectId, DbType.Int32));
                    }
                    var projectId = (int)cmd.ExecuteScalar(); // get user id
                    if (projectId > 0) // OK
                    {
                        project.ProjectId = projectId; // get id after insert
                        // user projects
                    }
                    return project;
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error saving project {project.ProjectName}.", ex);
            }
        }

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="context">The data context.</param>
        /// <exception cref="WorkLib.Model.AppException">
        /// Operation failed. No project deleted.
        /// </exception>
        public static void DeleteProject(Project project, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    DeleteProject(project, c);
                    return;
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "delete from UserProjects where ProjectId=@ProjectId;delete from Projects where ProjectId=@ProjectId;"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("ProjectId", project.ProjectId, DbType.Int32));
                    var ret = cmd.ExecuteNonQuery();
                    if (ret == 0)
                        throw new AppException("Operation failed. No project deleted.");
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error deleting project {project.ProjectName}.", ex);
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
                    @"select * from v_UserWorkList where UserId=@userId order by [From] asc",
                    CommandType.Text))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("UserId", userId, DbType.Int32));
                    using (var reader = cmd.ExecuteReader())
                    {
                        var works = new List<WorkHour>();
                        while (reader.Read())
                            works.Add(new WorkHour(reader));
                        return works.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error Error loading work.", ex);
            }
        }

        /// <summary>
        /// Saves the work hour.
        /// </summary>
        /// <param name="workHour">The work hour.</param>
        /// <param name="context">The data context.</param>
        /// <returns></returns>
        /// <exception cref="WorkLib.Model.AppException">
        /// Data cannot be saved.
        /// or
        /// Error saving work.
        /// </exception>
        internal static int SaveWorkHour(WorkHour workHour, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                    return SaveWorkHour(workHour, c);
            try
            {
                using (var cmd = context.CreateCommand())
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("Date", workHour.Date, DbType.DateTime));
                    cmd.Parameters.Add(
                        context.CreateParameter("From", workHour.From, DbType.DateTime));
                    cmd.Parameters.Add(
                        context.CreateParameter("To", workHour.To, DbType.DateTime));
                    cmd.Parameters.Add(
                        context.CreateParameter("Hours", workHour.Hours, DbType.DateTime));
                    cmd.Parameters.Add(
                        context.CreateParameter("ProjectId", workHour.ProjectId, DbType.Int32));
                    cmd.Parameters.Add(
                        context.CreateParameter("Subject", workHour.Subject));
                    cmd.Parameters.Add(
                        context.CreateParameter("Description", workHour.Description));
                    cmd.Parameters.Add(
                        context.CreateParameter("UserId", workHour.UserId, DbType.Int32));
                    if (workHour.WorkHourID == 0) // insert
                    {
                        cmd.CommandText =
@"INSERT INTO WorkHours
([Date],[From],[To],[Hours],[ProjectId],[Subject],[Description],[UserId])
VALUES (@Date,@From,@To,@Hours,@ProjectId,@Subject,@Description,@UserId);
SELECT CAST(@@IDENTITY as int)";
                    }
                    else // update
                    {
                        cmd.CommandText =
@"UPDATE [dbo].[WorkHours]
   SET [Date] = @Date
      ,[From] = @From
      ,[To] = @To
      ,[Hours] = @Hours
      ,[ProjectId] = @ProjectId
      ,[Subject] = @Subject
      ,[Description] = @Description
      ,[UserId] = @UserId
 WHERE WorkHourID=@workHourId;
SELECT @workHourId";
                        cmd.Parameters.Add(
                            context.CreateParameter("workHourId", workHour.WorkHourID, DbType.Int32));
                    }
                    workHour.WorkHourID = (int)cmd.ExecuteScalar();
                    if (workHour.WorkHourID == 0)
                        throw new AppException("Data cannot be saved.");
                    return workHour.WorkHourID;
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Error saving work.", ex);
            }
        }

        /// <summary>
        /// Deletes the work hour.
        /// </summary>
        /// <param name="workHour">The work hour.</param>
        /// <param name="context">The data context.</param>
        public static void DeleteWorkHour(WorkHour workHour, DbContext context)
        {
            if (context == null)
                using (var c = new DbContext())
                {
                    DeleteWorkHour(workHour, c);
                    return;
                }
            try
            {
                using (var cmd = context.CreateCommand(
                    "delete from WorkHours where WorkHourId=@workHourId;"))
                {
                    cmd.Parameters.Add(
                        context.CreateParameter("workHourId", workHour.WorkHourID, DbType.Int32));
                    var ret = cmd.ExecuteNonQuery();
                    if (ret == 0)
                        throw new AppException("Operation failed. No record deleted.");
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error deleting record.", ex);
            }
        }

        #endregion
    }
}
