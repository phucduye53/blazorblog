using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using blazorblog.Data;
using blazorblog.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using blazorblog.Entity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using blazorblog.Helpers;
using blazorblog.Data.Dto;
using Blazored.Toast;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace blazorblog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<blogContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Transient);
            services.AddDefaultIdentity<User>(
                  options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<blogContext>();
            services.Configure<IdentityOptions>(options =>  
            {  
                // Password settings.  
                options.Password.RequireDigit = true;  
                options.Password.RequireLowercase = false;  
                options.Password.RequireNonAlphanumeric = false;  
                options.Password.RequireUppercase = false;  
                options.Password.RequiredLength = 6;  

            });  
  

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider,ServerAuthenticationStateProvider>();

            services.AddTransient<BlogService>();
            services.AddTransient<UserService>();
            services.AddTransient<CategoryService>();
             services.AddScoped<DisqusState>();
             services.AddScoped<BlogSearchState>();
            services.AddTransient<UserResolverService>();
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
               services.AddBlazoredToast();
            services.AddHeadElementHelper();


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
                app.UseExceptionHandler("/Error");
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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
