using MessageBoard_2.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessageBoard_2.WebMVC.Startup))]
namespace MessageBoard_2.WebMVC
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
			CreateRolesAndUsers();
		}

		// In this method we will create default User roles and Admin user for login   
		private void CreateRolesAndUsers()
		{
			ApplicationDbContext context = new ApplicationDbContext();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


			// In Startup iam creating first Admin Role and creating a default Admin User    
			if (!roleManager.RoleExists("Admin"))
			{

				// first we create Admin role
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Admin";
				roleManager.Create(role);

				//Here we create a Admin super user who will maintain the website                  

				var user = new ApplicationUser();
				user.UserName = "SuperAdmin9001";
				user.Email = "admin2@admin.net";

				string userPWD = "password";

				var chkUser = UserManager.Create(user, userPWD);

				//Add default User to Role Admin   
				if (chkUser.Succeeded)
				{
					var result1 = UserManager.AddToRole(user.Id, "Admin");

				}
			}

			// creating Creating member role    
			if (!roleManager.RoleExists("Member"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Member";
				roleManager.Create(role);

			}

			if (!roleManager.RoleExists("Banned"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Banned";
				roleManager.Create(role);

			}
		}
	}
}