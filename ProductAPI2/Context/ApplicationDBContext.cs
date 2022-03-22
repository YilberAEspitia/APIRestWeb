using Microsoft.EntityFrameworkCore;
using ProductAPI2.Models;

namespace ProductAPI2.Context
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
                
        }
        public DbSet<Product> Product { get; set; }
    }
}
