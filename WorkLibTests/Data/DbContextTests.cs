using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLib.Model;

namespace WorkLib.Data.Tests
{
    [TestClass()]
    public class DbContextTests
    {
        [TestMethod()]
        public void DbContextConstructorTest()
        {
            try
            {
                using (var db = new DbContext())
                {

                }
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }
    }
}