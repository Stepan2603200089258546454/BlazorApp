namespace DataContext.Configuration
{
    public class PostgreSettings
    {
        public string DefaultConnection { get; set; } = "Host=localhost;Database=BlazorApp;Username=postgres;Password=Password;Port=5433";
        public string Version { get; set; } = "18.0.0";
    }
}
