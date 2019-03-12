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
			Guid id = Guid.NewGuid();
			var entity =
				new Thread()
				{
					ThreadID = id,
					CreatorID = _userId,
					Title = model.Title,
					CreatedUTC = DateTimeOffset.Now
				};
			var newpost = 
				new Post()
				{
					PostID = new Guid(),
					ThreadID = id,
					CreatorID = _userId,
					CreatedUTC = DateTimeOffset.Now,
					Body = model.Body
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.Threads.Add(entity);
				ctx.Posts.Add(newpost);
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
									CreatedUTC = e.CreatedUTC
								}
						);
				foreach (ThreadListItem item in query)
				{
					item.PostCount = GetPostCount(item.ThreadID);
					item.LastPostCreatorID = GetLastPost(item.ThreadID).CreatorID;
					item.LastPostUTC = GetLastPost(item.ThreadID).CreatedUTC;
				}


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

		//HELPER METHODS

		public int GetPostCount(Guid threadId) //returns how many posts are in this thread (how many posts have this threadID).
		{
			using (var ctx = new ApplicationDbContext())
			{
				var count = 0;
				count = ctx.Posts.Where(e => e.ThreadID == threadId).Count();
				return count;	
			}
		}

		public Post GetLastPost(Guid threadId) //returns the last post in this thread.
		{
			using (var ctx = new ApplicationDbContext())
			{
				if(ctx.Posts.Where(e => e.ThreadID == threadId).Count() == 0)
				{
					return new Post
					{
						CreatorID = _userId,
						CreatedUTC = DateTimeOffset.Now,
						PostID = new Guid(),
						ThreadID = new Guid(),
						Body = "ERROR NO POSTS"
					};
				}
				return ctx.Posts.Where(e => e.ThreadID == threadId).OrderByDescending(x => x.CreatedUTC).FirstOrDefault();
			}
		}
	}
}
