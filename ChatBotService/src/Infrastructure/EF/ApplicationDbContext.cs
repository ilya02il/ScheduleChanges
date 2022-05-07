using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<ChatBotEntity> Chats { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChatBotEntity>();

            builder.Entity<ChatBotEntity>().OwnsOne(cb => cb.UserInfo, nb =>
            {
                nb.Property(ui => ui.Username).IsRequired();
                nb.OwnsOne(ui => ui.EducationalInfo);
            });
        }
    }
}
