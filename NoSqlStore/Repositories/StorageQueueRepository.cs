using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSqlStore.Repositories
{
    public static class StorageQueueRepository
    {
        const string queueName = "emails";

        static CloudStorageAccount sa = CloudStorageAccount.Parse
            ("DefaultEndpointsProtocol=https;AccountName=<ACCOUNT-NAME>;AccountKey=<ACCOUNT-KEY>");

        public static CloudQueueClient queueClient;

        public static CloudQueue queue;


        public static void Insert(string guid)
        {
            EnsureQueue();
            var msg = new CloudQueueMessage(guid);
            queue.AddMessage(msg);
        }


        public static CloudQueueMessage Get()
        {
            EnsureQueue();
            var msg = queue.GetMessage(TimeSpan.FromSeconds(2));
            return msg;
        }

        public static void Delete(CloudQueueMessage msg)
        {
            EnsureQueue();
            queue.DeleteMessage(msg);
        }

        private static void EnsureQueue()
        {
            if (queue == null)
            {
                queueClient = sa.CreateCloudQueueClient();
                queue = queueClient.GetQueueReference(queueName);
                queue.CreateIfNotExists();
            }
        }
    }
}