using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.CustomClass
{
    /// <summary>
    /// JueSha.xaml 的交互逻辑
    /// </summary>
    public partial class JueSha : UserControl
    {
        public JueSha()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            Opacity = 0.8;
            IsHitTestVisible = false;
        }

        public void SetJueSha()
        {
            Visibility = Visibility.Visible;
            image.Visibility = Visibility.Visible;
            //image.Opacity = 1.0;
            DoubleAnimation PAx = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                FillBehavior = FillBehavior.HoldEnd,
                Duration = new Duration(TimeSpan.FromSeconds(3))
            };
            image.BeginAnimation(OpacityProperty, PAx);
        }
    }
}
