using System;
using System.Globalization;
using BLL.App;
using BLL.App.Helpers;
using BLL.Base.Helpers;
using Contracts.BLL.App;
using Contracts.BLL.Base.Helpers;
using Contracts.DAL.App;
using Contracts.DAL.Base.Helpers;
using DAL.App;
using DAL.App.Helpers;
using DAL.Base.Helpers;
using Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TimeableAppWeb.Services;

namespace TimeableAppWeb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            var conf = Configuration;
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<IDataContext, AppDbContext>();
            services.AddScoped<IBaseRepositoryProvider, BaseRepositoryProvider<AppDbContext>>();
            services.AddSingleton<IBaseRepositoryFactory<AppDbContext>, AppRepositoryFactory>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            services.AddScoped<IServiceFactoryBase<IAppUnitOfWork>, AppServiceFactory>(); //these
            services.AddScoped<IServiceProviderBase, ServiceProviderBase<IAppUnitOfWork>>();
            services.AddScoped<IBLLApp, BLLApp>();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 6;
            });

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddControllersWithViews();
            services.AddRazorPages();

            //  =============== i18n support ===============
            services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[]{
                    new CultureInfo(name: "en-GB"),
                    new CultureInfo(name: "et-EE")
                };

                // State what the default culture for your application is. 
                options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");

                // You must explicitly state which cultures your application supports.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings
                options.SupportedUICultures = supportedCultures;
            });

            services.AddHostedService<TimedScheduleUpdateHostedService>();

            // makes httpcontext injectable - needed to resolve username in dal layer
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            UpdateDatabase(app, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            context.Database.Migrate();
            
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            // ======= plug i18n locale switcher into pipeline ================
            app.UseRequestLocalization(options:
                app.ApplicationServices
                    .GetService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app,
            IConfiguration configuration)
        {
            // give me the scoped services (everyhting created by it will be closed at the end of service scope life).
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var ctx = serviceScope.ServiceProvider.GetService<AppDbContext>();
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (configuration["AppDataInitialization:DropDatabase"] == "True")
            {
                DataInitializers.DeleteDatabase(ctx);
            }

            if (configuration["AppDataInitialization:MigrateDatabase"] == "True")
            {
                DataInitializers.MigrateDatabase(ctx);
            }

            if (configuration["AppDataInitialization:SeedIdentity"] == "True")
            {
                DataInitializers.SeedIdentity(userManager, roleManager);
            }

            if (configuration.GetValue<bool>("AppDataInitialization:SeedData"))
            {
                DataInitializers.SeedData(userManager, ctx, configuration["ScreenDataPrefix"]);
            }
        }
    }
}
