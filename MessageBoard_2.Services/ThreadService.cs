using MessageBoard_2.Data;
using MessageBoard_2.Models.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class ThreadService
	{
		private readonly Guid _userId;

		public ThreadService(Guid userId)
		{
			_userId = userId;
		}

		public bool CreateThread(ThreadCreate model)
		{
			var entity =
				new Thread()
				{
					CreatorID = _userId,
					Title = model.Title,
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.Threads.Add(entity);
				return ctx.SaveChanges() == 1;
			}
		}

		public bool UpdateThread(ThreadEdit model)
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Threads
						.Single(e => e.ThreadID == model.ThreadID);

				entity.Title = model.Title;
				return ctx.SaveChanges() == 1;
			}
		}

		public bool DeleteThread(Guid threadId) //delete thread by id
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Threads
						.Single(e => e.ThreadID == threadId);

				ctx.Threads.Remove(entity);

				return ctx.SaveChanges() == 1;
			}
		}

		public IEnumerable<ThreadListItem> GetThreadsAll()
		{
			using (var ctx = new ApplicationDbContext())
			{
				var query =
					ctx
						.Threads
						.Select(
							e =>
								new ThreadListItem
								{
									ThreadID = e.ThreadID,
									Title = e.Title,
									CreatorID = e.CreatorID,
									CreatedUTC = e.CreatedUTC,
									//PostCount = GetPostCount(e.ThreadID),



									//this is where we will call the helper methods later



								}
						);

				return query.ToArray();
			}
		}

		public ThreadDetail GetThreadById(Guid threadId)
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Threads
						.Single(e => e.ThreadID == threadId);
				return
					new ThreadDetail
					{
						ThreadID = entity.ThreadID,
						Title = entity.Title,
						CreatorID = entity.CreatorID
					};
			}
		}

		//public int GetPostCount(Guid threadId)
		//{
		//	using (var ctx = new ApplicationDbContext())
		//	{
		//		var entity = ctx.Threads.Single(e => e.ThreadID == threadId);
				

		//	}
		//}
	}
}
