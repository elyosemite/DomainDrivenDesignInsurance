namespace DomainDrivenDesignInsurance.Infrastructure.Data.Models;

public class EndorsementModel
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }

    public string Type { get; set; }
    public DateTime EffectiveDate { get; set; }

    // Money
    public decimal PremiumDeltaAmount { get; set; }
    public string Currency { get; set; }

    public DateTime CreatedAt { get; set; }
}
