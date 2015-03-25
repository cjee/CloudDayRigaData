using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSqlStore.Models.Store
{
    public class Email : TableEntity
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime Inserted { get; set; }

        public bool Delivered { get; set; }

    }
}