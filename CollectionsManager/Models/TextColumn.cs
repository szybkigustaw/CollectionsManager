using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class TextColumn : BaseItemColumn<string>
    {
        public TextColumn(string name) : base(name)
        {
            Name = name;
        }
    }
}
