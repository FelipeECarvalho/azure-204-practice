using Azure.Storage.Blobs;

var blobServiceClient = new BlobServiceClient("");
var blobClientContainer = blobServiceClient.GetBlobContainerClient("containertest");

await blobClientContainer.CreateIfNotExistsAsync();

Console.WriteLine($"Current Policy {blobClientContainer.GetAccessPolicy().Value}");

blobClientContainer.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

Console.WriteLine($"New Policy {blobClientContainer.GetAccessPolicy().Value}");

Stream fs = File.OpenRead(@"C:\\Users\\felip\\OneDrive\\Documentos\\EstudosAzure\\Lab-03-AccountStorage\\AzureBlobTests\\test.txt");

await blobClientContainer.UploadBlobAsync($"myfile{DateTime.Now.Second}", fs);

var itemL = blobClientContainer.GetBlobs();

foreach (var item in itemL)
{
    var blobClient = blobClientContainer.GetBlobClient(item.Name);
    var result = blobClient.DownloadContent().Value.Content.ToStream();

    using var fileStream = new FileStream(@$"C:\\Users\\felip\\OneDrive\\Documentos\\EstudosAzure\\Lab-03-AccountStorage\\AzureBlobTests\\test{DateTime.Now.Second}.txt", FileMode.Create, FileAccess.Write);

    result.CopyTo(fileStream);
}

blobServiceClient.DeleteBlobContainer("containertest");