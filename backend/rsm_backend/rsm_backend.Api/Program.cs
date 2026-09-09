
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Resend;
using rsm_backend.Application;
using rsm_backend.Application.Services;
using rsm_backend.Application.Services.Admin;
using rsm_backend.Application.Services.Interfaces;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Application.Services.Interfaces.Infrastructure.IRepositories;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure;
using rsm_backend.Infrastructure.Data;
using rsm_backend.Infrastructure.Repositories;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


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

            builder.Services.AddDataProtection();

            // Add Identiy Core, roles and tables to hold user-data in the db + a signin manager for user login endpoint
            builder.Services
                        .AddIdentityCore<ApplicationUser>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddSignInManager()
                        .AddDefaultTokenProviders();


            //JWT and Cookie authentication setup for guests and users
            var secret = builder.Configuration["GuestJwt:Secret"]
                        ?? throw new InvalidOperationException("Guest JWT secret is not configured.");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                
            })
            .AddCookie(IdentityConstants.ApplicationScheme)
            .AddJwtBearer("GuestOrder", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["GuestJwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["GuestJwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                    ValidateLifetime = true,
                    ClockSkew=TimeSpan.Zero

                };
            });


            //Configuring lockout threshold for user cockie authentication

            var expirationMinutes =builder.Configuration.GetValue<double>("GuestJwt:ExpirationMinutes");


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(expirationMinutes);
                options.Lockout.AllowedForNewUsers = true;
            });

            // Configure CORS so that calls from the specified frontend origin is let through
            var frontEndOrigin = builder.Configuration["Frontend:Origin"]
                        ?? throw new InvalidOperationException("Frontend Origin is not configured.");

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins(frontEndOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            //Configure Cookies
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;

                if (builder.Environment.IsDevelopment())
                {
                    //http and https is allowed
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                }
                else
                {  
                    //https is only alowed
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                }
            });


            //Repositories
            builder.Services.AddScoped<IBrandRepository,BrandRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IDeliveryOptionRepository, DeliveryOptionRepository>();
            builder.Services.AddScoped<IDiscountCodeRepository,DiscountCodeRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
            builder.Services.AddScoped<IProductRepository,ProductRepository> ();
            builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            builder.Services.AddScoped<ITagRepository,TagRepository>();
            builder.Services.AddScoped<IGuestOrderVerificationRepository,GuestOrderVerificationRepository>();

            // Background services
            builder.Services.AddHostedService<ExpiredDataCleanupService>();

            // R2 object storage setup for the R2StorageService
            builder.Services.Configure<R2Settings>(builder.Configuration.GetSection("R2"));
            builder.Services.AddSingleton<IObjectStorage,R2StorageService>();

            

            //ImageConversion service used for images stored and used.
            builder.Services.AddScoped<IImageConversionService, ImageConversionService>();



            //Unit of work method to do SaveChangesAsync 
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

            //Services
            builder.Services.AddScoped<IAdminBrandService, AdminBrandService>();
            builder.Services.AddScoped<IAdminDeliveryOptionService, AdminDeliveryOptionService>();
            builder.Services.AddScoped<IAdminProductImageService, AdminProductImageService>();
            builder.Services.AddScoped<IAdminProductService, AdminProductService>();
            builder.Services.AddScoped<IAdminProductVariantService, AdminProductVariantService>();
            builder.Services.AddScoped<IAdminTagService, AdminTagService>();

            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentSummaryService,PaymentSummaryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IGuestOrderTokenService, GuestOrderTokenService>();
            builder.Services.AddScoped<IGuestOrderAccessService, GuestOrderAccessService>();
            
            // Resend (Email Service)

            var resendApiKey = builder.Configuration["Resend:ApiKey"]
                             ?? throw new InvalidOperationException("Resend API key is not configured.");

            builder.Services.AddOptions<ResendClientOptions>()
                .Configure(options =>
                {
                    options.ApiToken = resendApiKey;
                });

            builder.Services.AddHttpClient<ResendClient>();

            builder.Services.AddTransient<IResend, ResendClient>();

            builder.Services.AddScoped<IEmailService, ResendEmailService>();

           


            


            builder.Services.AddHttpContextAccessor();


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(
                        JsonNamingPolicy.CamelCase,
                        allowIntegerValues: false)
                );
            }); 

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

                        InvalidOperationException invalidOperationException => new
                        {
                            StatusCode = StatusCodes.Status409Conflict,
                            Message = invalidOperationException.Message

                        },
                        UnauthorizedAccessException ex => new
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = ex.Message
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


            app.UseHttpsRedirection();

            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();

			app.MapControllers();

        

            app.Run();
		}
	}
}
