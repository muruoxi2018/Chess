﻿using System;
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
            GlobalValue.YuanWeiZhi = new QiZi(); // 棋子原位置图片
            _ = qiziCanvas.Children.Add(GlobalValue.YuanWeiZhi);

            for (int i = 0; i < 32; i++)
            {
                GlobalValue.QiZiArray[i] = new QiZi(i);  // 32个棋子
                _ = qiziCanvas.Children.Add(GlobalValue.QiZiArray[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.PathPointImage[i, j] = new PathPoint(i, j);  // 走棋路径
                    _ = qiziCanvas.Children.Add(GlobalValue.PathPointImage[i, j]);
                }
            }
            GlobalValue.QiPanFanZhuan = false; // 棋盘翻转，初始为未翻转，黑方在上，红方在下
            QiPanChange(false);
            jipuwindow = new Window_JiPu(); // 棋谱记录窗口
            jipuwindow.SetValue(LeftProperty, SystemParameters.WorkArea.Width - 600);
            jipuwindow.SetValue(TopProperty, SystemParameters.WorkArea.Top);
            jipuwindow.SetValue(HeightProperty, SystemParameters.WorkArea.Height);
            jipuwindow.Hide();
            Spy_window = new SpyWindow(); // 棋盘数据监视窗口
            Spy_window.Hide();

            GlobalValue.Window_QiPuKu = new Window_QiPu(); // 棋谱库浏览窗口
            GlobalValue.Window_QiPuKu.SetValue(LeftProperty, SystemParameters.WorkArea.Left);
            GlobalValue.Window_QiPuKu.SetValue(TopProperty, SystemParameters.WorkArea.Top);
            GlobalValue.Window_QiPuKu.SetValue(HeightProperty, SystemParameters.WorkArea.Height);
            GlobalValue.Window_QiPuKu.Hide();

            GlobalValue.JiangJunTiShi = new() // 将军状态文字提示
            {
                Content = "战况",
                Height = 30.0,
                Foreground =Brushes.Goldenrod,
                Margin = new Thickness(100, 20, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            _ = JiangJunTiShi.Children.Add(GlobalValue.JiangJunTiShi);

            GlobalValue.jueShaImage = new(); // 绝杀图片
            _ = maingrid.Children.Add(GlobalValue.jueShaImage);
            
            DrawGrid.Children.Add(GlobalValue.Arrows.grid); // 走棋提示箭头

            GlobalValue.RedSideRect = new System.Windows.Shapes.Ellipse()
            {
                Width = 30,
                Height = 30,
                Margin = new Thickness(15, 500, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.Gray),
                Stroke = new SolidColorBrush(Colors.Goldenrod),
            };
            _ = maingrid.Children.Add(GlobalValue.RedSideRect);
            GlobalValue.BlackSideRect = new System.Windows.Shapes.Ellipse()
            {
                Width = 30,
                Height = 30,
                Margin = new Thickness(15, 260, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.Gray),
                Stroke = new SolidColorBrush(Colors.Goldenrod),
            };
            _ = maingrid.Children.Add(GlobalValue.BlackSideRect);

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
            GlobalValue.QiPanFanZhuan = !GlobalValue.QiPanFanZhuan;
            QiPanChange(GlobalValue.QiPanFanZhuan);  // 更换棋盘
            GlobalValue.YuanWeiZhi.FanZhuanPosition(); // 走棋原位置图片刷新
            foreach (QiZi item in GlobalValue.QiZiArray)
            {
                item.FanZhuanPosition(); // 棋盘翻转后，刷新显示所有棋子
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GlobalValue.PathPointImage[i, j].FanZhuPosition(); // 棋盘翻转后，刷新显示所有走棋路径
                }
            }
            GlobalValue.Arrows.HideAllPath(); //  隐藏提示箭头
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
                GlobalValue.RedSideRect.Margin = new Thickness(15, 260, 0, 0);
                GlobalValue.BlackSideRect.Margin = new Thickness(15, 500, 0, 0);
            }
            else
            {
                qipan_topBlack.Visibility = Visibility.Visible;
                qipan_topRed.Visibility = Visibility.Hidden;
                GlobalValue.RedSideRect.Margin = new Thickness(15, 500, 0, 0);
                GlobalValue.BlackSideRect.Margin = new Thickness(15, 260, 0, 0);
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
        /// 悔棋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HuiQiButton(object sender, RoutedEventArgs e)
        {
            GlobalValue.HuiQi();
        }

        private void SaveJiPuToBuffer(object sender, RoutedEventArgs e)
        {
            jipuwindow.Save_jipu();
        }
        /// <summary>
        /// 打开复盘窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFuPanWindow(object sender, RoutedEventArgs e)
        {
            if (GlobalValue.Window_QiPuKu.IsVisible)
            {
                GlobalValue.Window_QiPuKu.Hide();
            }
            else
            {
                GlobalValue.Window_QiPuKu.Show();
            }
        }
    }
}
