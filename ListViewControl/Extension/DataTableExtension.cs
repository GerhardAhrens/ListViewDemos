namespace ListViewControl.Extension
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    public static partial class DataTableExtension
    {
        public static DataTable ToGetDataTable<T>(this IEnumerable<T> @this)
        {
            Type entityType = typeof(T);

            if (entityType == typeof(object))
            {
                throw new ArgumentException("Es dürgen keine Typen vom Typ 'Object' übergeben werden.");
            }

            if (entityType == typeof(string))
            {
                DataTable dataTable = new DataTable(entityType.Name);
                dataTable.Columns.Add(entityType.Name);

                // Iterate through each item in the list. There is only one cell, so use index 0 to
                // set the value.
                foreach (T item in @this)
                {
                    DataRow row = dataTable.NewRow();
                    row[0] = item;
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }

            if (entityType.BaseType == typeof(Enum))
            {
                DataTable dataTable = new DataTable(entityType.Name);

                DataColumn columnA = new DataColumn(entityType.Name, typeof(string));
                columnA.AllowDBNull = false;
                dataTable.Columns.Add(columnA);

                DataColumn columnB = new DataColumn($"{entityType.Name}Value", typeof(int));
                columnB.AllowDBNull = false;
                dataTable.Columns.Add(columnB);

                foreach (string namedConstant in Enum.GetNames(entityType))
                {
                    DataRow row = dataTable.NewRow();
                    row[0] = namedConstant;
                    row[1] = (int)Enum.Parse(entityType, namedConstant);
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }

            Type underlyingType = Nullable.GetUnderlyingType(entityType);
            var primitiveTypes = new List<Type>
                                    {
                                        typeof (byte),
                                        typeof (char),
                                        typeof (decimal),
                                        typeof (double),
                                        typeof (short),
                                        typeof (int),
                                        typeof (long),
                                        typeof (sbyte),
                                        typeof (float),
                                        typeof (ushort),
                                        typeof (uint),
                                        typeof (ulong),
                                    };

            bool typeIsPrimitive = primitiveTypes.Contains(underlyingType == null ? entityType : underlyingType);

            if (typeIsPrimitive == true)
            {
                string tableName = underlyingType == null ? $"{entityType.Name}DT" : $"{underlyingType.Name}DT";
                string columNname = underlyingType == null ? $"Col{entityType.Name}" : $"Col{underlyingType.Name}";

                DataTable dataTable = new DataTable(tableName);
                Type columnType = underlyingType == null ? entityType : underlyingType;
                DataColumn column = new DataColumn(columNname, columnType);
                column.AllowDBNull = underlyingType == null ? false : true;
                dataTable.Columns.Add(column);

                foreach (T item in @this)
                {
                    DataRow row = dataTable.NewRow();
                    row[0] = item;
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            else
            {
                // TODOS: 1. Convert lists of type System.Object to a data table.
                // TODOS: 2. Handle objects with nested objects (make the column name the name of the
                //        object and print "system.object" as the value).

                var dataTable = new DataTable(entityType.Name);
                PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(entityType);

                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
                {
                    Type propertyType = Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ?? propertyDescriptor.PropertyType;
                    dataTable.Columns.Add(propertyDescriptor.Name, propertyType);
                }

                foreach (T item in @this)
                {
                    DataRow row = dataTable.NewRow();

                    foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
                    {
                        object value = propertyDescriptor.GetValue(item);

                        if (propertyDescriptor.PropertyType == typeof(DateTime))
                        {
                            row[propertyDescriptor.Name] = value != null
                                    ? ((DateTime)value == DateTime.MinValue
                                            ? DBNull.Value
                                            : value)
                                    : DBNull.Value;
                        }
                        else
                        {
                            row[propertyDescriptor.Name] = value ?? DBNull.Value;
                        }
                    }

                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
        }
    }
}
