using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
            SaveChangeTest(context => { return context.SaveChanges(); });
            SaveChangeTest(context => { return context.SaveChangesAsync().Result; });
            SaveChangeTest(context => { return context.SaveChangesAsync(System.Threading.CancellationToken.None).Result; });
        }


        void SaveChangeTest(Func<TestDbContext, int> doSave)
        {
            //正常更新
            using (var db = new TestDbContext())
            {
                var entity = new AccountEntity
                {
                    Name = "n1",
                    UpdateTime = DateTime.Now
                };
                db.AccountEntities.Add(entity);

                Assert.AreEqual(1, doSave(db));
                Assert.AreEqual(1, entity.RowVersion);
                entity.Name = "n2";
                Assert.AreEqual(1, doSave(db));
                Assert.AreEqual(2, entity.RowVersion);
            }

            //构造旧数据
            AccountEntity old = new AccountEntity
            {
                Name = "old",
                UpdateTime = DateTime.Now
            };

            using (var db = new TestDbContext())
            {
                db.AccountEntities.Add(old);
                Assert.AreEqual(1, doSave(db));
            }


            using (var db = new TestDbContext())
            {
                var entity2 = new AccountEntity
                {
                    Id = old.Id,
                    Name = "n3",
                    RowVersion = old.RowVersion,
                    UpdateTime = DateTime.Now
                };
                entity2 = db.AccountEntities.Attach(entity2);
                db.Entry(entity2).Property(p => p.Name).IsModified = true;

                Assert.AreEqual(1, doSave(db));
                Assert.AreEqual(old.RowVersion + 1, entity2.RowVersion);
            }

            #region 触发并发
            
            using (var db = new TestDbContext())
            {
                var entity2 = new AccountEntity
                {
                    Id = old.Id,
                    Name = "n3",
                    RowVersion = 100,
                    UpdateTime = DateTime.Now
                };
                entity2 = db.AccountEntities.Attach(entity2);

                db.Entry(entity2).Property(p => p.Name).IsModified = true;

                try
                {
                    doSave(db);
                    Assert.Fail("此处应该抛出异常");
                }
                catch (Exception ex)
                {
                    AssertException(ex);
                }
            }

            using (var db = new TestDbContext())
            {
                var entity = db.AccountEntities.FirstOrDefault();
                entity.Name += "1";
                entity.RowVersion += 1;
                try
                {
                    doSave(db);
                    Assert.Fail("此处应该抛出异常");
                }
                catch (Exception ex)
                {
                    AssertException(ex);
                }

            }
            #endregion
            
        }


        void AssertException(Exception ex)
        {
            if (ex is System.AggregateException)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(DbUpdateConcurrencyException));
            }
            else
            {
                Assert.IsInstanceOfType(ex, typeof(DbUpdateConcurrencyException));
            }
        }












    }
}
;