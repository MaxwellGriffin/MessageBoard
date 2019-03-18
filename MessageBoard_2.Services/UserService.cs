using MessageBoard_2.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class UserService
	{
		public string GetUserNameByID(Guid id)
		{
			using (var ctx = new ApplicationDbContext()) 
				return ctx.Users.Where(y => y.Id == id.ToString()).FirstOrDefault().UserName;
			
		}

		public int GetPostCount(Guid id)
		{
			using (var ctx = new ApplicationDbContext())
				return ctx.Posts.Where(e => e.CreatorID == id).Count();
		}

		public IEnumerable<IdentityUserRole> GetUserRoles(Guid id)
		{
			using (var ctx = new ApplicationDbContext())
			{
				return ctx.Users.Where(e => e.Id == id.ToString()).Single().Roles;
			}
		}

		//public void SetUserRole(Guid id)
		//{
		//	using (var ctx = new ApplicationDbContext())
		//	{

		//	}
		//}
	}
}
