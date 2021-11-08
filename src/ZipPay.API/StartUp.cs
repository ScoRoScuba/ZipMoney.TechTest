using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ZipPay.API.Repository;
using ZipPay.API.Services;

namespace ZipPay.API
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Zip Co Test", Version = "v1" }); });

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            containerBuilder.Register(c =>
            {
                var connectionString = Configuration["MYSQL_CONNECTION"];
                var opt = new DbContextOptionsBuilder<ZipPayDataContext>();
                opt.UseMySQL(connectionString);

                return new ZipPayDataContext(opt.Options);

            }).AsImplementedInterfaces().InstancePerDependency();

            containerBuilder.RegisterType<UserRepository>().AsImplementedInterfaces();
            containerBuilder.RegisterType<UserService>().AsImplementedInterfaces();

            containerBuilder.RegisterType<AccountRepository>().AsImplementedInterfaces();
            containerBuilder.RegisterType<AccountService>().AsImplementedInterfaces();

            containerBuilder.RegisterType<AccountNumberGenerator>().AsImplementedInterfaces();
            containerBuilder.RegisterType<CreditCheckerService>().AsImplementedInterfaces();

            Container = containerBuilder.Build();

            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger/ui";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ZipPay API V1");
                options.DisplayOperationId();
            });

            app.UseMvc();
        }
    }
}
