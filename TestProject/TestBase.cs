using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestProject
{
    public abstract class TestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; } = null!;
        protected IConfiguration Configuration { get; private set; } = null!;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Setup DI container
            ServiceCollection services = new ServiceCollection();

            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            // Add any other services needed for testing
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // Override in derived classes to add additional services
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            ServiceProvider?.Dispose();
        }

        protected T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        protected IOptions<T> GetOptions<T>() where T : class, new()
        {
            return ServiceProvider.GetRequiredService<IOptions<T>>();
        }
    }
}
