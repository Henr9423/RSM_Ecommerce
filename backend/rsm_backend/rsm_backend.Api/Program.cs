
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using rsm_backend.Application;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Admin;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.IRepositories;
using rsm_backend.Infrastructure.Data;
using rsm_backend.Infrastructure.Repositories;

namespace rsm_backend.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
				.UseSnakeCaseNamingConvention()
				);

			//Repositories
			builder.Services.AddScoped<IBrandRepository,BrandRepository>();
			builder.Services.AddScoped<IProductRepository,ProductRepository> ();
            builder.Services.AddScoped<ITagRepository,TagRepository>();
            builder.Services.AddScoped<IProductVariantRepository,ProductVariantRepository>();

			//Services
			builder.Services.AddScoped<IAdminBrandService, AdminBrandService>();
			builder.Services.AddScoped<IAdminProductService, AdminProductService>();
            builder.Services.AddScoped<IAdminTagService, AdminTagService>();
            builder.Services.AddScoped<IAdminProductVariantService, AdminProductVariantService>();

            builder.Services.AddScoped<IProductService, ProductService>();
           
            


			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = feature?.Error;

                    var logger = context.RequestServices
                        .GetRequiredService<ILogger<Program>>();

                    logger.LogError(exception, "Unhandled exception occurred.");

                    context.Response.ContentType = "application/json";

                    var response = exception switch
                    {
                        ArgumentException => new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = exception.Message
                        },

                        KeyNotFoundException => new
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = exception.Message
                        },

                        _ => new
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = "An unexpected error occurred."
                        }
                    };

                    context.Response.StatusCode = response.StatusCode;

                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = response.Message
                    });
                });
            });

            app.Run();
		}
	}
}
