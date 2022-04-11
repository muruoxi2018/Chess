using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.DirectoryServices;

namespace Chess
{
    public class MyQiZi
    {
        private int init_col, init_row;
        public int col { get; set; }
        public int row { get; set; }
        public int qiziid { get; set; }
        public bool selected { get; set; }
        public Image image;
        public MyQiZi(int id)
        {
            qiziid = id;
            string path = System.Environment.CurrentDirectory + "\\picture\\" + GlobalValue.qzimage[qiziid] + ".png";
            BitmapImage bi = new BitmapImage(new Uri(path, UriKind.Absolute));
            bi.Freeze();
            image = new Image
            {
                Source = bi,
                Width = 70,
                Height = 70,
                Tag = id
            };
            init_col = GlobalValue.qiziInitPosition[id, 0];
            init_row = GlobalValue.qiziInitPosition[id, 1];
            Setposition(init_col, init_row);
            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }

        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (QiZi item in GlobalValue.myqz)
            {
                item.Deselect();
            }
            select();
        }

        public void Deselect()
        {
            selected = false;
            image.SetValue(System.Windows.UIElement.EffectProperty, null);
        }
        public void select()
        {
            selected = true;
            image.SetValue(System.Windows.UIElement.EffectProperty, new DropShadowEffect() { ShadowDepth = 8,Opacity=0.7 });

        }

        public int GetId()
        {
            return qiziid;
        }
        public void Setposition(int x, int y)
        {
            col = x;
            row = y;
            image.SetValue(Canvas.LeftProperty, GlobalValue.qipanGridX[x]);
            image.SetValue(Canvas.TopProperty, GlobalValue.qipanGridY[y]);
        }
    }
}