using Asp.Versioning;
using Dapper;
using Evently.Common.Application.Caching;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastracture.Authentication;
using Evently.Common.Infrastracture.Authorization;
using Evently.Common.Infrastracture.Caching;
using Evently.Common.Infrastracture.Clock;
using Evently.Common.Infrastracture.Data;
using Evently.Common.Infrastracture.Interceptors;
using Evently.Common.Infrastracture.Outbox;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;

namespace Evently.Common.Infrastracture
{
    public static class InfrastractureConfiguration
    {
        public static IServiceCollection AddInfrastracture(this IServiceCollection services, Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
            string connectionString,string redisConnectionString)
        {
            services.AddAuthenticationInternal();

            services.AddAuthorizationInternal();

            services.TryAddSingleton<InsertOutboxMessagesInterceptor>();
            SqlMapper.AddTypeHandler(new GenericArrayHandler<string>());

            NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
            services.TryAddSingleton(npgsqlDataSource);
            services.AddQuartz();

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            try
            {
                IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                services.TryAddSingleton(connectionMultiplexer);

                services.AddStackExchangeRedisCache(options =>
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer));
            }
            catch (Exception e)
            {
                services.AddDistributedMemoryCache();
            }
          

            services.TryAddSingleton<PublishDomainEventsInterceptor>();
            services.TryAddSingleton<ICacheService, CacheService>();
            services.TryAddSingleton<IEventBus,EventBus.EventBus>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddApiVersioning();
            services.AddMassTransit(configure =>
            {
                foreach (Action<IRegistrationConfigurator> configureConsumers in moduleConfigureConsumers)
                {
                    configureConsumers(configure);
                }

                configure.SetKebabCaseEndpointNameFormatter();

                configure.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }
    }
}
