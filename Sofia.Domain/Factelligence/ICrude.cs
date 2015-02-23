using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Factelligence
{
    public interface ICrude
    {
        string Insert();
        string Update();
        string Delete();
    }
}
