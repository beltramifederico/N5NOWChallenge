using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Application.Mapper;
using N5NOW.UserPermissions.Application.Middleware;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;
using N5NOW.UserPermissions.Infrastructure.DAL.Repositories;
using N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.InventoryProducer.Services;
using Nest;
using Serilog;
using System.Reflection;

namespace N5NOW.UserPermissions.API.DependencyInjections
{
    public static class Installer
    {
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IPermissionRepository, PermissionRepository>();
            service.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
        }

        public static void AddDatabase(this IServiceCollection service, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("UserDatabase");
            service.AddDbContext<UserPermissionDbContext>(dbOption => dbOption.UseSqlServer(connectionString));
        }

        public static void AddMapper(this IServiceCollection service)
        {
            service.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper());
        }

        public static void AddMediatr(this IServiceCollection service)
        {
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.Load("N5NOW.UserPermissions.Application")));
        }

        public static void ConfigSeriLog(this ILoggingBuilder logger, IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            logger.ClearProviders();
            logger.AddSerilog(Log.Logger);
        }

        public static void AddSerilog(this IServiceCollection service)
        {
            service.AddSingleton(Log.Logger);
        }

        public static void AddProducer(this IServiceCollection service)
        {
            service.AddSingleton<IKafkaProducer<OperationMessage>, KafkaProducer<OperationMessage>>();
        }

        public static void AddElasticsearch(this IServiceCollection service, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));
            var defaultIndex = configuration["ElasticsearchSettings:defaultIndex"];
            if (!string.IsNullOrEmpty(defaultIndex))
                settings = settings.DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);

            service.AddSingleton<IElasticClient>(client);
            service.AddScoped<IElasticSearchService<Permission>, ElasticSearchService<Permission>>();
        }
        public static void AddUserMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
