using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Post
{
	public class PostCreate
	{
		[Required]
		public string Body { get; set; }

		public override string ToString() => Body;
	}
}
