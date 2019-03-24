using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Data
{
	public class Thread //Threads are user creatable and there will be a lot of them. Therefore there are more restrictions and more information saved about them.
	{
		[Key]
		public Guid ThreadID { get; set; } //Unique id of thread. Uses Guid because you could theoretically have thousands of threads.
		[Required]
		public Guid CreatorID { get; set; } //Who created the thread. Threads are user creatable, so we save more data about them.
		[Required]
		public DateTimeOffset CreatedUTC { get; set; } //Date created.
		[Required]
		[MinLength(2, ErrorMessage = "Your thread title must be no less than 2 characters.")]
		[MaxLength(50, ErrorMessage = "Your thread title must be no more than 50 characters.")]
		public string Title { get; set; } //Title of the thread.
	}
}