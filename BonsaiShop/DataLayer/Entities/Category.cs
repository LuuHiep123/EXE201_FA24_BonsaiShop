using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public int EcologicalCharacteristicsId { get; set; }

        public virtual EcologicalCharacteristic EcologicalCharacteristics { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
