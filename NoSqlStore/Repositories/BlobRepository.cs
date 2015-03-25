using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NoSqlStore.Repositories
{
    public static class BlobRepository
    {
        const string ContainerName = "images";

        static CloudStorageAccount sa = CloudStorageAccount.Parse
            ("DefaultEndpointsProtocol=https;AccountName=<SERVICE-NAME>;AccountKey=<ACCOUNT-KEY>");

        public static CloudBlobClient bc;

        public static CloudBlobContainer container;

        public static CloudBlockBlob Upload(Stream data, string extension, string contentType)
        {
            EnsureContainer();

            string uniqueBlobName = string.Format("productimages/image_{0}{1}",
                            Guid.NewGuid().ToString(), extension);
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(data);
            return blob;
        }


        public static CloudBlockBlob Update(string name, Stream data, string contentType)
        {
            EnsureContainer();

            CloudBlockBlob blob = container.GetBlockBlobReference(name.Remove(0, 50));
            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(data);
            return blob;
        }

        private static void EnsureContainer()
        {
            if (container == null)
            {
                bc = sa.CreateCloudBlobClient();
                container = bc.GetContainerReference(ContainerName);
                if (container.CreateIfNotExists())
                {
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);
                }
            }
        }
    }
}