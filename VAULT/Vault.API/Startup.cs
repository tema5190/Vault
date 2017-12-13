using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Vault.Services;
using Vault.DATA;
using Microsoft.EntityFrameworkCore;
using Vault.DATA.DTOs.Email;
using Vault.DATA.DTOs;
using FluentScheduler;
using Vault.Schedule;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Vault.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("VaultDataBase");
            services.AddDbContext<VaultContext>(option => option.UseSqlServer(connection, x=> x.MigrationsHistoryTable("Migrations")));
            services.AddOptions();

            services.Configure<EmailSMTPConfiguration>(Configuration.GetSection("EmailSMTPConfiguration"));
            services.Configure<SmsConfiguration>(Configuration.GetSection("PhoneConfiguration"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,

                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(), 
                        ValidateIssuerSigningKey = true,
                    };
                });

           
            services.AddTransient<AuthService>();
            services.AddTransient<UserService>();
            services.AddTransient<UserCardService>();
            services.AddTransient<EmailService>();
            services.AddTransient<VaultContextInitializer>();
            services.AddTransient<GoalService>();
            services.AddTransient<SmsService>();
            services.AddTransient<BankOperationService>();
            services.AddTransient<TransactionsService>();

            // Sheduler section
            services.AddTransient<BankJobFactory>();
            services.AddTransient<CalculateProfitJob>();
            services.AddTransient<ExecuteDailyTransactionsJob>();
            var provider = services.BuildServiceProvider();

            JobManager.JobFactory = new BankJobFactory(provider);
            JobManager.Initialize(new BankJobRegisty());
            JobManager.Start();
            //

            services.AddCors();

            services.AddMvc(); // <---

            // Swagger section
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Vault API v1", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });

            });
            //
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {   
            // Swagger section
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vault API V1");
            });
            //


            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
            //    RequestPath = new PathString("/.well-known")
            //});

            app.UseMvc();
        }
    }
}
