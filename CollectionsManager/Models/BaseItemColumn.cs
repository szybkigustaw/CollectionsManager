using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public abstract class BaseItemColumn<T>
    {
        private Guid id;
        private string name;
        private T? value;

        public string Name { get { return name; } set { name = value; } }
        public T Value { get { return value; } set { this.value = value; } }
        public Guid Id { get { return id; } set { id = value; } }

        public BaseItemColumn(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Value = default(T);
        }

        public BaseItemColumn(Guid id, string name, T value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public BaseItemColumn(Guid id, string name)
        {
            Id = id;
            Name = name;
            Value = default(T);
        }
    }
}
