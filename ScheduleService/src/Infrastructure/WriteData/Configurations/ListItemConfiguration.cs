using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.WriteData.Configurations
{
    public class ListItemConfiguration : IEntityTypeConfiguration<ListItemEntity>
    {
        public void Configure(EntityTypeBuilder<ListItemEntity> builder)
        {
            builder.OwnsOne(li => li.ItemInfo);
        }
    }
}
