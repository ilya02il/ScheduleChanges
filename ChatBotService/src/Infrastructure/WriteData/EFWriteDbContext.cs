using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.WriteData
{
    public class EFWriteDbContext : DbContext, IWriteDbContext
    {
        public DbSet<ChatBotEntity> Chats { get; set; }

        public EFWriteDbContext(DbContextOptions options) : base(options) { }

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
