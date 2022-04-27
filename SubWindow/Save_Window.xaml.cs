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
using Chess.SuanFa;

namespace Chess.SubWindow
{
    /// <summary>
    /// Save_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Save_Window : Window
    {
        public Save_Window()
        {
            InitializeComponent();
            qipustr.Text =Qipu.CnToString();
        }

        private void saveButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
