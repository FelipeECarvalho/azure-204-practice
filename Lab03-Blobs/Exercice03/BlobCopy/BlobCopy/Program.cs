using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace BlobCopy
{
    internal class Program
    {
        private const string ConnectionString = "";

        static async void Main(string[] args)
        {
            var sourceContainer = "firstcontainer";
            var destContainer = "secondcontainer";
            var sourceFile = "file1.txt";
            var destFile = "file1_copy.txt";

            await BlobCopy.Copy(ConnectionString, sourceContainer, destContainer, sourceFile, destFile);

            var sourceClient = new BlockBlobClient(ConnectionString, sourceContainer, sourceFile);
            BlobProperties properties = await sourceClient.GetPropertiesAsync();

            Console.WriteLine("The tier of the source blob is: " + properties.AccessTier);
            
            var metaData = new Dictionary<string, string>
            {
                {"CreatedBy", "Felipe"},
                {"CreateDate", DateTime.Now.ToShortDateString() }
            };

            await sourceClient.SetMetadataAsync(metaData);
        }
    }
}
