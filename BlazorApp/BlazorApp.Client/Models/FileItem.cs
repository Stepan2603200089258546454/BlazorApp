
namespace BlazorApp.Client.Models
{
    public class FileItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
