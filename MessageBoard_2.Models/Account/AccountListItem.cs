using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Account
{
	public class AccountListItem
	{
		public Guid UserID { get; set; }
		public string UserName { get; set; }
		public int PostCount { get; set; }
		public int ThreadCount { get; set; }
		public string UserRoles { get; set; }

	}
}
