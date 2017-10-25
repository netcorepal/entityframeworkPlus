using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace NetCorePal.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// 检测并设置RowVersion，需要在base.SaveChange以前调用
        /// </summary>
        /// <param name="context"></param>
        public static void DetectRowVersion(this DbContext context)
        {
            context.ChangeTracker.DetectChanges();
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            foreach (ObjectStateEntry entry in objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Added))
            {
                var v = entry.Entity as IRowVersion;
                if (v != null)
                {
                    var ps = entry.GetModifiedProperties();
                    if (ps.Any(p => p == "RowVersion"))
                    {
                        throw new DbUpdateConcurrencyException("RowVersion在更新前已经被修改"); //如果RowVersion在更新前就已经修改,说明并发异常了;
                    }
                    v.RowVersion += 1;
                }
            }
        }
    }
}
