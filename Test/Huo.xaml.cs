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

namespace Chess.Test
{
    /// <summary>
    /// Huo.xaml 的交互逻辑
    /// </summary>
    public partial class Huo : Page
    {
        private int index = 0;
        private int rate = 0;
        private Rect rect = new Rect
        {
            Width = 210,
            Height = 210,
            X = 0,
            Y = 0
        };
        private int[,] pos = new int[7, 2] {
            { -2, 206 },
            { 191, 201 },
            { 408, -6 },
            { 385, 192 },
            { 209, -5 },
            { 0, 0 },
            { 608, -5 } };
        public Huo()
        {
            InitializeComponent();
            System.Windows.Media.CompositionTarget.Rendering += DongHua; // 按每秒60帧速率调用

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        protected void DongHua(object Sender, EventArgs e)
        {
            rate++;
            if (rate == 12) // 降低帧率
            {
                rate = 0;
                index++;
                index %= 7;
                rect.X = pos[index, 0];
                rect.Y = pos[index, 1];
                HuoBrush.Viewbox = rect;
            }
        }
    }
}

