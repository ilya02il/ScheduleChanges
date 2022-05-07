using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.EF.Configurations;

namespace Infrastructure.EF
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<ListItemEntity> ListItems { get; set; }
        public DbSet<ScheduleListEntity> ScheduleLists { get; set; }
        public DbSet<ScheduleListItemEntity> ScheduleListItems { get; set; }
        public DbSet<ChangesListEntity> ChangesLists { get; set; }
        public DbSet<ChangesListItemEntity> ChangesListItems { get; set; }
        public DbSet<EducationalOrgEntity> EducationalOrgs { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<LessonCallEntity> LessonCalls { get; set; }
        public DbSet<DatedLessonCallEntity> DatedLessonCalls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListItemConfiguration());

            modelBuilder.Entity<DatedLessonCallEntity>().ToTable("DatedLessonCalls");

            //modelBuilder.Entity<ChangesListEntity>()
                //.HasOne(e => e.EducationalOrg)
                //.WithMany();
                //.OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<LessonCallEntity>()
            //    .HasOne(lc => lc.Date)
            //    .WithOne(dlc => dlc.LessonCall)
            //    .HasForeignKey<DatedLessonCallEntity>(dlc => dlc.LessonCallId);
        }
    }
}
