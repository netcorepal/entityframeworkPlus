using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace NetCorePal.EntityFramework
{
    /// <summary>
    ///    A DbContext instance represents a combination of the Unit Of Work and Repository
    ///    patterns such that it can be used to query from a database and group together
    ///    changes that will then be written back to the store as a unit. DbContext is conceptually
    ///    similar to ObjectContext.
    /// </summary>
    /// <remarks>
    ///    DbContext is usually used with a derived type that contains System.Data.Entity.DbSet`1
    ///    properties for the root entities of the model. These sets are automatically initialized
    ///    when the instance of the derived class is created. This behavior can be modified
    ///    by applying the System.Data.Entity.Infrastructure.SuppressDbSetInitializationAttribute
    ///    attribute to either the entire derived context class, or to individual properties
    ///    on the class. The Entity Data Model backing the context can be specified in several
    ///    ways. When using the Code First approach, the System.Data.Entity.DbSet`1 properties
    ///    on the derived context are used to build a model by convention. The protected
    ///    OnModelCreating method can be overridden to tweak this model. More control over
    ///    the model used for the Model First approach can be obtained by creating a System.Data.Entity.Infrastructure.DbCompiledModel
    ///    explicitly from a System.Data.Entity.DbModelBuilder and passing this model to
    ///    one of the DbContext constructors. When using the Database First or Model First
    ///    approach the Entity Data Model can be created using the Entity Designer (or manually
    ///    through creation of an EDMX file) and then this model can be specified using
    ///    entity connection string or an System.Data.Entity.Core.EntityClient.EntityConnection
    ///    object. The connection to the database (including the name of the database) can
    ///    be specified in several ways. If the parameterless DbContext constructor is called
    ///    from a derived context, then the name of the derived context is used to find
    ///    a connection string in the app.config or web.config file. If no connection string
    ///    is found, then the name is passed to the DefaultConnectionFactory registered
    ///    on the System.Data.Entity.Database class. The connection factory then uses the
    ///    context name as the database name in a default connection string. (This default
    ///    connection string points to .\SQLEXPRESS on the local machine unless a different
    ///    DefaultConnectionFactory is registered.) Instead of using the derived context
    ///    name, the connection/database name can also be specified explicitly by passing
    ///    the name to one of the DbContext constructors that takes a string. The name can
    ///    also be passed in the form "name=myname", in which case the name must be found
    ///    in the config file or an exception will be thrown. Note that the connection found
    ///    in the app.config or web.config file can be a normal database connection string
    ///    (not a special Entity Framework connection string) in which case the DbContext
    ///    will use Code First. However, if the connection found in the config file is a
    ///    special Entity Framework connection string, then the DbContext will use Database/Model
    ///    First and the model specified in the connection string will be used. An existing
    ///    or explicitly created DbConnection can also be used instead of the database/connection
    ///    name. A System.Data.Entity.DbModelBuilderVersionAttribute can be applied to a
    ///    class derived from DbContext to set the version of conventions used by the context
    ///    when it creates a model. If no attribute is applied then the latest version of
    ///    conventions will be used.
    /// </remarks>
    public class RowVersionDbContext : DbContext
    {
        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made. See the class remarks
        /// for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public RowVersionDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        /// <summary>
        ///    Constructs a new context instance using the given string as the name or connection
        ///    string for the database to which a connection will be made, and initializes it
        ///    from the given model. See the class remarks for how this is used to create a
        ///    connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public RowVersionDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
       
        /// <summary>
        ///    Constructs a new context instance using the existing connection to connect to
        ///    a database. The connection will not be disposed when the context is disposed
        ///    if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">
        ///     If set to true the connection is disposed when the context is disposed, otherwise
        ///     the caller must dispose the connection.
        /// </param>
        public RowVersionDbContext(DbConnection existingConnection, bool contextOwnsConnection) { }

        /// <summary>
        ///    Constructs a new context instance around an existing ObjectContext.
        /// </summary>
        /// <param name="objectContext">An existing ObjectContext to wrap with the new context.</param>
        /// <param name="dbContextOwnsObjectContext">
        ///    If set to true the ObjectContext is disposed when the DbContext is disposed,
        ///    otherwise the caller must dispose the connection.
        /// </param>
        public RowVersionDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) { }

        /// <summary>
        ///    Constructs a new context instance using the existing connection to connect to
        ///    a database, and initializes it from the given model. The connection will not
        ///    be disposed when the context is disposed if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        ///    If set to true the connection is disposed when the context is disposed, otherwise
        ///    the caller must dispose the connection.
        /// </param>
        public RowVersionDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) { }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.SetRowVersionAsConcurrencyToken();
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.DetectRowVersion();
            return base.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.DetectRowVersion();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
