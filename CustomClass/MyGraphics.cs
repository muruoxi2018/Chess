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
    public class MyGraphics
    {
        public Grid grid = new();
        private Path[] path = new Path[5];  // 箭头本体
        private TextBlock[] textBlocks = new TextBlock[5];  // 箭头上标识的数字
        private Ellipse[] ellipses = new Ellipse[5];    // 数字标识的背景圆圈
        private static int arrowAngle = 160; // 箭头斜边相对箭杆的偏角
        private static int arrowAngle1 = 170; // 箭头斜边相对箭杆的偏角
        private static int arrowLong = 30; // 箭头斜边的长度
        public MyGraphics()
        {
            grid.Opacity = 1;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Path
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
                textBlocks[i] = new TextBlock
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
                ellipses[i] = new Ellipse
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
            if (ind > 4) return; // 箭头数量限定为最多5个，从0开始编号
            double x0, y0, x1, y1, circleX, circleY;
            x0 = GlobalValue.QiPanGrid_X[point0.X];
            y0 = GlobalValue.QiPanGrid_Y[point0.Y];
            x1 = GlobalValue.QiPanGrid_X[point1.X];
            y1 = GlobalValue.QiPanGrid_Y[point1.Y];

            List<PointF> pointFs = new List<PointF>();

            double angle = Math.Atan2(y1 - y0, x1 - x0); //箭杆的角度
            double angle1;
            double xm, ym, xn, yn;

            angle1 = angle; // 箭头初端离开起始点10
            x0 = (float)Math.Floor(x0 + (10 * Math.Cos(angle1)));
            y0 = (float)Math.Floor(y0 + (10 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)x0, (float)y0));

            angle1 = angle + Radians(180); // 箭头末端离开终点10
            xm = (float)Math.Floor(x1 + (10 * Math.Cos(angle1)));
            ym = (float)Math.Floor(y1 + (10 * Math.Sin(angle1))); 

            angle1 = angle + Radians(arrowAngle1); // 计算箭头第一个斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));

            angle1 = angle + Radians(arrowAngle); // 计算箭头第一个斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));

            pointFs.Add(new PointF((float)xm, (float)ym));

            angle1 = angle - Radians(arrowAngle); // 计算箭头第一个斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));

            angle1 = angle - Radians(arrowAngle1); // 计算箭头第一个斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            xn = (float)Math.Floor(xm + (arrowLong * 2 / 3 * Math.Cos(angle1)));
            yn = (float)Math.Floor(ym + (arrowLong * 2 / 3 * Math.Sin(angle1)));
            pointFs.Add(new PointF((float)xn, (float)yn));

            path[ind].Data = Geometry.Parse(MakePathData(pointFs));
            path[ind].Visibility = Visibility.Visible;

            // 计算圆圈的位置，其中心设置在箭杆的中心线上
            circleX = Math.Floor(x1 + (1  * Math.Cos(angle))) - 10; // 10是圆圈的半径。计算结果为圆心位置，而margin是从其边界计算，因此需用半径修正数据。
            circleY = Math.Floor(y1 + (1  * Math.Sin(angle))) - 10;

            ellipses[ind].Margin = new Thickness(circleX, circleY, 0, 0);
            ellipses[ind].Visibility = Visibility.Visible;

            textBlocks[ind].Margin = new Thickness(circleX + 5, circleY, 0, 0); // 5是经验数据，用于修正文字的偏移。文字的字体大小为16时，在+5后，文字正好在圆圈中心。如果字体大小有改变，需修正此数据。
            textBlocks[ind].Visibility = Visibility.Visible;

        }
        /// <summary>
        /// 角度转为弧度
        /// </summary>
        /// <param name="degress">角度</param>
        /// <returns></returns>
        private double Radians(double degress)
        {
            return degress * Math.PI / 180;
        }
        private string MakePathData(List<PointF> pointFs)
        {
            string path = $"M {pointFs[0].X}, {pointFs[0].Y} ";
            foreach (PointF point in pointFs)
            {
                path += $"L {point.X} , {point.Y} ";
            }
            path += " Z";
            return path;
        }

    }
}
