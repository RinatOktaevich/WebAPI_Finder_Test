using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebAPI_Finder_Test.Models.Helpers
{
    public class AzureHelper
    {
        static string azurePath = "C:/Users/Ринат/documents/visual studio 2015/Projects/CopyDataSkitelDBToAzure/CopyDataSkitelDBToAzure/Data/";

        static CloudStorageAccount storageAccount;
        static CloudBlobClient blobClient;
        CloudBlobContainer container;
        public AzureHelper(string containerName)
        {
            container = blobClient.GetContainerReference(containerName);

            try
            {
                BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
                container.CreateIfNotExists(requestOptions, null);
            }
            catch (StorageException)
            {
                throw;
            }
        }

        static AzureHelper()
        {
            // Retrieve storage account information from connection string
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            blobClient = storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// This method uploud file from stream to azure blob container
        /// </summary>
        /// <param name="blobName">new name for blob,where it will be.AzurePath already start link name</param>
        /// <param name="source">Input stream to file</param>
        /// <returns>Absolute HTTP link to file</returns>
        public string UploudToContainer(string blobName, HttpPostedFile file)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(azurePath + blobName);
            blockBlob.Properties.ContentType = file.ContentType;
            blockBlob.UploadFromStream(file.InputStream);
            return blockBlob.Uri.AbsoluteUri;
        }
        /// <summary>
        /// Deletes file from container
        /// </summary>
        /// <returns></returns>
        public bool DeleteFromContainer(string path, string fileName)
        {
            CloudBlobDirectory virtualPath = container.GetDirectoryReference(azurePath + path);
            CloudBlockBlob oldBlockBlob = virtualPath.GetBlockBlobReference(fileName);
            return oldBlockBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
        }

    }
}