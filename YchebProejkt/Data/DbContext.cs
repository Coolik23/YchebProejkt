using Microsoft.EntityFrameworkCore;

namespace YchebProejkt.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Management> Managements => Set<Management>();
        public DbSet<Registry> Registries => Set<Registry>();
        public DbSet<Instruction> Instructions => Set<Instruction>();
    }

}
