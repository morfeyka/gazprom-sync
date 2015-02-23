using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Interface
{
    public class ProductContainer:DataImported
    {
        public ProductContainer()
            : base(TypeData.Product)
        {
        }

        public virtual Order.Production Production { get; set; }
    }
}
