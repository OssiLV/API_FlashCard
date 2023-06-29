using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Data.Configs;
using server.Data.Entities;
using System.Reflection.Emit;

namespace server.Data.Context
{
    public class CardDb : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CardDb(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            builder.ApplyConfiguration(new CardConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());

            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            var hashkey = new PasswordHasher<AppUser>();

            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = new Guid("99303566-de6e-4b51-81de-e94541077739"),
                UserName = "Admin",
                Email = "ad@gmail.com",
                PasswordHash = hashkey.HashPassword(null, "admin")
            });
        }


        public DbSet<Card> Cards { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }

}
