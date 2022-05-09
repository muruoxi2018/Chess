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

namespace Chess.CustomClass
{
    /// <summary>
    /// 走棋提示箭头
    /// </summary>
    public class MyGraphics
    {
        private readonly static int _maxNum = 9; //提示箭头数量上限为9个，对应变招数量，所以多了也没用。
        public Grid grid = new(); // 绘图板，承载所有绘图元素
        private readonly Path[] path = new Path[_maxNum];  // 箭头本体。
        private readonly TextBlock[] textBlocks = new TextBlock[_maxNum];  // 箭头上标识的数字
        private readonly Ellipse[] ellipses = new Ellipse[_maxNum];    // 数字标识的背景圆圈
        private static readonly int arrowAngle = 160; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowAngle1 = 170; // 箭头斜边相对箭杆的偏角
        private static readonly int arrowLong = 30; // 箭头斜边的长度

        /// <summary>
        /// 初始化走棋提示箭头
        /// </summary>
        public MyGraphics()
        {
            grid.Opacity = 1;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Path // 箭头本体
                {
                    Stroke = Brushes.ForestGreen,
                    Fill = Brushes.GreenYellow,
                    SnapsToDevicePixels = false,
                    StrokeThickness = 1,
                    Opacity = 0.8,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Hidden
                };
            }
            for (int i = 0; i < textBlocks.Length; i++)
            {
                textBlocks[i] = new TextBlock  // 箭头上标识的数字
                {
                    Text = (i + 1).ToString(),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Visibility = Visibility.Hidden,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = Brushes.Black
                };
            }
            for (int i = 0; i < ellipses.Length; i++)
            {
                ellipses[i] = new Ellipse  // 数字标识的背景圆圈
                {
                    Width = 20,
                    Height = 20,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Visibility = Visibility.Hidden,
                    Fill = Brushes.GreenYellow,
                    Opacity = 0.9
                };
            }

            foreach (Path item in path)
            {
                _ = grid.Children.Add(item);
            }
            foreach (Ellipse item in ellipses)
            {
                _ = grid.Children.Add(item);
            }
            foreach (TextBlock item in textBlocks)
            {
                _ = grid.Children.Add(item);
            }
        }
        /// <summary>
        /// 隐藏所有箭头
        /// </summary>
        public void HideAllPath()
        {
            foreach (Path item in path)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (Ellipse item in ellipses)
            {
                item.Visibility = Visibility.Hidden;
            }
            foreach (TextBlock item in textBlocks)
            {
                item.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// 根据箭头起始点，计算绘制箭头的各项数据，并显示到界面
        /// </summary>
        /// <param name="ind">箭头的编号，0-4 </param>
        /// <param name="point0">起始点</param>
        /// <param name="point1">终点</param>
        public void SetPathDataAndShow(int ind, System.Drawing.Point point0, System.Drawing.Point point1)
        {
            if (ind > _maxNum - 1) return; // 箭头从0开始编号，数量不能超过上限（_maxNum）
            int haveQizi = GlobalValue.QiPan[point1.X, point1.Y]; // 目标位置的棋子编号，-1表示没有棋子。
            if (GlobalValue.QiPanFanZhuan)
            {
                point0.X = 8 - point1.X;
                point0.Y = 9 - point1.Y;
                point1.X = 8 - point1.X;
                point1.Y = 9 - point1.Y;
            }

            double x0, y0, x1, y1;
            x0 = GlobalValue.QiPanGrid_X[point0.X];
            y0 = GlobalValue.QiPanGrid_Y[point0.Y];
            x1 = GlobalValue.QiPanGrid_X[point1.X];
            y1 = GlobalValue.QiPanGrid_Y[point1.Y];


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

            path[ind].Data = Geometry.Parse(MakePathData(pointFs));
            path[ind].Visibility = Visibility.Visible;

            double circleX, circleY;
            double cirlcePos;
            cirlcePos = haveQizi == -1 ? 1 : arrowLong * -1.75;  // 目标位置没有棋子时，圆圈及数字的位置设置在目标位置的棋盘交叉点上
            // 计算圆圈的位置，其中心设置在箭杆的中心线上
            circleX = Math.Floor(x1 + (cirlcePos * Math.Cos(angle))) - 10; // 10是圆圈的半径。计算结果为圆心位置，而margin是从其边界计算，因此需用半径修正数据。
            circleY = Math.Floor(y1 + (cirlcePos * Math.Sin(angle))) - 10;

            ellipses[ind].Margin = new Thickness(circleX, circleY, 0, 0);
            ellipses[ind].Visibility = Visibility.Visible;

            textBlocks[ind].Margin = new Thickness(circleX + 5, circleY, 0, 0); // 5是经验数据，用于修正文字的偏移。文字的字体大小为16时，在+5后，文字正好在圆圈中心。如果字体大小有改变，需修正此数据。
            textBlocks[ind].Visibility = Visibility.Visible;

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
        /// <param name="pointFs">路径点列表</param>
        /// <returns>路径绘图指令字符串</returns>
        private static string MakePathData(List<PointF> pointFs)
        {
            string path = $"M {pointFs[0].X}, {pointFs[0].Y} ";
            foreach (PointF point in pointFs)
            {
                path += $"L {point.X} , {point.Y} ";
            }
            path += " Z";  // Z=封闭路径
            return path;
        }

    }
}
