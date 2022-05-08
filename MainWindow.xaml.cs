using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Window_JiPu jipuwindow;  // 记谱窗口
        private static SpyWindow Spy_window;    // 棋盘数据监视窗口
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
            //qizi0.SetValue(VisibilityProperty, Visibility.Collapsed);
            GlobalValue.YuanWeiZhi = new QiZi();
            _ = qiziCanvas.Children.Add(GlobalValue.YuanWeiZhi);
            for (int i = 0; i < 32; i++)
            {
                GlobalValue.QiZiArray[i] = new QiZi(i);
                _ = qiziCanvas.Children.Add(GlobalValue.QiZiArray[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.PathPointImage[i, j] = new PathPoint(i, j);
                    _ = qiziCanvas.Children.Add(GlobalValue.PathPointImage[i, j]);
                }
            }
            GlobalValue.QiPanFanZhuan = false;
            QiPanChange(false);
            GlobalValue.Reset();
            jipuwindow = new Window_JiPu(); // 棋谱记录窗口
            jipuwindow.SetValue(LeftProperty, SystemParameters.WorkArea.Width - 600);
            jipuwindow.SetValue(TopProperty, SystemParameters.WorkArea.Top);
            jipuwindow.SetValue(HeightProperty, SystemParameters.WorkArea.Height);
            jipuwindow.Hide();
            Spy_window = new SpyWindow(); // 棋盘数据监视窗口
            Spy_window.Hide();

            GlobalValue.Window_Qi = new Window_QiPu(); // 棋谱库浏览窗口
            GlobalValue.Window_Qi.SetValue(LeftProperty, SystemParameters.WorkArea.Left);
            GlobalValue.Window_Qi.SetValue(TopProperty, SystemParameters.WorkArea.Top);
            GlobalValue.Window_Qi.SetValue(HeightProperty, SystemParameters.WorkArea.Height);
            GlobalValue.Window_Qi.Show();

            GlobalValue.JiangJunTiShi = new()
            {
                Content = "战况",
                Height = 30.0,
                Foreground = Brushes.White,
                Margin = new Thickness(100, 20, 0, 0),
                VerticalAlignment = VerticalAlignment.Top
            };

            _ = grid.Children.Add(GlobalValue.JiangJunTiShi);

            GlobalValue.jueShaImage = new();
            _ = grid.Children.Add(GlobalValue.jueShaImage);
            
            DrawGrid.Children.Add(GlobalValue.Arrows.grid); // 走棋提示箭头
        }

        /// <summary>
        /// 点击棋盘空白位置时的处理，暂时没用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QiPanMouseUp(object sender, MouseButtonEventArgs e)  // 点击棋盘空位置时
        {
            double x0 = 110.0;
            double y0 = 97.0;
            Point pnt = e.GetPosition(qipan_topBlack);
            int m = (int)(((pnt.X - x0) / GlobalValue.GRID_WIDTH) + 0.5);  // 加0.5以后向下取整，等同于四舍五入
            int n = (int)(((pnt.Y - y0) / GlobalValue.GRID_WIDTH) + 0.5);
            if (m > 8) { m = 8; }
            if (n > 9) { n = 9; }
            if (m < 0) { m = 0; }
            if (n < 0) { n = 0; }

            double plx = Math.Abs(pnt.X - (x0 + (m * GlobalValue.GRID_WIDTH)));   // 计算与交叉点的偏离
            double ply = Math.Abs(pnt.Y - (y0 + (n * GlobalValue.GRID_WIDTH)));
            if ((plx > 25.0) || (ply > 25.0))
            {
                return;
            }
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
            GlobalValue.QiPanFanZhuan = !GlobalValue.QiPanFanZhuan;
            QiPanChange(GlobalValue.QiPanFanZhuan);
            GlobalValue.YuanWeiZhi.FanZhuanPosition();
            foreach (QiZi item in GlobalValue.QiZiArray)
            {
                item.FanZhuanPosition();
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GlobalValue.PathPointImage[i, j].FanZhuPosition();
                }
            }
            GlobalValue.Arrows.HideAllPath();
        }
        /// <summary>
        /// 棋盘图像的更换
        /// </summary>
        /// <param name="change">false=上黑下红，true=上红下黑</param>
        private void QiPanChange(bool change)
        {
            if (change)
            {
                qipan_topBlack.Visibility = Visibility.Hidden;
                qipan_topRed.Visibility = Visibility.Visible;
            }
            else
            {
                qipan_topBlack.Visibility = Visibility.Visible;
                qipan_topRed.Visibility = Visibility.Hidden;
            }
        }


        /// <summary>
        /// 打开或关闭记谱窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenJiPuWindow(object sender, RoutedEventArgs e)
        {
            if (jipuwindow.IsVisible)
            {
                jipuwindow.Hide();
            }
            else
            {
                jipuwindow.Show();
            }
        }

        /// <summary>
        /// 打开或关闭棋盘数据监控窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSpyWindow(object sender, RoutedEventArgs e)
        {
            if (Spy_window.IsVisible)
            {
                Spy_window.Hide();
            }
            else
            {
                Spy_window.Show();
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
            Environment.Exit(0); // 关闭所有窗口，并释放所有资源，包括相关辅助窗口。
        }

        private void SaveQiPu(object sender, RoutedEventArgs e)
        {
            SubWindow.Save_Window window = new();
            _ = window.ShowDialog();
        }

        private void HuiQiButton(object sender, RoutedEventArgs e)
        {
            GlobalValue.HuiQi();
        }

        private void SaveJiPuToBuffer(object sender, RoutedEventArgs e)
        {
            jipuwindow.Save_jipu();
        }
    }
}
