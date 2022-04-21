using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Threading;
using System.Windows.Media.Animation;

namespace Chess
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class QiZi : UserControl
    {
        private readonly int init_col;  // 开局时棋子的列坐标
        private readonly int init_row;  // 开局时棋子的行坐标

        public int Col { get; set; }  // 棋子的列坐标
        public int Row { get; set; }  // 棋子的行坐标
        public int QiziId { get; set; }  // 棋子编号
        public bool Selected { get; set; }  // 棋子的选中状态
        public bool SideColor { get; set; }  // 棋子属于哪一方，false：黑棋，true：红棋

        /// <summary>
        /// 棋子类构造函数
        /// 空实例
        /// </summary>
        public QiZi()
        {
            InitializeComponent();
            QiziId = -1;
        }
        /// <summary>
        /// 棋子类构造函数
        /// 根据棋子编号，载入对应的棋子图像，设定在棋盘的初始位置
        /// </summary>
        /// <param name="id">棋子编号</param>
        public QiZi(int id)
        {
            InitializeComponent();
            if (id is < 0 or > 31)
            {
                return;
            }
            QiziId = id;
            string path = Environment.CurrentDirectory + "\\picture\\" + GlobalValue.QiZiImageFileName[QiziId] + ".png";
            BitmapImage bi = new(new Uri(path, UriKind.Absolute));
            bi.Freeze();
            qzimage.Source = bi;
            init_col = GlobalValue.QiZiInitPosition[id, 0];
            init_row = GlobalValue.QiZiInitPosition[id, 1];
            SetPosition(init_col, init_row);
            SideColor = id >= 16;
            Selected = false;
            //yuxuankuang.Visibility = Visibility.Hidden;
            //Scall(1);
        }

        /// <summary>
        /// 点击棋子时，其他棋子取消选中状态，本棋子设定选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (QiZi item in GlobalValue.QiZiArray)
            {
                item.Selected = false;
                item.PutDown();
                item.yuxuankuang.Visibility = Visibility.Hidden;
            }
            if (SideColor == GlobalValue.SideTag)
            {
                Select();
            }
        }

        /// <summary>
        /// 取消选中状态
        /// </summary>
        public void Deselect()
        {
            PutDown();
            Selected = false;
            yuxuankuang.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 选中时的处理
        /// </summary>
        public void Select()
        {
            Selected = true;
            DoubleAnimation DA = new DoubleAnimation
            {
                From = 8.0,
                To = 25.0,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(0.05))
            };
            qzimage.Effect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, DA);
            yuxuankuang.Visibility = Visibility.Visible;
            GlobalValue.CurrentQiZi = GetId();
            //Scall(1.01);
            SuanFa.MoveCheck.GetAndShowPathPoints(GlobalValue.CurrentQiZi);
            GlobalValue.YuanWeiZhi.SetPosition(Col, Row);
            GlobalValue.YuanWeiZhi.ShowYuanWeiZhiImage();
        }

        /// <summary>
        /// 棋子放下时，去除阴影
        /// </summary>
        public void PutDown()
        {
            qzimage.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 8, BlurRadius = 10, Opacity = 0.6 });
            //Scall(1);
        }

        /// <summary>
        /// 获取本棋子编号
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return QiziId;
        }

        /// <summary>
        /// 设置本棋子的坐标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            if (Visibility != Visibility.Visible) return;
            if (QiziId > -1) // 仅仅对棋子有效
            {
                GlobalValue.QiPan[Col, Row] = -1;
                int x0 = Col;
                int y0 = Row;
                Col = x;
                Row = y;
                GlobalValue.QiPan[x, y] = QiziId;

                if (Selected)  // 移动时的动画
                {
                    DoubleAnimation PAx = new DoubleAnimation
                    {
                        From = GlobalValue.QiPanGrid_X[x0],
                        To = GlobalValue.QiPanGrid_X[x],
                        FillBehavior = FillBehavior.Stop,
                        Duration = new Duration(TimeSpan.FromSeconds(0.15))
                    };
                    DoubleAnimation PAy = new DoubleAnimation
                    {
                        From = GlobalValue.QiPanGrid_Y[y0],
                        To = GlobalValue.QiPanGrid_Y[y],
                        FillBehavior = FillBehavior.Stop,
                        Duration = new Duration(TimeSpan.FromSeconds(0.15))
                    };

                    if (GlobalValue.QiPanFanZhuan)
                    {
                        PAx.From = GlobalValue.QiPanGrid_X[8 - x0];
                        PAx.To = GlobalValue.QiPanGrid_X[8 - x];
                        PAy.From = GlobalValue.QiPanGrid_Y[9 - y0];
                        PAy.To = GlobalValue.QiPanGrid_Y[9 - y];
                    }
                    BeginAnimation(Canvas.LeftProperty, PAx);
                    BeginAnimation(Canvas.TopProperty, PAy);
                }
            }

            if (GlobalValue.QiPanFanZhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x]);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y]);
            PutDown();
        }


        /// <summary>
        /// 设置棋子到开局时的初始位置
        /// </summary>
        public void SetInitPosition()
        {
            SetPosition(init_col, init_row);
            Deselect();
            Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaller">缩放参数，1.0=原始尺寸</param>
        public void Scall(double scaller)
        {
            if (scaller is > 0 and < 10)
            {
                TransformGroup group = qzimage.FindResource("UserControlRenderTransform1") as TransformGroup;
                ScaleTransform scaler = group.Children[0] as ScaleTransform;
                scaler.ScaleX = scaller;
                scaler.ScaleY = scaller;
            }
        }
        public void FanZhuanPosition()
        {
            SetPosition(Col, Row);
        }
        /// <summary>
        /// 棋子被杀死
        /// </summary>
        public void SetDied()
        {
            Visibility = Visibility.Collapsed;
            //yuxuankuang.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 棋子复活，用于复盘、悔棋等操作
        /// </summary>
        public void Setlived()
        {
            Visibility = Visibility.Visible;
        }

        public void ShowYuanWeiZhiImage()
        {
            yuanweizhi.Visibility = Visibility.Visible;
        }
        public void HiddenYuanWeiZhiImage()
        {
            yuanweizhi.Visibility = Visibility.Hidden;
        }
    }
}

