using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Chess
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
        System.Data.DataSet ds = new System.Data.DataSet();
        public Window_JiPu()
        {
            InitializeComponent();
        }

        private void WindowJiPu_Load(object sender, RoutedEventArgs e)
        {
            System.Data.SQLite.SQLiteConnection conn = new SQLiteConnection("data source=E:/source/repos/Chess/DB/KaiJuKu.db");
            conn.Open();
            
            System.Data.SQLite.SQLiteCommand comm = conn.CreateCommand();
            comm.CommandText = "select * from mybook";
            
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm);
            _ = adapter.Fill(ds);
            //Console.WriteLine(ds.Tables.ToString());
            lv.Items.Add(ds);
            text1.Text = ds.Tables.ToString();


        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
