

using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnboardingCS.DTO;
using OnboardingCS.Interface;

namespace OnboardingCS.Services
{
    public class TodoItemService : ITodoItemService
    {
       public async Task SendTodoItemToEventHub(TodoItemDTO todoItem, IConfiguration _config)
        {
            
            string connString = _config.GetValue<string>("EventHub:ConnectionString");
            string topic = _config.GetValue<string>("EventHub:EventHubNameTest");

            //create event hub producer
            await using var publisher = new EventHubProducerClient(connString, topic);

            //create batch
            using var eventBatch = await publisher.CreateBatchAsync();

            //add message, ini bisa banyak sekaligus
            var message = JsonConvert.SerializeObject(todoItem);
            eventBatch.TryAdd(new EventData(new BinaryData(message)));

            //send message
            await publisher.SendAsync(eventBatch);
        }

    }
}
