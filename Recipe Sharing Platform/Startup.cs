using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Sharing_Platform.Controllers;
using Recipe_Sharing_Platform.Models;
using Microsoft.Extensions.Hosting;

namespace Recipe_Sharing_Platform
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

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddControllersWithViews(); // Add MVC services

			// Add Identity
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			// Configure Identity options if needed

			// Add SignInManager service
			services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

			// Register your repository implementation here
			services.AddScoped<IRecipeRepository, SQLRecipeRepository>(); // Corrected typo

            // Disable endpoint routing
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Use static files
            app.UseStaticFiles();

            // Use authentication, authorization, etc. if needed

            // Use MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

