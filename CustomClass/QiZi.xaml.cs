﻿using System;
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
    /// 棋子类
    /// 主程序中有32个棋子实例
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
            string path = Environment.CurrentDirectory + @"\picture\" + GlobalValue.qiZiImageFileName[QiziId] + ".png";
            //string path = @"pack://application:,,,/picture/" + GlobalValue.QiZiImageFileName[QiziId] + ".png";
            BitmapImage bi = new(new Uri(path, UriKind.Absolute)); // 载入棋子图片
            bi.Freeze();
            QiZiImage.Source = bi;
            init_col = GlobalValue.qiZiInitPosition[id, 0]; // 开局时，棋子的位置
            init_row = GlobalValue.qiZiInitPosition[id, 1];
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
            foreach (QiZi item in GlobalValue.qiZiArray)
            {
                //item.Selected = false;
                //item.PutDown();
                //item.yuxuankuang.Visibility = Visibility.Hidden;
                item.Deselect();
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
            Selected = false;
            QiZiImage.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 8, BlurRadius = 10, Opacity = 0.6 });
            yuxuankuang.Visibility = Visibility.Hidden; // 本棋子的预先框隐藏
        }
        /// <summary>
        /// 选中时的处理
        /// </summary>
        public void Select()
        {
            Selected = true;
            DoubleAnimation DA = new DoubleAnimation  // 阴影动画
            {
                From = 8.0,
                To = 25.0,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(0.05))
            };
            QiZiImage.Effect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, DA);
            yuxuankuang.Visibility = Visibility.Visible;
            GlobalValue.CurrentQiZi = QiziId;
            //Scall(1.01);
            SuanFa.MoveCheck.GetAndShowPathPoints(GlobalValue.CurrentQiZi); // 获取可移动路径，并显示在棋盘上
            GlobalValue.yuanWeiZhi.SetPosition(Col, Row); // 棋子原位置标记，显示在当前位置
            GlobalValue.yuanWeiZhi.ShowYuanWeiZhiImage();
            GlobalValue.player.Open(new Uri("Sounds/select.mp3", UriKind.Relative));
            GlobalValue.player.Play();

        }

        /// <summary>
        /// 改变棋子的坐标位置
        /// 棋盘上设置了9列10行的坐标系，左上角第一个位置坐标为（0，0），右下角最后一个位置坐标为（8，9）
        /// </summary>
        /// <param name="x">列坐标</param>
        /// <param name="y">行坐标</param>
        public bool SetPosition(int x, int y)
        {
            //if (Visibility != Visibility.Visible) return false;

            if (QiziId > -1) // 仅仅对棋子有效
            {
                GlobalValue.QiPan[Col, Row] = -1;
                GlobalValue.QiPan[x, y] = QiziId;
            }
            Col = x;
            Row = y;
            if (GlobalValue.IsQiPanFanZhuan) // 如果棋盘翻转为上红下黑，则进行坐标转换
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x] - 33);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y] - 33);
            QiZiImage.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 8, BlurRadius = 10, Opacity = 0.6 });
            return true;

        }


        /// <summary>
        /// 设置棋子到开局时的初始位置
        /// </summary>
        public void SetInitPosition()
        {
            Visibility = Visibility.Visible;  // 棋子复活
            SetPosition(init_col, init_row);
            Deselect();
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaller">缩放参数，1.0=原始尺寸</param>
        public void Scall(double scaller)
        {
            if (scaller is > 0 and < 10)
            {
                TransformGroup group = QiZiImage.FindResource("UserControlRenderTransform1") as TransformGroup;
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

