using startup_trial.Cofiguration;
using startup_trial.Data;
using startup_trial.Models;
using startup_trial.Services.BackgroundJobs;
using startup_trial.Services.BasketService;
using startup_trial.Services.DishService;
using startup_trial.Services.Initialization;
using startup_trial.Services.OrderService;
using startup_trial.Services.TokenService;
using startup_trial.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Quartz;
using startup_trial.Models;
using startup_trial.Services.RestaurantService;
using System.Security.Claims;
using System.Text;

namespace startup_trial
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ITokenStorageService, TokenDbStorageService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();

            //add automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
            builder.Host.UseNLog();


            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<Restaurant, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            /*builder.Services.AddIdentity<Driver, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();*/

            /*builder.Services.AddIdentity<Restaurant, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
             .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<Driver, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();*/

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(ApplicationRoleNames.User,
                    new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

                options.AddPolicy(ApplicationRoleNames.Restaurant,
                    new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

                /*options.AddPolicy(ApplicationRoleNames.Administrator, new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(ApplicationRoleNames.Administrator)
                    .RequireClaim(ClaimTypes.Role, ApplicationRoleNames.Administrator)
                    .Build());*/
            });


            //Adding Quartz
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJob<BackgroundJob>(o => o.WithIdentity(nameof(BackgroundJob)));

                q.AddTrigger(o =>
                    o.ForJob(nameof(BackgroundJob))
                        .WithIdentity(nameof(BackgroundJob))
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInHours(24)
                            .RepeatForever()));
            });

            builder.Services.AddQuartzHostedService(x => x.WaitForJobsToComplete = true);

            var jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");
            builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);

            var jwtConfiguration = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtConfiguration.SecretKey);

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidIssuer = jwtConfiguration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            using var serviceScope = app.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            await app.ConfigureIdentityAsync();

            app.MapControllers();

            app.Run();
        }
    }
}