using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLib.Model;

namespace WorkLib.Repository
{
    /// <summary>
    /// Wrapper for data
    /// </summary>
    public static class WorkRepository
    {
        #region Users

        /// <summary>Gets the user by username.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>User</returns>
        public static User GetUser(string userName)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting user by name: {userName}", ex);
            }
            using (var context = new MyWorkEntities())
            {
                return
                    (from user in context.Users
                    where user.UserName == userName
                    select user).FirstOrDefault<User>();
            }
        }

        #endregion

        #region Projects
        #endregion

        #region Work

        #endregion
    }
}
