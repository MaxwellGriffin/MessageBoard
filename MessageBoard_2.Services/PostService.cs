using MessageBoard_2.Data;
using MessageBoard_2.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class PostService
	{
		private readonly Guid _userId;

		public PostService(Guid userId)
		{
			_userId = userId;
		}

		public bool CreatePost(PostCreate model)
		{
			var entity =
				new Post()
				{
					PostID = Guid.NewGuid(),
					ThreadID = model.ThreadID,
					CreatorID = _userId,
					Body = model.Body,
					CreatedUTC = DateTimeOffset.Now
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.Posts.Add(entity);
				return ctx.SaveChanges() == 1;
			}
		}

		public bool UpdatePost(PostEdit model)
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Posts
						.Single(e => e.PostID == model.PostID);

				entity.Body = model.Body;
				entity.ModifiedUTC = DateTimeOffset.Now;
				return ctx.SaveChanges() == 1;
			}
		}

		public bool DeletePost(Guid postId) //delete post by id
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Posts
						.Single(e => e.PostID == postId);

				ctx.Posts.Remove(entity);

				return ctx.SaveChanges() == 1;
			}
		}

		public PostListItem GetPostById(Guid postId) //Get an individual post
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Posts
						.Single(e => e.PostID == postId);
				return
					new PostListItem
					{
						PostID = entity.PostID,
						ThreadID = entity.ThreadID,
						Body = entity.Body,
						CreatorID = entity.CreatorID,
						CreatedUTC = entity.CreatedUTC,
						ModifiedUTC = entity.ModifiedUTC
					};
			}
		}

		public IEnumerable<PostListItem> GetPostsByThread(Guid threadId)
		{
			using (var ctx = new ApplicationDbContext())
			{
				var query =
					ctx
						.Posts
						.Where(e => e.ThreadID == threadId)
						.Select(
							e =>
								new PostListItem
								{
									PostID = e.PostID,
									ThreadID = e.ThreadID,
									Body = e.Body,
									CreatorID = e.CreatorID,
									CreatedUTC = e.CreatedUTC,
									ModifiedUTC = e.ModifiedUTC,
									CreatorUsername = ctx.Users.Where(x => x.Id == e.CreatorID.ToString()).FirstOrDefault().UserName
								}
						);

				return query.OrderBy(x => x.CreatedUTC).ToArray();
			}
		}
	}
}
