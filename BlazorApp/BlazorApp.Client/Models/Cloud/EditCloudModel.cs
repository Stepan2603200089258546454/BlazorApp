namespace BlazorApp.Client.Models.Cloud
{
    public class EditCloudModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPersonal { get; set; } = true;
    }
}
