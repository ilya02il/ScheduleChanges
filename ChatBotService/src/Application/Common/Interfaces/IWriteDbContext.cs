using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IWriteDbContext
    {
        public DbSet<ChatBotEntity> Chats { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
