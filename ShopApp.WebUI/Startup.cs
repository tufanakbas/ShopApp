using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Middlewares;
using ShopApp.WebUI.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using ShopApp.WebUI.EmailServices;

namespace ShopApp.WebUI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationIdentityDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				//passw

				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;

				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.AllowedForNewUsers = true;

				options.User.RequireUniqueEmail = true;

				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/account/login";
				options.LogoutPath = "/account/logout";
				options.AccessDeniedPath = "/account/accessdenied";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.SlidingExpiration = true;
				options.Cookie = new CookieBuilder
				{
					HttpOnly = true,
					Name = "ShopApp.Security.Cookie",
					SameSite = SameSiteMode.Strict
				};
			});

			services.AddRazorPages();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddMvc(option => option.EnableEndpointRouting = false);

			services.AddScoped<IProductDal, EfCoreProductDal>();
			services.AddScoped<IProductService, ProductManager>();

			services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
			services.AddScoped<ICategoryService, CategoryManager>();

			services.AddScoped<ICartDal, EfCoreCartDal>();
			services.AddScoped<ICartService, CartManager>();

			services.AddScoped<IOrderDal, EfCoreOrderDal>();
			services.AddScoped<IOrderService, OrderManager>();

			services.AddTransient<IEmailSender, EmailSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//SeedDatabase.Seed();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.CustomStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "adminProducts",
					pattern: "admin/products/",
					defaults: new { controller = "Admin", action = "ProductList" });
				endpoints.MapControllerRoute(
					name: "adminProducts",
					pattern: "admin/products/{id?}",
					defaults: new { controller = "Admin", action = "EditProduct" });

				endpoints.MapControllerRoute(
					name: "products",
					pattern: "products/{category?}",
					defaults: new { controller = "Shop", action = "List" });

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapControllerRoute(
					name: "cart",
					pattern: "cart",
					defaults: new { controller = "Cart", action = "Index" });

				endpoints.MapControllerRoute(
					name: "checkout",
					pattern: "checkout",
					defaults: new { controller = "Cart", action = "Checkout" });
				endpoints.MapControllerRoute(
					name: "orders",
					pattern: "orders",
					defaults: new { controller = "Cart", action = "GetOrders" });
			});

			SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
		}
	}
}
