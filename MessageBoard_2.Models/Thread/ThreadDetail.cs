using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Thread
{
	public class ThreadDetail
	{
		public Guid ThreadID { get; set; }
		public string Title { get; set; }
		public Guid? CreatorID { get; set; }
		public string CreatorUsername { get; set; }
		public DateTimeOffset CreatedUTC { get; set; }
		public int PostCount { get; set; }
	}
}