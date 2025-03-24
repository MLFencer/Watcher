using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Watcher.Data.SQL.Repositories;

namespace Watcher.Data.SQL
{
    public static class Startup
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            // services.AddDbContext<SqlDbContext>(options => options.UseSqlite(configuration.GetConnectionString("SQL")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddTransient<ISqlUnitOfWork, SqlUnitOfWork>();
        }
    }
}
