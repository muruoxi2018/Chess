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
    /// SetupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
        }

        private void BkColorSelect(object sender, SelectionChangedEventArgs e)
        {
            //MainWindow.BackgroundProperty=((Rectangle)QiPanBKColor.SelectedValue)
            Rectangle value =(e.Source as ComboBox).SelectedValue as Rectangle;
            textb1.Text = value.Fill.ToString();
            Application.Current.Resources = (ResourceDictionary)Application.Current.Properties[value.Fill];
            
        }
    }
}
