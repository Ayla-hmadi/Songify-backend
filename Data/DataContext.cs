using Microsoft.EntityFrameworkCore;


namespace Songify.Data 
{ 
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> user => Set<User>();
    }
}
