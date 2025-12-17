using BacklogTracker.Infrastructure.Data;
using BacklogTracker.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Infrastructure.Repositories;
using BacklogTracker.Infrastructure.Entities;
using BacklogTracker.Infrastructure.Services;
using BacklogTracker.Infrastructure.Configuration;

namespace BacklogTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<BacklogTrackerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();

            var GiantBombConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();
            builder.Services.Configure<GiantBombConfiguration>(GiantBombConfiguration.GetSection("GiantBombConfiguration"));

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddScoped<IGameService, GiantBombService>();
            builder.Services.AddScoped<IBacklogService, BacklogService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

			builder.Services.AddHttpClient("GiantBomb", httpClient =>
			{
				httpClient.BaseAddress = new Uri("https://www.giantbomb.com/");
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Backlog Tracker app");
			});

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.Run();
        }
    }
}
