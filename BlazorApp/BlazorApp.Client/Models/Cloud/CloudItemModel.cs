namespace BlazorApp.Client.Models.Cloud
{
    public class CloudItemModel
    {
        public enum ItemType
        {
            File, Folder
        }

        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
    }
}
