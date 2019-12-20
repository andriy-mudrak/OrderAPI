//using System;
//using System.Collections.Concurrent;
//using System.Text;
//using BLL.DTOs;
//using BLL.Helpers.MQ.Interfaces;
//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

//public class RpcClient: IRpcClient
//{
//    private readonly IConnection connection;
//    private readonly IModel channel;
//    private readonly string replyQueueName;
//    private readonly EventingBasicConsumer consumer;
//    private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
//    private readonly IBasicProperties props;

//    public RpcClient()
//    {
//        var factory = new ConnectionFactory() { HostName = "localhost" };

//        connection = factory.CreateConnection();
//        channel = connection.CreateModel();
//        replyQueueName = channel.QueueDeclare().QueueName;
//        consumer = new EventingBasicConsumer(channel);

//        props = channel.CreateBasicProperties();
//        var correlationId = Guid.NewGuid().ToString();
//        props.CorrelationId = correlationId;
//        props.ReplyTo = replyQueueName;

//        consumer.Received += (model, ea) =>
//        {
//            var body = ea.Body;
//            var response = Encoding.UTF8.GetString(body);
//            if (ea.BasicProperties.CorrelationId == correlationId)
//            {
//                respQueue.Add(response);
//            }
//        };
//    }

//    public string Call(PaymentModel message)
//    {
//        var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
//        channel.BasicPublish(
//            exchange: "",
//            routingKey: "rpc_queue",
//            basicProperties: props,
//            body: messageBytes);

//        channel.BasicConsume(
//            consumer: consumer,
//            queue: replyQueueName,
//            autoAck: true);

//        var test = respQueue.Take();
//        return test;
//    }

//    public void Close()
//    {
//        connection.Close();
//    }
//}


using System;
using System.Collections.Concurrent;
using System.Text;
using BLL.DTOs;
using BLL.Helpers.MQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace BLL.Helpers.MQ
{
    public class RabbitMQPublish : IRabbitMQPublish
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private static BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private const string ExchangeName = "Topic_Exchange";
        private const string CardPaymentQueueName = "CardPaymentTopic_Queue";
        private static EventingBasicConsumer _consumer;
        private static string correlationId;
        private static string replyQueueName;
        private static IBasicProperties _props;
        public RabbitMQPublish()
        {
            CreateConnection();
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",

            };


            var factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _consumer = new EventingBasicConsumer(_model);
            _props = _model.CreateBasicProperties();
            correlationId = Guid.NewGuid().ToString();
            replyQueueName = _model.QueueDeclare().QueueName;
            _props.CorrelationId = correlationId;
            _props.ReplyTo = replyQueueName;



            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(CardPaymentQueueName, true, false, false, null);
            _model.QueueBind(CardPaymentQueueName, ExchangeName, "payment");
            _model.BasicQos(0, 10, false);
        }

        public void Close()
        {
            _connection.Close();
        }

        public void Send<T>(T payment, string routingKey) where T : class
        {
            var message = JsonConvert.SerializeObject(payment);
            SendMessage(Encoding.UTF8.GetBytes(message), routingKey);
        }

        private void SendMessage(byte[] message, string routingKey)
        {
            try
            {
                _model.BasicPublish(
                    exchange: ExchangeName,
                    routingKey: routingKey,
                    basicProperties: _props,
                    body: message);

                Subscription subscription = new Subscription(_model, CardPaymentQueueName, false);

                while (true)
                {

                    BasicDeliverEventArgs deliveryArguments = subscription.Next();

                    var props = deliveryArguments.BasicProperties;
                    var replyProps = _model.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    var messageResponse = deliveryArguments.Body;
                    var payment = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(messageResponse));

                }
            }
            catch (Exception ex)
            {

            }
           

            //_model.BasicConsume(
            //consumer: _consumer,
            //queue: replyQueueName,
            //autoAck: true);

            var test = respQueue.Take();
        }
    }
}