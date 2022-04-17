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
using System.Windows.Threading;

namespace Chess
{
    /// <summary>
    /// Window_JiPu.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public Window_JiPu()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(Timer_tick);
            timer.Interval = TimeSpan.FromSeconds(0.3);
            //timer.Start();

        }

        private void Timer_tick(object sender, EventArgs e)
        {
            JiPuData.Items.Refresh();
            
        }

        private void FormLoad(object sender, RoutedEventArgs e)
        {
            JiPuData.ItemsSource = Qipu.QiPuList;
            
        }
    }
}
