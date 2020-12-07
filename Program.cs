using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Azure.Storage.Blobs;

namespace AzureStorageAccountSample
{
    class Program
    {
        static BlobContainerClient container;
        static void Main(string[] args)
        {
            //get connection string from configuration file.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json");

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("StorageAccountConnectionString");

            //create bobcointainer where container is used for photos.
            string containerName = "photos";
            container = new BlobContainerClient(connectionString, containerName);
            container.CreateIfNotExists();

            //upload image to blob container
            string blobName = "docs-and-friends-selfie-stick";
            string fileName = "docs-and-friends-selfie-stick.png";
            UploadResourceToBobContainer(blobName, fileName);

            //list the blob items names
            ListBlobFiles();
        }

        private static void UploadResourceToBobContainer(string blobName, string fileName)
        {
            BlobClient blobClient = container.GetBlobClient(blobName);
            blobClient.Upload(fileName, true);
        }

        private static void ListBlobFiles(){
            var blobs = container.GetBlobs();
            foreach (var blob in blobs)
            {
                Console.WriteLine($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:YYYY-MM-dd HH:mm:ss}  Size: {blob.Properties.ContentLength}");
            }
        }
    }
        
}
