namespace BlazorApp.Models.Request
{
    public class CloudFileDownloadRequest
    {
        public string SystemName { get; set; }
        public Guid? CloudId { get; set; }
        public Guid? ParrentId { get; set; }
    }
}
