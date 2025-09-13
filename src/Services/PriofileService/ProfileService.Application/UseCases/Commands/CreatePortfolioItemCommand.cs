namespace ProfileService.Application.UseCases.Commands;

public class CreatePortfolioItemCommand
{
    public string ProjectName { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }
}