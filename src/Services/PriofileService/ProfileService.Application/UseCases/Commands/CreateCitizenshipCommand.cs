namespace ProfileService.Application.UseCases.Commands;

public class CreateCitizenshipCommand
{
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string UndergruondStation { get; set; }
}