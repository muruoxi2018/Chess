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
        private static string rowId { get; set; }
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
            FuPanDataGrid.ItemsSource = Qipu.ContractQiPu.ChildSteps;
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
            QipuListRefresh();
        }
        /// <summary>
        /// 更新棋谱库列表
        /// </summary>
        public void QipuListRefresh()
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
            GlobalValue.Reset(); // 棋盘复位
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
            qiPuSteps = GlobalValue.fuPanDataList.ToArray();

            remarksTextBlock.Text = ((DataRowView)DbDataGrid.SelectedItem).Row["memo"].ToString();
            remarksTextBlock.Text += System.Environment.NewLine + jsonStr;

            FuPanDataGrid.ItemsSource = Qipu.ContractQiPu.ChildSteps;
            TrueTree.ItemsSource = GlobalValue.qiPuRecordRoot.ChildNode;
            CompressTree.ItemsSource = Qipu.ContractQiPu.ChildSteps;
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
        /// 重新开始复盘
        /// </summary>
        public static void ReStart()
        {
            qpIndex = -1;
        }
        public string GetRowid()
        {
            return rowId;
        }
    }
}
