using System.Windows;
using Newtonsoft.Json;
using Chess.SuanFa;
using Chess.DataClass;
using Chess.OpenSource;
using System.Data;

namespace Chess.SubWindow
{
    /// <summary>
    /// Save_Window.xaml 的交互逻辑
    /// 保存棋谱窗口
    /// </summary>
    public partial class Save_Window : Window
    {
        public Save_Window()
        {
            InitializeComponent();
            qipustr.Text = Qipu.CnToString();
            DataTable dt = SqliteHelper.Select("mybook", "author");
            dt = new DataView(dt).ToTable(true, "author"); // 去除重复数据
            author.DisplayMemberPath = "author";
            author.ItemsSource = dt.DefaultView;

            dt = SqliteHelper.Select("mybook", "type");
            dt = new DataView(dt).ToTable(true, "type"); // 去除重复数据
            type.DisplayMemberPath = "type";
            type.ItemsSource = dt.DefaultView;

            dt = SqliteHelper.Select("mybook", "title");
            dt = new DataView(dt).ToTable(true, "title"); // 去除重复数据
            title.DisplayMemberPath = "title";
            title.ItemsSource = dt.DefaultView;

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            QiPuBook book = new();
            book.author = author.Text;
            book.date = date.DisplayDate;
            book.type = type.Text;
            book.title = title.Text;
            book.video = videoLink.Text;
            book.memo = memoText.Text;
            book.record = Qipu.CnToString();
            book.jsonrecord = JsonConvert.SerializeObject(Qipu.QiPuList);
            _ = SqliteHelper.Insert("mybook", book.getDictionary());
            Close();
        }
    }
}
