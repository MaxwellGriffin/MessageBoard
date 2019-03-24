using MessageBoard_2.Data;
using MessageBoard_2.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class AdminService
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

		public int GetThreadCount(Guid id)
		{
			using (var ctx = new ApplicationDbContext())
				return ctx.Threads.Where(e => e.CreatorID == id).Count();
		}

		public IEnumerable<IdentityUserRole> GetUserRoles(Guid id)
		{
			using (var ctx = new ApplicationDbContext())
			{
				return ctx.Users.Where(e => e.Id == id.ToString()).Single().Roles;
			}
		}

		public IEnumerable<AccountListItem> GetAccounts()
		{
			using (var ctx = new ApplicationDbContext())
			{
				var query =
						ctx
							.Users
							.Select(
								e =>
									new AccountListItem
									{
										UserID = Guid.Parse(e.Id),
										UserName = e.UserName,
										UserRoles = e.Roles.ToString(),
										PostCount = ctx.Posts.Where(p => p.CreatorID == Guid.Parse(e.Id)).Count(),
										ThreadCount = ctx.Threads.Where(p => p.CreatorID == Guid.Parse(e.Id)).Count()
									}
							);
				return query;
			}
		}

		public void SetNewRoles()
		{

		}

		//public void SetUserRole(Guid id)
		//{
		//	using (var ctx = new ApplicationDbContext())
		//	{

		//	}
		//}
	}
}
