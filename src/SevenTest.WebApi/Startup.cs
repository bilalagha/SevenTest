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
        const string SourceApiUrlKey= "SourceApiUrl";
        const string SourceApiTimeoutInSecondsKey= "SourceApiTimeoutInSeconds";
        const string DistributedCacheEnabledKey = "DistributedCacheEnabled";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
           

            var distributedCacheConfig = new DistributedCacheConfiguration();
            Configuration.GetSection(DistributedCacheConfiguration.ConfigurationSectionKey).Bind(distributedCacheConfig);
            if (distributedCacheConfig.DistributedCacheEnabled)
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration.GetConnectionString("Redis");
                    options.InstanceName = "User_";
                });
            }

            services.AddScoped(typeof(DistributedCacheConfiguration), p => { return distributedCacheConfig; });
            string sourceApiUrl = Configuration[SourceApiUrlKey];
            var sourceApiTimeout = long.Parse(Configuration[SourceApiTimeoutInSecondsKey]);
            services.AddScoped(typeof(IPersonRepository), p => { return new PersonApiRepository(sourceApiUrl, sourceApiTimeout);  });           
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
    }
}
