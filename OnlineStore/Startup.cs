using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnlineStore.Data;
using OnlineStore.Data.Entities;
using OnlineStore.Helpers;
using Vereyon.Web;

namespace OnlineStore
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
            services.AddIdentity<User, IdentityRole>(cfg => // Replace 'User' by 'IdentityUser' to use ASP.NET Core built-in user.
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; // Use default token generator.
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            })
                .AddDefaultTokenProviders() // Use default token providers for email address confirmation and password recovery.
                .AddEntityFrameworkStores<DataContext>(); // Use simple data context after authentication completion.


            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters // Get configuration parameters from 'appsettings.json' to application middleware.
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });


            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("LocalConnectionString")); // Get connection string from 'appsettings.json'.
            });

            //services.AddSingleton(); // Keep object in memory throughout application run-time.

            services.AddTransient<SeedDb>(); // 'AddTransient': Remove object from memory after completion.

            services.AddFlashMessage();

            #region Helpers: do not interfere with the database.
            services.AddScoped<IUserHelper, UserHelper>(); // 'AddScoped': Keep object in memory until another of same type is created and replaces it.
            services.AddScoped<IImageHelper, ImageHelper>(); // Instantiated at 'ProductsController' constructor.
            services.AddScoped<IConverterHelper, ConverterHelper>();
            services.AddScoped<IMailHelper, MailHelper>();
            #endregion

            #region Repositories: interfere with the database.
            services.AddScoped<IProductRepository, ProductRepository>(); // Everytime the products are loaded, create new products and replace the previous ones.
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            #endregion

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/Account/NotAuthorized"; // Replace redirected login view from '[Authorized]' actions if user is logged out (401 Unauthorized).
                cfg.AccessDeniedPath = "/Account/NotAuthorized"; // Replace default forbidden view if user is logged in but without action permissions (403 Forbidden).
            });

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
                app.UseExceptionHandler("/Errors/Error"); // Default error action called here (middleware) to apply to all unhandled exceptions. // Moved from HomeController.cs to ErrorsController.
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseDeveloperExceptionPage(); // Show development error views.

            app.UseStatusCodePagesWithReExecute("/error/{0}"); // Replace specific error code action views. // Originally defined at HomeController.cs to execute at start-up.

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Allow user authentication. // Define before 'UseAuthorization'.
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
