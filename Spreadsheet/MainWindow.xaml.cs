using System;
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
using System.Numerics;

namespace Spreadsheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Item
    {
        List<string> dependencies = new List<string>();
        BigInteger value = 0;
        bool expression = false;
        string text;

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
        public BigInteger Value
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
        public Item this[string var]
        {
            get
            {
                int index = 0;
                while (index < var.Length && Char.IsUpper(var[index])) ++index;
                string column = var.Substring(0, index),
                    row = var.Substring(index);
                return this[row, column];
            }
        }
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
        ItemsTable dataBase = new ItemsTable();
        DataTable UITable = new DataTable();
        char column = 'A';
        public ItemsTable DataBase { get { return dataBase; } }
        public void addRow()
        {
            DataBase.Items.Add(new List<Item>(DataBase.Items[0].Count - 1));
            for (int i = 0; i < DataBase.Items[0].Count; i++)
            {
                DataBase.Items[DataBase.Items.Count - 1].Add(new Item());
            }
        }
        public void addColumn()
        {
            for (int i = 0; i < DataBase.Items.Count; i++)
            {
                DataBase.Items[i].Add(new Item());
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            addColumn();
            UITable.Columns.Add((column++).ToString());
            UITable.Columns.Add((column++).ToString());
            addRow();
            UITable.Rows.Add(UITable.NewRow());
            UITable.Rows.Add(UITable.NewRow());
            UpdateDataGrid();
        }
        public void UpdateDataGrid()
        {
            dataGrid.ItemsSource = UITable.AsDataView();
        }
        private void OnButton1Click(object sender, RoutedEventArgs e)
        {
            DataRow addedRow = UITable.NewRow();
            UITable.Rows.Add(addedRow);
            addRow();
        }
        private void OnButton2Click(object sender, RoutedEventArgs e)
        {
            UITable.Columns.Add((column++).ToString());
            dataGrid.ItemsSource = UITable.AsDataView();
            addColumn();
        }
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex().ToString();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Tree<ArithmExpr> tree = new Tree<ArithmExpr>();
            string row = e.Row.Header.ToString();
            string column = e.Column.Header.ToString();
            DataBase[row, column].Text = ((TextBox)e.EditingElement).Text;
            ArithmExpr temp = new ArithmExpr(DataBase[row, column].Text);
            //DataBase[row, column].Value = (BigInteger)tree.calculate(new ArithmExpr(DataBase[row, column].Text));
            for (int i = 0; i < temp.AllVars.Count; i++)
            {
                DataBase[temp.AllVars[i].Value].Dependencies.Add(column + row);
            }
            if (!recalculation(column + row, column + row, true))
                MessageBox.Show("Oooops!");
            else
                //DataBase[row, column].Value = (BigInteger)tree.calculate(temp);
                //DataBase[row, column].Dependencies = temp.AllVars;
                update(row, column);
            //UpdateDataGrid();
        }
        private bool recalculation(string rootItem, string root, bool flag)
        {
            Tree<ArithmExpr> tree = new Tree<ArithmExpr>();
            ArithmExpr temp = new ArithmExpr(DataBase[rootItem].Text);
            DataBase[rootItem].Value = (BigInteger)tree.calculate(temp);
            if ((DataBase[rootItem].Dependencies.Count > 0) && (flag))
            {
                foreach (var x in DataBase[rootItem].Dependencies)
                    if (x == root)
                        return false;
                    else { flag = recalculation(x, root, flag); }
            }
            if (DataBase[rootItem].Dependencies.Count == 0)
                return true;
            return flag;

        }

        private void update(string row, string column)
        {
            UITable.Rows[Int32.Parse(row)].SetField(column, DataBase[row, column].Value.ToString());
            foreach(var x in DataBase[row, column].Dependencies)
            {
                int index = 0;
                while (index < x.Length && Char.IsUpper(x[index])) ++index;
                update(x.Substring(index), x.Substring(0, index));
            }
        }
        private void dataGrig_CellGetFocus(object sender, DataGridBeginningEditEventArgs e)
        {
            textBox.Text = DataBase[e.Row.Header.ToString(), e.Column.Header.ToString()].Text;
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Tree<ArithmExpr> tree = new Tree<ArithmExpr>();
            //MessageBox.Show(Spreadsheet.Tree<ArithmExpr>.calculate(new ArithmExpr("A0 - A1/A3")).ToString());
            //string debug = "";
            //for (int i = 0; i < DataBase.Items.Count; i++)
            //{
            //    for (int j = 0; j < DataBase.Items[0].Count; j++)
            //        debug += DataBase.Items[i][j].Text + "|";
            //    debug += "\n";
            //}
            //MessageBox.Show(debug);
        }
    }
}
