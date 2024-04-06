using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class NumberColumn : BaseItemColumn<double>
    {
        public NumberColumn(string name) : base(name) 
        {
            Name = name;
        }
    }
}
