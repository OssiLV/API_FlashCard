using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Data.Entities;

namespace server.Data.Configs
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure( EntityTypeBuilder<Tag> builder )
        {
            builder.ToTable("Tags");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Tags).HasForeignKey(x => x.UserId);
            builder.Property(x => x.Name).HasMaxLength(12);
            builder.Property(x => x.Description).HasMaxLength(12);
        }
    }
}