using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class PickerColumn : BaseItemColumn<string>
    {
        private List<string> options;

        public List<string> Options { get { return options; } set { options = value; } }

        public PickerColumn(string name, List<string> options) : base(name) 
        {
            Name = name;
            Options = options;
        }
    }
}
