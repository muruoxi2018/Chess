using Chess.SubWindow;
using System;
using System.Windows;
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
            Settings.Default.Save(); // 保存用户更改的设置
            Environment.Exit(0); // 关闭所有窗口，并释放所有资源，包括相关辅助窗口。
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            menuItem = 0;
            ReturnButton.Visibility = Visibility.Hidden;
        }

        private void ReturnMainMenu(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = null;
            MainMenu.Visibility = Visibility.Visible;
            ReturnButton.Visibility = Visibility.Hidden;
            menuItem = 0;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void AllwayOnTop(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }

        private void MainMenuClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Tag.ToString())
            {
                case "1":
                    menuItem = 1;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "2":
                    menuItem = 2;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "3":
                    menuItem = 3;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "4":
                    menuItem = 4;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "5":
                    menuItem = 5;
                    MainFrame.Source = new Uri("CanJuSheJi.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "6":
                    menuItem = 6;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    menuItem = 100;
                    MainMenu.Visibility = Visibility.Visible;
                    ReturnButton.Visibility = Visibility.Hidden;
                    break;
            }
            if (menuItem is >= 1 and <= 6)
            {
                MainMenu.Visibility = Visibility.Hidden;
                ReturnButton.Visibility = Visibility.Visible;
            }
        }

        private void SystemSetup(object sender, RoutedEventArgs e)
        {
            SystemSetting setWindow = new();
            setWindow.ShowDialog();
        }
    }
}
