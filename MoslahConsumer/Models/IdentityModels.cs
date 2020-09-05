using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moslah.Models.OwnModels;

namespace Moslah.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            news = new HashSet<News>();
            description = new HashSet<Description>();

        }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [Range(minimum: 18, maximum: 100)]
        public int? Age { get; set; }
        public string Phone { get; set; }
        [StringLength(20)]
        public string City { get; set; }
        public virtual ICollection<News> news { get; set; }
        public virtual ICollection<Description> description { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<City> cities { set; get; }
        public DbSet<Stations> stations { set; get; }
        public DbSet<BusLocation> busLocations { set; get; }
        public DbSet<MetroLocation> metroLocations{ set; get; }
        public DbSet<MicroBus> microbus { set; get; }
        public DbSet<News> news{ set; get; }
        public DbSet<Description> description { set; get; }
        public DbSet<QuickSearch> quicksearch { set; get; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}