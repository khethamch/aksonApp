using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AksonApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<ServiceTypes> ServiceTypes { get; set; }
        public DbSet<TestDrive> TestDrive { get; set; }
        public DbSet<PhoneCodes> PhoneCodes { get; set; }
        public DbSet<LikeTable> LikeTable { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<MyCart> MyCart { get; set; }
        public DbSet<OrderModel> OrderModel { get; set; }
        public DbSet<CarHireModel> CarHireModel { get; set; }

        public System.Data.Entity.DbSet<AksonApp.Models.ContactModel> ContactModels { get; set; }
    }
}