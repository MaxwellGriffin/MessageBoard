using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Section
{
	public class SectionListItem
	{
		[Display(Name ="Name")]
		public string Title { get; set; }
	}
}
