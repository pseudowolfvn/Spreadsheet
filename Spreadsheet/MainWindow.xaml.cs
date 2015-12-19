﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spreadsheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Item
    {
        List<Item> dependencies;
        bool expression;
        string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
    public class ItemsTable
    {
        List<List<Item>> items = new List<List<Item>> { new List<Item> { new Item() } };
        public List<List<Item>> Items { get { return items; } } 
        public Item this[string row, string column]
        {
            get
            {
                int indexColumn = 0,
                    indexRow = Int32.Parse(row);
                int powBase = 1;
                for (int i = column.Length - 1; i >= 0; --i)
                {
                    indexColumn += powBase * (column[i] - 'A');
                    powBase *= 26;
                }
                return items[indexRow][indexColumn];
            } 
        }
    }
    public partial class MainWindow : Window
    {
        ItemsTable data = new ItemsTable();
        DataTable Excel = new DataTable();
        char column = 'A';
        public ItemsTable Data { get { return data; } }
        public void addRow()
        {
            Data.Items.Add(new List<Item>(Data.Items[0].Count - 1));
            for (int i = 0; i < Data.Items[0].Count; i++)
            {
                Data.Items[Data.Items.Count - 1].Add(new Item());
            }
        }
        public void addColumn()
        {
            for (int i = 0; i < Data.Items.Count; i++)
            {
                Data.Items[i].Add(new Item());
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            addColumn();
            Excel.Columns.Add((column++).ToString());
            Excel.Columns.Add((column++).ToString());
            addRow();
            Excel.Rows.Add(Excel.NewRow());
            Excel.Rows.Add(Excel.NewRow());
            dataGrid.ItemsSource = Excel.AsDataView();
        }
        private void OnButton1Click(object sender, RoutedEventArgs e)
        {
            DataRow addedRow = Excel.NewRow();
            Excel.Rows.Add(addedRow);
            addRow();
        }
        private void OnButton2Click(object sender, RoutedEventArgs e)
        {
            Excel.Columns.Add((column++).ToString());
            dataGrid.ItemsSource = Excel.AsDataView();
            addColumn();
        }
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Data[e.Row.Header.ToString(), e.Column.Header.ToString()].Value = ((TextBox)e.EditingElement).Text;
        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Tree<ArithmExpr> tree = new Tree<ArithmExpr>();
            MessageBox.Show(tree.calculate(new ArithmExpr("A0 + A1 + B0 + B1")).ToString());
            //string debug = "";
            //for (int i = 0; i < Data.Items.Count; i++)
            //{
            //    for (int j = 0; j < Data.Items[0].Count; j++)
            //        debug += Data.Items[i][j].Value + "|";
            //    debug += "\n";
            //}
            //MessageBox.Show(debug);
        }
    }
}
