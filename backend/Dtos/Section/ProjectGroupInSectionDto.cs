namespace backend.Dtos.Section
{
    public class ProjectGroupInSectionDto
    {
        public int Id { get; set; }
        public bool ConfirmationState { get; set; }
        public int ConfirmedUserNumber { get; set; }
        public string ProjectInformation { get; set; }
    }
}