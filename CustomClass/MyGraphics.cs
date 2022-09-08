using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using Chess;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace Chess.CustomClass
{
    /// <summary>
    /// 走棋提示箭头
    /// </summary>
    public class MyGraphics
    {
        private readonly static int _maxNum = 9; //提示箭头数量上限为9个，对应变招数量，所以多了也没用。
        public Grid grid = new(); // 绘图板，承载所有绘图元素
        private readonly Path[] ArrowPath = new Path[_maxNum];  // 箭头本体。
        private readonly TextBlock[] ArrowText = new TextBlock[_maxNum];  // 箭头上标识的数字
        private readonly Ellipse[] ArrowEllipses = new Ellipse[_maxNum];    // 数字标识的背景圆圈
        private readonly MyPrompt[] MemoPrompt = new MyPrompt[_maxNum];
        private static readonly int arrowAngle = 160; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowAngle1 = 170; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowLong = 30; // 箭头斜边的长度
        private static int arrowCount = 0;// 当前有效的箭头数量
        /// <summary>
        /// 初始化走棋提示箭头
        /// </summary>
        public MyGraphics()
        {
            #region 初始化提示箭头
            grid.Opacity = 1;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            var pathStroke = new Binding("Stroke")
            {
                AsyncState = BindingMode.OneWay,
                Source = Application.Current.Resources,
                
            };
            var pathFill = new Binding("Fill")
            {
                AsyncState = BindingMode.OneWay,
                Source = Application.Current.Resources.Source,

            };

            for (int i = 0; i < ArrowPath.Length; i++)
            {
                ArrowPath[i] = new Path // 箭头本体
                {
                    SnapsToDevicePixels = false,
                    Stroke =Brushes.ForestGreen,
                    StrokeThickness = 1,
                    Fill = Brushes.GreenYellow,
                    Opacity = 0.8 - i * 0.1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Hidden
                };
                //BindingOperations.SetBinding(ArrowPath[i], Path.StretchProperty, pathStroke);
                //BindingOperations.SetBinding(ArrowPath[i], Path.FillProperty, pathFill);
                ArrowPath[i].SetBinding(Path.StrokeProperty, pathStroke);
                ArrowPath[i].SetBinding(Path.FillProperty, pathFill);
                _ = grid.Children.Add(ArrowPath[i]);
            }
            for (int i = 0; i < ArrowPath.Length; i++)
            {
                ArrowEllipses[i] = new Ellipse  // 数字标识的背景圆圈
                {
                    Width = 20,
                    Height = 20,
                    Stroke = Brushes.ForestGreen,
                    StrokeThickness = 1,
                    Fill = Brushes.GreenYellow,
                    Opacity = 0.8 - i * 0.1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Hidden,
                };
                _ = grid.Children.Add(ArrowEllipses[i]);

                ArrowText[i] = new TextBlock  // 箭头上标识的数字
                {
                    Text = (i + 1).ToString(),
                    FontSize = 16,
                    FontWeight = (i == 0) ? FontWeights.Bold : FontWeights.Normal,
                    Visibility = Visibility.Hidden,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = (i == 0) ? Brushes.ForestGreen : Brushes.Black
                };

                _ = grid.Children.Add(ArrowText[i]);
            }
            for (int i = ArrowPath.Length - 1; i >= 0; i--)
            {
                MemoPrompt[i] = new MyPrompt();
                if (i == 0) MemoPrompt[i].SetBold();
                _ = grid.Children.Add(MemoPrompt[i]);
            }
            #endregion
        }
        /// <summary>
        /// 隐藏所有箭头
        /// </summary>
        public void HideAllPath()
        {
            foreach (Path item in ArrowPath)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (Ellipse item in ArrowEllipses)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (TextBlock item in ArrowText)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (MyPrompt prompt in MemoPrompt)
            {
                prompt.SetHidden();
            }
        }

        public void ShowAllPath()
        {
            if (Settings.Default.ArrowVisable)
            {
                for (int i = 0; i < arrowCount; i++)
                {
                    ArrowPath[i].Visibility = Visibility.Visible;
                    ArrowEllipses[i].Visibility = Visibility.Visible;
                    ArrowText[i].Visibility = Visibility.Visible;

                    MemoPrompt[i].SetVisible();
                }
            }
        }
        /// <summary>
        /// 根据箭头起始点，计算绘制箭头的各项数据，并显示到界面
        /// </summary>
        /// <param name="arrowId">箭头的编号，0-4 </param>
        /// <param name="point0">起始点</param>
        /// <param name="point1">终点</param>
        /// <param name="sameTargetPoint">箭头指向同一位置时，第二个参数为true，避免编号位置重叠</param>
        public void SetPathData(int arrowId, System.Drawing.Point point0, System.Drawing.Point point1, bool sameTargetPoint, string memo)
        {
            arrowCount = arrowId + 1;// 有效箭头数量取最后一次提交的编号
            if (arrowId > _maxNum - 1) return; // 箭头从0开始编号，数量不能超过上限（_maxNum）
            int haveQizi = GlobalValue.QiPan[point1.X, point1.Y]; // 目标位置的棋子编号，-1表示没有棋子。
            if (GlobalValue.IsQiPanFanZhuan)
            {
                point0.X = 8 - point0.X;  // 棋盘处于翻转状态时，转换坐标
                point0.Y = 9 - point0.Y;
                point1.X = 8 - point1.X;
                point1.Y = 9 - point1.Y;
            }

            double x0, y0, x1, y1;
            x0 = GlobalValue.QiPanGrid_X[point0.X];
            y0 = GlobalValue.QiPanGrid_Y[point0.Y];
            x1 = GlobalValue.QiPanGrid_X[point1.X];
            y1 = GlobalValue.QiPanGrid_Y[point1.Y];

            #region  计算提示箭头及数字编号标识
            List<PointF> pointFs = new();

            double angle = Math.Atan2(y1 - y0, x1 - x0); //箭杆的角度
            double angle1;
            double xm, ym, xn, yn;

            angle1 = angle; // 箭头初端离开起始点10
            x0 = (float)Math.Floor(x0 + (10 * Math.Cos(angle1)));
            y0 = (float)Math.Floor(y0 + (10 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)x0, (float)y0)); // 存入第一个点

            angle1 = angle + Radians(180); // 箭头末端(xm,ym)离开终点10
            xm = (float)Math.Floor(x1 + (10 * Math.Cos(angle1)));
            ym = (float)Math.Floor(y1 + (10 * Math.Sin(angle1)));

            // 以下均以箭头末端为基点，计算其他各点位置
            angle1 = angle + Radians(arrowAngle1); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第二个点

            angle1 = angle + Radians(arrowAngle); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第三个点

            pointFs.Add(new PointF((float)xm, (float)ym)); // 箭头末端，是第四个点

            angle1 = angle - Radians(arrowAngle); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第五个点

            angle1 = angle - Radians(arrowAngle1); // 斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));  // 存入第六个点

            ArrowPath[arrowId].Data = Geometry.Parse(MakePathData(pointFs));
            //ArrowPath[arrowId].Visibility = Visibility.Visible;

            double circleX, circleY;
            double cirlcePos = 1.0;
            /*if (haveQizi > -1 || sameTargetPoint)
            {
                // 目标位置没有棋子时，圆圈及数字的位置设置在目标位置的棋盘交叉点上，有棋子时，位置设置至箭头后面
                // 多个箭头指向同一位置时，圆圈及数字的位置设置在箭头后面
                cirlcePos = arrowLong * -1.75;
            }*/
            cirlcePos = arrowLong * -1.75;
            //计算圆圈的位置，其中心设置在箭杆的中心线上
            circleX = Math.Floor(x1 + (cirlcePos * Math.Cos(angle))) - 10; // 10是圆圈的半径。计算结果为圆心位置，而margin是从其边界计算，因此需用半径修正数据。
            circleY = Math.Floor(y1 + (cirlcePos * Math.Sin(angle))) - 10;

            ArrowEllipses[arrowId].Margin = new Thickness(circleX, circleY, 0, 0);
            //ArrowEllipses[arrowId].Visibility = Visibility.Visible;

            ArrowText[arrowId].Margin = new Thickness(circleX + 5, circleY, 0, 0); // 5是经验数据，用于修正文字的偏移。文字的字体大小为16时，在+5后，文字正好在圆圈中心。如果字体大小有改变，需修正此数据。
            //ArrowText[arrowId].Visibility = Visibility.Visible;

            if (memo == null || string.IsNullOrEmpty(memo)) return;
            memo = $"{arrowId + 1}：{memo}";

            MemoPrompt[arrowId].SetText(memo);
            //MemoPrompt[arrowId].SetVisible();

            MemoPrompt[arrowId].Margin = new Thickness(circleX, circleY + 22, 80, 0);
            #endregion
        }
        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="degress">角度</param>
        /// <returns>弧度值</returns>
        private static double Radians(double degress)
        {
            return degress * Math.PI / 180;
        }
        /// <summary>
        /// 根据路径点，转换为路径绘图指令
        /// </summary>
        /// <param name="pointFList">路径点列表</param>
        /// <returns>路径绘图指令字符串</returns>
        private static string MakePathData(List<PointF> pointFList)
        {
            string path = $"M {pointFList[0].X}, {pointFList[0].Y} ";
            foreach (PointF point in pointFList)
            {
                path += $"L {point.X} , {point.Y} ";
            }
            path += " Z";  // Z=封闭路径
            return path;
        }

    }
}
