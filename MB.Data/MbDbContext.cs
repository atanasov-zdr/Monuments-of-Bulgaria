namespace MB.Data
{
    using System;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Models.Enums;

    public class MbDbContext : IdentityDbContext<MbUser>
    {
        public MbDbContext(DbContextOptions<MbDbContext> options)
            : base(options)
        { }

        public DbSet<Oblast> Oblasts { get; set; }

        public DbSet<Monument> Monuments { get; set; }

        public DbSet<MonumentReview> MonumentReviews { get; set; }

        public DbSet<MonumentComment> MonumentComments { get; set; }

        public DbSet<MonumentCommentLike> MonumentCommentLikes { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<HotelReview> HotelReviews { get; set; }

        public DbSet<HotelComment> HotelComments { get; set; }

        public DbSet<HotelCommentLike> HotelCommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MonumentReview>()
                .Property(e => e.Rating)
                .HasConversion(
                    v => v.ToString(),
                    v => (Rating)Enum.Parse(typeof(Rating), v));
            
            builder.Entity<MonumentReview>()
                .Property(e => e.TravellerType)
                .HasConversion(
                    v => v.ToString(),
                    v => (TravellerType)Enum.Parse(typeof(TravellerType), v));
            
            builder.Entity<MonumentReview>()
                .Property(e => e.TimeOfYear)
                .HasConversion(
                    v => v.ToString(),
                    v => (Season)Enum.Parse(typeof(Season), v));
            
            builder.Entity<HotelReview>()
                .Property(e => e.Rating)
                .HasConversion(
                    v => v.ToString(),
                    v => (Rating)Enum.Parse(typeof(Rating), v));

            base.OnModelCreating(builder);
        }
    }
}
