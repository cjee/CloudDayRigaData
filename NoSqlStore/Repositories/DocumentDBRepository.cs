using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace NoSqlStore.Repositories
{
    public static class DocumentDBRepository<T>
    {
        //wrong practice for production code
        private static string EndpointUrl = "https://<SERVICE-NAME>.documents.azure.com:443/";
        private static string AuthorizationKey = "<API-KEY";

        private const string DatabaseName = "StoreDatabase";

        private static DocumentClient _client;
        private static DocumentClient Client
        {
            get
            {
                if(_client == null)
                    _client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
                return _client;
            }
        }

        private static Database _database;

        private static Database Database
        {
            get
            {
            if(_database == null)
            {
                _database = Client.CreateDatabaseQuery().Where( x=> x.Id == DatabaseName).AsEnumerable().FirstOrDefault();
                if(_database == null)
                    _database = Client.CreateDatabaseAsync(new Database { Id = DatabaseName }).Result;
            }
            return _database;
            }
        }


        private static DocumentCollection _collection;

        private static DocumentCollection Collection
        {
            get
            {
                if(_collection == null)
                {
                    _collection = Client
                        .CreateDocumentCollectionQuery(Database.SelfLink)
                        .Where( x=> x.Id == typeof(T).Name)
                        .AsEnumerable()
                        .FirstOrDefault();

                    if(_collection == null)
                        _collection = Client.
                            CreateDocumentCollectionAsync(Database.SelfLink, 
                            new DocumentCollection {Id = typeof(T).Name }).Result;
                }
                return _collection;
            }

        }

        public static IEnumerable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            return Client.CreateDocumentQuery<T>(Collection.DocumentsLink).Where(predicate).AsEnumerable();
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await Client.CreateDocumentAsync(Collection.SelfLink, item);
        }

        public static T GetItem(Expression<Func<T, bool>> predicate)
        {
            return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable()
                        .FirstOrDefault();
        }

        public static Document GetDocument(string id)
        {
            return Client.CreateDocumentQuery(Collection.DocumentsLink)
                .Where(d => d.Id == id)
                .AsEnumerable()
                .FirstOrDefault();
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            Document doc = GetDocument(id);
            return await Client.ReplaceDocumentAsync(doc.SelfLink, item);
        }

        public static async Task<Document> Delete(string id)
        {
            Document doc = GetDocument(id);
            return await Client.DeleteDocumentAsync(doc.SelfLink);
        }

    }
}