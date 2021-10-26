using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLib.Data;
using WorkLib.Model;

namespace WorkLib.Repository.Tests
{
    [TestClass()]
    public class WorkRepositoryTests
    {
        static DbContext context;

        #region Initialize & terminate

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            context = new DbContext();
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        [ClassCleanup]
        public static void Close()
        {
            context.Dispose();
        }

        #endregion

        #region User

        [TestMethod()]
        public void GetUserTest1()
        {
            // check null value
            var user = WorkRepository.GetUser(0, context);
            Assert.IsNull(user);
            // check not null value
            user = WorkRepository.GetUser(1, context);
            Assert.IsNotNull(user);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            // check null value
            var user = WorkRepository.GetUser("peter1", context);
            Assert.IsNull(user);
            // check not null value
            user = WorkRepository.GetUser("Peter", context);
            Assert.IsNotNull(user);
        }

        #endregion

        #region Project

        [TestMethod()]
        public void GetProjectTest()
        {
            try
            {
                var proj = WorkRepository.GetProject(1, context);
                Assert.IsNotNull(proj);
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }
        
        #endregion
    }
}