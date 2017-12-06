using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DigiQueue.Models.Entities;
using DigiQueue.Models.Repositories;
using DigiQueue.Models.Hubs;

namespace DigiQueue
{
    public class Startup
    {
        IConfiguration conf;

        public Startup(IConfiguration conf)
        {
            this.conf = conf;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Digibase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string connString = conf.GetConnectionString("connString");

            // Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Digibase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "Models/Entities" -Context "DigibaseContext" -Schema "DigiSchema" -Force


            services.AddDbContext<DigibaseContext>(o => o.UseSqlServer(connString));
            services.AddDbContext<IdentityDbContext>(o => o.UseSqlServer(connString));


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(o => o.LoginPath = "/Home/Login");

            services.AddTransient<IRepository, DigiBaseRepository>();
            services.AddSignalR();
            services.AddMvc();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSignalR(routes => { routes.MapHub<DigiHub>("digihub"); });
            app.UseMvcWithDefaultRoute();
        }
    }
}
