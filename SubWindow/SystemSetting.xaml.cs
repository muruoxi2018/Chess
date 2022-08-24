using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess.SubWindow
{
    /// <summary>
    /// SystemSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSetting : Window
    {
        public SystemSetting()
        {
            InitializeComponent();
        }
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //MoveDelayTime.Value = Settings.Default.MoveDelayTime;
            //ArrowsShowOrHidden.IsChecked = Settings.Default.ArrowVisable;
            //ArrowMaxNumSlider.Value = Settings.Default.ArrowsMaxNum;
        }
        private void OnWindowUnloaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }
        /// <summary>
        /// 选择窗口背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBKImage(object sender, RoutedEventArgs e)
        {
            string imageDefaultPath = $"{AppDomain.CurrentDomain.BaseDirectory}picture\\BackGround\\";
            OpenFileDialog openFileDialog = new()
            {
                Filter = "图像文件|*.jpg;*.png;*.bmp|所有文件|*.*",
                InitialDirectory = imageDefaultPath,
                DefaultExt = string.Empty,
                RestoreDirectory = true,
                Title = "选择窗口背景图片"
            };
            if ((bool)openFileDialog.ShowDialog())
            {
                FileInfo sourceFile = new(openFileDialog.FileName);
                string targetFile = imageDefaultPath + sourceFile.Name;
                if (!File.Exists(targetFile))
                {
                    // 如果在\picture\BackGround\文件夹下没有该文件，再复制到该文件夹。
                    File.Copy(sourceFile.FullName, targetFile, true);
                }
                Settings.Default.mainBKImage = sourceFile.Name;
                Settings.Default.Save();

            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
