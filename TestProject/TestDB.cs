using DataContext;
using DataContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject
{
    [TestClass]
    public sealed class TestDB : TestBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            RegistrationDataContext.UseDB(services, Configuration);

            base.ConfigureServices(services);
        }

        [TestMethod]
        public async Task TestMethod()
        {
            IDbContextFactory<DBContext> contextFactory = GetService<IDbContextFactory<DBContext>>();
            using DBContext context = await contextFactory.CreateDbContextAsync();
        }
    }
}
