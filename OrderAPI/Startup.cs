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
using BLL.Helpers.OrderCanceling;
using BLL.Helpers.OrderCanceling.Interfaces;
using BLL.Helpers.Queries;
using BLL.Helpers.Queries.Interfaces;
using BLL.MessagesTest;
using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Subscribers;
using BLL.Subscribers.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.HttpContext;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;

namespace OrderAPI
{
    public class Startup
    {
        private static ISubscriber _client;

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
            services.AddTransient<BLL.Services.Interfaces.IPromoCodeService, PromoCodeService>();

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerQueryCreator, CustomerQueryCreator>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderQueryCreator, OrderQueryCreator>();

            services.AddTransient<IOrderingService, OrderingService>();

            services.AddTransient<IShipmentMethodService, ShipmentMethodServiceFake>();
            services.AddTransient<IItemService, ItemServiceFake>();

            var rawRabbitOptions = new RawRabbitOptions
            {
                ClientConfiguration = GetRawRabbitConfiguration(),
                Plugins = p => p
                    .UseStateMachine()
                    .UseGlobalExecutionId()
                    .UseHttpContext()
                    .UseMessageContext(c =>
                    {
                        return new MessageContext
                        {
                            Source = c.GetHttpContext().Request.GetDisplayUrl()
                        };
                    })
            };
            services.AddTransient<ICancelOrder, CancelOrder>();

            services.AddSingleton(provider => new Subscriber(RawRabbitFactory.CreateSingleton(rawRabbitOptions), provider.GetService<ICancelOrder>())); 

            services.AddRawRabbit(rawRabbitOptions).AddControllers();

            _client = services.BuildServiceProvider().GetService<Subscriber>();
            _client.Start();
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

        private RawRabbitConfiguration GetRawRabbitConfiguration()
        {
            var section = Configuration.GetSection("RawRabbit");
            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }
            var test = section.Get<RawRabbitConfiguration>();
            return test;
        }
    }
}
