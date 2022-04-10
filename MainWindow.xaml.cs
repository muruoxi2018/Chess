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
                GlobalValue.myqz[i].image.MouseLeftButtonUp += QiziMouseUp;

                _ = qiziCanvas.Children.Add(GlobalValue.myqz[i]);
            }
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathImage[i, j] = new PathPoint(i, j);
                    _ = qiziCanvas.Children.Add(GlobalValue.pathImage[i, j]);
                    GlobalValue.pathImage[i, j].image.MouseLeftButtonUp += QiPanMouseUp;

                }
            }

                    //testbox1.Text = GlobalValue.qipan.ToString();
                    Reset();
        }

        private void QiziMouseUp(object sender, MouseButtonEventArgs e) // 点击棋子时
        {
            foreach (QiZi item in GlobalValue.myqz)
            {
                item.Deselect();
            }
            
            //testbox1.Text = e.OriginalSource.ToString();
            
        }
        private void pathPiontMouseUp(object sender, MouseButtonEventArgs e) // 点击棋子时
        {
            
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

            /*foreach (QiZi item in GlobalValue.myqz)
            {
                if (item.Selected)
                {

                    item.Setposition(m, n);
                    item.PutDown();
                }
            }*/
            if (GlobalValue.qipan[m, n] == -1)
            {
                // 点击位置没有棋子时
                if (GlobalValue.CurrentQiZi > 32)
                {
                    // 当前没有预选棋子时
                    // 点击无效，不需要做任何处理
                }
                else
                {
                    // 当前有预选棋子时，将预选棋子运子到(m,n)位置================= 运子
                    QiZiMoveTo(GlobalValue.CurrentQiZi, m, n, -1, true);
                    
                }
            }
            else
            {
                // 点击位置有棋子时
                if (GlobalValue.myqz[GlobalValue.qipan[m, n]].Pcolor != GlobalValue.sidetag)
                {
                    //  点击的棋子不是当前走棋方的棋子时
                    if (GlobalValue.CurrentQiZi > 32)
                    {
                        // 如果没有预选棋子
                        // 点击无效，声音提示
                    }
                    else
                    {
                        // 如果有预选棋子，将预选棋子运子到(m,n)位置，并吃掉目标位置的对方棋子===== 吃子
                        int dieqz = GlobalValue.qipan[m, n];
                        QiZiMoveTo(GlobalValue.CurrentQiZi, m, n, dieqz, true);
                    }
                }
                else
                {
                    // 点击了走棋方的棋子时，将该棋子设为预选棋子 ================ 选子
                    GlobalValue.CurrentQiZi = GlobalValue.qipan[m, n];
                    
                    //            Form2.mp1.FileName := 'sounds/select.mp3';
                    //            Form2.mp1.Open;
                    //            Form2.mp1.Play;
                }

            }


        }
        private void QiZiMoveTo(int QiZi, int m, int n, int DieQz, bool sound)  // 运子
        {

            if (!GlobalValue.qzpath[m, n])
            {
                // 不符合走子规范时
                // 操作错误，声音提示
                if (sound)
                {
                    //FileName = 'sounds/check2.wav';
                }
            }
            else
            {
                // 符合走子规范时
                // 运子到(m,n)位置
                int x0 = GlobalValue.myqz[QiZi].Col;
                int y0 = GlobalValue.myqz[QiZi].Row;

                GlobalValue.qipan[x0, y0] = -1;
                //yuanweizhi.setpoint(x0, y0);
                //yuanweizhi.Show;
                GlobalValue.myqz[QiZi].Setposition(m, n);
                GlobalValue.qipan[m, n] = QiZi;
                //xianweizhi.setpoint(m, n);
                //xianweizhi.Show;
                if (DieQz != -1)
                {
                    GlobalValue.myqz[DieQz].SetDied();
                    if (sound)
                    {
                        /*Form2.mp1.FileName := 'sounds/eat.mp3';
                Form2.mp1.Open;
                        Form2.mp1.Play;*/
                    }
                    if (DieQz == 4)  // 黑将被吃，则红方胜
                    {
                        GlobalValue.gameover = true;
                        //Form2.lbl3.Caption := '战斗结束！红方胜！！';
                    }
                    if (DieQz == 20)  // 红帅被吃，则黑方胜
                    {
                        GlobalValue.gameover = true;
                        //Form2.lbl3.Caption := '战斗结束！黑方胜！！';
                    }

                }
                else
                {
                    if (sound)
                    {
                        /*Form2.mp1.FileName := 'sounds/go.wav';
                Form2.mp1.Open;
                        Form2.mp1.Play;*/
                    }
                }
                //checkjiangjun(QiZi);

                //AddJilu(QiZi, x0, y0, m, n, DieQz);

                GlobalValue.sidetag = !GlobalValue.sidetag;  // 变换走棋方
                
                GlobalValue.CurrentQiZi = 100;
                // 取消棋子预选状态
                testbox1.Text = GlobalValue.sidetag.ToString();
                
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
