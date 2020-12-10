using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Persistence.Configurations
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
    {
        public void Configure(EntityTypeBuilder<AttributeValue> builder)
        {
            builder.HasKey(av => new {av.TextEntryId, av.TextAttributeId});
            builder.HasOne(av => av.TextEntry)
                .WithMany()
                .HasForeignKey(av => av.TextEntryId);
            builder.HasOne(av => av.TextAttribute)
                .WithMany()
                .HasForeignKey(av => av.TextAttributeId);
        }
    }
}