using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
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
        [TestMethod]
        public void CreateCommandTest()
        {
            try
            {
                using (var db = new DbContext())
                using (var cmd = db.CreateCommand("select @@version"))
                {
                    var version = cmd.ExecuteScalar() as string;
                    Assert.IsNotNull(version);
                }
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }
        [TestMethod]
        public void CreateParameterTest()
        {
            try
            {
                using (var db = new DbContext())
                using (var cmd = db.CreateCommand("select * from Users where UserId=@id"))
                {
                    cmd.Parameters.Add(db.CreateParameter("id", 1, DbType.Int32));
                    using (var r = cmd.ExecuteReader())
                        Assert.IsTrue(r.Read());
                }
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }

        [TestMethod()]
        public void GetDataSetTest()
        {
            try
            {
                using (var db = new DbContext())
                using (var cmd = db.CreateCommand("select top 10 * from Users; select * from UserGroups;"))
                {
                    var ds = db.GetDataSet(cmd);
                    Assert.IsNotNull(ds);
                    Assert.AreEqual(2, ds.Tables.Count);
                }
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }

        [TestMethod()]
        public void GetTableTest()
        {
            try
            {
                using (var db = new DbContext())
                using (var cmd = db.CreateCommand("select top 10 * from Users"))
                {
                    var tbl = db.GetDataSet(cmd);
                    Assert.IsNotNull(tbl);
                }
            }
            catch (AppException ex)
            {
                Assert.Fail(ex.FullMessage);
            }
        }
    }
}