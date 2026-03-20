using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;
    public BlobStorageService(string connStr)
    {
        String containerName = "images2";
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10493000;AccountKey=chD7bTOl90fdP83GDesYvlwHS7t43Z7NonGIu2yH0kj6pWMToG8oGnVtSPFk8zuNzfwvek8cLQCs+AStzqfUFw==;EndpointSuffix=core.windows.net";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

    }

 
    public async Task UploadFileAsync(string containerName, string blobName, Stream content)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, overwrite: true);
    }

    public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }

    public async Task<List<string>> ListImageUrlsAsync(string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var imageUrls = new List<string>();
        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            imageUrls.Add(blobClient.Uri.ToString());
        }

        return imageUrls;
    }

    public async Task DeleteFileAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
