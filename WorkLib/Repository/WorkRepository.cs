using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /// <summary>Gets the user by username.</summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>User</returns>
        public static User GetUser(string userName)
        {
            try
            {
                using (var context = new DbContext())
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new AppException($"Error getting user by name: {userName}", ex);
            }
        }

        #endregion

        #region Projects
        #endregion

        #region Work

        #endregion
    }
}
