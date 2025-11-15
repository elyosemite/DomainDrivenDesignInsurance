using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class PolicyConfiguration : IEntityTypeConfiguration<PolicyModel>
{
    public void Configure(EntityTypeBuilder<PolicyModel> b)
    {
        b.ToTable("Policies");

        b.HasKey(p => p.Id);

        b.Property(p => p.Status).IsRequired();

        b.Property(p => p.PeriodFrom).IsRequired();
        b.Property(p => p.PeriodTo).IsRequired();

        b.HasMany(p => p.Coverages)
         .WithOne()
         .HasForeignKey(c => c.PolicyId);

        b.HasMany(p => p.Endorsements)
         .WithOne()
         .HasForeignKey(e => e.PolicyId);
    }
}
