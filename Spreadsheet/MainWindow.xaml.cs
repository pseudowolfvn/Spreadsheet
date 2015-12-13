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
        public string expression;
        public string Value
        {
            get { return expression; }
            set { expression = value; }
        }
    }
    public class ItemsRow
    {
        List<Item> item;
    }
    public partial class MainWindow : Window
    {
        DataTable Excel = new DataTable();
        char column = 'A';
        int row = 1;

        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = Excel.AsDataView();
        }
        private void OnButton1Click(object sender, RoutedEventArgs e)
        {
            //Excel.Rows.Add(row.ToString());
            DataRow addedRow = Excel.NewRow();
            Excel.Rows.Add(addedRow);
            ++row;
        }
        private void OnButton2Click(object sender, RoutedEventArgs e)
        {

            Excel.Columns.Add(column.ToString());
            dataGrid.ItemsSource = Excel.AsDataView();
            ++column;
        }
        private void Calc(object sender, RoutedEventArgs e)
        {
            Tree<ArithmExpr> t = new Tree<ArithmExpr>();
            //textBox1.Text = t.calculate(new ArithmExpr(textBox.Text)).ToString();
        }
    }
}
