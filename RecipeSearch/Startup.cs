using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using RecipeSearch.DataAccess.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeSearch.DataAccess.Initializer;
using RecipeSearch.DataAccess.Repository;
using RecipeSearch.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using RecipeSearch.Utility;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RecipeSearch
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)         
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
           
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                
                options.User.RequireUniqueEmail = true;
            });
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<EmailOptions>(Configuration);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            

            services.AddControllersWithViews().AddRazorRuntimeCompilation(); 
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            }
            );
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "1658891364274069";
                options.AppSecret = "7cb8d4caa7661b5b413ce94772377a5f";
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "273338108308-rcuk1hu9nnemt84udj1e5c2p935n4ara.apps.googleusercontent.com";
                options.ClientSecret = "ri0YbTg46GRYySnBsWSt4Ku8";
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();
          //  app.UseCookiePolicy();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            dbInitializer.Initialize();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
