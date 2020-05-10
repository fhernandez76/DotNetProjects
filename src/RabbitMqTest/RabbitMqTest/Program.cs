using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMethod3();
        }

        internal static void TestMethod1()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://developer:developer@localhost:5672/vh-test");

                Console.WriteLine("Connecting...");
                IConnection conn = factory.CreateConnection();
                Console.WriteLine("Connected");

                IModel model = conn.CreateModel();

                var exchangeName = "console-test-exchange";
                var queueName = "console-test-queue";
                var consoleTestRoutingKey = "console-test-routing-key";

                model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                model.QueueDeclare(queueName, false, false, false, null);

                byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello World!");
                model.BasicPublish(exchangeName, consoleTestRoutingKey, null, messageBodyBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal static void TestMethod2()
        {
            const string virtualHostName = "vh-test";
            const string exchangeName = "vhtest-ex-test";
            const string routingKey = "vhtest-direct-rk";
            //const string queueName = "vhtestQueue";

            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "developer",
                    Password = "developer",
                    Port = 5672,
                    RequestedConnectionTimeout = 3000,
                    VirtualHost = virtualHostName,
                };

                using (var rabbitConnection = connectionFactory.CreateConnection())
                {
                    using (var channel = rabbitConnection.CreateModel())
                    {
                        string body = $"Payload message content";
                        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(body));
                        Console.WriteLine("Message sent");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();
        }

        internal static void TestMethod3()
        {
            const string virtualHostName = "vh-centos-dev";
            const string exchangeName = "vhcentos-ex-test";
            const string routingKey = "vh-centos-test-rk";
            bool exit = false;
            //const string queueName = "vhtestQueue";

            try
            {
                while (!exit)
                {
                    Console.WriteLine("Please enter payload message content: ");
                    string message = Console.ReadLine();

                    var connectionFactory = new ConnectionFactory()
                    {
                        HostName = "172.17.60.37",
                        UserName = "developer",
                        Password = "developer",
                        Port = 5672,
                        RequestedConnectionTimeout = 3000,
                        VirtualHost = virtualHostName,
                    };

                    using (var rabbitConnection = connectionFactory.CreateConnection())
                    {
                        using (var channel = rabbitConnection.CreateModel())
                        {
                            //string body = $"Payload message content";
                            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(message));
                            Console.WriteLine("Message sent");
                        }
                    }

                    Console.WriteLine("Another message? [Y/N]");
                    string response = Console.ReadLine();
                    if (response.ToUpper().Equals("Y")) { exit = false; }
                    else { exit = true;  }
                }
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();
        }
    }
}
