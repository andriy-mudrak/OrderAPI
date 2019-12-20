using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using BLL.DTOs;
using BLL.Fake.Models.Item;
using BLL.Fake.Models.Shipment;
using BLL.Fake.Services;
using BLL.Fake.Services.Interfaces;
using BLL.Helpers.Mapping;
using BLL.Helpers.MQ;
using BLL.Helpers.MQ.Interfaces;
using BLL.Helpers.Queries;
using BLL.Helpers.Queries.Interfaces;
using BLL.Services;
using BLL.Services.Interfaces;
using Chronicle;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Scrutor;

namespace OrderAPI
{
    public class Startup
    {
        private IRpcClient _rpcServer;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderDbTestContext>(options => 
                options.UseSqlServer(Configuration.GetSection("ConnectionStrings:DefaultConnection").Value));

            services.AddAutoMapper(options => options.AddProfile<AutoMapperProfile>(), typeof(Startup));

            services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
            services.AddTransient<IPromoCodeQueryCreator, PromoCodeQueryCreator>();
            services.AddTransient<IPromoCodeService, PromoCodeService>();

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerQueryCreator, CustomerQueryCreator>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderQueryCreator, OrderQueryCreator>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<IOrderingService, OrderingService>();
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddTransient<IShipmentMethodService, ShipmentMethodServiceFake>();
            services.AddTransient<IItemService, ItemServiceFake>();

            services.AddTransient<IRabbitMQPublish, RabbitMQPublish>();
            //services.AddTransient<IRabbitMQConsumer, RabbitMQConsumer>();
            //services.AddRabbitMq();
            services.AddControllers();
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //var coordinator = app.ApplicationServices.GetService<ISagaCoordinator>();


            //var context = SagaContext
            //    .Create()
            //    .WithSagaId(SagaId.NewSagaId())
            //    .WithOriginator("Test")
            //    .WithMetadata("key", "lulz")
            //    .Build();

            //     coordinator.ProcessAsync(new List<ItemModelDTO>(), context);
            //     coordinator.ProcessAsync(new ShipmentModelDTO(), context);
            //     coordinator.ProcessAsync(new OrderInfoDTO(), context);
            //     coordinator.ProcessAsync(new PaymentInfoDTO(), context);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
