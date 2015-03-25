using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoSqlStore.Models.Store
{
    public class Product
    {
        public string id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Categorie { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(typeof(decimal), "0,01", "10000000", ErrorMessage = "Enter decimal value")]
        [RegularExpression(@"^\d+(,\d{1,2})?$", ErrorMessage = "Enter decimal value of format 9.99")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

    }
}