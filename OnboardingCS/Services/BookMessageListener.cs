using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.Text;

namespace OnboardingCS.Services
{
    public class TodoItemMessageListener : IHostedService, IDisposable
    {
        private readonly EventProcessorClient? processor;

        private readonly ILogger? _logger;

        public TodoItemMessageListener(IConfiguration configuration, ILogger<TodoItemMessageListener> logger){
            _logger = logger;
            
            string topic = configuration.GetValue<string>("EventHub:EventHubNameTest");
            string azureContainername = configuration.GetValue<string>("AzureStorage:AzureContainerEventHubTest");
            string eventHubConn = configuration.GetValue<string>("EventHub:ConnectionString");
            string azStorageConn = configuration.GetValue<string>("AzureStorage:ConnectionString");
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            BlobContainerClient storageClient = new BlobContainerClient(azStorageConn, azureContainername);

            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubConn, topic);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;
        }

        public async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            /* handle logic consume data here*/
            _logger.LogInformation(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        public async Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            /* handle error here*/
            _logger.LogError(eventArgs.Exception.Message);
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await processor.StartProcessingAsync(); //Ngejalanin Processornya si EventHub 
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await processor.StopProcessingAsync();
        }

        #region IDisposable Support 
        //TODO jadinya disposable ini ga kepake ya? soalnya udah ada stopAsync, terus kenapa harus implement ID

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                // Harusnya hapus listener ya?
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MessageListernerService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}