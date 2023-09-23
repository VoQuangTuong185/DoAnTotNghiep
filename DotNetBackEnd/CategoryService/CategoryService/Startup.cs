using AutoMapper;
using CategoryService.AsyncDataServices;
using CategoryService.Data;
using CategoryService.Model.Services.Model;
using CategoryService.SyncDataServices.Grpc;
using CategoryService.SyncDataServices.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CategoryService
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
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CATEGORY SERVICE - API MANAGEMENT", Version = "v1" });
            });
            services.AddEndpointsApiExplorer();
            services.AddDbContext<AppDbContext>(
               options =>
               {
                   options.UseSqlServer(Configuration.GetConnectionString("CategoryDb"));
                   options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
               });
            //services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMen"));
            services.AddScoped<ICategoryRepo, CategoryRepo>();

            services.AddHttpClient<IProductDataClient, HttpProductDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
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
            //Console.Writeline($"--> CategoryService Endpoint {Configuration["CategoryService"]}");
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

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
                c.SwaggerEndpoint($"{Config.BackEndUrl}/swagger/v1/swagger.json", "THUC TAP TOT NGHIEP API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcCategoryService>();

                endpoints.MapGet("/protos/category.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/category.proto"));
                });
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
