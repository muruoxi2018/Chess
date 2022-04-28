using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using Chess.SuanFa;
using Chess.DataClass;
using Chess.OpenSource;
using System.Data;

namespace Chess.SubWindow
{
    /// <summary>
    /// Save_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Save_Window : Window
    {
        public Save_Window()
        {
            InitializeComponent();
            qipustr.Text =Qipu.CnToString();
            DataTable dt = SqliteHelper.QueryTable("mybook", "author");
            dt = new DataView(dt).ToTable(true, "author"); // 去除重复数据
            author.DisplayMemberPath = "author";
            author.ItemsSource = dt.DefaultView;

            dt = SqliteHelper.QueryTable("mybook", "type");
            dt = new DataView(dt).ToTable(true, "type"); // 去除重复数据
            type.DisplayMemberPath = "type";
            type.ItemsSource = dt.DefaultView;

            dt = SqliteHelper.QueryTable("mybook", "title");
            dt = new DataView(dt).ToTable(true, "title"); // 去除重复数据
            title.DisplayMemberPath = "title";
            title.ItemsSource = dt.DefaultView;

        }

        private void saveButtonClick(object sender, RoutedEventArgs e)
        {
            QiPuBook book = new();
            book.author = author.Text;
            book.date = date.DisplayDate;
            book.type = type.Text;
            book.title = title.Text;
            book.video = videoLink.Text;
            book.memo = memoText.Text;
            book.record =Qipu.CnToString();
            book.jsonrecord= JsonConvert.SerializeObject(Qipu.QiPuList);
            SqliteHelper.ExecuteInsert("mybook", book.getDictionary());
            this.Close();
        }
    }
}
