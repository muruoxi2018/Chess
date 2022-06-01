using Chess.OpenSource;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chess.SubWindow;
using Chess.CustomClass;

namespace Chess
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Window_JiPu jipuWindow;  // 记谱窗口
        private static SpyWindow spyWindow;    // 棋盘数据监视窗口

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主窗口载入时，初始化自定义控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
            GlobalValue.yuanWeiZhi = new QiZi(); // 棋子原位置图片
            _ = qiziCanvas.Children.Add(GlobalValue.yuanWeiZhi);

            for (int i = 0; i < 32; i++)
            {
                GlobalValue.qiZiArray[i] = new QiZi(i);  // 初始化32个棋子
                _ = qiziCanvas.Children.Add(GlobalValue.qiZiArray[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathPointImage[i, j] = new PathPoint(i, j);  // 走棋路径
                    _ = qiziCanvas.Children.Add(GlobalValue.pathPointImage[i, j]);
                }
            }
            GlobalValue.isQiPanFanZhuan = false; // 棋盘翻转，初始为未翻转，黑方在上，红方在下
            QiPanChange(false);
            jipuWindow = new Window_JiPu(); // 棋谱记录窗口
            jipuWindow.Hide();
            spyWindow = new SpyWindow(); // 棋盘数据监视窗口
            spyWindow.Hide();

            GlobalValue.qiPuKuForm = new Window_QiPu(); // 棋谱库浏览窗口
            GlobalValue.qiPuKuForm.Hide();

            GlobalValue.jiangJunTiShi = new() // 将军状态文字提示
            {
                Content = "",
                Height = 30.0,
                Foreground = Brushes.Goldenrod,
                Margin = new Thickness(100, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            _ = JiangJunTiShi.Children.Add(GlobalValue.jiangJunTiShi);

            GlobalValue.jueShaImage = new(); // 绝杀图片
            _ = mainGrid.Children.Add(GlobalValue.jueShaImage);

            DrawGrid.Children.Add(GlobalValue.arrows.grid); // 走棋提示箭头

            GlobalValue.redSideRect = new System.Windows.Shapes.Ellipse()
            {
                Width = 30,
                Height = 30,
                Margin = new Thickness(15, 500, 30, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.Gray),
                Stroke = new SolidColorBrush(Colors.Goldenrod),
            };
            _ = mainGrid.Children.Add(GlobalValue.redSideRect);
            GlobalValue.blackSideRect = new System.Windows.Shapes.Ellipse()
            {
                Width = 30,
                Height = 30,
                Margin = new Thickness(15, 260, 30, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.Gray),
                Stroke = new SolidColorBrush(Colors.Goldenrod),
            };
            _ = mainGrid.Children.Add(GlobalValue.blackSideRect);

            GlobalValue.Reset();

        }

        /// <summary>
        /// 重新开局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtnClick(object sender, RoutedEventArgs e)
        {
            GlobalValue.Reset();
        }

        /// <summary>
        /// 点击“棋盘翻转”button时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFanZhuanQiPan(object sender, RoutedEventArgs e)
        {
            GlobalValue.isQiPanFanZhuan = !GlobalValue.isQiPanFanZhuan;
            QiPanChange(GlobalValue.isQiPanFanZhuan);  // 更换棋盘
            GlobalValue.yuanWeiZhi.FanZhuanPosition(); // 走棋原位置图片刷新
            foreach (QiZi item in GlobalValue.qiZiArray)
            {
                item.FanZhuanPosition(); // 棋盘翻转后，刷新显示所有棋子
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GlobalValue.pathPointImage[i, j].FanZhuPosition(); // 棋盘翻转后，刷新显示所有走棋路径
                }
            }
            GlobalValue.arrows.HideAllPath(); //  隐藏提示箭头
        }

        /// <summary>
        /// 棋盘翻转时更换棋盘
        /// </summary>
        /// <param name="isChange">false=上黑下红，true=上红下黑</param>
        private void QiPanChange(bool isChange)
        {
            if (isChange)
            {
                qipan_topBlack.Visibility = Visibility.Hidden;
                qipan_topRed.Visibility = Visibility.Visible;
                GlobalValue.redSideRect.Margin = new Thickness(15, 260, 30, 0);
                GlobalValue.blackSideRect.Margin = new Thickness(15, 500, 30, 0);
            }
            else
            {
                qipan_topBlack.Visibility = Visibility.Visible;
                qipan_topRed.Visibility = Visibility.Hidden;
                GlobalValue.redSideRect.Margin = new Thickness(15, 500, 30, 0);
                GlobalValue.blackSideRect.Margin = new Thickness(15, 260, 30, 0);
            }
        }

        /// <summary>
        /// 打开或关闭记谱窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenJiPuWindow(object sender, RoutedEventArgs e)
        {
            if (jipuWindow.IsVisible)
            {
                jipuWindow.Close();
            }
            else
            {
                jipuWindow = new();
                jipuWindow.Show();
            }
        }

        /// <summary>
        /// 打开或关闭棋盘数据监控窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSpyWindow(object sender, RoutedEventArgs e)
        {
            if (spyWindow.IsVisible)
            {
                spyWindow.Close();
            }
            else
            {
                spyWindow = new SpyWindow();
                spyWindow.Show();
            }
        }

        /// <summary>
        /// 打开软件设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetupOption(object sender, RoutedEventArgs e)
        {
            SetupWindow sw = new();
            sw.Show();
        }

        /// <summary>
        /// 软件关闭退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainWindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
            Environment.Exit(0); // 关闭所有窗口，并释放所有资源，包括相关辅助窗口。
        }

        /// <summary>
        /// 保存棋谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveQiPu(object sender, RoutedEventArgs e)
        {
            SubWindow.Save_Window window = new();
            _ = window.ShowDialog();
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HuiQiButton(object sender, RoutedEventArgs e)
        {
            GlobalValue.HuiQi();
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep(object sender, RoutedEventArgs e)
        {
            int childCount=GlobalValue.NextStep();
            if (childCount > 1)
            {
                ChildSelecteWindow selectPage = new(childCount);
                selectPage.ShowDialog();
                string childid=Clipboard.GetText();
                GlobalValue.NextStep(childid);
            }
        }

        /// <summary>
        /// 打开复盘窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFuPanWindow(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.qiPuKuForm.IsVisible)
            {
                GlobalValue.qiPuKuForm.Close();
            }
            else
            {
                GlobalValue.qiPuKuForm = new();
                GlobalValue.qiPuKuForm.Show();
            }
        }
        /// <summary>
        /// 添加注释
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRemark(object sender, RoutedEventArgs e)
        {
            remarkGrid.Visibility = Visibility.Visible;
            string str;
            str = GlobalValue.qiPuRecordRoot.Cursor.Remarks;
            str += GlobalValue.qiPuRecordRoot.Cursor.Id + "、" + GlobalValue.qiPuRecordRoot.Cursor.Cn + "：下一步，";
            foreach (var item in GlobalValue.qiPuRecordRoot.Cursor.ChildNode)
            {
                str += item.Cn;
            }
            remarkTextBox.Text = System.Environment.NewLine + str;
        }
        /// <summary>
        /// 点击保存按钮后，保存到内存中，同时更新数据显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveRemark(object sender, RoutedEventArgs e)
        {
            GlobalValue.qiPuRecordRoot.Cursor.Remarks = remarkTextBox.Text;
            remarkGrid.Visibility = Visibility.Hidden;

            GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
            Qipu.ContractQiPu.ConvertFromQiPuRecord(GlobalValue.qiPuRecordRoot); // 压缩树更新
            GlobalValue.qiPuKuForm.remarksTextBlock.Text = JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot);
        }

        private void UpdateQiPu(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Window_QiPu.GetRowid()))
            {
                GlobalValue.qiPuSimpleRecordRoot = GlobalValue.ConvertQiPuToSimple(GlobalValue.qiPuRecordRoot);  // 更新简易棋谱记录
                System.Collections.Generic.Dictionary<string, object> dic = new();
                dic.Add("jsonrecord", JsonConvert.SerializeObject(GlobalValue.qiPuSimpleRecordRoot));
                if (SqliteHelper.Update("mybook", $"rowid={Window_QiPu.GetRowid()}", dic) > 0)
                {
                    MessageBox.Show("数据保存成功！", "提示");
                }
                else
                {
                    MessageBox.Show("数据没有能够保存，请查找原因！", "提示");
                }
            }
            else
            {
                //  如果棋谱库编号为空，则另存为新棋谱。
                SubWindow.Save_Window window = new();
                _ = window.ShowDialog();
            }
            //  更新数据后，刷新棋谱列表
            GlobalValue.qiPuKuForm.QipuDBListRefresh();
        }
    }
}
