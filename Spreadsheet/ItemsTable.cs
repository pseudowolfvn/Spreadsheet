﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    public class Item
    {
        List<string> dependencies = new List<string>();
        string value = "0";
        bool expression = false;
        string text = "";

        public string Text
        {
            get { return text; }
            set { this.text = value; }
        }
        public bool Expression
        {
            get { return expression; }
            set { this.expression = value; }
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public List<string> Dependencies
        {
            get { return dependencies; }
            set { this.dependencies = value; }
        }
    }

    public class ItemsTable
    {
        List<List<Item>> items = new List<List<Item>> { new List<Item> { new Item() } };
        public List<List<Item>> Items { get { return items; } }
        public Item this[string fullPath]
        {
            get
            {
                string column = GetColumn(fullPath),
                    row = GetRow(fullPath);
                return this[row, column];
            }
        }
        public Item this[string row, string column]
        {
            get
            {
                int indexColumn = FromColumnToInt(column),
                    indexRow = Int32.Parse(row);
                return items[indexRow][indexColumn];
            }
        }
        public static string GetRow(string fullPath)
        {
            int index = 0;
            while (index < fullPath.Length && Char.IsUpper(fullPath[index])) ++index;
            return fullPath.Substring(index);
        }
        public static string GetColumn(string fullPath)
        {
            int index = 0;
            while (index < fullPath.Length && Char.IsUpper(fullPath[index])) ++index;
            return fullPath.Substring(0, index);
        }
        public static int FromColumnToInt(string column)
        {
            int powBase = 1, indexColumn = 0;
            for (int i = column.Length - 1; i >= 0; --i)
            {
                indexColumn += powBase * (column[i] - 'A');
                powBase *= 26;
            }
            return indexColumn;
        }
        public static string FromIntToColumn(int column)
        {
            string result = "";
            while (column > 0)
            {
                char mod = (char)((column % 26) + 'A');
                result += mod.ToString();
                column /= 26;
            }
            char[] toReverse = result.ToArray();
            Array.Reverse(toReverse);
            return new string(toReverse);
        }
        public static string ColumnInc(ref string column)
        {
            int index = FromColumnToInt(column);
            string oldColumn = column;
            column = FromIntToColumn(++index);
            return oldColumn;
        }
    }
}
