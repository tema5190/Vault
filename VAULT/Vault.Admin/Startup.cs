using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vault.DATA;
using Vault.Services;

namespace Vault.Admin
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
            string connection = Configuration.GetConnectionString("VaultDataBase");
            services.AddDbContext<VaultContext>(option => option.UseSqlServer(connection, x => x.MigrationsHistoryTable("Migrations")));

            services.AddCors();

            services.AddTransient<AuthService>();
            services.AddTransient<UserService>();
            services.AddTransient<UserCardService>();
            services.AddTransient<EmailService>();
            services.AddTransient<VaultContextInitializer>();
            services.AddTransient<GoalService>();
            services.AddTransient<SmsService>();
            services.AddTransient<BankOperationService>();
            services.AddTransient<TransactionsService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}
