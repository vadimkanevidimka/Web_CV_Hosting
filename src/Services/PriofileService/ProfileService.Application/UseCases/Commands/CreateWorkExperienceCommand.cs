namespace ProfileService.Application.UseCases.Commands;

public class CreateWorkExperienceCommand
{
    public string CompanyName { get; set; }
    public string? City { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
    public string? JobDescription { get; set; }
}