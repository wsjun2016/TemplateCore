using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BasisFrameWork.Configuration;
using BasisFrameWork.Dependency.Installers;
using BasisFrameWork.Extension.Hangfire;
using BasisFrameWork.Extension.Hangfire.Filter;
using BasisFrameWork.Extension.RabbitMQ;
using BasisFrameWork.Extension.RabbitMQ.Base;
using BasisFrameWork.Extension.Redis;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;

namespace TemplateCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //设置 默认配置上下文
            DefaultConfiguration.SetDefaultConfiguration(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Web Api"
                });

                string basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string xmlPath = Path.Combine(basePath, "TemplateCore.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddCustomedServices();

            //加入Hangfire任务服务
           // services.AddHangfire();

            //加入Options选项服务
            services.AddOptions();

            //加入hangfire服务
            services.AddHangfire();

            //加入Redis分布式缓存服务
            services.AddCSRedis();

            //配置RabbitMQOption
            services.Configure<RabbitMQOption>(DefaultConfiguration.Configuration.GetSection("RabbitMq"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider svp)
        {
            //设置 服务定位器
            ServiceLocator.SetServices(svp);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else app.UseExceptionHandler("/Home/Error");

            //这种写法不能定位Scope作用域的接口实例
            // Cdpc.Infrastructure.Applicatiuon.ServiceLocator.SetServices(app.ApplicationServices);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            //使用Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUi("swagger");

            //使用Hangfire中间件。 会自动开启IHangfireTask任务
            app.UseHangfire(new DashboardOptions
            {
                Authorization = new[] { new HangfireCustomAuthorizeFilter() }
            });

            //配置支持Nginx
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}
