using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Interface
{
    public class ItemContainer:DataImported
    {
        private readonly Inventory.Item _item;
        public ItemContainer():base(TypeData.ItemInventory)
        {
        }

        public virtual string Start()
        {
            switch (Action)
            {
                case Action.Delete:
                    return Item.Delete();
                case Action.Insert:
                    return Item.Insert();
                case Action.Update:
                    return Item.Update();
                default:
                    return "Неизвестное действие";
            }
        }

        public virtual Inventory.Item Item { get
        {
            return _item;
        }
        }
    }
}
