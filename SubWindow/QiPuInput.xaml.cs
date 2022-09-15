using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Chess.CustomClass.Qipu;

namespace Chess.SubWindow
{
    /// <summary>
    /// QiPuInput.xaml 的交互逻辑
    /// </summary>
    public partial class QiPuInput : Window
    {
        public IObservable<string> Input { get; set; }
        ObservableCollection<QPBase> qipudata = new();
        public QiPuInput()
        {
            InitializeComponent();
        }


        private void OnInputChanged(object sender, TextChangedEventArgs e)
        {
            string inputstr=UserInput.Text;
            inputstr=inputstr.Trim();
            Regex regex = new(@"\d+\.|[\*mB!]");  // 所有空白字符
            
            inputstr = regex.Replace(inputstr, " ");
            regex = new(@"\s+");  // 所有空白字符
            inputstr = regex.Replace(inputstr, " ");
            output.Text=inputstr;
            string[] spite=inputstr.Split('\u0020');
            foreach(string s in spite)
            {
                QPBase thebase = new()
                {
                    Cn=s,
                };
                qipudata.Add(thebase);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            QipuDataGrid.ItemsSource = qipudata;
        }
    }
}
