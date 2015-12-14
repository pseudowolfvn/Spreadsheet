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
    public class ItemsRow
    {
        List<Item> item;
    }
    public partial class MainWindow : Window
    {
        List<List<Item>> data = new List<List<Item>> { new List<Item> { new Item() } };
        DataTable Excel = new DataTable();
        char column = 'A';
        public List<List<Item>> Data { get { return data; } }
        public void addRow()
        {
            data.Add(new List<Item>(data[0].Count - 1));
            for (int i = 0; i < data[0].Count; i++)
            {
                data[data.Count - 1].Add(new Item());
            }
        }
        public void addColumn()
        {
            for (int i = 0; i < data.Count; i++)
            {
                data[i].Add(new Item());
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
        private void Calc(object sender, RoutedEventArgs e)
        {
            Tree<ArithmExpr> t = new Tree<ArithmExpr>();
            //textBox1.Text = t.calculate(new ArithmExpr(textBox.Text)).ToString();
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            data[e.Row.GetIndex()][e.Column.Header.ToString()[0] - 'A'].Value = ((TextBox)e.EditingElement).Text;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string debug = "";
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[0].Count; j++)
                    debug += data[i][j].Value + "|";
                debug += "\n";
            }
            MessageBox.Show(debug);
        }
    }
}
