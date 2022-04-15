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
using Newtonsoft.Json;

namespace Chess
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
<<<<<<< HEAD
=======
        System.Data.DataSet ds = new System.Data.DataSet();
>>>>>>> 91aa539efa0436e39ac7f0d6c2a49ad828577092
        public Window_JiPu()
        {
            InitializeComponent();
        }
<<<<<<< HEAD
=======

        private void WindowJiPu_Load(object sender, RoutedEventArgs e)
        {
            System.Data.DataTable sr = SqliteHelper.ExecuteTable("select rowid,* from mybook");
            datagrid.ItemsSource = sr.DefaultView;

            qplst.Text = JsonConvert.SerializeObject(Qipu.QiPuList);
            dglist.ItemsSource = Qipu.QiPuList;
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
>>>>>>> 91aa539efa0436e39ac7f0d6c2a49ad828577092
    }
}
