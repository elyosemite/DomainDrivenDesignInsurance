using DomainDrivenDesignInsurance.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignInsurance.Infrastructure.Data.Context;

public class InsuranceDbContext : DbContext
{
    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options) { }

    public DbSet<PolicyModel> Policies { get; set; }
    public DbSet<CoverageModel> Coverages { get; set; }
    public DbSet<EndorsementModel> Endorsements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);
    }
}
