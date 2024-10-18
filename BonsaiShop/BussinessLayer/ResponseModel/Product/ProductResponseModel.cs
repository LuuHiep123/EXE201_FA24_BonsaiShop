using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.ResponseModel.Product
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int CountSell { get; set; }
        public string Description { get; set; }
        public string Ingredient { get; set; }
        public string UserManual { get; set; }
        public string WarrantyPolicy { get; set; }
        public string Story { get; set; }
        public int? Score { get; set; }
        public string UrlImg { get; set; }
        public bool Status { get; set; }
    }
}
