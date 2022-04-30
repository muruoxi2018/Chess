using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using Chess.SuanFa;
using System.Data;
using System.Collections.ObjectModel;

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
            DataTable sr = OpenSource.SqliteHelper.Select("mybook", "rowid,*");
            datagrid.ItemsSource = sr.DefaultView;
        }

        public void QipuListRefresh(object sender, RoutedEventArgs e)
        {
            DataTable sr = OpenSource.SqliteHelper.Select("mybook", "rowid,*");
            datagrid.ItemsSource = sr.DefaultView;
        }

        private void OnMouseLeftButtonUP(object sender, MouseButtonEventArgs e)
        {
            string jsonstr = ((DataRowView)datagrid.SelectedItem).Row["jsonrecord"].ToString(); // 获得点击行的数据
            jsontext.Text = jsonstr;
            TreeViewItem tree = new();
            tree.Header = "root";
            tree.IsExpanded = true;
            if (jsonstr.Length > 5)
            {
                ObservableCollection<Qipu.QPStep> ql = JsonConvert.DeserializeObject<ObservableCollection<Qipu.QPStep>>(jsonstr);  // 反序列化为对象
                foreach (Qipu.QPStep qp in ql)
                {
                    _ = tree.Items.Add(qp.ToTreeNode());
                }
            }
            jsonTree.Items.Clear();
            _ = jsonTree.Items.Add(tree);
            memostr.Text = ((DataRowView)datagrid.SelectedItem).Row["memo"].ToString();
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            menu.IsOpen = true;
        }

        private void DeleteRowData(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedIndex > -1)
            {
                string rowid = ((DataRowView)datagrid.SelectedItem).Row["rowid"].ToString();
                _ = OpenSource.SqliteHelper.Delete("mybook", "rowid=" + rowid);
                QipuListRefresh(sender, e);
            }
        }
    }
}
