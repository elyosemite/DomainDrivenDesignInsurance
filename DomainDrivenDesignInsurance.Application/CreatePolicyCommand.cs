namespace DomainDrivenDesignInsurance.Application;

public record CreatePolicyCommand(
    Guid PolicyId,
    Guid InsuredId,
    Guid BrokerId,
    DateTime PeriodFrom,
    DateTime PeriodTo,
    List<CreateCoverageDto> Coverages);

public record UpdatePolicyPeriodCommand(Guid PolicyId, DateTime NewFrom, DateTime NewTo);

public record CancelPolicyCommand(Guid PolicyId, string Reason);

public record CreateEndorsementCommand(Guid PolicyId, string Type, DateTime EffectiveDate, decimal PremiumDelta);

// DTO for input of coverage
public record CreateCoverageDto(string CoverageCode, string Description, decimal SumInsured, decimal Rate);