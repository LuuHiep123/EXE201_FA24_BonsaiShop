using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.RequestModel.Category
{
    public class UpdateCategoryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int EcologicalCharacteristicsId { get; set; }
    }
}
