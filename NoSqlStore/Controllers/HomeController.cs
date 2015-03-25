using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using NoSqlStore.Models;
using NoSqlStore.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoSqlStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string search = "", int page = 0, string category = "")
        {
            var par = new SearchParams
            {
                Search = search,
                Page = page,
                Category = category,
            };

            var searchResult = Search(par);

            var model = new CatalogViewModel();
            model.Products = searchResult.Select(x => x.Document).ToList();
            model.SearchParams = par;
            model.TotalCount = searchResult.Count;
            model.Categories = searchResult.Facets.First().Value.Select( x=> x.Value.ToString()).ToList();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private DocumentSearchResponse<Product> Search(SearchParams par)
        {
            string searchServiceName = "<ACCOUNT-NAME";
            string apiKey = "<ACCOUNT-KEY>";
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            SearchIndexClient indexClient = serviceClient.Indexes.GetClient("productsindex");

            var sp = new SearchParameters()
            {
                IncludeTotalResultCount = true,
                Skip = par.Page * par.PageSize,
                Top = par.PageSize,
                Facets = new List<string>
                {
                    "Categorie",
                },
                Filter = string.IsNullOrEmpty(par.Category) ? null : string.Format("Categorie eq '{0}'", par.Category)
            };


            return indexClient.Documents.Search<Product>(par.Search, sp);
        }
    }
}