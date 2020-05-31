using emarket.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace emarket.ViewModels.Product
{
    public class ProductViewModel
    {
        public Models.Product Product { get; set; }
        public List<Models.Product> Products { get; set; }
        public string CategoryName { get; set; }
        public ProductFilter Filter { get; set; }
    }
}