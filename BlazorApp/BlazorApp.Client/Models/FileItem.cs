
namespace BlazorApp.Client.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
