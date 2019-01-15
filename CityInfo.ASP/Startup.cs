using CityInfo.ASP.Entities;
using CityInfo.ASP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.ASP
{
    public class Startup
    {
        public static IConfiguration Configuration { get; internal set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(option =>
                    option.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            
            //.AddJsonOptions(o =>
            //{
            //    if(o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;

            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            services.AddTransient<IMailService, LocalMailService>();
            
            // Init DB Context
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=true";

            services.AddDbContext<Entities.CityInfoContext>( o => o.UseSqlServer(connectionString));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, CityInfoContext cityInfoContext)
        {
            // Add the exception handling middleware
            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler();
            }
            
            cityInfoContext.EnsureSeedDataForContext();

            applicationBuilder.UseStatusCodePages();

            // add MVC middleware
            applicationBuilder.UseMvc();

            //applicationBuilder.Run(async context => await context.Response.WriteAsync("Hello World!!!"));
        }
    }
}
