using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NoSqlStore.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSqlStore.Repositories
{
    public static class StorageTableRepository
    {
        const string TableName = "emails";

        static CloudStorageAccount sa = CloudStorageAccount.Parse
            ("DefaultEndpointsProtocol=https;AccountName=<ACCOUNT-NAME>;AccountKey=<ACCOUNT-KEY>");

        public static CloudTableClient tableClient;

        public static CloudTable table;


        public static string InsertOrUpdateEmail(Email data, string emailType, string guid = "")
        {
            EnsureTable();

            string key = guid;
            if(string.IsNullOrEmpty(key))
                key = Guid.NewGuid().ToString();


            data.PartitionKey = emailType;
            data.RowKey = key;

            var operation = TableOperation.InsertOrMerge(data);
            table.Execute(operation);

            return key;
        }

        public static Email GetEmail(string emailType, string guid)
        {
            EnsureTable();

            var operation = TableOperation.Retrieve<Email>(emailType, guid);

			var tableResult = table.Execute(operation);

			return (Email)tableResult.Result;
        }


     

        private static void EnsureTable()
        {
            if (table == null)
            {
                tableClient = sa.CreateCloudTableClient();
                table = tableClient.GetTableReference(TableName);
                table.CreateIfNotExists();
            }
        }

    }
}