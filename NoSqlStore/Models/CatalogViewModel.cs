using NoSqlStore.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSqlStore.Models
{
    public class CatalogViewModel
    {
        public SearchParams SearchParams { get; set; }

        public List<Product> Products { get; set; }

        public long? TotalCount { get; set; }

        public List<string> Categories { get; set; }
    }
}