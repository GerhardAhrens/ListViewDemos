namespace ListViewControl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Cryptography;

    internal class DemoData
    {
        private const int COUNT = 10_000;
        private readonly List<DataRow> demoModelsDR = null;

        public DemoData()
        {
            this.demoModelsDR = new List<DataRow>();
        }

        public List<DataRow> BuildDataRow()
        {

            for (int i = 0; i < COUNT; i++)
            {
                DemoModel model = new DemoModel();
                using (RandomDataContent rdn = new RandomDataContent())
                {
                    model.Zahlen = rdn.NumbersInt(1, COUNT);
                    model.Datum = rdn.Dates(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
                    model.TextA = rdn.AlphabetAndNumeric(10);
                    model.TextB = rdn.AlphabetAndNumeric(20);
                    model.BoolValue = rdn.Boolean();
                    model.IsSelected = false;
                    if (model.Zahlen > 3000)
                    {
                        model.SymbolColor = "Red";
                        model.Symbol = "M12,8A4,4 0 0,0 8,12A4,4 0 0,0 12,16A4,4 0 0,0 16,12A4,4 0 0,0 12,8Z";
                    }
                    else
                    {
                        model.SymbolColor = "Green";
                        model.Symbol = "M16,8H8V16H16V8Z";
                    }
                }

                this.demoModelsDR.Add(ToDataRow(model));
            }

            return this.demoModelsDR;
        }

        public List<DemoModel> BuildData()
        {

            List<DemoModel> liste = new List<DemoModel>();
            for (int i = 0; i < COUNT; i++)
            {
                DemoModel model = new DemoModel();
                using (RandomDataContent rdn = new RandomDataContent())
                {
                    model.Zahlen = rdn.NumbersInt(1, COUNT);
                    model.Datum = rdn.Dates(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
                    model.TextA = rdn.AlphabetAndNumeric(10);
                    model.TextB = rdn.AlphabetAndNumeric(20);
                    model.BoolValue = rdn.Boolean();
                    model.IsSelected = false;
                    if (model.Zahlen > 3000)
                    {
                        model.SymbolColor = "Red";
                        model.Symbol = "M12,8A4,4 0 0,0 8,12A4,4 0 0,0 12,16A4,4 0 0,0 16,12A4,4 0 0,0 12,8Z";
                    }
                    else
                    {
                        model.SymbolColor = "Green";
                        model.Symbol = "M16,8H8V16H16V8Z";
                    }
                }

                liste.Add(model);
            }

            return liste;
        }

        private static DataRow ToDataRow(object from)
        {

            DataTable dt = new DataTable();

            foreach (PropertyInfo property in from.GetType().GetProperties())
            {
                DataColumn column = new DataColumn();

                column.ColumnName = property.Name;
                column.DataType = property.PropertyType;

                dt.Columns.Add(column);

            }

            DataRow dr = dt.NewRow();

            foreach (PropertyInfo property in from.GetType().GetProperties())
            {
                dr[property.Name] = property.GetValue(from);
            }

            return dr;
        }

        public static DataTable GetDataTable(object from)
        {

            DataTable dt = new DataTable();

            foreach (PropertyInfo property in from.GetType().GetProperties())
            {
                DataColumn column = new DataColumn();

                column.ColumnName = property.Name;
                column.DataType = property.PropertyType;

                dt.Columns.Add(column);

            }

            return dt;
        }
    }

    internal class DemoModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public DemoModel()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int Zahlen { get; set; }

        public DateTime Datum { get; set; }

        public string TextA { get; set; }

        public string TextB { get; set; }

        public bool BoolValue { get; set; }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Symbol { get; set; }

        public string SymbolColor { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        public override string ToString()
        {
            return $"{this.TextA}- {this.TextB}";
        }
    }
}
