using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.ResponseModel.Category;

namespace BussinessLayer.ResponseModel.EcologicalCharacteristic
{
    public class EcologicalCharacteristicResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public bool Status { get; set; }
        public List<CategoryResponseModel> listCategory { get; set; }
    }
}
