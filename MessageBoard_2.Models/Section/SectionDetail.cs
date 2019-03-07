using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Models.Section
{
	public class SectionDetail
	{
		public int SectionId { get; set; }
		public string Title { get; set; }
		public Guid? CreatorID { get; set; }
	}
}