using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            Opacity = 1.0;
            IsHitTestVisible = false; // 是否接受鼠标事件
        }

        public void ShowJueShaImage()
        {
            Visibility = Visibility.Visible;
            image.Visibility = Visibility.Visible;
            #region 绝杀时播放动画
            DoubleAnimation PAx = new()
            {
                From = 1.0,
                To = 0.0,
                FillBehavior = FillBehavior.HoldEnd,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };
            image.BeginAnimation(OpacityProperty, PAx); // 透明度动画

            ScaleTransform scale = new();
            DoubleAnimation DAscaleX = new()
            {
                From = 1.75,
                To = 7.0,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(4))
            };
            DoubleAnimation DAscaleY = new()
            {
                From = 1.45,
                To = 5.8,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(4))
            };
            image.RenderTransform = scale;
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, DAscaleX); // x方向放大
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, DAscaleY); // y方向放大
            #endregion
        }
    }
}
