using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using WebApiCountries.Model;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiCountries
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

			services.AddCors();
			services.AddOptions();
			services.AddMemoryCache();

			services.AddHttpContextAccessor();

			services.AddControllers();
			services.AddDbContext<CountriesDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddSwaggerGen(c =>
			{


				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Web Api - Test",
					Version = "v1",
					Description = "Api, con jwt, swagger, EF  y Asp.Net Core 5.0",
					Contact = new OpenApiContact
					{
						Name = "Ing. Gregor Duarte, Msc.",
						Email = "gregorgeraldo@hotmail.com"
					}
				});


				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath, true);

				//First we define the security scheme
				c.AddSecurityDefinition("Bearer", //Name the security scheme
					new OpenApiSecurityScheme
					{
						Description = "JWT Authorization header using the Bearer scheme.",
						Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
						Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
					});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement{
					{
						new OpenApiSecurityScheme{
							Reference = new OpenApiReference{
								Id = "Bearer", //The name of the previously defined security scheme.
								Type = ReferenceType.SecurityScheme
							}
						},new List<string>()
					}
				});
			});

			services.AddAuthentication
			(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "https://localhost:44322/",
					ValidAudience = "https://localhost:44322/",
					IssuerSigningKey = new SymmetricSecurityKey
					(Encoding.UTF8.GetBytes("GmCuH67mzPWYbb4W4uDLPprHjzLh01jTpJWmZoLy620jfTT9nWP5zg=="))
				};
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors(options =>
			{
				options.WithOrigins("https://localhost:44363/");
				options.AllowAnyMethod();
				options.AllowAnyHeader();

			});




			app.UseCors(options => options.AllowAnyOrigin());


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiCountries v1"));
			}


			app.UseAuthentication();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
