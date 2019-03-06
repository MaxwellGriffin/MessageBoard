using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MessageBoard_2.Data
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
		public string AvatarURL { get; set; } //URL to the custom avatar of this user. Not sure if this will be used

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

		public DbSet<Post> Posts { get; set; }
		public DbSet<Thread> Threads { get; set; }
		public DbSet<Section> Sections { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder
				.Conventions
				.Remove<PluralizingTableNameConvention>();

			modelBuilder
				.Configurations
				.Add(new IdentityUserLoginConfiguration())
				.Add(new IdentityUserRoleConfiguration());
		}
	}

	public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
	{
		public IdentityUserLoginConfiguration()
		{
			HasKey(iul => iul.UserId);
		}
	}

	public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
	{
		public IdentityUserRoleConfiguration()
		{
			HasKey(iur => iur.RoleId);
		}
	}
}