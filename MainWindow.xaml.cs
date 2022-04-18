﻿using Chess.SuanFa;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Reset();
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
            Reset();
        }
        private static void Reset()
        {
            foreach (QiZi item in GlobalValue.QiZiArray)
            {
                item.SetInitPosition();
            }
            GlobalValue.SideTag = GlobalValue.REDSIDE;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.QiPan[i, j] = -1;
                    GlobalValue.PathPointImage[i, j].SetHidden();
                    GlobalValue.PathPointImage[i, j].HasPoint = false;
                }
            }
            for (int i = 0; i < 32; i++)
            {
                GlobalValue.QiPan[GlobalValue.QiZiArray[i].Col, GlobalValue.QiZiArray[i].Row] = i;
            }
            GlobalValue.YuanWeiZhi.HiddenYuanWeiZhiImage();
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

        private static Window_JiPu jipuwindow;

        private void OpenJiPuWindow(object sender, RoutedEventArgs e)
        {
            if (jipuwindow != null)
            {
                return;
            }
            jipuwindow = new Window_JiPu();
            double value = MainWin.Left + MainWin.Width;
            jipuwindow.SetValue(LeftProperty, value);
            jipuwindow.SetValue(TopProperty, MainWin.Top);
            jipuwindow.Show();

        }
        private static SpyWindow Spy_window;

        private void OpenSpyWindow(object sender, RoutedEventArgs e)
        {
            if (Spy_window != null)
            {
                return;
            }
            Spy_window = new SpyWindow();
            Spy_window.Show();
        }

        private void SetupOption(object sender, RoutedEventArgs e)
        {
            SetupWindow sw = new();
            sw.Show();
        }

        private void HuiQi(object sender, RoutedEventArgs e)
        {
            if (Qipu.QiPuList.Count < 1)
            {
                return;
            }

            Qipu.Step step = Qipu.QiPuList[Qipu.QiPuList.Count - 1].StepRecode;
            GlobalValue.QiZiArray[step.QiZi].Select();  // 重新计算可移动路径
            GlobalValue.QiZiArray[step.QiZi].SetPosition(step.x0, step.y0);
            GlobalValue.QiZiArray[step.QiZi].Select();  // 重新计算可移动路径
            GlobalValue.QiZiArray[step.QiZi].Deselect();

            if (step.DieQz > -1)
            {
                GlobalValue.QiZiArray[step.DieQz].Setlived();
                GlobalValue.QiZiArray[step.DieQz].SetPosition(step.x1, step.y1);
            }
            GlobalValue.QiPan[step.x0, step.y0] = step.QiZi;
            GlobalValue.QiPan[step.x1, step.y1] = step.DieQz;
            Qipu.QiPuList.RemoveAt(Qipu.QiPuList.Count - 1);
            GlobalValue.SideTag = !GlobalValue.SideTag;

        }

        private void OnMainWindowClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (jipuwindow != null)
            {
                jipuwindow.Close();
            }
            if (Spy_window != null)
            {
                Spy_window.Close();
            }
        }

    }
}
