﻿using CollectionsManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Services
{
    public class FileService
    {
        private const string COLLECTION_DATA_DELIM = ";;";
        private const string COLLECTION_ITEMS_DELIM = ",,";
        private const string ITEM_DATA_DELIM = ";*;";
        private const string ITEM_COLUMNS_DELIM = "$$";
        private const string COLUMN_DATA_DELIM = ",*,";
        private const string PICKER_COLUMN_OPTIONS_DELIM = "$*$";

        private readonly string _baseFilePath = Path.Combine(FileSystem.AppDataDirectory, "collections.txt");

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
                foreach (var collection in collections)
                {
                    string stringified_collection = "";
                    stringified_collection += collection.Id.ToString() + COLLECTION_DATA_DELIM;
                    stringified_collection += collection.Name + COLLECTION_DATA_DELIM;
                    stringified_collection += collection.CreationDate.ToString() + COLLECTION_DATA_DELIM;

                    string stringified_collection_items = "[";
                    foreach (var item in collection.Items)
                    {
                        string stringified_item = "";
                        stringified_item += item.Id.ToString() + ITEM_DATA_DELIM;
                        stringified_item += item.Name + ITEM_DATA_DELIM;
                        stringified_item += item.Image + ITEM_DATA_DELIM;
                        stringified_item += item.AddDate.ToString() + ITEM_DATA_DELIM;

                        if (item.TextColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.TextColumns.Count; i++)
                            {
                                var column = item.TextColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value;
                                stringified_column += i < item.TextColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]" + ITEM_DATA_DELIM;
                        }

                        if (item.NumberColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.NumberColumns.Count; i++)
                            {
                                var column = item.NumberColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value;
                                stringified_column += i < item.NumberColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]" + ITEM_DATA_DELIM;
                        }

                        if (item.PickerColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.PickerColumns.Count; i++)
                            {
                                var column = item.PickerColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value + COLUMN_DATA_DELIM;

                                stringified_column += "[";
                                for (int j = 0; j < column.Options.Count; j++)
                                {
                                    stringified_column += column.Options[j];
                                    stringified_column += j < column.Options.Count - 1 ? PICKER_COLUMN_OPTIONS_DELIM : "";
                                }
                                stringified_column += "]";

                                stringified_column += i < item.PickerColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]";
                        }

                        stringified_collection_items += stringified_item + COLLECTION_ITEMS_DELIM;
                    }

                    stringified_collection += stringified_collection_items + "]";

#if DEBUG
                    Debug.WriteLine($"Stringified collection: {stringified_collection}");
#endif

                    sr.WriteLine(stringified_collection);
                }
            }
        }

        public void SaveDataTo(List<ItemsCollection> collections, string path)
        {
            using (StreamWriter sr = new StreamWriter(path, false))
            {
                foreach (var collection in collections)
                {
                    string stringified_collection = "";
                    stringified_collection += collection.Id.ToString() + COLLECTION_DATA_DELIM;
                    stringified_collection += collection.Name + COLLECTION_DATA_DELIM;
                    stringified_collection += collection.CreationDate.ToString() + COLLECTION_DATA_DELIM;

                    string stringified_collection_items = "[";
                    foreach (var item in collection.Items)
                    {
                        string stringified_item = "";
                        stringified_item += item.Id.ToString() + ITEM_DATA_DELIM;
                        stringified_item += item.Name + ITEM_DATA_DELIM;
                        stringified_item += item.Image + ITEM_DATA_DELIM;
                        stringified_item += item.AddDate.ToString() + ITEM_DATA_DELIM;

                        if (item.TextColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.TextColumns.Count; i++)
                            {
                                var column = item.TextColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value;
                                stringified_column += i < item.TextColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]" + ITEM_DATA_DELIM;
                        }

                        if (item.NumberColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.NumberColumns.Count; i++)
                            {
                                var column = item.NumberColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value;
                                stringified_column += i < item.NumberColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]" + ITEM_DATA_DELIM;
                        }

                        if (item.PickerColumns.Count > 0)
                        {
                            stringified_item += "[";

                            for (int i = 0; i < item.PickerColumns.Count; i++)
                            {
                                var column = item.PickerColumns[i];
                                string stringified_column = column.Name + COLUMN_DATA_DELIM + column.Value + COLUMN_DATA_DELIM;

                                stringified_column += "[";
                                for (int j = 0; j < column.Options.Count; j++)
                                {
                                    stringified_column += column.Options[j];
                                    stringified_column += j < column.Options.Count - 1 ? PICKER_COLUMN_OPTIONS_DELIM : "";
                                }
                                stringified_column += "]";

                                stringified_column += i < item.PickerColumns.Count - 1 ? ITEM_COLUMNS_DELIM : "";
                                stringified_item += stringified_column;
                            }

                            stringified_item += "]" + ITEM_DATA_DELIM;
                        }
                        else
                        {
                            stringified_item += "[]";
                        }

                        stringified_collection_items += stringified_item + COLLECTION_ITEMS_DELIM;
                    }

                    stringified_collection += stringified_collection_items + "]";

#if DEBUG
                    Debug.WriteLine($"Stringified collection: {stringified_collection}");
#endif

                    sr.WriteLine(stringified_collection);
                }
            }
        }

        public void LoadData()
        {
            using (StreamReader sr = new StreamReader(_baseFilePath))
            {
                try
                {
                    int collections_count = 0;
                    List<ItemsCollection> collections = new List<ItemsCollection>();
                    while (!sr.EndOfStream)
                    {
                        collections_count++;
                        string collection_string = sr.ReadLine();
                        string[] collection_parts = collection_string.Split(COLLECTION_DATA_DELIM);
                        if (collection_parts.Length != 4)
                        {
                            throw new Exception($"Malformed collection {collections_count}");
                        }

                        string[] collection_items = collection_parts[3].Substring(1, collection_parts[3].Length - 1).Split(COLLECTION_ITEMS_DELIM);

                        int items_count = 0;
                        List<Item> items = new List<Item>();
                        foreach (var item in collection_items)
                        {
                            items_count++;
                            string[] item_parts = item.Split(ITEM_DATA_DELIM);
                            if (item_parts.Length != 7)
                            {
                                throw new Exception($"Malformed item {items_count} in collection {collections_count}");
                            }

                            List<TextColumn> text_columns = new List<TextColumn>();
                            string[] text_columns_strings = item_parts[3].Substring(1, item_parts[3].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int text_columns_count = 0;
                            foreach (var text_column in text_columns_strings)
                            {
                                text_columns_count++;
                                string[] text_column_parts = text_column.Split(COLUMN_DATA_DELIM);
                                if (text_column_parts.Length != 2)
                                {
                                    throw new Exception($"Malformed column {text_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                text_columns.Add(new TextColumn(text_column_parts[0])
                                {
                                    Value = text_column_parts[1]
                                });
                            }


                            List<NumberColumn> number_columns = new List<NumberColumn>();
                            string[] number_columns_strings = item_parts[4].Substring(1, item_parts[4].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int number_columns_count = text_columns_count;
                            foreach (var number_column in number_columns_strings)
                            {
                                number_columns_count++;
                                string[] number_column_parts = number_column.Split(COLUMN_DATA_DELIM);
                                if (number_column_parts.Length != 2)
                                {
                                    throw new Exception($"Malformed column {number_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                number_columns.Add(new NumberColumn(number_column_parts[0])
                                {
                                    Value = Convert.ToDouble(number_column_parts[1])
                                });
                            }


                            List<PickerColumn> picker_columns = new List<PickerColumn>();
                            string[] picker_columns_strings = item_parts[5].Substring(1, item_parts[5].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int picker_columns_count = number_columns_count;
                            foreach (var picker_column in picker_columns_strings)
                            {
                                picker_columns_count++;
                                string[] picker_column_parts = picker_column.Split(COLUMN_DATA_DELIM);
                                if (picker_column_parts.Length != 3)
                                {
                                    throw new Exception($"Malformed column {picker_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                string[] picker_column_options_strings = picker_column_parts[2].Substring(1, picker_column_parts[2].Length - 1).Split(PICKER_COLUMN_OPTIONS_DELIM);

                                picker_columns.Add(new PickerColumn(picker_column_parts[0], picker_column_options_strings.ToList())
                                {
                                    Value = picker_column_parts[1],
                                }
                                );
                            }

                            items.Add(new Item(Guid.Parse(item_parts[0]), item_parts[1], item_parts[2], text_columns, number_columns, picker_columns, Convert.ToDateTime(item_parts[3])));
                        }
                        collections.Add(new ItemsCollection(Guid.Parse(collection_parts[0]), collection_parts[1], items, Convert.ToDateTime(collection_parts[2])));
                    }

                    OnFilesLoaded(collections);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                    OnLoadingError(ex);
                }
            }
        }

        public void LoadDataFrom(string path)
        {
            using (StreamReader sr = new StreamReader(_baseFilePath))
            {
                try
                {
                    int collections_count = 0;
                    List<ItemsCollection> collections = new List<ItemsCollection>();
                    while (!sr.EndOfStream)
                    {
                        collections_count++;
                        string collection_string = sr.ReadLine();
                        string[] collection_parts = collection_string.Split(COLLECTION_DATA_DELIM);
                        if (collection_parts.Length != 4)
                        {
                            throw new Exception($"Malformed collection {collections_count}");
                        }

                        string[] collection_items = collection_parts[3].Substring(1, collection_parts[3].Length - 1).Split(COLLECTION_ITEMS_DELIM);

                        int items_count = 0;
                        List<Item> items = new List<Item>();
                        foreach (var item in collection_items)
                        {
                            items_count++;
                            string[] item_parts = item.Split(ITEM_DATA_DELIM);
                            if (item_parts.Length != 7)
                            {
                                throw new Exception($"Malformed item {items_count} in collection {collections_count}");
                            }

                            List<TextColumn> text_columns = new List<TextColumn>();
                            string[] text_columns_strings = item_parts[3].Substring(1, item_parts[3].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int text_columns_count = 0;
                            foreach (var text_column in text_columns_strings)
                            {
                                text_columns_count++;
                                string[] text_column_parts = text_column.Split(COLUMN_DATA_DELIM);
                                if (text_column_parts.Length != 2)
                                {
                                    throw new Exception($"Malformed column {text_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                text_columns.Add(new TextColumn(text_column_parts[0])
                                {
                                    Value = text_column_parts[1]
                                });
                            }


                            List<NumberColumn> number_columns = new List<NumberColumn>();
                            string[] number_columns_strings = item_parts[4].Substring(1, item_parts[4].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int number_columns_count = text_columns_count;
                            foreach (var number_column in number_columns_strings)
                            {
                                number_columns_count++;
                                string[] number_column_parts = number_column.Split(COLUMN_DATA_DELIM);
                                if (number_column_parts.Length != 2)
                                {
                                    throw new Exception($"Malformed column {number_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                number_columns.Add(new NumberColumn(number_column_parts[0])
                                {
                                    Value = Convert.ToDouble(number_column_parts[1])
                                });
                            }


                            List<PickerColumn> picker_columns = new List<PickerColumn>();
                            string[] picker_columns_strings = item_parts[5].Substring(1, item_parts[5].Length - 1).Split(ITEM_COLUMNS_DELIM);
                            int picker_columns_count = number_columns_count;
                            foreach (var picker_column in picker_columns_strings)
                            {
                                picker_columns_count++;
                                string[] picker_column_parts = picker_column.Split(COLUMN_DATA_DELIM);
                                if (picker_column_parts.Length != 3)
                                {
                                    throw new Exception($"Malformed column {picker_columns_count} in item {items_count} in collection {collections_count}");
                                }

                                string[] picker_column_options_strings = picker_column_parts[2].Substring(1, picker_column_parts[2].Length - 1).Split(PICKER_COLUMN_OPTIONS_DELIM);

                                picker_columns.Add(new PickerColumn(picker_column_parts[0], picker_column_options_strings.ToList())
                                {
                                    Value = picker_column_parts[1],
                                }
                                );
                            }

                            items.Add(new Item(Guid.Parse(item_parts[0]), item_parts[1], item_parts[2], text_columns, number_columns, picker_columns, Convert.ToDateTime(item_parts[3])));
                        }
                        collections.Add(new ItemsCollection(Guid.Parse(collection_parts[0]), collection_parts[1], items, Convert.ToDateTime(collection_parts[2])));
                    }

                    OnFilesLoaded(collections);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                    OnLoadingError(ex);
                }
            }
        }
    }
}