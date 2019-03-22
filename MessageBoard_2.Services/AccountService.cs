using MessageBoard_2.Data;
using MessageBoard_2.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class AccountService
	{
		private readonly Guid _userId;

		public AccountService(Guid userId)
		{
			_userId = userId;
		}

		public bool ChangeUserAvatar(ChangeAvatarViewModel model)
		{
			var ctx = new ApplicationDbContext();
			var entity = ctx.Users.Where(e => e.Id == _userId.ToString()).FirstOrDefault();
			entity.AvatarURL = model.AvatarURL;
			return ctx.SaveChanges() == 1;
		}

		public string GetUserAvatar()
		{
			var ctx = new ApplicationDbContext();
			return ctx.Users.Where(e => e.Id == _userId.ToString()).FirstOrDefault().AvatarURL;
		}
	}
}
