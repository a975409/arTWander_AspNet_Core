using arTWander.Models;
using Microsoft.EntityFrameworkCore;

namespace arTWander.Data
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
    }
}
