using Microsoft.EntityFrameworkCore;

namespace Exciting.Database;

public class ExcitingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TeamMember> Members { get; set; }

    public DbSet<TaskItem> Tasks { get; set; }
}
