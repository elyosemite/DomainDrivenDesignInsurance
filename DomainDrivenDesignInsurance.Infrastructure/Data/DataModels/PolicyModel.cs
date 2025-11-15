namespace DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class PolicyModel
{
    public Guid Id { get; set; }
    public Guid InsuredId { get; set; }
    public Guid BrokerId { get; set; }
    public int Status { get; set; }

    // Period
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }

    public ICollection<CoverageModel> Coverages { get; set; } = new List<CoverageModel>();
    public ICollection<EndorsementModel> Endorsements { get; set; } = new List<EndorsementModel>();
}
