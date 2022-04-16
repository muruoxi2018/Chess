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

namespace Chess
{
    /// <summary>
    /// Window_JiPu.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
        public Window_JiPu()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, RoutedEventArgs e)
        {
            JiPuData.ItemsSource = Qipu.QiPuList;
            
        }
    }
}
