namespace BlazorApp.Client.Models
{
    public class Result
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
    public class Result<T> : Result
    {
        public T? Value { get; set; }
    }
}
