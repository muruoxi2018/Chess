using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Chess
{
    public static class GlobalValue
    {
        public static bool qipanfanzhuan; // 棋盘上下翻转，默认值为false，下红上黑，设为true后，翻转后为下黑上红
        public static int[,] qipan = new int[9, 10]; // 棋盘坐标，记录棋子位置，如果为-1，则表示该位置没有棋子。
        public static bool[,] qzpath = new bool[9, 10]; // 棋子可走的路径
        public static PathPoint[,] pathImage = new PathPoint[9, 10];
        public static QiZi[] myqz=new QiZi[32];
        public static string qipustr;
        public static int CurrentQiZi;
        public const bool BLACKSIDE = false;
        public const bool REDSIDE = true;
        public static bool sidetag, gameover;
        public const float girdwidth = 67.5f;   //棋盘格为 67.5*67.5
        public static string[] qzimage = {
            "黑将", "黑士", "黑士", "黑象", "黑象", "黑马", "黑马", "黑车", "黑车", "黑炮", "黑炮", "黑卒", "黑卒", "黑卒", "黑卒", "黑卒",
            "红帅", "红仕", "红仕", "红相", "红相", "红马", "红马", "红车", "红车", "红炮", "红炮", "红兵", "红兵", "红兵", "红兵", "红兵"
        };
        public static string[] qzimage1 = {
            "将", "士", "士", "象", "象", "马", "马", "车", "车", "炮", "炮", "卒", "卒", "卒", "卒", "卒",
            "帅", "仕", "仕", "相", "相", "马", "马", "车", "车", "炮", "炮", "兵", "兵", "兵", "兵", "兵"
        };
        public static int[,] qiziInitPosition =new int[32,2]
        {
            {4, 0},{3, 0},{5, 0},{2, 0},{6, 0},{1, 0},{7, 0},{0, 0},{8, 0},
            {1, 2},{7, 2},
            {0, 3},{2, 3},{4, 3},{6, 3},{8, 3},
            {4, 9},{3, 9},{5, 9},{2, 9},{6, 9},{1, 9},{7, 9},{0, 9},{8, 9},
            {1, 7},{7, 7},
            {0, 6},{2, 6},{4, 6},{6, 6},{8, 6}
        };
        public static double[] qipanGridX = new double[9]
        {
            75.0, 143.0, 211.0, 278.0, 346.0, 413.0, 480.0, 548.0, 616.0
        };
        public static double[] qipanGridY = new double[10]
        {
            61.0, 130.0, 197.0, 264.0, 332.0, 400.0, 467.0, 535.0, 603.0, 669.0
        };



        public static string[] CnNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        //public static Image[] images;

    }
}
