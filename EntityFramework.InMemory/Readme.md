# EntityFramework.InMemory

### Packages:

EntityFrameworkCore:

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.11

EntityFrameworkCore.InMemory:

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/5.0.11

### Create context

```csharp
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }

    public DbSet<Pet> Pets { get; set; }
}
```

### Setup in Startup.cs

```csharp
services.AddDbContext<DataContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "EntityFrameworkInMemory");
});
```

### Example

https://github.com/byCodeXp/asp.net/tree/main/EntityFramework.InMemory/Api
