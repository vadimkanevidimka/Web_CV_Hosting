namespace ProfileService.Application.UseCases.Commands;

public class CreateEducationCommand
{
    public string InstitutionName { get; set; }
    public string? Specialization { get; set; }
    public string Degree { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
}