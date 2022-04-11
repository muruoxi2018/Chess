using System;
using System.Windows;
using System.Windows.Input;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
            qizi0.SetValue(VisibilityProperty, Visibility.Collapsed);
            for (int i = 0; i < 32; i++)
            {
                GlobalValue.myqz[i] = new QiZi(i);
                _ = qiziCanvas.Children.Add(GlobalValue.myqz[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathImage[i, j] = new PathPoint(i, j);
                    _ = qiziCanvas.Children.Add(GlobalValue.pathImage[i, j]);
                }
            }
            Reset();
        }

        private void QiPanMouseUp(object sender, MouseButtonEventArgs e)  // 点击棋盘空位置时
        {
            double x0 = 110.0;
            double y0 = 97.0;
            Point pnt = e.GetPosition(qipan);
            int m = (int)(((pnt.X - x0) / GlobalValue.girdwidth) + 0.5);  // 加0.5以后向下取整，等同于四舍五入
            int n = (int)(((pnt.Y - y0) / GlobalValue.girdwidth) + 0.5);
            if (m > 8) { m = 8; }
            if (n > 9) { n = 9; }
            if (m < 0) { m = 0; }
            if (n < 0) { n = 0; }

            double plx = Math.Abs(pnt.X - (x0 + (m * GlobalValue.girdwidth)));   // 计算与交叉点的偏离
            double ply = Math.Abs(pnt.Y - (y0 + (n * GlobalValue.girdwidth)));
            if ((plx > 25.0) || (ply > 25.0))
            {
                return;
            }


        }
         private void ResetBtnClick(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            foreach (QiZi item in GlobalValue.myqz)
            {
                item.SetInitPosition();
            }
            GlobalValue.sidetag = GlobalValue.REDSIDE;
            GlobalValue.qipanfanzhuan = false;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.qipan[i, j] = -1;
                    GlobalValue.pathImage[i, j].SetHidden();
                    GlobalValue.pathImage[i, j].hasPoint = false;

                    //GlobalValue.pathImage[i, j].hasPoint = true;
                    //GlobalValue.pathImage[i, j].SetVisable();
                }
            }
            //Console.WriteLine(GlobalValue.qipan.ToString());

            for (int i = 0; i < 32; i++)
            {

                GlobalValue.qipan[GlobalValue.myqz[i].Col, GlobalValue.myqz[i].Row] = i;

            }


        }
    }
}
