using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class OrderActivity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public virtual User Order { get; set; }
    }
}
