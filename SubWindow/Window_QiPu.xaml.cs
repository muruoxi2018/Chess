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
using Chess.SuanFa;
using Chess.OpenSource;
using System.Text.Json;


namespace Chess
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window_QiPu : Window
    {
        public Window_QiPu()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗口打开时，显示棋谱库列表，以及走棋记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowQiPu_Load(object sender, RoutedEventArgs e)
        {
            System.Data.DataTable sr =OpenSource.SqliteHelper.ExecuteTable("select rowid,* from mybook");
            datagrid.ItemsSource = sr.DefaultView;

            //qplst.Text = JsonConvert.SerializeObject(Qipu.QiPuList);
            //qplst.Text =JsonSerializer.Serialize(Qipu.QiPuList);

        }

        private void qipuListRefresh(object sender, RoutedEventArgs e)
        {
            //qplst.Text = JsonSerializer.Serialize(Qipu.QiPuList);
            qplst.Text = JsonConvert.SerializeObject(Qipu.QiPuList);
        }
    }
}
