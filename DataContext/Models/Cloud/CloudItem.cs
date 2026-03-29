namespace DataContext.Models.Cloud
{
    public class CloudItem
    {
        public enum ItemType
        {
            File, Folder
        }

        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Guid? ParrentId { get; set; }
        public CloudItem? Parrent { get; set; }
        public Guid PersonalCloudId { get; set; }
        public PersonalCloud PersonalCloud { get; set; }
        public Guid? FileDataId { get; set; }
        public CloudFileData? FileData { get; set; }
        public List<CloudItem> Children { get; set; } = [];
    }
}
