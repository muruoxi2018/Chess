using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;
using Chess.SuanFa;
using System.Data;
using System.Collections.ObjectModel;
using Chess.OpenSource;
using Chess.DataClass;
using static Chess.SuanFa.Qipu;
using System.Linq;
using System.Diagnostics;

namespace Chess
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window_QiPu : Window
    {
        //private static SubWindow.FuPan_Window FuPanWidow;
        private static string rowid;
        private static int qpIndex = -1;
        private static QPStep[] qPSteps;
        public Window_QiPu()
        {
            InitializeComponent();
            //FuPanWidow = new SubWindow.FuPan_Window(); // 复盘窗口
            //FuPanWidow.Hide();
            FuPanDataGrid.ItemsSource = GlobalValue.QiPuFuPanList;

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
            ObservableCollection<Qipu.QPStep> ql = JsonConvert.DeserializeObject<ObservableCollection<Qipu.QPStep>>(jsonstr);
            GlobalValue.QiPuFuPanList = ql;
            videoUrl.Text = ((DataRowView)datagrid.SelectedItem).Row["video"].ToString();
            memostr.Text = ((DataRowView)datagrid.SelectedItem).Row["memo"].ToString() + DrawTree(ql);
            rowid = ((DataRowView)datagrid.SelectedItem).Row["rowid"].ToString();
            RowIdText.Text = $"棋谱编号：{rowid}";
            qPSteps = GlobalValue.QiPuFuPanList.ToArray();
            FuPanDataGrid.ItemsSource = GlobalValue.QiPuFuPanList;


        }
        public string DrawTree(ObservableCollection<Qipu.QPStep> QpList)
        {

            TreeViewItem tree = new();
            tree.Header = "root";
            tree.IsExpanded = true;
            string memostrs = "";

            foreach (Qipu.QPStep qp in QpList)
            {
                _ = tree.Items.Add(qp.ToTreeNode());
                if (!string.IsNullOrEmpty(qp.Memo))
                {
                    memostrs += System.Environment.NewLine + $"第{qp.Id}步：{qp.Memo}";
                }
            }
            jsonTree.Items.Clear();
            _ = jsonTree.Items.Add(tree);
            return memostrs;
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //menu.IsOpen = true;
        }

        private void DeleteRowData(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedIndex > -1)
            {
                string rowid = ((DataRowView)datagrid.SelectedItem).Row["rowid"].ToString();
                _ = OpenSource.SqliteHelper.Delete("mybook", $"rowid={rowid}");
                QipuListRefresh(sender, e);
            }
        }
        private void FuPan(object sender, RoutedEventArgs e)
        {

            /*if (FuPanWidow != null)
            {
                FuPanWidow.Close();
            }
            FuPanWidow = new SubWindow.FuPan_Window();
            FuPanWidow.Show();*/
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            System.Collections.Generic.Dictionary<string, object> dic = new();
            dic.Add("jsonrecord", JsonConvert.SerializeObject(GlobalValue.QiPuFuPanList));
            if (SqliteHelper.Update("mybook", $"rowid={rowid}", dic) > 0)
            {
                MessageBox.Show("数据保存成功！", "提示");
            }
            else
            {
                MessageBox.Show("数据没有能够保存，请查找原因！", "提示");
            }
        }

        private void OnFuPanDataGridClick(object sender, MouseButtonEventArgs e)
        {
            GlobalValue.Reset();
            qpIndex = -1;

            int index = FuPanDataGrid.SelectedIndex;
            while (qpIndex < index)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepRecode;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
            }
            for (int i = 0; i < Qipu.QiPuList.Count; i++)
            {
                QiPuList[i].Memo = GlobalValue.QiPuFuPanList[i].Memo;
            }

        }

        private void OpenVideo(object sender, RoutedEventArgs e)
        {
            Process proc = new();
            proc.StartInfo.UseShellExecute = true;
            // 在 .Net Core 中，为了保证跨平台性，
            // 需要委托 Windows Shell 来实现的一些事情
            // 需要使用 Process.StartUseShellExecute=true 来显式的声明
            proc.StartInfo.FileName = videoUrl.Text;
            _ = proc.Start();
        }

        private void NextStep(object sender, RoutedEventArgs e)
        {
            if (qpIndex < qPSteps.Length - 1)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepRecode;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);

            }
        }

        private void PreStep(object sender, RoutedEventArgs e)
        {
            if (qpIndex > -1)
            {
                GlobalValue.HuiQi();
                qpIndex--;

            }
        }
    }
}
