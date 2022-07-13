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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.CustomClass
{
    /// <summary>
    /// MyPrompt.xaml 的交互逻辑
    /// </summary>
    public partial class MyPrompt : UserControl
    {

        public MyPrompt()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }
        public void SetText(string txt)
        {
            textBlock.Text = txt;
            if (txt == null || string.IsNullOrEmpty(txt))
            {
                Visibility = Visibility.Hidden;
            }
        }
        public void SetBold()
        {
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
        }
        public void SetVisible()
        {
            if (textBlock.Text == null || string.IsNullOrEmpty(textBlock.Text)) Visibility = Visibility.Hidden;
            else Visibility = Visibility.Visible;
        }
        public void SetHidden() { Visibility = Visibility.Hidden; }
    }
}
