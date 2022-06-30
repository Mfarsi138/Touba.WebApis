using Touba.WebApis.Helpers.MessageBroker.Models;
using Touba.WebApis.Helpers.MessageBroker.Models.Notification;
using Touba.WebApis.Helpers.SecurityHelper;
using Touba.WebApis.API.Data;
using Touba.WebApis.API.Helpers.Authorization;
using Touba.WebApis.API.MessageBroker;
using Touba.WebApis.API.Models;
using Touba.WebApis.API.Models.Account;
using Touba.WebApis.API.Services;
using Touba.WebApis.API.Services.Implementation;
using Touba.WebApis.IdentityServer.DataLayer;
using Touba.WebApis.IdentityServer.DataLayer.Models;
using IdentityServer4.Models;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using Touba.WebApis.API.Services.Contracts;

namespace Touba.WebApis.API
{
    public class Startup
    {
        private readonly string AllowedOriginsName = "_allowedOrigins";

        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        private AppSettings appSettings;
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            appSettings = Configuration.Get<AppSettings>();

            ConfigureSwagger(services);

            ConfigureVersioning(services);
            ConfigureLogger(services);
            ConfigureMassTransit(services);
            ConfigureDataBase(services);
            ConfigureIdentityServer(services);
            ConfigureAuthorization(services);
            ConfigureHttpClients(services);

            services.AddAutoMapper(typeof(Startup));
            AddServices(services);

            if (appSettings.EnableCORS)
                services.AddCors(options =>
                    options.AddPolicy(AllowedOriginsName,
                        builder =>
                        {
                            builder
                            .WithOrigins(appSettings.CORSOrigins)
                            //.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                        })
                    );

            services.AddControllers();
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddEncryptionDecryptionHelper(conf =>
            {
                conf.Key = appSettings.Security.EncDecHelperKey;
            });
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IdentityErrorDescriber, CustomIdentityErrorDescriber>();
            services.AddTransient<IMessageService, Touba.WebApis.API.Services.Implementation.MessageService>();

            //Transient is enough, but it also could be scoped (depends on background scheduler's implementation).
            // because it's using IOptionsSnapshot, singleton is out of options.
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

            services.AddIdentity<User, IdentityRole>()
             .AddEntityFrameworkStores<DataContext>()
             .AddDefaultTokenProviders();

            var encDecHelper = new EncryptionDecryptionHelper(appSettings.Security.EncDecHelperKey);
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(f =>
            {
                f.RequireHttpsMetadata = false;
                f.SaveToken = true;
                f.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encDecHelper.DecryptString(appSettings.Security.TokenSecret))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };
            });

            var builder = services.AddIdentityServer()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext =
                         builder => builder.UseSqlServer(appSettings.ConnectionStrings.MSSQL, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30; // interval in seconds
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext =
                         builder => builder.UseSqlServer(appSettings.ConnectionStrings.MSSQL, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                })
                .AddProfileService<CustomProfleService>()
                .AddAspNetIdentity<User>()
                .AddInMemoryClients(Clients.GetClients(appSettings))
                .AddInMemoryApiResources(Data.IdentityResources.GetApiResources())
                .AddInMemoryApiScopes(Data.IdentityResources.GetApiScopes())
                .AddInMemoryIdentityResources(Data.IdentityResources.GetIdentityResources());

            if (Environment.IsDevelopment())
                builder.AddDeveloperSigningCredential();
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(x => 
            {
                x.AddPolicy("NotCustomer", policy => policy.AddRequirements(new ExcludeRolesRequirement(new[] { Roles.Customer })));
            });
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient("TokenClient", client => client.BaseAddress = new Uri($"{appSettings.IdentityServerAddress}/connect/token"));
        }

        private void ConfigureDataBase(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.MSSQL, sqlserverOptions =>
                {
                    sqlserverOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name);
                    sqlserverOptions.EnableRetryOnFailure(3);
                    sqlserverOptions.CommandTimeout(180); // 3 minutes
                });

                options.EnableDetailedErrors(true);
                options.EnableSensitiveDataLogging(false);
            });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Touba.WebApis.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Description = "JWt Auth Using Bearer Scheme",
                    In =  ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement( new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>() 
                    }
                });
            });
        }

        private static void ConfigureVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        private void ConfigureLogger(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(f =>
            {
                var logConf = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationId()
                   // .WriteTo.Http(appSettings.Elk.Logstash)
                    .WriteTo.File(Configuration["Serilog:LogFileName"],
                                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                                  flushToDiskInterval: TimeSpan.FromMinutes(1),
                                  rollOnFileSizeLimit: true,
                                  encoding: System.Text.Encoding.UTF8,
                                  outputTemplate: "{NewLine}{Timestamp:yyyy/MM/dd HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                    .WriteTo.MSSqlServer(connectionString: appSettings.ConnectionStrings.MSSQL,
                        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                        {
                            AutoCreateSqlTable = true,
                            TableName = "Logs.Infrastructure.IdentityServer.API"
                        })
                        .Enrich.WithProperty("ModuleName", "IdentityServer"); 

                return logConf.CreateLogger();
            });
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<GetUserByRoleConsumer>();
                c.AddConsumer<GetUserInfoByIdConsumer>();
                c.AddConsumer<GetRoleByUserIDConsumer>();
                c.AddConsumer<PersonalInformationChangedConsumer>();
                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(appSettings.QueueSetting.HostName, appSettings.QueueSetting.VirtualHost, h => {
                        h.Username(appSettings.QueueSetting.UserName);
                        h.Password(appSettings.QueueSetting.Password);
                    });

                    cfg.ExchangeType = ExchangeType.Direct;
                });
                c.AddRequestClient<MB_EmailSend>();
                c.AddRequestClient<MB_MessageSend>();
                c.AddRequestClient<MB_UserProfileChange>();
            });

            services.AddMassTransitHostedService();
       
        }

        private static void UpgradeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var logger = serviceScope.ServiceProvider.GetService<ILogger>();

            try
            {
                SeedData.Seed(app).Wait();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred when seeding information.");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(appSettings.EnableCORS)
                app.UseCors(AllowedOriginsName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Touba.WebApis.API v1"));

            //app.UseHttpsRedirection();
            UpgradeDatabase(app);
            app.UseRouting();


            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
