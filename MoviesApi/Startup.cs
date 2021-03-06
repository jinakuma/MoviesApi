      using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Filters;
using MoviesApi.Helpers;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace MoviesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Microsoft.EntityFrameworkCore.SqlServer
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.UseNetTopologySuite()));

           
            //Allow cross-origin request to the 'frontendURL' 
            services.AddCors(options =>
            {
                var frontendUrl = Configuration.GetValue<string>("frontendUrl");
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendUrl).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("totalAmountOfRecords");
                });
            });

            services.AddAutoMapper(typeof(Startup));

            //Dependency injection of GeometryFactory into AutoMapper class 
            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                config.AddProfile(new AutoMapperProfiles(geometryFactory));
            }).CreateMapper());

            //Microsoft.EntityFrameworkCore.SqlService.NetTopologySuite
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));


            // Save files in Azure.Storage.Blobs
            //services.AddScoped<IFileStorageService, AzureStorageService>();


            // Save files in Firebase Storage Cloud.
            services.AddScoped<IFileStorageService, FirebaseFileStorageService>();


            // Save files in app folder
            //services.AddScoped<IFileStorageService, InAppStorageService>();
            //services.AddHttpContextAccessor();


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(MyExceptionFilter));
            });
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoviesApi", Version = "v1" });
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["keyjwt"])),
                    ClockSkew = TimeSpan.Zero

                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesApi v1"));
            }
           

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //Cors must me after Routing and before Authorization
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
