# NetCorePal.EntityFramework.RowVersion

Row concurrency control  support for entityframework 6

## Install
```
Install-Package NetCorePal.EntityFramework.RowVersion
```

## How to use

1. Replace DbContext whith RowVersionDbContext or use DbModelBuilderExtensions and DbContextExtensions
```
public class YourDbContext : RowVersionDbContext
{

}

or

public class YourDbContext : DbContext
{
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.SetRowVersionAsConcurrencyToken(); //add this code
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        this.DetectRowVersion();  //add this code
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        this.DetectRowVersion(); //add this code
        return base.SaveChangesAsync(cancellationToken);
    }
}
```


2. Add RowVersion to your entity
```
public class YourEntity : IRowVersion
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTime UpdateTime { get; set; }

    public int RowVersion { get; set; } //add this code
}
```


3. Update your data like this:
```
using (var db = new YourDbContext())
{
    var entity = db.YourEntities.Find(id);
    entity.Name += "your new name";
    db.SaveChange();
}

or

using (var db = new YourDbContext())
{
    var entity = new YourEntity
    {
        Id = Id,
        Name = "new name",
        RowVersion = oldRowVerion  //use the old RowVersion value
    };
    entity = db.YourEntities.Attach(entity);
    db.Entry(entity).Property(p => p.Name).IsModified = true;
    db.SaveChange();
}
```