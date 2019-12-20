using BLL.Helpers.MQ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //RabbitMQConsumer client = new RabbitMQConsumer();
            //client.CreateConnection();
            //client.ProcessMessages();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
