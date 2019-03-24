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
		public Guid ThreadID { get; set; }
		public string Body { get; set; }
		public DateTimeOffset CreatedUTC { get; set; }
		public DateTimeOffset? ModifiedUTC { get; set; }
		//creator info
		public Guid CreatorID { get; set; }
		public string CreatorUsername { get; set; }
		public int CreatorPostCount { get; set; }
		public string CreatorType { get; set; } //based on postcount
		public string CreatorAvaURL { get; set; }

		public override string ToString() => Body;
	}
}
