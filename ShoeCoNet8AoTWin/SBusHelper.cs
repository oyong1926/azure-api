using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;


namespace SBusNet8AoTWin
{
    public class SBusHelper
    {
        // Connection string to your Service Bus namespace
        private const string connectionString = "Endpoint=sb://sunsbstudent.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9XbWvhJ/l+03GiGHld4dBbUY7GO4RS0Db+ASbCXwE8I=";

        // Name of the topic
        private const string topicName = "primarystudents";

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

