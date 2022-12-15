using Microsoft.EntityFrameworkCore;


namespace Songify.Data 
{ 
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

    public DbSet<User> user => Set<User>();
    public DbSet<Playlist> playlist => Set<Playlist>();
    }
}
