using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.EntityFramework.Test
{
    public class TestDbContext : RowVersionDbContext
    {
        public TestDbContext() : base("testDb")
        {

        }
        public DbSet<AccountEntity> AccountEntities { get; set; }
    }
}
