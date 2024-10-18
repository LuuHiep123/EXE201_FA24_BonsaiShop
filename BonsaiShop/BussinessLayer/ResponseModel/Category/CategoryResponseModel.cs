using BussinessLayer.ResponseModel.Product;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.ResponseModel.Category
{
    public class CategoryResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int EcologicalCharacteristicsId { get; set; }
        public string EcologicalCharacteristicsName { get; set; }
        public virtual List<ProductResponseModel> listProducts { get; set; }
    }
}
