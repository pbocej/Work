using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLib.Repository.Tests
{
    [TestClass()]
    public class WorkRepositoryTests
    {
        [TestMethod()]
        public void GetUserTest()
        {
            // check null value
            var user = WorkRepository.GetUser("peter1");
            Assert.IsNull(user);
            // check not null value
            user = WorkRepository.GetUser("Peter");
            Assert.IsNotNull(user);
        }
    }
}