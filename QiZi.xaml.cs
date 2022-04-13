using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Media;

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
        public int Qiziid { get; set; }  // 棋子编号
        public bool Selected { get; set; }  // 棋子的选中状态
        public bool Pcolor { get; set; }  // 棋子属于哪一方，false：黑棋，true：红棋

        /// <summary>
        /// 棋子类构造函数
        /// 空实例
        /// </summary>
        public QiZi()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 棋子类构造函数
        /// 根据棋子编号，载入对应的棋子图像，设定在棋盘的初始位置
        /// </summary>
        /// <param name="id">棋子编号</param>
        public QiZi(int id)
        {
            InitializeComponent();
            if ((id < 0) || (id > 31)) return;
            Qiziid = id;
            string path = Environment.CurrentDirectory + "\\picture\\" + GlobalValue.qzimage[Qiziid] + ".png";
            BitmapImage bi = new(new Uri(path, UriKind.Absolute));
            bi.Freeze();
            image.Source = bi;
            init_col = GlobalValue.qiziInitPosition[id, 0];
            init_row = GlobalValue.qiziInitPosition[id, 1];
            Setposition(init_col, init_row);
            Pcolor = id < 16 ? GlobalValue.BLACKSIDE : GlobalValue.REDSIDE;
            yuxuankuang.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 点击棋子时，其他棋子取消选中状态，本棋子设定选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (QiZi item in GlobalValue.myqz)
            {
                item.Deselect();
            }
            if (Pcolor == GlobalValue.sidetag)
            {
                Select();
            }
        }

        /// <summary>
        /// 取消选中状态
        /// </summary>
        public void Deselect()
        {
            Selected = false;
            PutDown();
            yuxuankuang.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 选中时的处理
        /// </summary>
        public void Select()
        {
            Selected = true;
            TransformGroup group = image.FindResource("UserControlRenderTransform1") as TransformGroup;
            ScaleTransform scaler = group.Children[0] as ScaleTransform;
            scaler.ScaleX *= 1.01;
            scaler.ScaleY *= 1.01;
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 8, Opacity = 0.7 });
            yuxuankuang.Visibility = Visibility.Visible;
            GlobalValue.CurrentQiZi = GetId();
            MoveCheck.Getpath(GlobalValue.CurrentQiZi);
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathImage[i, j].SetVisable();
                }
            }
            

        }

        /// <summary>
        /// 棋子放下时，去除阴影
        /// </summary>
        public void PutDown()
        {
            image.SetValue(EffectProperty, null);
            TransformGroup group = image.FindResource("UserControlRenderTransform1") as TransformGroup;
            ScaleTransform scaler = group.Children[0] as ScaleTransform;
            scaler.ScaleX = 1.0;
            scaler.ScaleY = 1.0;
            
        }

        /// <summary>
        /// 获取本棋子编号
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return Qiziid;
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
            if (GlobalValue.qipanfanzhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.qipanGridX[x]);
            SetValue(Canvas.TopProperty, GlobalValue.qipanGridY[y]);
        }

        /// <summary>
        /// 设置棋子到开局时的初始位置
        /// </summary>
        public void SetInitPosition()
        {
            Setposition(init_col, init_row);
            Deselect();
            Visibility = Visibility.Visible;
        }
        public void FanZhuanPosition()
        {
            Setposition(Col, Row);
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

    }
}

