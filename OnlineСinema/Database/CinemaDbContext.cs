using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Models;

namespace OnlineСinema.Database
{
    public partial class CinemaDbContext: IdentityDbContext
    {
        public virtual DbSet<Episode> Episodes { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Seasone> Seasones { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<Title> Titles { get; set; }

        public virtual DbSet<UserSeen> UserSeens { get; set; }

        public CinemaDbContext()
        {
        }

        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Configurations.EpisodeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ImageConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SeasoneConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TagConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.TitleConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserSeenConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
