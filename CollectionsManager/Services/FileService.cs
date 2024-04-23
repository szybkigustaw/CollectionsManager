using CollectionsManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CollectionsManager.Services
{
    public class FileService
    {
        private readonly string _baseFilePath = Path.Combine(FileSystem.AppDataDirectory, "collections.xml");
        private char COLUMN_DATA_DELIM = '!';
        private char ITEM_COLUMNS_DELIM = '!';
        private char PICKER_COLUMN_OPTIONS_DELIM = '!';
        private char ITEM_DATA_DELIM = '!';
        private char COLLECTION_ITEMS_DELIM = ' ';
        private char COLLECTION_DELIM = '!';
        private char COLLECTION_DATA_DELIM = '!';

        public event EventHandler<List<ItemsCollection>> FilesLoaded;
        public event EventHandler<Exception> LoadingError;
        
        private void OnFilesLoaded(List<ItemsCollection> loaded_collections)
            => FilesLoaded?.Invoke(this, loaded_collections);

        private void OnLoadingError(Exception exception)
            => LoadingError?.Invoke(this, exception);

        public void SaveData(List<ItemsCollection> collections)
        {
            using (StreamWriter sr = new StreamWriter(_baseFilePath, false))
            {
                XElement collections_doc = new XElement("collections");
                foreach(var collection in collections) 
                {
                    XElement collection_el = new XElement("collection");
                    collection_el.SetAttributeValue("id", collection.Id);
                    collection_el.SetAttributeValue("name", collection.Name);
                    collection_el.SetAttributeValue("creation_date", collection.CreationDate);
                    collection_el.SetAttributeValue("modification_date", collection.ModificationDate);

                    foreach(var item in collection.Items)
                    {
                        XElement item_el = new XElement("item");
                        item_el.SetAttributeValue("id", item.Id);
                        item_el.SetAttributeValue("name", item.Name);
                        item_el.SetAttributeValue("add_date", item.AddDate);
                        item_el.SetAttributeValue("modification_date", item.ModificationDate);

                        item_el.Add(new XElement("item_image", item.Image));

                        XElement text_columns = new XElement("text_columns");
                        foreach(var text_column in  item.TextColumns)
                        {
                            XElement text_column_el = new XElement("text_column");
                            text_column_el.SetAttributeValue("id", text_column.Id);
                            text_column_el.SetAttributeValue("name", text_column.Name);
                            text_column_el.SetAttributeValue("value", text_column.Value);

                            text_columns.Add(text_column_el);
                        }
                        item_el.Add(text_columns);

                        XElement number_columns = new XElement("number_columns");
                        foreach(var number_column in item.NumberColumns)
                        {
                            XElement number_column_el = new XElement("number_column");
                            number_column_el.SetAttributeValue("id", number_column.Id);
                            number_column_el.SetAttributeValue("name", number_column.Name);
                            number_column_el.SetAttributeValue("value", number_column.Value);

                            number_columns.Add(number_column_el);
                        }
                        item_el.Add(number_columns);

                        XElement picker_columns = new XElement("picker_columns");
                        foreach(var picker_column in item.PickerColumns)
                        {
                            XElement picker_column_el = new XElement("picker_column");
                            picker_column_el.SetAttributeValue("id", picker_column.Id);
                            picker_column_el.SetAttributeValue("name", picker_column.Name);

                            XElement picker_column_value_el = new XElement("value");
                            picker_column_value_el.SetAttributeValue("id", picker_column.Value.Id);
                            picker_column_value_el.Add(picker_column.Value.Option);
                            picker_column_el.Add(picker_column_value_el);

                            XElement picker_column_options_el = new XElement("options");
                            foreach(var option in picker_column.Options)
                            {
                                XElement option_el = new XElement("option");
                                option_el.SetAttributeValue("id", option.Id);
                                option_el.Add(option.Option);
                                picker_column_options_el.Add(option_el);
                            }
                            picker_column_el.Add(picker_column_options_el);

                            picker_columns.Add(picker_column_el);
                        }
                        item_el.Add(picker_columns);

                        collection_el.Add(item_el);
                    }

                    collections_doc.Add(collection_el);
                }
#if DEBUG
                Debug.WriteLine($"Saved file path: {_baseFilePath}");
#endif
                collections_doc.Save(sr);
            }
        }

        public void SaveDataTo(List<ItemsCollection> collections, string path)
        {
            using (StreamWriter sr = new StreamWriter(path, false))
            {
                XElement collections_doc = new XElement("collections");
                foreach(var collection in collections) 
                {
                    XElement collection_el = new XElement("collection");
                    collection_el.SetAttributeValue("id", collection.Id);
                    collection_el.SetAttributeValue("name", collection.Name);
                    collection_el.SetAttributeValue("creation_date", collection.CreationDate);
                    collection_el.SetAttributeValue("modification_date", collection.ModificationDate);

                    foreach(var item in collection.Items)
                    {
                        XElement item_el = new XElement("item");
                        item_el.SetAttributeValue("id", item.Id);
                        item_el.SetAttributeValue("name", item.Name);
                        item_el.SetAttributeValue("add_date", item.AddDate);
                        item_el.SetAttributeValue("modification_date", item.ModificationDate);

                        item_el.Add(new XElement("item_image", item.Image));

                        XElement text_columns = new XElement("text_columns");
                        foreach(var text_column in  item.TextColumns)
                        {
                            XElement text_column_el = new XElement("text_column");
                            text_column_el.SetAttributeValue("id", text_column.Id);
                            text_column_el.SetAttributeValue("name", text_column.Name);
                            text_column_el.SetAttributeValue("value", text_column.Value);

                            text_columns.Add(text_column_el);
                        }
                        item_el.Add(text_columns);

                        XElement number_columns = new XElement("number_columns");
                        foreach(var number_column in item.NumberColumns)
                        {
                            XElement number_column_el = new XElement("number_column");
                            number_column_el.SetAttributeValue("id", number_column.Id);
                            number_column_el.SetAttributeValue("name", number_column.Name);
                            number_column_el.SetAttributeValue("value", number_column.Value);

                            number_columns.Add(number_column_el);
                        }
                        item_el.Add(number_columns);

                        XElement picker_columns = new XElement("picker_columns");
                        foreach(var picker_column in item.PickerColumns)
                        {
                            XElement picker_column_el = new XElement("picker_column");
                            picker_column_el.SetAttributeValue("id", picker_column.Id);
                            picker_column_el.SetAttributeValue("name", picker_column.Name);

                            XElement picker_column_value_el = new XElement("value");
                            picker_column_value_el.SetAttributeValue("id", picker_column.Value.Id);
                            picker_column_value_el.Add(picker_column.Value.Option);
                            picker_column_el.Add(picker_column_value_el);

                            XElement picker_column_options_el = new XElement("options");
                            foreach(var option in picker_column.Options)
                            {
                                XElement option_el = new XElement("option");
                                option_el.SetAttributeValue("id", option.Id);
                                option_el.Add(option.Option);
                                picker_column_options_el.Add(option_el);
                            }
                            picker_column_el.Add(picker_column_options_el);

                            picker_columns.Add(picker_column_el);
                        }
                        item_el.Add(picker_columns);

                        collection_el.Add(item_el);
                    }

                    collections_doc.Add(collection_el);
                }
#if DEBUG
                Debug.WriteLine($"Saved file path: {path}");
#endif
                collections_doc.Save(sr);
            }
        }

        public void LoadData()
        {
            try
            {
                XDocument xdoc = XDocument.Load(_baseFilePath);
                if (xdoc.Root == null) throw new InvalidDataException("There's no data in this file!");
                XElement collections_data = xdoc.Root;
                    try
                    {
                        int collections_count = 0;
                        List<ItemsCollection> collections = new List<ItemsCollection>();
                         foreach( XElement collection_data in collections_data.Elements())
                         {
                            collections_count++;
                            if(collection_data.Attribute("id") == null) throw new Exception($"Malformed collection {collections_count}");
                            Guid collection_id = Guid.Parse(collection_data.Attribute("id").Value);
                            if(collection_data.Attribute("name") == null) throw new Exception($"Malformed collection {collections_count}");
                            string collection_name = collection_data.Attribute("name").Value;
                            if(collection_data.Attribute("creation_date") == null) throw new Exception($"Malformed collection {collections_count}");
                            DateTime collection_creation_date = DateTime.Parse(collection_data.Attribute("creation_date").Value);

                            List<Item> collection_items = new List<Item>();
                            int items_count = 0;
                            foreach (XElement item in collection_data.Elements())
                            {
                                items_count++;
                               
                                if(item.Attribute("id") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: id");
                                Guid item_id = Guid.Parse(item.Attribute("id").Value);
                                if(item.Attribute("name") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: name");
                                string item_name = item.Attribute("name").Value;
                                if(item.Attribute("add_date") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: add_date");
                                DateTime item_add_time = DateTime.Parse(item.Attribute("add_date").Value);

                                XElement? item_image = item.Element("item_image");
                                string? image;
                                if(item_image != null && item_image.Value != null)
                                {
                                    image = item_image.Value;
                                }
                                else
                                {
                                    image = null;
                                }

                                List<TextColumn> text_columns = new List<TextColumn>();
                                List<NumberColumn> number_columns = new List<NumberColumn>();
                                List<PickerColumn> picker_columns = new List<PickerColumn>();

                                int columns_count = 0;

                                XElement? text_columns_el = item.Element("text_columns");
                                if(text_columns_el != null)
                                {
                                    foreach(XElement text_column_el in text_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(text_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid text_column_id = Guid.Parse(text_column_el.Attribute("id").Value);
                                        if(text_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: name");
                                        string text_column_name = text_column_el.Attribute("name").Value;
                                        if(text_column_el.Attribute("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        string text_column_value = text_column_el.Attribute("value").Value;

                                        text_columns.Add(new TextColumn(text_column_id, text_column_name, text_column_value));
                                    }
                                }

                                XElement? number_columns_el = item.Element("number_columns");
                                if(number_columns_el != null)
                                {
                                    foreach(XElement number_column_el in number_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(number_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid number_column_id = Guid.Parse(number_column_el.Attribute("id").Value);
                                        if(number_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: name");
                                        string number_column_name = number_column_el.Attribute("name").Value;
                                        if(number_column_el.Attribute("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        double number_column_value = Convert.ToDouble(number_column_el.Attribute("value").Value);

                                        number_columns.Add(new NumberColumn(number_column_id, number_column_name, number_column_value));
                                    }
                                }

                                XElement? picker_columns_el = item.Element("picker_columns");
                                if(picker_columns_el != null)
                                {
                                    foreach(XElement picker_column_el in picker_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(picker_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid picker_column_id = Guid.Parse(picker_column_el.Attribute("id").Value);
                                        if(picker_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        string picker_column_name = picker_column_el.Attribute("name").Value;

                                        if(picker_column_el.Element("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        XElement picker_column_value_el = picker_column_el.Element("value");
                                        if(picker_column_value_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        if(picker_column_value_el.Value == null || picker_column_value_el.Value == String.Empty) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");

                                        PickerColumnOption picker_column_value = new PickerColumnOption(Guid.Parse(picker_column_value_el.Attribute("id").Value), picker_column_value_el.Value);
                                        List<PickerColumnOption> picker_column_options = new List<PickerColumnOption>();

                                        XElement? picker_column_options_el = picker_column_el.Element("options");
                                        if(picker_column_options_el == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: options");
                                        int options_count = 0;
                                        foreach (XElement picker_column_option_el in picker_column_options_el.Elements())
                                        {
                                            options_count++;
                                            if(picker_column_option_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");
                                            if(picker_column_option_el.Value == null || picker_column_value_el.Value == String.Empty) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");

                                            picker_column_options.Add(new PickerColumnOption(Guid.Parse(picker_column_option_el.Attribute("id").Value), picker_column_option_el.Value));
                                        }

                                        if(picker_column_options.Where(o => o.Id == picker_column_value.Id ).Count() == 0) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");

                                        picker_columns.Add(new PickerColumn(picker_column_id, picker_column_name, picker_column_options, picker_column_value));
                                    }
                                }

                                collection_items.Add(new Item(item_id, item_name, image, text_columns, number_columns, picker_columns, item_add_time));
                            }
                            collections.Add(new ItemsCollection(collection_id, collection_name, collection_items, collection_creation_date));
                         }

                        OnFilesLoaded(collections);
#if DEBUG
                        Debug.WriteLine($"Base file path: {_baseFilePath}");
#endif
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                        OnLoadingError(ex);
                    }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"Error occurred during loading data: {ex.Message}");
#endif
            }
        }

        public void LoadDataFrom(string path)
        {
            try
            {
                XDocument xdoc = XDocument.Load(path);
                if (xdoc.Root == null) throw new InvalidDataException("There's no data in this file!");
                XElement collections_data = xdoc.Root;
                    try
                    {
                        int collections_count = 0;
                        List<ItemsCollection> collections = new List<ItemsCollection>();
                         foreach( XElement collection_data in collections_data.Elements())
                         {
                            collections_count++;
                            if(collection_data.Attribute("id") == null) throw new Exception($"Malformed collection {collections_count}");
                            Guid collection_id = Guid.Parse(collection_data.Attribute("id").Value);
                            if(collection_data.Attribute("name") == null) throw new Exception($"Malformed collection {collections_count}");
                            string collection_name = collection_data.Attribute("name").Value;
                            if(collection_data.Attribute("creation_date") == null) throw new Exception($"Malformed collection {collections_count}");
                            DateTime collection_creation_date = DateTime.Parse(collection_data.Attribute("creation_date").Value);

                            List<Item> collection_items = new List<Item>();
                            int items_count = 0;
                            foreach (XElement item in collection_data.Elements())
                            {
                                items_count++;
                               
                                if(item.Attribute("id") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: id");
                                Guid item_id = Guid.Parse(item.Attribute("id").Value);
                                if(item.Attribute("name") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: name");
                                string item_name = item.Attribute("name").Value;
                                if(item.Attribute("add_date") == null) throw new Exception($"Malformed item {items_count} in collection {collections_count}: add_date");
                                DateTime item_add_time = DateTime.Parse(item.Attribute("add_date").Value);

                                XElement? item_image = item.Element("item_image");
                                string? image;
                                if(item_image != null && item_image.Value != null)
                                {
                                    image = item_image.Value;
                                }
                                else
                                {
                                    image = null;
                                }

                                List<TextColumn> text_columns = new List<TextColumn>();
                                List<NumberColumn> number_columns = new List<NumberColumn>();
                                List<PickerColumn> picker_columns = new List<PickerColumn>();

                                int columns_count = 0;

                                XElement? text_columns_el = item.Element("text_columns");
                                if(text_columns_el != null)
                                {
                                    foreach(XElement text_column_el in text_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(text_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid text_column_id = Guid.Parse(text_column_el.Attribute("id").Value);
                                        if(text_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: name");
                                        string text_column_name = text_column_el.Attribute("name").Value;
                                        if(text_column_el.Attribute("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        string text_column_value = text_column_el.Attribute("value").Value;

                                        text_columns.Add(new TextColumn(text_column_id, text_column_name, text_column_value));
                                    }
                                }

                                XElement? number_columns_el = item.Element("number_columns");
                                if(number_columns_el != null)
                                {
                                    foreach(XElement number_column_el in number_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(number_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid number_column_id = Guid.Parse(number_column_el.Attribute("id").Value);
                                        if(number_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: name");
                                        string number_column_name = number_column_el.Attribute("name").Value;
                                        if(number_column_el.Attribute("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        double number_column_value = Convert.ToDouble(number_column_el.Attribute("value").Value);

                                        number_columns.Add(new NumberColumn(number_column_id, number_column_name, number_column_value));
                                    }
                                }

                                XElement? picker_columns_el = item.Element("picker_columns");
                                if(picker_columns_el != null)
                                {
                                    foreach(XElement picker_column_el in picker_columns_el.Elements())
                                    {
                                        columns_count++;
                                        if(picker_column_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        Guid picker_column_id = Guid.Parse(picker_column_el.Attribute("id").Value);
                                        if(picker_column_el.Attribute("name") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: id");
                                        string picker_column_name = picker_column_el.Attribute("name").Value;

                                        if(picker_column_el.Element("value") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        XElement picker_column_value_el = picker_column_el.Element("value");
                                        if(picker_column_value_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");
                                        if(picker_column_value_el.Value == null || picker_column_value_el.Value == String.Empty) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: value");

                                        PickerColumnOption picker_column_value = new PickerColumnOption(Guid.Parse(picker_column_value_el.Attribute("id").Value), picker_column_value_el.Value);
                                        List<PickerColumnOption> picker_column_options = new List<PickerColumnOption>();

                                        XElement? picker_column_options_el = picker_column_el.Element("options");
                                        if(picker_column_options_el == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: options");
                                        int options_count = 0;
                                        foreach (XElement picker_column_option_el in picker_column_options_el.Elements())
                                        {
                                            options_count++;
                                            if(picker_column_option_el.Attribute("id") == null) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");
                                            if(picker_column_option_el.Value == null || picker_column_value_el.Value == String.Empty) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");

                                            picker_column_options.Add(new PickerColumnOption(Guid.Parse(picker_column_option_el.Attribute("id").Value), picker_column_option_el.Value));
                                        }

                                        if(picker_column_options.Where(o => o.Id == picker_column_value.Id ).Count() == 0) throw new Exception($"Malformed column {columns_count} in item {items_count} in collection {collections_count}: option {options_count}");

                                        picker_columns.Add(new PickerColumn(picker_column_id, picker_column_name, picker_column_options, picker_column_value));
                                    }
                                }

                                collection_items.Add(new Item(item_id, item_name, image, text_columns, number_columns, picker_columns, item_add_time));
                            }
                            collections.Add(new ItemsCollection(collection_id, collection_name, collection_items, collection_creation_date));
                         }

                        OnFilesLoaded(collections);
#if DEBUG
                        Debug.WriteLine($"Base file path: {_baseFilePath}");
#endif
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                        OnLoadingError(ex);
                    }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"Error occurred during loading data: {ex.Message}");
#endif
            }
        }
    }
}
