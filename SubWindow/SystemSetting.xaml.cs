using System;
using System.Collections.Generic;
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
    }
}
