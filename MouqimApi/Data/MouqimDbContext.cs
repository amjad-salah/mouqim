using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace MouqimApi.Data;

public class MouqimDbContext(DbContextOptions<MouqimDbContext> options) : DbContext(options)
{
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.UpdatedAt = DateTime.Now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e =>
                e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<Occupation> Occupations { get; set; }
    public DbSet<EducationLevel> EducationLevels { get; set; }
    public DbSet<User> Users { get; set; }
}