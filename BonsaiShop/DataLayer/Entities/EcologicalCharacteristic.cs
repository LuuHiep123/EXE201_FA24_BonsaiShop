using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class EcologicalCharacteristic
    {
        public EcologicalCharacteristic()
        {
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
