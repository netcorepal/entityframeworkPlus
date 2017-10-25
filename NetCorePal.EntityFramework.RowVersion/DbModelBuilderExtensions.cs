using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace NetCorePal.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbModelBuilderExtensions
    {
        /// <summary>
        /// 将IRowVersion接口相关字段设置为并发控制字段，一般在DbContext的OnModelCreating时调用
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        public static void SetRowVersionAsConcurrencyToken(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(o => typeof(IRowVersion).IsAssignableFrom(o.DeclaringType) && o.PropertyType == typeof(int) && o.Name == "RowVersion")
                                     .Configure(o => o.IsConcurrencyToken().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None));
        }
    }
}
