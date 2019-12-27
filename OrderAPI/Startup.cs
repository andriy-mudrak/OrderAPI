using System;
using System.Linq;
using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.GlobalExecutionId;
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

            services.AddTransient<ICancelOrder, CancelOrder>();
            services.AddTransient<IShipmentMethodService, ShipmentMethodServiceFake>();
            services.AddTransient<IItemService, ItemServiceFake>();

            services.AddRawRabbit(new RawRabbitOptions
            {
                ClientConfiguration = GetRawRabbitConfiguration(),
                Plugins = p => p
                    .UseGlobalExecutionId()
                    .UseMessageContext<MessageContext>()

            }).AddControllers();

            services.AddTransient<ISubscriber, Subscriber>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISubscriber subscriber)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            subscriber.Start();
        }

        private RawRabbitConfiguration GetRawRabbitConfiguration()
        {
            var section = Configuration.GetSection("RawRabbit");
            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }
            return section.Get<RawRabbitConfiguration>();
        }
    }
}
