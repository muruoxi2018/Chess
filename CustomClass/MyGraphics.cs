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
        private Path[] path = new Path[5];
        private TextBlock[] textBlocks = new TextBlock[5];
        private Ellipse[] ellipses = new Ellipse[5];
        private static int lineWidth = 10;
        private static int arrowAngle = 10; // 箭头斜边相对箭杆的偏角
        private static int arrowLong = 30; // 箭头斜边的长度
        public MyGraphics()
        {
            grid.Opacity = 0.8;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Path
                {
                    Stroke = Brushes.ForestGreen,
                    Fill = Brushes.ForestGreen,
                    SnapsToDevicePixels = false,
                    StrokeThickness = 1,
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
                    Fill = Brushes.Yellow,
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
            foreach (Path pathItem in path)
            {
                pathItem.Visibility = Visibility.Hidden;
            }
        }
        public void SetPathData(int ind, System.Drawing.Point point0, System.Drawing.Point point1)
        {
            double x0, y0, x1, y1, x2, y2, x3, y3, x4, y4;
            x0 = GlobalValue.QiPanGrid_X[point0.X] + 35;
            y0 = GlobalValue.QiPanGrid_Y[point0.Y] + 35;
            x1 = GlobalValue.QiPanGrid_X[point1.X] + 35;
            y1 = GlobalValue.QiPanGrid_Y[point1.Y] + 35;

            // 计算箭头斜边相对坐标轴的角度 = 箭杆的角度 - 箭头斜边相对箭杆的偏角
            double angle = Math.Atan2(y1 - y0, x1 - x0) - (Math.PI * (0 - arrowAngle) / 180);
            x2 = Math.Floor(x1 - (arrowLong * Math.Cos(angle)));
            y2 = Math.Floor(y1 - (arrowLong * Math.Sin(angle)));
            angle = Math.Atan2(y1 - y0, x1 - x0) - (Math.PI * arrowAngle / 180);
            x3 = Math.Floor(x1 - (arrowLong * Math.Cos(angle)));
            y3 = Math.Floor(y1 - (arrowLong * Math.Sin(angle)));
            string pathdatastr = $"M {x0},{y0} L {x2},{y2} L {x1},{y1} L {x3},{y3} Z";
            path[ind].Data = Geometry.Parse(pathdatastr);

            path[ind].Visibility = Visibility.Visible;

            ellipses[ind].Margin = new Thickness((x2 + x3) / 2, (y2 + y3) / 2, 0, 0);
            ellipses[ind].Visibility = Visibility.Visible;

            textBlocks[ind].Margin = new Thickness((x2 + x3) / 2 + 5, (y2 + y3) / 2, 0, 0);
            textBlocks[ind].Visibility = Visibility.Visible;

        }

    }
}
