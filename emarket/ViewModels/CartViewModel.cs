using emarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace emarket.ViewModels
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public string AddedAt { get; set; }
        public Models.Product Product { get; set; }
    }
}