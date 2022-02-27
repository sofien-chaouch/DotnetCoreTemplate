using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.AsyncDataServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.EventProcessing;
using PlatformService.GraphQL;
using PlatformService.GraphQL.Command;
using PlatformService.GraphQL.Platforms;
using PlatformService.Hubs;
using PlatformService.Repositories;
using PlatformService.Repositories.Car;
using PlatformService.Repositories.Platform;
using PlatformService.Repositories.RepositoriesPatterns;
using PlatformService.Services.Car;
using PlatformService.Services.Platform;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;
using WebSocketServer.Middleware;

namespace PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                services.AddPooledDbContextFactory<AppDbContext>(opt =>
                     opt.UseInMemoryDatabase("InMem"));
            }
            
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            
            services.AddSignalR();
            services.AddWebSocketServerConnectionManager();
            
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<PlatformType>()
                .AddType<AddPlatformInputType>()
                .AddType<AddPlatformPayloadType>()
                .AddType<CommandType>()
                .AddType<AddCommandInputType>()
                .AddType<AddCommandPayloadType>()
                .AddFiltering()
                .AddSorting()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = _env.IsDevelopment())
                .AddInMemorySubscriptions();
            
            
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());

            // Register the RedisCache service
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Redis")["ConnectionString"];
            });
            
            services.AddSingleton<AppDbContext>(p => p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            services.AddScoped(typeof(IRepository<>), implementationType: typeof(Repository<>));
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<IPlatformService, Services.Platform.PlatformService>();

            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<ICarService, CarService>();

            services.AddScoped<ICommandRepo, CommandRepo>();
            
            //services.AddHostedService<MessageBusSubscriber>();

            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");
            
            //services.AddSingleton<IHostedService, KafkaConsumerService>();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
                endpoints.MapGrpcService<GrpcPlatformService>();
                endpoints.MapHub<GenericHub>("/genericSocket");
                endpoints.MapGet("/protos/platforms.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
                });
            });
            

            app.UseGraphQLVoyager();

            app.UseWebSockets();
            app.UseWebSocketServer();
            
            //PrepDb.PrepPopulation(app, env.IsProduction());

        }
    }
}
