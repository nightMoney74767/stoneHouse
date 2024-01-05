using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace J85452___CO5227_Restaurant_Project
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
            services.AddRazorPages();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));
            services.AddIdentity<AppUserClass, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Authorisation; non-admins are redirected to the AccessDenied page when attempting to access admin pages
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/AccessDenied");
                options.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Index");
            });

            // Stripe payment
            services.Configure<StripeSettingsClass>(Configuration.GetSection("Stripe"));
        }

        // Assign roles for users
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUserClass>>();

            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _user = await UserManager.FindByEmailAsync("admin@stonehouse.co.uk");
            if (_user != null)
            {
                await UserManager.AddToRoleAsync(_user, "Admin");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Stripe payment
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

            // Added for assigning admins
            //CreateRoles(serviceProvider).Wait();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Added for login
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

    
}
