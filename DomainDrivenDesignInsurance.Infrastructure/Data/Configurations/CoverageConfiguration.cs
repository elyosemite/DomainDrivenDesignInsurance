using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class CoverageConfiguration : IEntityTypeConfiguration<CoverageModel>
{
    public void Configure(EntityTypeBuilder<CoverageModel> b)
    {
        b.ToTable("Coverages");

        b.HasKey(c => c.Id);

        b.Property(c => c.CoverageCode).HasMaxLength(50).IsRequired();
        b.Property(c => c.Description).HasMaxLength(255);

        b.Property(c => c.SumInsuredAmount).IsRequired();
        b.Property(c => c.Currency).HasMaxLength(10).IsRequired();
    }
}
