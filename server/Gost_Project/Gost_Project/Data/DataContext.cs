using Gost_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Gost_Project.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<GostEntity> Gosts { get; set; } = null!;
    public DbSet<FieldEntity> Fields { get; set; } = null!;
    public DbSet<NormativeReferenceEntity> NormativeReferences { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.Migrate();
    }
}