using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// SpyWindow.xaml 的交互逻辑
    /// </summary>

    public partial class SpyWindow : Window
    {
        private static ObservableCollection <qpcol> ObserArray;
        public SpyWindow()
        {
            InitializeComponent();
            ObserArray = new();
            SpyQipan.ItemsSource = ObserArray;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            ObserArray.Clear();
            for (int j = 0; j < 10; j++)
            {
                qpcol item = new();
                item.id = j;
                item.col0 = (GlobalValue.QiPan[0, j] == -1) ? "" : GlobalValue.QiPan[0, j].ToString();
                item.col1 = (GlobalValue.QiPan[1, j] == -1) ? "" : GlobalValue.QiPan[1, j].ToString();
                item.col2 = (GlobalValue.QiPan[2, j] == -1) ? "" : GlobalValue.QiPan[2, j].ToString();
                item.col3 = (GlobalValue.QiPan[3, j] == -1) ? "" : GlobalValue.QiPan[3, j].ToString();
                item.col4 = (GlobalValue.QiPan[4, j] == -1) ? "" : GlobalValue.QiPan[4, j].ToString();
                item.col5 = (GlobalValue.QiPan[5, j] == -1) ? "" : GlobalValue.QiPan[5, j].ToString();
                item.col6 = (GlobalValue.QiPan[6, j] == -1) ? "" : GlobalValue.QiPan[6, j].ToString();
                item.col7 = (GlobalValue.QiPan[7, j] == -1) ? "" : GlobalValue.QiPan[7, j].ToString();
                item.col8 = (GlobalValue.QiPan[8, j] == -1) ? "" : GlobalValue.QiPan[8, j].ToString();
                ObserArray.Add(item);
            }
            SpyQipan.Items.Refresh();

        }
        public class qpcol
        {
            public int id { get; set; }
            public string col0 { get; set; }
            public string col1 { get; set; }
            public string col2 { get; set; }
            public string col3 { get; set; }
            public string col4 { get; set; }
            public string col5 { get; set; }
            public string col6 { get; set; }
            public string col7 { get; set; }
            public string col8 { get; set; }

        }

        private void DataRefresh(object sender, RoutedEventArgs e)
        {
            ObserArray.Clear();
            for (int j = 0; j < 10; j++)
            {
                qpcol item = new();
                item.id = j;
                item.col0 = (GlobalValue.QiPan[0, j] == -1) ? "" : GlobalValue.QiPan[0, j].ToString();
                item.col1 = (GlobalValue.QiPan[1, j] == -1) ? "" : GlobalValue.QiPan[1, j].ToString();
                item.col2 = (GlobalValue.QiPan[2, j] == -1) ? "" : GlobalValue.QiPan[2, j].ToString();
                item.col3 = (GlobalValue.QiPan[3, j] == -1) ? "" : GlobalValue.QiPan[3, j].ToString();
                item.col4 = (GlobalValue.QiPan[4, j] == -1) ? "" : GlobalValue.QiPan[4, j].ToString();
                item.col5 = (GlobalValue.QiPan[5, j] == -1) ? "" : GlobalValue.QiPan[5, j].ToString();
                item.col6 = (GlobalValue.QiPan[6, j] == -1) ? "" : GlobalValue.QiPan[6, j].ToString();
                item.col7 = (GlobalValue.QiPan[7, j] == -1) ? "" : GlobalValue.QiPan[7, j].ToString();
                item.col8 = (GlobalValue.QiPan[8, j] == -1) ? "" : GlobalValue.QiPan[8, j].ToString();
                ObserArray.Add(item);
            }
            //SpyQipan.Items.Refresh();
        }
    }
}
