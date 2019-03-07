using MessageBoard_2.Data;
using MessageBoard_2.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard_2.Services
{
	public class SectionService
	{
		private readonly Guid _userId;

		public SectionService(Guid userId)
		{
			_userId = userId;
		}

		public bool CreateSection(SectionCreate model)
		{
			var entity =
				new Section()
				{
					CreatorID = _userId,
					Title = model.Title,
				};

			using (var ctx = new ApplicationDbContext())
			{
				ctx.Sections.Add(entity);
				return ctx.SaveChanges() == 1;
			}
		}

		public IEnumerable<SectionListItem> GetSectionsAll()
		{
			using (var ctx = new ApplicationDbContext())
			{
				var query =
					ctx
						.Sections
						.Select(
							e =>
								new SectionListItem
								{
									Title = e.Title,
									SectionID = e.SectionID
								}
						);

				return query.ToArray();
			}
		}

		public SectionDetail GetSectionById(int sectionId)
		{
			using (var ctx = new ApplicationDbContext())
			{
				var entity =
					ctx
						.Sections
						.Single(e => e.SectionID == sectionId);
				return
					new SectionDetail
					{
						SectionId = entity.SectionID,
						Title = entity.Title,
						CreatorID = entity.CreatorID
					};
			}
		}
	}

}