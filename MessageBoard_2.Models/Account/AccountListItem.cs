using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Account
{
	class AccountListItem
	{
		public Guid UserID { get; set; }
		public string UserName { get; set; }
		public int PostCount { get; set; }
		public string Type { get; set; }
	}
}
