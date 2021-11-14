using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<HorseInfo> HorseInfo { get; set; }
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HorseInfo>().HasKey(e => new { e.Birthday, e.HorseName });
        }

    }
}
