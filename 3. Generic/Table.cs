using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<RowType,ColumnType,ValueType>: Hashtable
    {
        public List<RowType> Rows = new List<RowType>();
        public List<ColumnType> Columns = new List<ColumnType>();
        public List<KeyValuePair<Tuple<RowType, ColumnType>, ValueType>>
            Cells = new List<KeyValuePair<Tuple<RowType, ColumnType>, ValueType>>();

        public void AddRow(RowType row)
        {
            if (Rows.Where(e => e.Equals(row)) == null || Rows.Count == 0)
                Rows.Add(row);
        }

        public void AddColumn(ColumnType column)
        {
            if (Columns.Where(e => e.Equals(column)) == null || Columns.Count == 0)
                Columns.Add(column);
        }

        public bool RowExists(RowType row)
        {
            return Rows.Find(e => e.Equals(row)) == null || Rows.Count == 0  ? false : true;
        }       

        public bool ColumnExists(ColumnType column)
        {
            return Columns.Find(e => e.Equals(column)) == null || 
                Columns.Count == 0 ? false : true;
        }

        public ValueType this[RowType row, ColumnType column]
        {
            get
            {
                return Cells.Where(e => e.Key.Item1.Equals(row) && e.Key.Item2.Equals(column))
                    .Select(e => e.Value)
                    .FirstOrDefault();
            }

            set
            {
                var cellsForRemove = Cells.Where(e => e.Key.Item1.Equals(row) && 
                                                 e.Key.Item2.Equals(column));
                foreach (var cell in cellsForRemove)
                {
                        Cells.Remove(cell);
                }

                Cells.Add(new KeyValuePair<Tuple<RowType, ColumnType>, ValueType>
                    (new Tuple<RowType, ColumnType>(row, column), value));
                AddRow(row);
                AddColumn(column);
            }
        }

        public Table<RowType, ColumnType, ValueType> Open
        {
            get
            {
                return this;
            }
        }

        public TableAccessor<RowType, ColumnType, ValueType> Existed
        {
            get
            {
                return new TableAccessor<RowType, ColumnType, ValueType>(this);
            }
        }
    }

    public class TableAccessor<RowType, ColumnType, ValueType>
    {
        private Table<RowType, ColumnType, ValueType> table;

        public ValueType this[RowType row, ColumnType column] 
        {
            get
            {
                if (table.RowExists(row) && table.ColumnExists(column))
                    return table[row, column];
                else throw new ArgumentException();
            }

           set
            {
               if (table.RowExists(row) && table.ColumnExists(column))
                    table[row, column] = value;
                else throw new ArgumentException();
            }
        }

        public TableAccessor(Table<RowType, ColumnType, ValueType> table)
        {
            this.table = table;
        }
    }
}