using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineAuctionApp.Domain.Entities;
namespace OnlineAuctionApp.Persistence.Configurations;
public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        
        builder.Property(b => b.Amount)
               .HasColumnType("decimal(18,2)");


        builder.HasOne(b => b.Buyer)
               .WithMany() 
               .HasForeignKey(b => b.BuyerId)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}