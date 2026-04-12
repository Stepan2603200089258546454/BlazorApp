namespace BlazorApp.Client.Models.Cloud
{
    public class CloudModel
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public bool IsPersonal { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
    }
}
