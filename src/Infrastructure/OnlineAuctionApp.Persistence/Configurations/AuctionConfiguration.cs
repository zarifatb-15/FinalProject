using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Persistence.Configurations;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
       public void Configure(EntityTypeBuilder<Auction> builder)
       {
              builder.Property(a => a.StartingPrice)
                     .HasColumnType("decimal(18,2)");

              builder.Property(a => a.CurrentPrice)
                     .HasColumnType("decimal(18,2)");


              builder.HasOne(a => a.Seller)
                     .WithMany()
                     .HasForeignKey(a => a.SellerId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(auction => auction.Winner)
                     .WithMany()
                     .HasForeignKey(auction => auction.WinnerId)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}