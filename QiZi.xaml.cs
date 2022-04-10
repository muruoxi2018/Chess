using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class QiZi : UserControl
    {
        private readonly int init_col;
        private readonly int init_row;

        public int Col { get; set; }
        public int Row { get; set; }
        public int Qiziid { get; set; }
        public bool Selected { get; set; }
        public bool Pcolor { get; set; }
        public QiZi()
        {
            InitializeComponent();
        }
        public QiZi(int id)
        {
            InitializeComponent();
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
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Pcolor == GlobalValue.sidetag)  Select();
        }

        public void Deselect()
        {
            Selected = false;
            image.SetValue(EffectProperty, null);
            yuxuankuang.Visibility = Visibility.Hidden;
        }
        public void Select()
        {
            Selected = true;
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
        public void PutDown()
        {
            image.SetValue(EffectProperty, null);
        }

        public int GetId()
        {
            return Qiziid;
        }
        public void Setposition(int x, int y)
        {
            Col = x;
            Row = y;
            SetValue(Canvas.LeftProperty, GlobalValue.qipanGridX[x]);
            SetValue(Canvas.TopProperty, GlobalValue.qipanGridY[y]);
        }
        public void SetInitPosition()
        {
            Setposition(init_col, init_row);
            Deselect();
            Visibility = Visibility.Visible;
        }
        public void SetDied()
        {
            Visibility = Visibility.Collapsed;
            yuxuankuang.Visibility = Visibility.Hidden;
        }
        public void Setlived()
        {
            Visibility = Visibility.Visible;
        }

    }
}

