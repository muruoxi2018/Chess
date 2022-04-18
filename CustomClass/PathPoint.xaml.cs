using Chess.SuanFa;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Chess
{
    /// <summary>
    /// 路径标记点
    /// </summary>
    public partial class PathPoint : UserControl
    {
        public bool HasPoint { get; set; }  // 是否是有效的走棋路径点
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
            if (GlobalValue.QiPanFanZhuan)
            {
                x = 8 - x;
                y = 9 - y;
            }
            SetValue(Canvas.LeftProperty, GlobalValue.QiPanGrid_X[x] + 5.0);
            SetValue(Canvas.TopProperty, GlobalValue.QiPanGrid_Y[y] + 5.0);
        }
        public void FanZhuPosition()
        {
            Setposition(Col, Row);
        }
        /// <summary>
        /// 隐藏标记
        /// </summary>
        public void SetHidden()
        {
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 显示标记
        /// 条件是此处为有效移动路径
        /// </summary>
        public void SetVisable()
        {
            if (!HasPoint)
            {
                return;
            }
            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 当鼠标进入标记范围内时，显示阴影效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, new DropShadowEffect() { ShadowDepth = 3, Opacity = 0.7 });
        }

        /// <summary>
        /// 当鼠标离开标记范围时，去除阴影效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            image.SetValue(EffectProperty, null);
        }

        /// <summary>
        /// 鼠标点击时，棋子移动处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseup(object sender, MouseButtonEventArgs e)
        {
            // 当前有预选棋子时，将预选棋子运子到(m,n)位置================= 运子
            QiZiMoveTo(GlobalValue.CurrentQiZi, Col, Row, GlobalValue.QiPan[Col, Row], true);
            // 点击位置有棋子时，将预选棋子运子到(m,n)位置，并吃掉目标位置的对方棋子===== 吃子
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.PathPointImage[i, j].SetHidden();
                    GlobalValue.PathPointImage[i, j].HasPoint = false;
                }
            }

        }

        /// <summary>
        /// 棋子移动的处理
        /// </summary>
        /// <param name="QiZi">棋子编号</param>
        /// <param name="m">目的地的列</param>
        /// <param name="n">目的地的行</param>
        /// <param name="DieQz">所杀死的棋子的编号，-1表示没有杀死棋子</param>
        /// <param name="sound">是否打开声音效果</param>
        private static void QiZiMoveTo(int QiZi, int m, int n, int DieQz, bool sound)  // 运子
        {
            // 运子到(m,n)位置
            int x0 = GlobalValue.QiZiArray[QiZi].Col;
            int y0 = GlobalValue.QiZiArray[QiZi].Row;
            Qipu.AddItem(QiZi, x0, y0, m, n, DieQz); // 棋谱记录


            GlobalValue.QiZiArray[QiZi].SetPosition(m, n);

            if (DieQz != -1)
            {
                GlobalValue.QiZiArray[DieQz].SetDied();
                if (sound)
                {
                    /*Form2.mp1.FileName := 'sounds/eat.mp3';
                    Form2.mp1.Open;
                    Form2.mp1.Play;*/
                }
                if (DieQz == 4)  // 黑将被吃，则红方胜
                {
                    GlobalValue.GameOver = true;
                    //Form2.lbl3.Caption := '战斗结束！红方胜！！';
                }
                if (DieQz == 20)  // 红帅被吃，则黑方胜
                {
                    GlobalValue.GameOver = true;
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

            GlobalValue.SideTag = !GlobalValue.SideTag;  // 变换走棋方
            //GlobalValue.QiZiArray[QiZi].PutDown();

            GlobalValue.CurrentQiZi = 100;
            // 取消棋子预选状态
            //testbox1.Text = GlobalValue.sidetag.ToString();

        }
   }
}
