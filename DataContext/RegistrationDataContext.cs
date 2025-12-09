using DataContext.Configuration;
using DataContext.Context;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataContext
{
    public static class RegistrationDataContext
    {
        /// <summary>
        /// (расширение) Проверить и применить миграцию
        /// </summary>
        public static async Task AutoMigrateAsync(this IHost host)
        {
            // Миграции EF Core
            using (IServiceScope scope = host.Services.CreateScope())
            {
                await AutoMigrateAsync(scope);
            }
        }
        /// <summary>
        /// Проверить и применить миграцию
        /// </summary>
        public static async Task AutoMigrateAsync(IServiceScope scope)
        {
            DBConfiguration dbOptions = scope.ServiceProvider.GetRequiredService<IOptions<DBConfiguration>>().Value;
            // Актуализируем БД только когда она не в памяти
            if (dbOptions.Type == DBType.InMemory) return;

            IDbContextFactory<DBContext> dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DBContext>>();
            using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                if ((await dbContext.Database.GetPendingMigrationsAsync())?.Any() == true) //проверяем нужны ли миграции
                    dbContext.Database.Migrate(); //Пытаемся актуализировать и принять миграции
            }
        }
        /// <summary>
        /// (расширение) Проверить и применить миграцию
        /// </summary>
        public static void AutoMigrate(this IHost host)
        {
            // Миграции EF Core
            using (IServiceScope scope = host.Services.CreateScope())
            {
                AutoMigrate(scope);
            }
        }
        /// <summary>
        /// Проверить и применить миграцию
        /// </summary>
        public static void AutoMigrate(IServiceScope scope)
        {
            DBConfiguration dbOptions = scope.ServiceProvider.GetRequiredService<IOptions<DBConfiguration>>().Value;
            // Актуализируем БД только когда она не в памяти
            if (dbOptions.Type == DBType.InMemory) return;

            IDbContextFactory<DBContext> dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DBContext>>();
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                if (dbContext.Database.GetPendingMigrations()?.Any() == true) //проверяем нужны ли миграции
                    dbContext.Database.Migrate(); //Пытаемся актуализировать и принять миграции
            }
        }

        /// <summary>
        /// Подключить базу данных
        /// </summary>
        public static void UseDB(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureConfiguration(services, configuration);

            ConfigureDBContext(services);

            services.AddDatabaseDeveloperPageExceptionFilter();
            
            ConfigureIdentity(services);

            ConfigureRepositories(services);
        }
        /// <summary>
        /// (расширение) Подключить базу данных
        /// </summary>
        public static void UseDB(this IHostApplicationBuilder builder)
        {
            UseDB(builder.Services, builder.Configuration);
        }

        /// <summary>
        /// Добавить и сконфигурировать конфигурации
        /// </summary>
        private static void ConfigureConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DBConfiguration>(configuration.GetSection(nameof(DBConfiguration)));
        }
        private static void ConfigureInMemoryDB(DBConfiguration dbOptions, DbContextOptionsBuilder options)
        {
            if (dbOptions.Type != DBType.InMemory) return;

            if (dbOptions.MemorySettings is null)
                throw new ArgumentNullException(nameof(dbOptions.MemorySettings), "Настройки подключения к InMemory не найдены");

            options.UseInMemoryDatabase(dbOptions.MemorySettings.Name ?? "Default");
        }
        private static void ConfigurePostgreSQLDB(DBConfiguration dbOptions, DbContextOptionsBuilder options)
        {
            if (dbOptions.Type != DBType.PostgreSQL) return;

            if (dbOptions.PostgreSettings is null)
                throw new ArgumentNullException(nameof(dbOptions.PostgreSettings), "Настройки подключения к PostgreSQL не найдены");
            if (dbOptions.PostgreSettings.DefaultConnection is null)
                throw new ArgumentNullException(nameof(dbOptions.PostgreSettings.DefaultConnection), "Строка подключения к PostgreSQL не найдена");
            if (dbOptions.PostgreSettings.Version is null)
                throw new ArgumentNullException(nameof(dbOptions.PostgreSettings.Version), "Версия PostgreSQL не найдена");

            options.UseNpgsql(dbOptions.PostgreSettings.DefaultConnection, npgsqlOptions =>
            {
                npgsqlOptions.SetPostgresVersion(Version.Parse(dbOptions.PostgreSettings.Version));
            });
        }
        /// <summary>
        /// Добавить и сконфигурировать контекс БД
        /// </summary>
        private static void ConfigureDBContext(IServiceCollection services)
        {
            services.AddDbContextFactory<DBContext>((provider, options) =>
            {
                DBConfiguration dbOptions = provider.GetRequiredService<IOptions<DBConfiguration>>().Value;

                ConfigureInMemoryDB(dbOptions, options);
                ConfigurePostgreSQLDB(dbOptions, options);
            });
        }
        /// <summary>
        /// Добавить и сконфигурировать репозитории
        /// </summary>
        private static void ConfigureRepositories(IServiceCollection services)
        {
            //services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<DBContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }
}
