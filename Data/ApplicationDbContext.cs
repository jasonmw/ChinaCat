using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace ChinaCatSunflower.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL uses the public schema by default - not dbo.
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);

        //Rename Identity tables to lowercase
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetDefaultTableName();
            modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());
        }
    } 
    
}

internal class MyHistoryRepository : NpgsqlHistoryRepository
{
    public MyHistoryRepository(HistoryRepositoryDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
    {
        base.ConfigureTable(history);

        history.Property(h => h.MigrationId).HasColumnName("id");
    }
}