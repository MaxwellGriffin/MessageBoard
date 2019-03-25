﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Account
{
	public class ChangeAvatarViewModel
	{
		[DataType(DataType.ImageUrl)]
		public string AvatarURL { get; set; }
	}
}
