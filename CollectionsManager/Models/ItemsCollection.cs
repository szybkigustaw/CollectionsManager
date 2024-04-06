using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class ItemsCollection
    {
        private Guid id;
        private string name;
        private List<Item> items;
        private DateTime creation_date;
        private DateTime modification_date;

        public Guid Id { get { return id; } private set { id = value; modification_date = DateTime.Now;  } }
        public string Name { get { return name; } set { name = value; modification_date = DateTime.Now;  } }
        public List<Item> Items { get {  return items; } set { items = value; modification_date = DateTime.Now; } }
        public DateTime CreationDate { get { return creation_date; } private set { creation_date = value; } }
        public DateTime ModificationDate { get { return modification_date; } set { modification_date = value; } }

        public ItemsCollection(string name, List<Item> items)
        {
            Id = Guid.NewGuid();
            Name = name;
            Items = items;
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        public ItemsCollection(Guid id, string name, List<Item> items, DateTime creation_date) : this(name, items) 
        {
            Id = id;
            Name = name;
            Items = items;
            CreationDate = creation_date;
            ModificationDate = DateTime.Now;
        }
    }
}
