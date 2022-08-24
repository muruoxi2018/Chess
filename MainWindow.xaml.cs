using Chess.SubWindow;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                    menuItem = GlobalValue.PERSON_PC;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "2":
                    menuItem = GlobalValue.PC_PC;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "3":
                    menuItem = GlobalValue.FREE_DAPU;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "4":
                    menuItem = GlobalValue.QIPU_RECORD;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "5":
                    menuItem = GlobalValue.CANJU_DESIGN;
                    MainFrame.Source = new Uri("CanJuSheJi.xaml", UriKind.RelativeOrAbsolute);
                    break;
                case "6":
                    menuItem = GlobalValue.CANJU_POJIE;
                    MainFrame.Source = new Uri("QiPanPage.xaml", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    menuItem = 0;
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
    public class StringToImageSourceConverter : IValueConverter
    {
        #region Converter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {
                FileInfo fileInfo = new FileInfo(path);

                return new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory+"/picture/BackGround/" + fileInfo.Name, UriKind.Absolute));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
