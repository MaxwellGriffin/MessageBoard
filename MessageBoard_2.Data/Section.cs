using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Data
{
	public class Section //Only Admins should be messing with sections. Therefore i have not placed very many restrictions on what is needed to create one, as it is left up to the admin.
	{
		[Key]
		public int SectionID { get; set; } //The unique identifier of the section. You should never see more than a few dozen sections at most, so an int will suffice here.
		[Required]
		public string Title { get; set; } //The name of the section.
		public Guid? CreatorID { get; set; } //Who created the section. For administrative purposes. Not required.
	}
}
