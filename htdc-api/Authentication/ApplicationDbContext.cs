using htdc_api.Models;
using htdc_api.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace htdc_api.Authentication;

public class ApplicationDbContext: IdentityDbContext<IdentityUser>
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    #region DbSets
    //Add Models to DbSet
    public DbSet<UserProfile> UserProfiles { get; set; }
    #endregion
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        DbInitializer.Initialize(builder);
        this.SeedUsers(builder);
        this.SeedRoles(builder);
        this.SeedUserRoles(builder);
    }
    
    #region UserSeeds
    private void SeedUsers(ModelBuilder builder)
    {
        IdentityUser user = new IdentityUser()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            NormalizedEmail = "ADMIN@TEST.COM",
            Email = "admin@test.com",
            LockoutEnabled = false,
            PhoneNumber = "1234567890",
            PasswordHash = "AQAAAAEAACcQAAAAEC+JtF2rYLpMmJtZ5Yt6vVyplT2sEucaRDGi533vIB1Tg1BRwqUyYmjzvN7J1EPqyQ==",
            SecurityStamp = "30e5f131-4d2a-4f34-9225-f8d8390ba5d4",
            ConcurrencyStamp = "505d3481-aefd-4c46-9901-01867e1ba956"
        };

        builder.Entity<IdentityUser>().HasData(user);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
            new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" }
        );
    }

    private void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
        );
    }

    #endregion
}