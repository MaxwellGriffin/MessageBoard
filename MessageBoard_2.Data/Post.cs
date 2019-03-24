using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Data
{
	public class Post //The post is the most basic unit.
	{
		[Key]
		public Guid PostID { get; set; } //Unique identifier of the post.
		[Required]
		public Guid ThreadID { get; set; } //Which thread this post falls under.
		[Required]
		public Guid CreatorID { get; set; } //Who created this post.
		[Required]
		public DateTimeOffset CreatedUTC { get; set; } //When this post was created.
		public DateTimeOffset? ModifiedUTC { get; set; } //When this post was modified. Not required.
		[Required]
		[MinLength(2, ErrorMessage ="Your post must contain more than 1 character.")]
		public string Body { get; set; } //The content of the post. Posts have no title, only a body.
	}
}