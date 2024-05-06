using Concessionária.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concessionária.Data
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> DefaultConnection) : DbContext(DefaultConnection)

    {
        public DbSet<Car> Cars {  get; set; }
    }
}
