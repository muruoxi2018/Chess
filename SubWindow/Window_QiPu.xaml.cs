using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using Chess.SuanFa;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;

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
            System.Data.DataTable sr = OpenSource.SqliteHelper.QueryTable("mybook", "rowid,*");
            datagrid.ItemsSource = sr.DefaultView;
        }

        public void qipuListRefresh(object sender, RoutedEventArgs e)
        {
            string jsonstr = JsonConvert.SerializeObject(Qipu.QiPuList);
            System.Data.DataTable sr = OpenSource.SqliteHelper.QueryTable("mybook", "rowid,*");
            datagrid.ItemsSource = sr.DefaultView;
        }

        private void onMouseLeftButtonUP(object sender, MouseButtonEventArgs e)
        {
            string jsonstr = ((DataRowView)datagrid.SelectedItem).Row["jsonrecord"].ToString(); // 获得点击行的数据
            jsontext.Text = jsonstr;
            var ql = JsonConvert.DeserializeObject<ObservableCollection<Qipu.QPStep>>(jsonstr);  // 反序列化为对象
            var tree = new TreeViewItem();
            tree.Header = "root";
            tree.IsExpanded = true;
            foreach (Qipu.QPStep qp in ql)
            {
                tree.Items.Add(qp.toTreeNode());
            }
            jsonTree.Items.Clear();
            jsonTree.Items.Add(tree);
        }
    }
}
