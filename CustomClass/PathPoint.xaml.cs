using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// 路径标记点
    /// </summary>
    public partial class PathPoint : UserControl
    {
        private bool _haspoint = false;
        public bool HasPoint
        {
            get { return _haspoint; }
            set
            {
                _haspoint = value;
                if (value) Visibility = Visibility.Visible;
                else Visibility = Visibility.Hidden;
            }
        }  // 是否是有效的走棋路径点
        public int Col { get; set; }    // 路径点的列坐标
        public int Row { get; set; }    // 路径点的行坐标

        /// <summary>
        /// 棋子移动目的地标记类，
        /// 在棋子可移动到的有效位置，设置标记。
        /// 点击此标记时，当前棋子移动到标记位置。
        /// </summary>
        /// <param name="x">列位置</param>
        /// <param name="y">行位置</param>
        public PathPoint(int x, int y)
        {
            InitializeComponent();
            if (x is < 0 or > 8)
            {
                return;
            }
            if (y is < 0 or > 9)
            {
                return;
            }
            HasPoint = false;
            Setposition(x, y);

        }

        /// <summary>
        /// 设置本棋子的坐标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Setposition(int x, int y)
        {
            Col = x;
            Row = y;
            if (GlobalValue.IsQiPanFanZhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x] - 30);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y] - 30);
        }
        public void FanZhuPosition()
        {
            Setposition(Col, Row);
        }

        /// <summary>
        /// 当鼠标进入标记范围内时，显示阴影效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 4, Opacity = 0.7 });
        }

        /// <summary>
        /// 当鼠标离开标记范围时，去除阴影效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 2, Opacity = 0.7 });
        }

        /// <summary>
        /// 鼠标点击时，棋子移动处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseup(object sender, MouseButtonEventArgs e)
        {
            // 当前有预选棋子时，将预选棋子运子到(m,n)位置================= 运子
            GlobalValue.QiZiMoveTo(GlobalValue.CurrentQiZi, Col, Row, GlobalValue.QiPan[Col, Row], true);
            // 点击位置有棋子时，将预选棋子运子到(m,n)位置，并吃掉目标位置的对方棋子===== 吃子
        }

    }
}
