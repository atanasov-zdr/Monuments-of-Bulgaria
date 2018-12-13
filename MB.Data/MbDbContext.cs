using MB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MB.Data
{
    public class MbDbContext : IdentityDbContext<MbUser>
    {
        public MbDbContext(DbContextOptions<MbDbContext> options)
            : base(options)
        { }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Monument> Monuments { get; set; }

        public DbSet<Oblast> Oblasts { get; set; }
    }
}
