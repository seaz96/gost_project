using Gost_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<DocEntity> Docs { get; set; } = null!;
    public DbSet<FieldEntity> Fields { get; set; } = null!;
    public DbSet<DocReferenceEntity> DocsReferences { get; set; } = null!;
    public DbSet<DocStatisticEntity> DocStatistics { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.Migrate();
    }
}