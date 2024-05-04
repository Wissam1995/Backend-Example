using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System.ComponentModel;
using System.Reflection;
using WeatherCheck.Data;
using WeatherCheck.Data.Interfaces;
using WeatherCheckApi.Extensions;
using WeatherCheckApi.Services.WeatherApi;
using IContainer = Autofac.IContainer;

namespace WeatherCheckApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("test")
            //    .AddJwtBearer("test", options =>
            //    {
            //        options.Authority = Configuration.GetValue<string>("AuthorityUrl");
            //        options.TokenValidationParameters.ValidateAudience = false;
            //        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            //    })
            //    .AddJwtBearer("awsIdp",
            //        options => { options.TokenValidationParameters = GetAwsTokenValidationParams(); });

            services.AddOpenTracing();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherCheckApi", Version = "v1" });
                c.CustomSchemaIds(x =>
                    x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ??
                    x.FullName);
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description =
                //        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] { }
                //    }
                //});
                //Set the comments path for the Swagger JSON and UI.* *
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var connectionString = Configuration.GetValue<string>("ConnectionString");
            services.RegisterServiceForwarder<IWeatherApiService>("weather-api-service");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()!)
                .AsImplementedInterfaces();
            builder.Populate(services);
            //builder.AddRabbitMq();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherCheckApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseRabbitMq();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            applicationLifetime.ApplicationStopped.Register(() => { Container.Dispose(); });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}