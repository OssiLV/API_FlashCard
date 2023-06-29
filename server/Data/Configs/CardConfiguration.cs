using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Data.Entities;

namespace server.Data.Configs
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure( EntityTypeBuilder<Card> builder )
        {
            builder.ToTable("Cards");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Tag).WithMany(x => x.Cards).HasForeignKey(x => x.TagId);
            builder.Property(x => x.Title).HasMaxLength(12);
            builder.Property(x => x.Translate).HasMaxLength(12);
            builder.Property(x => x.Status).HasMaxLength(10);
        }
    }
}