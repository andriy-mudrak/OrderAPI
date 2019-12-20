//using System.Text;
//using System.Threading.Tasks;
//using BLL.DTOs;
//using BLL.Helpers.MQ.Interfaces;
//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using RabbitMQ.Client.MessagePatterns;

//namespace BLL.Helpers.MQ
//{
//    public class RabbitMQConsumer : IRabbitMQConsumer
//    {
//        private static ConnectionFactory _factory;
//        private static IConnection _connection;
//        private readonly IPaymentRabbitMQController _paymentRabbitMqController;
//        private const string ExchangeName = "Topic_Exchange";
//        private const string CardPaymentQueueName = "CardPaymentTopic_Queue";


//        public RabbitMQConsumer(IPaymentRabbitMQController paymentRabbitMqController)
//        {
//            _paymentRabbitMqController = paymentRabbitMqController;

//        }

//        public void Start()
//        {
//            CreateConnection();
//            ProcessMessages();
//        }

//        private void CreateConnection()
//        {
//            _factory = new ConnectionFactory
//            {
//                HostName = "localhost",
//                UserName = "guest",
//                Password = "guest"
//            };
//        }

//        private async Task ProcessMessages()
//        {
//            using (_connection = _factory.CreateConnection())
//            {
//                using (var channel = _connection.CreateModel())
//                {
//                    channel.ExchangeDeclare(ExchangeName, "topic");
//                    channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);

//                    channel.QueueBind(CardPaymentQueueName, ExchangeName, "payment");

//                    channel.BasicQos(0, 10, false);
//                    Subscription subscription = new Subscription(channel, CardPaymentQueueName, false);

//                    while (true)
//                    {
//                        BasicDeliverEventArgs deliveryArguments = subscription.Next();

//                        var message = deliveryArguments.Body;
//                        Encoding.UTF8.GetString(message);
//                        var payment = JsonConvert.DeserializeObject<PaymentModel>(Encoding.UTF8.GetString(message));
//                        await _paymentRabbitMqController.Post(payment);

//                        subscription.Ack(deliveryArguments);
//                    }
//                }
//            }
//        }

//        public void Close()
//        {
//            _connection.Close();
//        }
//    }
//}