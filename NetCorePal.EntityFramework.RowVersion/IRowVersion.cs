namespace NetCorePal.EntityFramework
{
    /// <summary>
    /// 表示使用int类型进行Row Version并发控制
    /// </summary>
    public interface IRowVersion
    {
        /// <summary>
        /// 行版本好，用以行级写并发控制
        /// </summary>
        int RowVersion { get; set; }
    }
}
