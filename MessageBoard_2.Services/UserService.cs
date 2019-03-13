using MessageBoard_2.Data;
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
			{
				return ctx.Users.Where(y => y.Id == id.ToString()).FirstOrDefault().UserName;
			}
		}
	}
}
