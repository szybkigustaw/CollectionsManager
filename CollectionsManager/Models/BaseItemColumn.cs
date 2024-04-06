using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public abstract class BaseItemColumn<T>
    {
        private string name;
        private T? value;

        public string Name { get { return name; } set { name = value; } }
        public T Value { get { return value; } set { this.value = value; } }

        public BaseItemColumn(string name)
        {
            Name = name;
            Value = default(T);
        }
    }
}
