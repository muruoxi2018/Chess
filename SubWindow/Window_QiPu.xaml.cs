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
        private static string rowId;
        private static int qpIndex = -1;
        private static ContractQPClass[] qiPuSteps;
        /// <summary>
        /// 棋谱库窗口
        /// </summary>
        public Window_QiPu()
        {
            InitializeComponent();
            //FuPanWidow = new SubWindow.FuPan_Window(); // 复盘窗口。调试用，暂不删除。
            //FuPanWidow.Hide();
            Left = SystemParameters.WorkArea.Left;
            Top = SystemParameters.WorkArea.Top;
            Height = SystemParameters.WorkArea.Height;
            FuPanDataGrid.ItemsSource = GlobalValue.fuPanDataList;
            TrueTree.ItemsSource = GlobalValue.qiPuRecordRoot.ChildNode;
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
            rowId = ((DataRowView)DbDataGrid.SelectedItem).Row["rowid"].ToString();
            RowIdText.Text = $"棋谱编号：{rowId}";
            videoUrl.Text = ((DataRowView)DbDataGrid.SelectedItem).Row["video"].ToString();

            string jsonStr = ((DataRowView)DbDataGrid.SelectedItem).Row["jsonrecord"].ToString(); // 获得点击行的棋谱数据
            int maxDepth = 1000;
            var simpleRecord = JsonConvert.DeserializeObject<Qipu.QiPuSimpleRecord>(jsonStr, new JsonSerializerSettings
            {
                //  MaxDepth默认值为64，此处加大该值
                TypeNameHandling = TypeNameHandling.None,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                MaxDepth = maxDepth
            }); // 反序列化 
            GlobalValue.qiPuRecordRoot = GlobalValue.ConvertQiPuToFull(simpleRecord); // 转换为完全树数据结构
            Qipu.ContractQiPu.ConvertFromQiPuRecord(GlobalValue.qiPuRecordRoot); // 转换为收缩树数据结构

            GlobalValue.fuPanDataList.Add(Qipu.ContractQiPu);

            remarksTextBlock.Text = ((DataRowView)DbDataGrid.SelectedItem).Row["memo"].ToString() + GetRemarks(GlobalValue.fuPanDataList);
            qiPuSteps = GlobalValue.fuPanDataList.ToArray();
            FuPanDataGrid.ItemsSource = Qipu.ContractQiPu.ChildSteps;
            TrueTree.ItemsSource = GlobalValue.qiPuRecordRoot.ChildNode;
            CompressTree.ItemsSource = Qipu.ContractQiPu.ChildSteps;
        }
        /// <summary>
        /// 棋谱中的所有注释
        /// </summary>
        /// <param name="qiPuLst">走棋步骤列表</param>
        /// <returns>棋谱中的全部说明文字</returns>
        public static string GetRemarks(ObservableCollection<Qipu.ContractQPClass> qiPuLst)
        {
            string memoStrs = "";
            foreach (Qipu.ContractQPClass qp in qiPuLst)
            {
                if (!string.IsNullOrEmpty(qp.Remarks))
                {
                    memoStrs += System.Environment.NewLine + $"第{qp.Id}步：{qp.Remarks}";
                }
            }
            return memoStrs;
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
                string rowId = ((DataRowView)DbDataGrid.SelectedItem).Row["rowid"].ToString();
                _ = OpenSource.SqliteHelper.Delete("mybook", $"rowid={rowId}");
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
            if (GlobalValue.fuPanDataList.Count < 1) return;
            System.Collections.Generic.Dictionary<string, object> dic = new();
            dic.Add("jsonrecord", JsonConvert.SerializeObject(GlobalValue.fuPanDataList));
            if (SqliteHelper.Update("mybook", $"rowid={rowId}", dic) > 0)
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
            if (GlobalValue.fuPanDataList.Count < 1) return;
            GlobalValue.Reset();
            qpIndex = -1;

            int index = FuPanDataGrid.SelectedIndex;
            while (qpIndex < index)
            {
                qpIndex++;
                StepCode step = qiPuSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
            }
            for (int i = 0; i < Qipu.QiPuList.Count; i++)
            {
                QiPuList[i].Remarks = GlobalValue.fuPanDataList[i].Remarks;
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
            if (GlobalValue.fuPanDataList.Count < 1) return;
            if (qpIndex < qiPuSteps.Length - 1)
            {
                qpIndex++;
                StepCode step = qiPuSteps[qpIndex].StepData;
                GlobalValue.QiZiMoveTo(step.QiZi, step.X1, step.Y1, step.DieQz, false);
                if (qpIndex <= qiPuSteps.Length - 2)
                {
                    ContractQPClass nextstep = GlobalValue.fuPanDataList[qpIndex + 1]; // 取出下一条走棋指令，绘制走棋提示箭头，并显示
                    GlobalValue.arrows.SetPathDataAndShow(0,
                        new System.Drawing.Point(nextstep.StepData.X0, nextstep.StepData.Y0),
                        new System.Drawing.Point(nextstep.StepData.X1, nextstep.StepData.Y1));
                    var points = Qipu.GetListPoint(GlobalValue.fuPanDataList[qpIndex]);
                    int index = 1;
                    foreach (var point in points)
                    {
                        GlobalValue.arrows.SetPathDataAndShow(index, point[0], point[1]);
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
            if (GlobalValue.fuPanDataList.Count < 1) return;
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
