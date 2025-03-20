using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderingSystem.Core.Entities.Order;


namespace OrderingSystem.Repository.Identity.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(O => O.Status)
             .HasConversion(
                 OStatus => OStatus.ToString(),
                  OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
             );
        }
    }
}
