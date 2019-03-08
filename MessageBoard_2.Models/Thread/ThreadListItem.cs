using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Thread
{
	public class ThreadListItem
	{
		public Guid ThreadID { get; set; }
		public string Title { get; set; }
		[Display(Name = "By")]
		public Guid CreatorID { get; set; }
		[Display(Name = "Created")]
		public DateTimeOffset CreatedUTC { get; set; }
		[Display(Name = "Posts")]
		public int PostCount { get; set; } //How many posts are in the thread.
		[Display(Name = "Last Post")]
		public Guid LastPostCreatorID { get; set; } 
		[Display(Name = "")]
		public DateTimeOffset LastPostUTC { get; set; }

		public override string ToString() { return base.ToString(); }
	}
}
