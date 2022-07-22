using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

        public void ShowJueShaImage()
        {
            Visibility = Visibility.Visible;
            image.Visibility = Visibility.Visible;
            //image.Opacity = 1.0;
            DoubleAnimation PAx = new()
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
