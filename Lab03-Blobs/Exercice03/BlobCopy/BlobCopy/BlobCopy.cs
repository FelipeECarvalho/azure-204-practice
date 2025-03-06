using Azure.Storage.Blobs.Specialized;

namespace BlobCopy
{
    static class BlobCopy
    {
        public static async Task Copy(string connectionString, string sourceContainer, string destContainer, string sourceFile, string destFile)
        {
            var sourceClient = new BlockBlobClient(connectionString, sourceContainer, sourceFile);
            var destinationClient = new BlockBlobClient(connectionString, destContainer, destFile);

            await destinationClient.StartCopyFromUriAsync(sourceClient.Uri);
        }
    }
}
