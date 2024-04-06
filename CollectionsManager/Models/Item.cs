using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class Item
    {
        private Guid id;
        private string name;
        private string image;
        private List<TextColumn> text_columns;
        private List<NumberColumn> number_columns;
        private List<PickerColumn> picker_Columns;
        private DateTime modification_date;
        private DateTime add_date;

        public Guid Id { get { return id; } private set { id = value; modification_date = DateTime.Now; } }
        public string Name { get { return name; } set { name = value; modification_date = DateTime.Now;} }
        public string Image { get { return image; } set { image = value; modification_date = DateTime.Now;} }
        public List<TextColumn> TextColumns { get { return text_columns; } set { text_columns = value; modification_date = DateTime.Now;} }
        public List<NumberColumn> NumberColumns { get { return number_columns; } set { number_columns = value; modification_date = DateTime.Now;}}
        public List<PickerColumn> PickerColumns { get { return picker_Columns; } set { picker_Columns = value; modification_date = DateTime.Now;} }

        public DateTime ModificationDate { get {  return modification_date; } set {  modification_date = value; } }
        public DateTime AddDate { get { return add_date; } private set { add_date = value; } }

        public Item(string name, string image, List<TextColumn> text_column, List<NumberColumn> number_column, List<PickerColumn> pickerColumns)
        {
            Id = Guid.NewGuid();
            Name = name;
            Image = image;
            TextColumns = text_column;
            NumberColumns = number_column;
            PickerColumns = pickerColumns;
            ModificationDate = DateTime.Now;
            AddDate = DateTime.Now;

        }

        public Item(Guid id, string name, string image, List<TextColumn> text_columns, List<NumberColumn> number_columns, List<PickerColumn> picker_columns, DateTime add_date) : this(name, image, text_columns, number_columns, picker_columns) 
        {
            Id = id;
            Name = name;
            Image = image;
            TextColumns = text_columns;
            NumberColumns = number_columns;
            PickerColumns = picker_columns;
            ModificationDate = DateTime.Now;
            AddDate = add_date;
        }
    }
}
