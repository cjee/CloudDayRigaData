using NoSqlStore.Models.Store;
using NoSqlStore.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NoSqlStore.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View(DocumentDBRepository<Product>.GetList( x => true));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product data)
        {
            if (!ModelState.IsValid)
                return View(data);

            var image = Request.Files["image"];
            if(image != null)
            {
               var blob = BlobRepository.Upload(image.InputStream, 
                   Path.GetExtension(image.FileName), image.ContentType);
               data.ImageUrl = blob.Uri.ToString();
            }

            await DocumentDBRepository<Product>.CreateItemAsync(data);

            InformClients(data);

            return RedirectToAction("Index");
        }

        private void InformClients(Product data)
        {
            var clientEmails = new List<string>
            {
                "edgars@outlook.com"
            };

            clientEmails.ForEach(x =>
                {
                    var email = GetProductEmail(data, x);
                    var guid = StorageTableRepository.InsertOrUpdateEmail(email, "ProductEmail");
                    StorageQueueRepository.Insert(guid);
                });
            
        }

        private Email GetProductEmail(Product data, string email)
        {
            return new Email
            {
                To = email,
                Subject = string.Format("New product in store: {0}!", data.Name),
                Body = string.Format("New product in store: {0}! More info in store.", data.Name),
                Inserted = DateTime.Now,
            };
        }

        public ActionResult Edit(string id)
        {
            var data = DocumentDBRepository<Product>.GetItem( x=> x.id == id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product data)
        {
            if (!ModelState.IsValid)
                return View(data);

            var image = Request.Files["image"];
            if (image != null && image.ContentLength > 0)
            {
                var blob = BlobRepository.Update(data.ImageUrl, image.InputStream, image.ContentType);
            }

            await DocumentDBRepository<Product>.UpdateItemAsync(data.id, data);

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            var data = DocumentDBRepository<Product>.GetItem( x=> x.id == id);
            return View(data);
        }


        public ActionResult Delete(string id)
        {
            var data = DocumentDBRepository<Product>.GetItem( x=> x.id == id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeletePost(string id)
        {


            await DocumentDBRepository<Product>.Delete(id);

            return RedirectToAction("Index");
        }

    }
}