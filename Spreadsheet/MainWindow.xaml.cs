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
            string row = e.Row.Header.ToString()
                , column = e.Column.Header.ToString()
                , newText = ((TextBox)e.EditingElement).Text
                , oldText = DataBase[row, column].Text;
            ArithmExpr newExpr = new ArithmExpr(newText)
                , oldExpr = new ArithmExpr(oldText);
            RemoveDependencies(column + row, oldExpr.AllVars);
            AddDependencies(column + row, newExpr.AllVars);
            DataBase[row, column].Text = newText;
            if (cycling(column + row, column + row, false))
            {
                MessageBox.Show("Oooops!");
                RemoveDependencies(column + row, newExpr.AllVars);
                AddDependencies(column + row, oldExpr.AllVars);
                DataBase[row, column].Text = oldText;
                UITable.Rows[Int32.Parse(row)].SetField(column, oldText);
            }
            else
            {
                recalculate(column + row);
                update(row, column);
            }
            FormulaBox.Text = DataBase[row, column].Text;
        }

        private void RemoveDependencies(string rootItem, List<Lexem> vars)
        {
            foreach (var x in vars)
            {
                DataBase[x.Value].Dependencies.Remove(rootItem);
            }
        }

        private void AddDependencies(string rootItem, List<Lexem> vars)
        {
            foreach (var x in vars)
            {
                DataBase[x.Value].Dependencies.Add(rootItem);
            }
        }

        private bool cycling(string rootItem, string root, bool flag)
        {
            if ((DataBase[rootItem].Dependencies.Count > 0) && !(flag))
            {
                foreach (var x in DataBase[rootItem].Dependencies)
                    if (x == root)
                        return true;
                    else { flag = cycling(x, root, flag); }
            }
            if (DataBase[rootItem].Dependencies.Count == 0)
                return false;
            return flag;
        }
        private void recalculate(string rootItem)
        {
            Tree<ArithmExpr> tree = new Tree<ArithmExpr>();
            ArithmExpr temp = new ArithmExpr(DataBase[rootItem].Text);
            DataBase[rootItem].Value = (BigInteger)tree.calculate(temp);
            if ((DataBase[rootItem].Dependencies.Count > 0))
            {
                foreach (var x in DataBase[rootItem].Dependencies)
                    recalculate(x);
            }

        }

        private void update(string row, string column)
        {
            UITable.Rows[Int32.Parse(row)].SetField(column, DataBase[row, column].Value.ToString());
            foreach(var x in DataBase[row, column].Dependencies)
            {
                update(ItemsTable.GetRow(x), ItemsTable.GetColumn(x));
            }
        }
        private void dataGrig_CellGetFocus(object sender, DataGridBeginningEditEventArgs e)
        {
            FormulaBox.Text = DataBase[e.Row.Header.ToString(), e.Column.Header.ToString()].Text;
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
