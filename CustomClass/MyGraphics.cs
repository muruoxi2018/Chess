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

namespace Chess.CustomClass
{
    public class MyGraphics
    {
        public Grid grid = new();
        private Path[] path = new Path[5];
        private TextBlock[] textBlocks = new TextBlock[5];
        private static int lineWidth = 10;
        private static int arrowAngle = 6; // 箭头斜边相对箭杆的偏角
        private static int arrowLong = 50; // 箭头斜边的长度
        public MyGraphics()
        {
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = new Path();
            }
            for (int i = 0; i < textBlocks.Length; i++)
            {
                textBlocks[i] = new TextBlock();
                textBlocks[i].Text = (i + 1).ToString();
                textBlocks[i].FontSize = 11;
                textBlocks[i].Visibility = Visibility.Hidden;
                textBlocks[i].HorizontalAlignment = HorizontalAlignment.Left;
                textBlocks[i].VerticalAlignment = VerticalAlignment.Top;
            }
            foreach (var item in path)
            {
                item.Stroke = Brushes.ForestGreen;
                item.Fill = Brushes.ForestGreen;
                item.SnapsToDevicePixels = false;
                item.StrokeThickness = 1;
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Top;
                item.Visibility = Visibility.Hidden;
                grid.Children.Add(item);
            }
            foreach (var item in textBlocks)
            {
                grid.Children.Add(item);
            }
        }
        public void HideAllPath()
        {
            foreach (var pathItem in path)
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
            double angle = Math.Atan2(y1 - y0, x1 - x0) - Math.PI * (0 - arrowAngle) / 180;
            x2 = Math.Floor(x1 - arrowLong * Math.Cos(angle));
            y2 = Math.Floor(y1 - arrowLong * Math.Sin(angle));
            angle = Math.Atan2(y1 - y0, x1 - x0) - Math.PI * (arrowAngle) / 180;
            x3 = Math.Floor(x1 - arrowLong * Math.Cos(angle));
            y3 = Math.Floor(y1 - arrowLong * Math.Sin(angle));
            string pathdatastr = string.Format("M {0},{1} L {2},{3} L {4},{5} L {6},{7} Z", x0, y0, x2, y2, x1, y1, x3, y3);
            path[ind].Data = Geometry.Parse(pathdatastr);

            path[ind].Visibility = Visibility.Visible;

            textBlocks[ind].Margin=new Thickness(x1,y1,0,0);
            textBlocks[ind].Visibility=Visibility.Visible;
        }

    }
}
