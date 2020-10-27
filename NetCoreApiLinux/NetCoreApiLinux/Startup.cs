using DataLayer;
using System;
using System.IO;
using System.Reflection;
using DataLayer.Dbo;
using DataLayer.Kafka;
using DataLayer.Models.AppInfo;
using DataLayer.Providers;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mongo.Migration.Documents;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using NetCoreApiLinux.Models.AppInfo;
using NetCoreApiLinux.Models.AppInfo.Requests;
using NotifierDaemon;

namespace NetCoreApiLinux
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Statistics API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddSingleton<IMongoClientProvider, MongoClientProvider>();
            services.AddSingleton(x => x.GetRequiredService<IMongoClientProvider>().Client);

            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<ICriticalEventsProducer, CriticalEventsProducer>();
            services.AddSingleton<ICriticalEventMessageBuilder,CriticalEventMessageBuilder>();
            services.AddSingleton<IKafkaSettings, KafkaSettings>();

            services.AddSingleton<IStatisticsEventProvider, StatisticsEventProvider>();
            services.AddSingleton<IStatisticsEventTypeProvider, StatisticsEventTypeProvider>();
            services.AddSingleton<IStatisticsEventTypeSaver, StatisticsEventTypeSaver>();

            var mongoSettings = new MongoDbSettings();
            Configuration.GetSection("MongoDbSettings").Bind(mongoSettings);
            services.AddSingleton<IMongoDbSettings>(mongoSettings);

            var consumerSettings = new ConsumerSettings();
            Configuration.GetSection("ConsumerSettings").Bind(consumerSettings);
            services.AddSingleton<IConsumerSettings>(consumerSettings);

            services.AddSingleton<ICriticalEventsConsumer, CriticalEventsConsumer>();
            services.AddSingleton<IMailSender, MailSender>();
            services.AddHostedService<NotifierDaemonService>();

            TypeAdapterConfig<StatisticsEventDbo, StatisticsEvent>.NewConfig()
                .Unflattening(true);

            TypeAdapterConfig<StatisticsEventRequestDto, StatisticsEvent>.NewConfig()
                .Unflattening(true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Statistics API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
