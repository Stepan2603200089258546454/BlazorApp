using static BlazorApp.Client.Models.Cloud.CloudItemModel;

namespace BlazorApp.Client.Models.Cloud
{
    public class EditCloudItemModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ItemType Type { get; set; }
    }
}
