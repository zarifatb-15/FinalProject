using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineAuctionApp.Domain.Entities;
namespace OnlineAuctionApp.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.HasMany(c => c.Auctions)
               .WithOne(a => a.Category)
               .HasForeignKey(a => a.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}