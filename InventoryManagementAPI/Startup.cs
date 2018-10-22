using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementAPI
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
            services.AddMvc().AddJsonOptions(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();

            //ADD DATABASE CONTEXT


            //ADD THE AUTHENTICATION SERVICES
            var connectionString = "";

            //Check if there is an evironment parameter assigned. 
            //If not will default to LocalDB. 
            if (string.IsNullOrEmpty(Configuration["DatabaseServer"]))
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                var server = Configuration["DatabaseServer"];
                var database = Configuration["DatabaseName"];
                var user = Configuration["DatabaseUser"];
                var password = Configuration["DatabasePassword"];
                connectionString = string.Format("Server={0};Database={1};User={2};Password={3};",
                    server, database, user, password);
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            //ADD DEPENDENCY INJECTION FOR REPOSITORIES
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            //ADD AUTOMAPPER
            /*
             * Need to have this dependency added in the Nuget Package Manager
             * AutoMapper.Extensions.Microsoft.DependencyInjection
             * 
            */
            services.AddAutoMapper();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(option => option.AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin()
                                        .AllowCredentials());
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
