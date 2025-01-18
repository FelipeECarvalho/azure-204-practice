using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlobManager
{
    public class Program
    {
        private const string blobServiceEndpoint = "https://festorageaz204.blob.core.windows.net/";
        private const string storageAccountName = "festorageaz204";
        private const string storageAccountKey = "EjVu0+yziwli/SqCtVn4BS4G6a0Avf8nqJgBdcnH5AoMsymDnX52TuN5viwXEmcai06WFesGnoXl+AStTKmoqw==";

        public static async Task Main(string[] args)
        {
            StorageSharedKeyCredential accountCredentials = new(storageAccountName, storageAccountKey);
            BlobServiceClient serviceClient = new(new Uri(blobServiceEndpoint), accountCredentials);

            AccountInfo info = await serviceClient.GetAccountInfoAsync();

            await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");

            await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");

            await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");

            await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

            await EnumerateContainersAsync(serviceClient);

            var newContainer = await GetContainerAsync(serviceClient, "vector-graphics");
            var blobClient = await GetBlobAsync(newContainer, "graph.svg");
        }

        private static async Task EnumerateContainersAsync(BlobServiceClient client)
        {
            await foreach (BlobContainerItem container in client.GetBlobContainersAsync()) 
            {
                await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
                await EnumerateBlobsAsync(client, container.Name);
            }
        }

        private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
        {
            var containerClient = client.GetBlobContainerClient(containerName);

            await Console.Out.WriteLineAsync($"Searching:\t{containerClient.Name}");

            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
            }
        }

        private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
        {
            var container = client.GetBlobContainerClient(containerName);

            await container.CreateIfNotExistsAsync();

            await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");

            return container;
        }

        private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
        {
            var blobClient = client.GetBlobClient(blobName);
            bool exist = await blobClient.ExistsAsync();

            if (!exist)
                await Console.Out.WriteLineAsync($"Blob {blobClient.Name} not found!");
            else
                await Console.Out.WriteLineAsync($"Blob Found, URI:\t{blobClient.Uri}");

            return blobClient;
        }
    }
}