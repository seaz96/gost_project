using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        //Database.Migrate();
        //Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Document> Documents { get; set; } = null!;
    public DbSet<Field> Fields { get; set; } = null!;
    public DbSet<DocumentReference> References { get; set; } = null!;
    public DbSet<UserAction> UserActions { get; set; } = null!;
}