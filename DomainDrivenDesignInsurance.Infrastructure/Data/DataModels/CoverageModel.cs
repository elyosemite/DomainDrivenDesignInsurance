namespace DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class CoverageModel
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }

    public string CoverageCode { get; set; }
    public string Description { get; set; }

    // Money
    public decimal SumInsuredAmount { get; set; }
    public string Currency { get; set; }

    public decimal Rate { get; set; }
}
