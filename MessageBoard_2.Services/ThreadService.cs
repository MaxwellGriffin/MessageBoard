﻿using MessageBoard_2.Data;
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
			//Guid id = Guid.NewGuid();
			var entity =
				new Thread()
				{
					ThreadID = model.ThreadID,
					CreatorID = _userId,
					Title = model.Title,
					CreatedUTC = DateTimeOffset.Now
				};
			var newpost = 
				new Post()
				{
					PostID = Guid.NewGuid(),
					ThreadID = model.ThreadID,
					CreatorID = _userId,
					CreatedUTC = DateTimeOffset.Now,
					Body = model.Body
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.Threads.Add(entity);
				bool result = ctx.SaveChanges() == 1;
				ctx.Posts.Add(newpost);
				result &= ctx.SaveChanges() == 1;
				return result;
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
				bool result = ctx.SaveChanges() == 1;
			
				var posts =
					ctx
						.Posts
						.Where(e => e.ThreadID == threadId);

				ctx.Posts.RemoveRange(posts);
				result &= ctx.SaveChanges() == 1;
				return result;
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
									CreatorUsername = ctx.Users.Where(y => y.Id == e.CreatorID.ToString()).FirstOrDefault().UserName,
									CreatedUTC = e.CreatedUTC,//ok
									PostCount = ctx.Posts.Where(p => p.ThreadID == e.ThreadID).Count(),
									LastPostCreatorUsername = ctx.Users.Where(y => y.Id == ctx.Posts.Where(p => p.ThreadID == e.ThreadID).OrderByDescending(x => x.CreatedUTC).FirstOrDefault().CreatorID.ToString()).FirstOrDefault().UserName, //lol
									LastPostUTC = ctx.Posts.Where(p => p.ThreadID == e.ThreadID).OrderByDescending(x => x.CreatedUTC).FirstOrDefault().CreatedUTC
								}
						);
				return query.OrderByDescending(x => x.LastPostUTC).ToArray();
			}
		}

		//details
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
						CreatorID = entity.CreatorID,
						CreatedUTC = entity.CreatedUTC,
						CreatorUsername = ctx.Users.Where(y => y.Id == entity.CreatorID.ToString()).FirstOrDefault().UserName,
						PostCount = ctx.Posts.Where(p => p.ThreadID == entity.ThreadID).Count()
					};
			}
		}

		//HELPER METHODS
		public string GetThreadTitle(Guid threadId)
		{
			using (var ctx = new ApplicationDbContext())
				return ctx.Threads.Where(e => e.ThreadID == threadId).Single().Title;
			
		}

		public string GetThreadCreatorID(Guid threadId)
		{
			using (var ctx = new ApplicationDbContext())
				return ctx.Threads.Where(e => e.ThreadID == threadId).Single().CreatorID.ToString();
		}
	}
}
