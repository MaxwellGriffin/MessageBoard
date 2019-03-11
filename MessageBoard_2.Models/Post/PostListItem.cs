using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Post
{
	public class PostListItem //Used for index/delete
	{
		public Guid PostID { get; set; } //should be hidden
		public string Body { get; set; }
		public Guid CreatorID { get; set; }
		public DateTimeOffset CreatedUTC { get; set; }
		public DateTimeOffset? ModifiedUTC { get; set; }

		public override string ToString() => Body;
	}
}
