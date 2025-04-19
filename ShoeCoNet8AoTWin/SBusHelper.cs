using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;


namespace SBusNet8AoTWin
{
    public class SBusHelper
    {
        // Connection string to your Service Bus namespace
        private static string connectionString = string.Empty;
        private const string topicName = "primarystudents";

        internal static void Init(ConfigurationManager config)
        {
            // Ensure the connection string is not null or empty
            connectionString = config.GetConnectionString("SBusConnectionString")
                               ?? throw new InvalidOperationException("Connection string 'SBusConnectionString' is not configured.");
        }

        internal static async Task SendMessageToTopic(string messageBody = "Hello, Service Bus!")
        {
            // Create a ServiceBusClient object
            await using var client = new ServiceBusClient(connectionString);

            // Create a sender for the topic
            ServiceBusSender sender = client.CreateSender(topicName);

            // Create a message to send
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            message.PartitionKey = "student";
            message.SessionId = "S1";
            try
            {
                // Send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Message sent to topic: {messageBody}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }

}

