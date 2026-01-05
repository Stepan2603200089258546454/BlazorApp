using System.Text.Json.Serialization;

namespace BlazorApp.Client.Models.Request
{
    public class FileGetRequest
    {
        [JsonIgnore]
        public static IEnumerable<int> Sizes = new int[] { 1, 2, 3, 5, 10, 20, 50, 100, 200 };
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        public void Next()
        {
            Page++;
        }
        public void Back()
        {
            Page--;
        }
        public void SetSize(int value)
        {
            Size = value;
        }
    }
}
