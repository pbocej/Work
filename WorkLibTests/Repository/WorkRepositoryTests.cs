using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLib.Data;

namespace WorkLib.Repository.Tests
{
    [TestClass()]
    public class WorkRepositoryTests
    {
        DbContext context;
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [ClassInitialize]
        public void Init()
        {
            context = new DbContext();
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        [ClassCleanup]
        public void Close()
        {
            context.Dispose();
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
    }
}