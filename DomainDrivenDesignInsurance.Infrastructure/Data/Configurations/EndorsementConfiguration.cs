using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class EndorsementConfiguration : IEntityTypeConfiguration<EndorsementModel>
{
    public void Configure(EntityTypeBuilder<EndorsementModel> b)
    {
        b.ToTable("Endorsements");

        b.HasKey(e => e.Id);

        b.Property(e => e.Type).HasMaxLength(100).IsRequired();
        b.Property(e => e.Currency).HasMaxLength(10).IsRequired();
    }
}
