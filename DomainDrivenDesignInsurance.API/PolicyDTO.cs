namespace DomainDrivenDesignInsurance.API;

public class PolicyDTO
{
    public string PolicyHolderName { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
