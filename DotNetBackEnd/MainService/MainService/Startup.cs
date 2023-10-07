using AutoMapper;
using CategoryService.AsyncDataServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RBVH.HRL.Services.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using DoAnTotNghiep.AsyncDataServices;
using DoAnTotNghiep.Data;
using DoAnTotNghiep.EventProcessing;
using DoAnTotNghiep.SyncDataServices.Grpc;
using WebAppAPI.Data;
using WebAppAPI.Extensions;
using WebAppAPI.Services.Model;

namespace WebAppAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Config.Configuration = configuration;
            Configuration = configuration;          
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            services.AddHttpContextAccessor();
            services.AddRazorPages();
            services.AddControllers();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProjectMapper());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DO AN TOT NGHIEP - MAIN SERVICE - API MANAGEMENT", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                    {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    };
                });
            services.AddEndpointsApiExplorer();
            services.AddAPIRepositories();
            services.AddAPIServices();
            ///add configs to the services 
            services.AddDbContext<ApplicationDbContext>(
                options => 
                { 
                    options.UseSqlServer(Configuration.GetConnectionString("DoAnTotNghiep"));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddScoped<ICategoryDataClient, CategoryDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.Configure<FormOptions>(o =>
             {
                 o.ValueLengthLimit = int.MaxValue;
                 o.MultipartBodyLengthLimit = int.MaxValue;
                 o.MemoryBufferThreshold = int.MaxValue;
             });
            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Config.BackEndUrl}/swagger/v1/swagger.json", "DO AN TOT NGHIEP - MAIN SERVICE - API MANAGEMENT V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            //PrepDb.PrepPopulation(app);
        }
    }
}
