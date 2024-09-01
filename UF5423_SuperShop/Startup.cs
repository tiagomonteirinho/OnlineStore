using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UF5423_SuperShop.Data;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Helpers;

namespace UF5423_SuperShop
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
            services.AddIdentity<User, IdentityRole>(cfg => // Replace 'User' by 'IdentityUser' to use ASP.NET Core default user.
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            }
            ).AddEntityFrameworkStores<DataContext>(); // Use simple data context after authentication completion.

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("LocalConnectionString")); // Get connection string from 'appsettings.json'.
            });

            //services.AddSingleton(); // Keep object in memory throughout application run-time.

            services.AddTransient<SeedDb>(); // 'AddTransient': Remove object from memory after completion.

            services.AddScoped<IUserHelper, UserHelper>(); // 'AddScoped': Keep object in memory until another of same type is created and replaces it.
            services.AddScoped<IImageHelper, ImageHelper>(); // Instantiated at 'ProductsController' constructor.
            services.AddScoped<IConverterHelper, ConverterHelper>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
