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

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<DocEntity> Docs { get; set; } = null!;
    public DbSet<FieldEntity> Fields { get; set; } = null!;
    public DbSet<DocReferenceEntity> DocsReferences { get; set; } = null!;
    public DbSet<DocStatisticEntity> DocStatistics { get; set; } = null!;
}