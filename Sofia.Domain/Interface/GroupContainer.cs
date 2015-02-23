using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sofia.Domain.Inventory;

namespace Sofia.Domain.Interface
{
    public class GroupContainer:DataImported
    {
        public GroupContainer(): base(TypeData.GroupInventory)
        {
        }

        public virtual string Start()
        {
            switch (Action)
            {
                case Action.Delete:
                    return Group.Delete();
                case Action.Insert:
                    return Group.Insert();
                case Action.Update:
                    return Group.Update();
                default:
                    return "Неизвестное действие";
            }
        }

        public virtual Group Group { get; set; }
    }
}
