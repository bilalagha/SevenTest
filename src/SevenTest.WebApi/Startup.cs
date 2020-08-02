using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SevenTest.ApiRepository;
using SevenTest.Business;
using SevenTest.Core;
using SevenTest.Core.Configuration;

namespace SevenTest.WebApi
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
            services.AddControllers();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "User_";
            });

            var cacheTimeoutsConfiguration = new CacheTimeoutsConfiguration();
            Configuration.GetSection(CacheTimeoutsConfiguration.ConfigurationName).Bind(cacheTimeoutsConfiguration);
            services.AddScoped(typeof(CacheTimeoutsConfiguration), p => { return cacheTimeoutsConfiguration; });
            string url = Configuration["ApiUrl"];
            services.AddScoped(typeof(IPersonRepository), p => { return new PersonApiRepository(url);  });           
            services.AddScoped(typeof(IPersonService), typeof(PersonService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public static IPersonRepository PersonRepositoryFactory()
        {
            return new PersonApiRepository("");
        }
    }
}
