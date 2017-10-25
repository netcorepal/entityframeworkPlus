# NetCorePal.EntityFramework.RowVersion

Row concurrency control  support for entityframework 6

## install
```
Install-Package NetCorePal.EntityFramework.RowVersion
```

## How to use

Replace DbContext whith RowVersionDbContext
```
public class YourDbContext : RowVersionDbContext
{

}
```

or

use DbModelBuilderExtensions and DbContextExtensions
```
public class YourDbContext : DbContext
{
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.SetRowVersionAsConcurrencyToken(); //add this row
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        this.DetectRowVersion();  //add this row
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        this.DetectRowVersion(); //add this row
        return base.SaveChangesAsync(cancellationToken);
    }
}
```