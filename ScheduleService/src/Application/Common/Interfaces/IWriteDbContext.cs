using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ListItemEntity> ListItems { get; }
        DbSet<ScheduleListEntity> ScheduleLists { get; }
        DbSet<ScheduleListItemEntity> ScheduleListItems { get; }
        DbSet<ChangesListEntity> ChangesLists { get; }
        DbSet<ChangesListItemEntity> ChangesListItems { get; }
        DbSet<EducationalOrgEntity> EducationalOrgs { get; }
        DbSet<GroupEntity> Groups { get; }
        DbSet<LessonCallEntity> LessonCalls { get; }
        DbSet<DatedLessonCallEntity> DatedLessonCalls { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
