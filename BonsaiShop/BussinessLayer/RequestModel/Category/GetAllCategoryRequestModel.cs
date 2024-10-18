using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.RequestModel.Category
{
    public class GetAllCategoryRequestModel
    {
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 1;
        public string? Name { get; set; }
        public bool? status { get; set; }
    }
}
