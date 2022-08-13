using Chess.OpenSource;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chess.SubWindow;
using Chess.CustomClass;
using System.Windows.Controls;

namespace Chess
{
    /// <summary>
    /// 主窗口类
    /// </summary>
    public partial class MainWindow : Window
    {

        public static int menuItem;
        public MainWindow()
        {
            InitializeComponent();
            
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

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            //this.Width = SystemParameters.PrimaryScreenWidth;
            //this.WindowState = WindowState.Maximized;
            menuItem = 0;
            ReturnButton.Visibility = Visibility.Hidden;
        }

        private void ReturnMainMenu(object sender, RoutedEventArgs e)
        {
            MainFram.Source = null;
            MainMenu.Visibility = Visibility.Visible;
            ReturnButton.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 人机对战
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonVsPC(object sender, RoutedEventArgs e)
        {
            menuItem = 1;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 电脑对战
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PCVsPC(object sender, RoutedEventArgs e)
        {
            menuItem = 2;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 自由打谱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreeDaPu(object sender, RoutedEventArgs e)
        {
            menuItem = 3;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }

        private void FuPan(object sender, RoutedEventArgs e)
        {
            menuItem = 4;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void AllwayOnTop(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }
        /// <summary>
        /// 残局设计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanJuSheJi(object sender, RoutedEventArgs e)
        {
            menuItem = 5;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("CanJuSheJi.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 残局练习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanJuLianXi(object sender, RoutedEventArgs e)
        {
            menuItem = 6;
            MainMenu.Visibility = Visibility.Hidden;
            MainFram.Source = new Uri("CanJuSheJi.xaml", UriKind.RelativeOrAbsolute);
            ReturnButton.Visibility = Visibility.Visible;
        }
    }
}
