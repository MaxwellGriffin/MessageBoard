using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Data
{
	class UserMetaData
	{
		public Guid CurrentThreadID { get; set; }
		public Guid CurrentSectionID { get; set; }
	}
}
