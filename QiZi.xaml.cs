﻿using System;
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
            image.Source = bi;
            init_col = GlobalValue.QiZiInitPosition[id, 0];
            init_row = GlobalValue.QiZiInitPosition[id, 1];
            Setposition(init_col, init_row);
            SideColor = id >= 16;
            yuxuankuang.Visibility = Visibility.Hidden;
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
            PutDown();
            yuxuankuang.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 选中时的处理
        /// </summary>
        public void Select()
        {
            Selected = true;
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 15, Opacity = 0.6 });
            yuxuankuang.Visibility = Visibility.Visible;
            GlobalValue.CurrentQiZi = GetId();
            _ = MoveCheck.Getpath(GlobalValue.CurrentQiZi);
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.PathPointImage[i, j].SetVisable();
                }
            }
            GlobalValue.YuanWeiZhi.Setposition(Col, Row);
            GlobalValue.YuanWeiZhi.ShowYuanWeiZhiImage();
        }

        /// <summary>
        /// 棋子放下时，去除阴影
        /// </summary>
        public void PutDown()
        {
            image.SetValue(EffectProperty, null);
            Scall(1);
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
        public void Setposition(int x, int y)
        {
            GlobalValue.QiPan[Col, Row] = -1;
            GlobalValue.QiPan[x, y] = QiziId;
            Col = x;
            Row = y;
            if (GlobalValue.QiPanFanZhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x]);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y]);
            
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
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaller">缩放参数，1.0=原始尺寸</param>
        public void Scall(double scaller)
        {
            if (scaller is > 0 and < 10)
            {
                TransformGroup group = image.FindResource("UserControlRenderTransform1") as TransformGroup;
                ScaleTransform scaler = group.Children[0] as ScaleTransform;
                scaler.ScaleX = scaller;
                scaler.ScaleY = scaller;
            }
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

