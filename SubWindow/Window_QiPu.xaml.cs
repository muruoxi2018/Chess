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
    /// 棋谱库窗口的交互逻辑
    /// </summary>
    public partial class Window_QiPu : Window
    {
        private static string rowid;
        private static int qpIndex = -1;
        private static ContractQPClass[] qPSteps;
        /// <summary>
        /// 棋谱库窗口
        /// </summary>
        public Window_QiPu()
        {
            InitializeComponent();
            //FuPanWidow = new SubWindow.FuPan_Window(); // 复盘窗口。调试用，暂不删除。
            //FuPanWidow.Hide();
            FuPanDataGrid.ItemsSource = GlobalValue.FuPanDataList;
            TrueTree.ItemsSource = GlobalValue.QiPuRecordRoot.ChildNode;
            CompressTree.ItemsSource = Qipu.ContractQiPu.ChildSteps;

        }
        /// <summary>
        /// 窗口打开时，显示棋谱库列表，以及走棋记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowQiPu_Load(object sender, RoutedEventArgs e)
        {
            DataTable sr = OpenSource.SqliteHelper.Select("mybook", "rowid,*");
            if (sr == null) return;
            DbDataGrid.ItemsSource = sr.DefaultView;
            
        }
        /// <summary>
        /// 重新入棋谱库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QipuListRefresh(object sender, RoutedEventArgs e)
        {
            DataTable sr = OpenSource.SqliteHelper.Select("mybook", "rowid,*");
            if (sr == null) return;
            DbDataGrid.ItemsSource = sr.DefaultView;
        }
        /// <summary>
        /// 点击棋谱时，选中棋谱数据载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonUP(object sender, MouseButtonEventArgs e)
        {
            if (DbDataGrid.Items.Count == 0) return;
            rowid = ((DataRowView)DbDataGrid.SelectedItem).Row["rowid"].ToString();
            RowIdText.Text = $"棋谱编号：{rowid}";
            videoUrl.Text = ((DataRowView)DbDataGrid.SelectedItem).Row["video"].ToString();

            string jsonstr = ((DataRowView)DbDataGrid.SelectedItem).Row["jsonrecord"].ToString(); // 获得点击行的数据
            GlobalValue.FuPanDataList = JsonConvert.DeserializeObject<ObservableCollection<Qipu.ContractQPClass>>(jsonstr);
            memostr.Text = ((DataRowView)DbDataGrid.SelectedItem).Row["memo"].ToString() + GetMemo(GlobalValue.FuPanDataList);
            qPSteps = GlobalValue.FuPanDataList.ToArray();
            FuPanDataGrid.ItemsSource = GlobalValue.FuPanDataList;
        }
        /// <summary>
        /// 棋谱中的所有注释
        /// </summary>
        /// <param name="QpList">走棋步骤列表</param>
        /// <returns>棋谱中的全部说明文字</returns>
        public string GetMemo(ObservableCollection<Qipu.ContractQPClass> QpList)
        {
            string memostrs = "";
            foreach (Qipu.ContractQPClass qp in QpList)
            {
                if (!string.IsNullOrEmpty(qp.Remarks))
                {
                    memostrs += System.Environment.NewLine + $"第{qp.Id}步：{qp.Remarks}";
                }
            }
            return memostrs;
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //menu.IsOpen = true;
        }
        /// <summary>
        /// 删除当前选中的棋谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRowData(object sender, RoutedEventArgs e)
        {
            if (DbDataGrid.SelectedIndex > -1)
            {
                string rowid = ((DataRowView)DbDataGrid.SelectedItem).Row["rowid"].ToString();
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
        /// <summary>
        /// 将复盘数据保存到棋谱库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.FuPanDataList.Count < 1) return;
            System.Collections.Generic.Dictionary<string, object> dic = new();
            dic.Add("jsonrecord", JsonConvert.SerializeObject(GlobalValue.FuPanDataList));
            if (SqliteHelper.Update("mybook", $"rowid={rowid}", dic) > 0)
            {
                MessageBox.Show("数据保存成功！", "提示");
            }
            else
            {
                MessageBox.Show("数据没有能够保存，请查找原因！", "提示");
            }
        }
        /// <summary>
        /// 点击棋谱步骤时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFuPanDataGridClick(object sender, MouseButtonEventArgs e)
        {
            if (GlobalValue.FuPanDataList.Count < 1) return;
            GlobalValue.Reset();
            qpIndex = -1;

            int index = FuPanDataGrid.SelectedIndex;
            while (qpIndex < index)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
            }
            for (int i = 0; i < Qipu.QiPuList.Count; i++)
            {
                QiPuList[i].Remarks = GlobalValue.FuPanDataList[i].Remarks;
            }

        }
        /// <summary>
        /// 在浏览器中打开视频链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenVideo(object sender, RoutedEventArgs e)
        {
            Process proc = new();
            proc.StartInfo.UseShellExecute = true;
            // 在 .Net中，为了保证跨平台性，
            // 需要委托 Windows Shell 做一些事情时，
            // 需要显式声明 Process.StartUseShellExecute=true
            proc.StartInfo.FileName = videoUrl.Text;
            _ = proc.Start();
        }
        /// <summary>
        /// 下一步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.FuPanDataList.Count < 1) return;
            if (qpIndex < qPSteps.Length - 1)
            {
                qpIndex++;
                StepCode step = qPSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
                if (qpIndex <= qPSteps.Length - 2)
                {
                    ContractQPClass nextstep = GlobalValue.FuPanDataList[qpIndex + 1]; // 取出下一条走棋指令，绘制走棋提示箭头，并显示
                    GlobalValue.Arrows.SetPathDataAndShow(0,
                        new System.Drawing.Point(nextstep.StepData.X0,nextstep.StepData.Y0),
                        new System.Drawing.Point(nextstep.StepData.X1,nextstep.StepData.Y1));
                    var points = Qipu.GetListPoint(GlobalValue.FuPanDataList[qpIndex]);
                    int index = 1;
                    foreach (var point in points)
                    {
                        GlobalValue.Arrows.SetPathDataAndShow(index,point[0],point[1]);
                        index++;
                    }
                }
            }
        }
        /// <summary>
        /// 上一步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreStep(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.FuPanDataList.Count < 1) return;
            if (qpIndex > -1)
            {
                GlobalValue.HuiQi();
                qpIndex--;

            }
        }
        /// <summary>
        /// 重新开始复盘
        /// </summary>
        public static void ReStart()
        {
            qpIndex = -1;
        }
    }
}
