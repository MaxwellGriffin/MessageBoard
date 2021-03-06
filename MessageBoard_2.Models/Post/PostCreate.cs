﻿using System;
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
		[MinLength(2, ErrorMessage ="Your post must contain more than 1 character")]
		public string Body { get; set; }
		[Required]
		public Guid ThreadID { get; set; }
		public override string ToString() => Body;
	}
}
