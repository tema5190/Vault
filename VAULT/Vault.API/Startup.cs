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

namespace Vault.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services, IHostingEnvironment env)
        {
            string connection;

            if (env.IsDevelopment())
            {
                connection = Configuration.GetConnectionString("VaultDataBase");
            }
            else
            {
                connection = Configuration.GetConnectionString("HostingDataBase");
            }

            services.AddDbContext<VaultContext>(option => option.UseSqlServer(connection));

            

            services.AddOptions();
            services.Configure<EmailSMTPConfiguration>(Configuration.GetSection("EmailSMTPConfiguration"));

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
            services.AddTransient<CreditCardService>();
            services.AddTransient<EmailService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
