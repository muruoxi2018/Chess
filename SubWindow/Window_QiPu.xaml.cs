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
            //System.Data.DataTable sr =OpenSource.SqliteHelper.ExecuteTable("select rowid,* from mybook");
            System.Data.DataTable sr = OpenSource.SqliteHelper.QueryTable("mybook", "rowid,*");
            datagrid.ItemsSource = sr.DefaultView;

            //qplst.Text = JsonConvert.SerializeObject(Qipu.QiPuList); // Newtonsoft.Json
            //qplst.Text =JsonSerializer.Serialize(Qipu.QiPuList);  // System.Text.Json

        }

        public void qipuListRefresh(object sender, RoutedEventArgs e)
        {
            //qplst.Text = JsonSerializer.Serialize(Qipu.QiPuList);
            string jsonstr = JsonConvert.SerializeObject(Qipu.QiPuList);
            //qplst.Text = Qipu.NmToJson();
            //string filename = System.Environment.CurrentDirectory + @"\DB\" + DateTime.Now.ToFileTimeUtc().ToString()+".txt";
            //File.WriteAllText(filename, jsonstr);

            //qplst.Text=filename+File.ReadAllText(filename);
            System.Data.DataTable sr = OpenSource.SqliteHelper.QueryTable("mybook","rowid,*");
            datagrid.ItemsSource = sr.DefaultView;
        }

        private void onMouseLeftButtonUP(object sender, MouseButtonEventArgs e)
        {
            //int index=datagrid.SelectedIndex;
            string jsonstr=((DataRowView)datagrid.SelectedItem).Row["jsonrecord"].ToString();
            //qplst.Text=jsonstr;
            jsontext.Text = jsonstr;
            //jsonTree.Background = null;
            var ql =  JsonConvert.DeserializeObject<List<Qipu.QPStep>>(jsonstr);
            /*var tree = new TreeViewItem();
            tree.Header = "root";

            foreach(Qipu.QPStep qp in ql)
            {
                tree.Items.Add(qp.toTreeNode());
            }
            jsonTree.Items.Clear();
            jsonTree.Items.Add(tree);*/
            jsonTree.ItemsSource = ql;
            
        }
    }
}
