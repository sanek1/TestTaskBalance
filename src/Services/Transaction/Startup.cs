namespace Transaction.WebApi
{
    using Transaction.WebApi.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Swagger;
    using Transaction.Framework.Extensions;
    using Transaction.WebApi.Services;
    using System.IO;
    using Transaction.WebApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransactionFramework(Configuration);
            services.AddScoped<ExceptionHandlerMiddleware>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Simple Transaction Processing", Version = "v1" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IRabbitMqService, RabbitMqService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            var myConnectionString1 = _configuration.GetConnectionString("MyConnectionString1");

            app.UseExceptionHandlerMiddleware(); 
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Transaction Processing v1");
            });
            app.UseMvc();
        }
    }
}
