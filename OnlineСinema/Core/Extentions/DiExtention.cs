using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using OnlineСinema.Core.Configurations;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Implements;
using OnlineСinema.Logic.Storages.Interfases;
using PIHelperSh.Configuration;
using PIHelperSh.Configuration.Attributes;
using System.Reflection;
using System.Text;
using LogLevel = NLog.LogLevel;

namespace OnlineСinema.Core.Extentions
{
    public static class DiExtention
    {
        private static WebApplication _app;

        public static void ConfigureApp(this WebApplicationBuilder builder)
        {
            builder.AddLogger();

            builder.Services.AddConfigurations(builder.Configuration);
            builder.Configuration.AddConstants();

            builder.Services.AddServices();
        }

        public static void SettingUpApplication(this WebApplication app)
        {
            _app = app;

            var initTask = app.Services.CreateScope().ServiceProvider.GetRequiredService<IdentityInitializer>().Init();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    await next();
                    return;
                }

                string authHeader = context.Request.Headers["Authorization"];
                bool hasToken = !string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ");

                if (hasToken && context.Items.ContainsKey("TokenExpired"))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Token expired");
                    return;
                }

                await next();
            });

            app.MapControllers();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".mp4"] = "video/mp4";
            provider.Mappings[".webm"] = "video/webm";
            provider.Mappings[".mkv"] = "video/x-matroska";

            // Конфигурация статических файлов
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true, // Разрешить неизвестные MIME-типы
                DefaultContentType = "video/mp4", // Тип по умолчанию
                ContentTypeProvider = provider,
                OnPrepareResponse = ctx =>
                {
                    // Включаем поддержку потоковой передачи
                    ctx.Context.Response.Headers.Append("Accept-Ranges", "bytes");
                }
            });

            // Дожидаемся полного заврешнения всей инициализации
            initTask.Wait();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddDatabase();

            services.AddAuthorizationBuilder();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<CinemaDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ITagStorage, TagStorage>();
            services.AddScoped<ITitleStorage, TitleStorage>();
            services.AddScoped<ISeasonStorage, SeasonStorage>();
            services.AddScoped<IEpisodeStorage, EpisodeStorage>();
            services.AddScoped<IImageStorage, ImageStorage>();
            services.AddScoped<ITitleCashStorage, TitleCashStorage>();
            services.AddScoped<IUserStorage, UserStorage>();

            services.AddScoped<IdentityInitializer>();

            services.AddHttpContextAccessor();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var config = _app.Services.GetRequiredService<IOptions<JwtConfiguration>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.TokenIssuer,
                    ValidAudience = config.TokenAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config.AccessTokenSecretKey))
                };

                // Обработчик неудачной аутентификации
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.HttpContext.Items["TokenExpired"] = true; // Помечаем просроченный токен
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Введите токен",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                             Reference = new OpenApiReference
                             {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                             }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<CinemaDbContext>((serviceProvider, options) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

                options.UseNpgsql(config.ConnectionString);
            });
        }

        public static void AddLogger(this WebApplicationBuilder builder)
        {
            var nLogConfig = new LoggingConfiguration();
            var logConsole = new ConsoleTarget();
            var blackhole = new NullTarget();

            var logFile = new FileTarget()
            {
                FileName = "${basedir}/logs/${shortdate}_logs.log"
            };

            nLogConfig.AddRule(LogLevel.Trace, LogLevel.Trace, blackhole, "Microsoft.AspNetCore.*", true);
            nLogConfig.AddRule(LogLevel.Info, LogLevel.Warn, logFile, "Microsoft.EntityFrameworkCore.*", true);
            nLogConfig.AddRule(LogLevel.Info, LogLevel.Warn, logFile, "Microsoft.AspNetCore.*", true);
            nLogConfig.AddRule(LogLevel.Info, LogLevel.Warn, logFile, "System.Net.Http.HttpClient.Refit.*", true);
            nLogConfig.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
            nLogConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            builder.Logging.ClearProviders();
            builder.Services.AddLogging(m => m.AddNLog(nLogConfig));
        }
    }
}
