using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignInsurance.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PolicyController : ControllerBase
{
    public readonly List<string> holders = new()
    {
        "Alice Johnson",
        "Bob Smith",
        "Charlie Brown",
        "Diana Prince",
        "Ethan Hunt",
        "Fiona Gallagher",
        "George Clooney"
    };

    [HttpGet]
    public IEnumerable<PolicyDTO> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new PolicyDTO
        {
            EndDate = DateTime.Now.AddDays(index).ToString("yyyy-MM-dd"),
            PolicyHolderName = holders[Random.Shared.Next(holders.Count)],
            StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
            Value = Random.Shared.Next(2000, 10000)
        })
        .ToArray();
    }
}
