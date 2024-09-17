namespace CVRecognizingService.Domain.DTOs.Incoming
{
    public class Root
    {
        public string FullName { get; set; }
        public Contacts Contacts { get; set; }
        public string Location { get; set; }
        public Links Links { get; set; }
        public DesiredPosition DesiredPosition { get; set; }
        public AboutMe AboutMe { get; set; }
        public List<ProfessionalExperience> ProfessionalExperience { get; set; }
        public Education Education { get; set; }
        public List<Course> Courses { get; set; }
        public Skills Skills { get; set; }
        public List<Language> Languages { get; set; }
    }
}
