using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSqlStore.Models
{
    public class SearchParams
    {
        
        public string Search {get; set;}

        public int PageSize { get { return 4; } }

        public int Page { get; set; }

        public string Category { get; set; }
    }
}