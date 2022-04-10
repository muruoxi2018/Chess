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
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class PathPoint : UserControl
    {
        public bool hasPoint { get; set; }
        public PathPoint(int x,int y)
        {
            InitializeComponent();
            SetValue(Canvas.LeftProperty, GlobalValue.qipanGridX[x]);
            SetValue(Canvas.TopProperty, GlobalValue.qipanGridY[y]);
            hasPoint = false;

        }
        public void SetHidden()
        {
            Visibility = Visibility.Collapsed;
        }
        public void SetVisable()
        {
            if (hasPoint)
            {
                Visibility = Visibility.Visible;
            }
        }

        private void onMouseEnter(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 3, Opacity = 0.7 });
        }

        private void onMouseLeave(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, null);
        }
    }
}
