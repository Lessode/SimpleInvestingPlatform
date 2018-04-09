using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Coins.Data;
using Coins.Entities;
using Coins.Services.Interfaces;
using Coins.Services;
using Coins.Repositories;
using Hangfire;
using System;
using Microsoft.AspNetCore.Http;

namespace Coins
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        private readonly IHttpContextAccessor httpContextAccessor;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddEntityFramework()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );
           // services.AddHangfire(x => x.UseSQLiteStorage(Configuration.GetConnectionString("HangfireConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // User settings
                options.User.RequireUniqueEmail = true;
            });


           



            // Add framework services.
            services.AddMvc();

            // Add application services.

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPaymentsService, PaymentsService>();
            services.AddTransient<IEarningsService, EarningsService>();
            services.AddTransient<ITicketsService, TicketsService>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            services.AddSingleton<IConfigurationRoot>(provider => Configuration);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();

            var serviceProvider = services.BuildServiceProvider();
            var earningsService = serviceProvider.GetService<IEarningsService>();

            earningsService.AppendBonus();



            // ROLES
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string roleName = "Admin";
            IdentityResult roleResult;

            var roleExist = roleManager.RoleExistsAsync(roleName).Result;

            if(roleExist)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
