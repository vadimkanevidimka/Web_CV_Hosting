namespace ProfileService.Application.UseCases.Commands;

public class CreateSalaryExpectationsCommand
{
    public decimal? Amount { get; set; }
    public string Currency { get; set; }
    public string? Type { get; set; }
}