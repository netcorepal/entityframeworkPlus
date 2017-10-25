using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.EntityFramework.Test
{
    [TestClass]
    public class RowVersionDbContextTest
    {
        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

            using (var db = new TestDbContext())
            {
                db.Database.Delete();

            }

            using (var db = new TestDbContext())
            {
                db.Database.CreateIfNotExists();
            }
        }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        [TestMethod]
        public void SaveChangesTest()
        {
            using (var db = new TestDbContext())
            {
                var entity = new AccountEntity
                {
                    Name = "n1",
                    UpdateTime = DateTime.Now
                };
                db.AccountEntities.Add(entity);
                db.SaveChanges();

                Assert.AreEqual(1, entity.RowVersion);

                entity.Name = "n2";

                db.SaveChanges();

                Assert.AreEqual(2, entity.RowVersion);
            }
        }


        [TestMethod]
        public void SaveChangesAsyncTest()
        {
            using (var db = new TestDbContext())
            {
                var entity = new AccountEntity
                {
                    Name = "n1",
                    UpdateTime = DateTime.Now
                };
                db.AccountEntities.Add(entity);
                var i=  db.SaveChangesAsync().Result;

                Assert.AreEqual(1, entity.RowVersion);

                entity.Name = "n2";

                i= db.SaveChangesAsync().Result;

                Assert.AreEqual(2, entity.RowVersion);
            }
        }

    }
}
;