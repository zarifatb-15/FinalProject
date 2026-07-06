using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Persistence.Configurations;

public class AuctionImageConfiguration : IEntityTypeConfiguration<AuctionImage>
{
    public void Configure(EntityTypeBuilder<AuctionImage> builder)
    {
        builder.HasKey(image => image.Id);

        builder.Property(image => image.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(image => image.IsPrimary)
            .IsRequired();

        builder.HasOne(image => image.Auction)
            .WithMany(auction => auction.Images)
            .HasForeignKey(image => image.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}