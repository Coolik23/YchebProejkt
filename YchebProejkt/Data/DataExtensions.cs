using Microsoft.EntityFrameworkCore;

namespace YchebProejkt.Data
{
    public static class DataExtensions
    {
        public static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbcontext.Database.Migrate();
        }
    }
}
