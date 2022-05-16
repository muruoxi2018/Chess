using Chess.SuanFa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Chess.SuanFa.Qipu;
using Chess.CustomClass;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Chess
{
    public static class GlobalValue
    {
        public const float GRID_WIDTH = 67.5f;   //棋盘格大小为 67.5*67.5
        public const bool BLACKSIDE = false;  // 黑方
        public const bool REDSIDE = true;   //红方
        public static bool SideTag;  // 当前走棋方
        public static bool GameOver; // 游戏结束
        public static bool QiPanFanZhuan; // 棋盘上下翻转，默认值为false，下红上黑，设为true后，翻转后为下黑上红
        public static string qipustr;   // 棋谱转换后的字符串
        public static int CurrentQiZi;  // 当前选定的棋子
        public static int[,] QiPan = new int[9, 10]; // 棋盘坐标，记录棋子位置，如果为-1，则表示该位置没有棋子。
        
        #region // 用户界面元素
        public static PathPoint[,] PathPointImage = new PathPoint[9, 10];  // 棋子可走路径的圆点标记
        public static QiZi[] QiZiArray = new QiZi[32]; // 棋子数组，所有棋子均在此数组中
        public static QiZi YuanWeiZhi;  // 棋子走动后在原位置显示圆圈
        public static Label JiangJunTiShi; // 将军时的文字提示
        public static JueSha jueShaImage; // 绝杀时显示图片
        public static Window_QiPu Window_QiPuKu; // 棋谱库窗口
        public static MyGraphics Arrows = new(); // 走棋指示箭头
        public static Ellipse RedSideRect = new();  // 红方走棋提示灯
        public static Ellipse BlackSideRect = new();  // 黑方走棋提示灯
        #endregion

        #region 数据存储
        public static ObservableCollection<QPStep> FuPanDataList = new(); // 复盘棋谱步骤列表，后期将弃用本变量
        public static QiPuRecord QiPuRecordRoot = new(); // 棋谱树型数据结构
        public static QiPuSimpleRecord QiPuSimpleRecordRoot = new(); // 棋谱树型数据结构的精简版
        public static List<Qipu.QPStep> CnNumberList = new();  // 棋谱中文步骤列表
        #endregion

        #region 棋子及棋盘基础数据
        /// <summary>
        /// 棋子编号0-31，分别对应的图像文件名
        /// </summary>
        public static readonly string[] QiZiImageFileName = {
            "黑将", "黑士", "黑士", "黑象", "黑象", "黑马", "黑马", "黑车", "黑车", "黑炮", "黑炮", "黑卒", "黑卒", "黑卒", "黑卒", "黑卒",
            "红帅", "红仕", "红仕", "红相", "红相", "红马", "红马", "红车", "红车", "红炮", "红炮", "红兵", "红兵", "红兵", "红兵", "红兵"
        };
        /// <summary>
        /// 棋子的中文简称，用于棋谱翻译
        /// </summary>
        public static readonly string[] QiZiCnName = {
            "将", "士", "士", "象", "象", "马", "马", "车", "车", "炮", "炮", "卒", "卒", "卒", "卒", "卒",
            "帅", "仕", "仕", "相", "相", "马", "马", "车", "车", "炮", "炮", "兵", "兵", "兵", "兵", "兵"
        };
        /// <summary>
        /// 棋子初始位置
        /// </summary>
        public static readonly int[,] QiZiInitPosition = new int[32, 2]
        {
            {4, 0},{3, 0},{5, 0},{2, 0},{6, 0},{1, 0},{7, 0},{0, 0},{8, 0},
            {1, 2},{7, 2},
            {0, 3},{2, 3},{4, 3},{6, 3},{8, 3},
            {4, 9},{3, 9},{5, 9},{2, 9},{6, 9},{1, 9},{7, 9},{0, 9},{8, 9},
            {1, 7},{7, 7},
            {0, 6},{2, 6},{4, 6},{6, 6},{8, 6}
        };
        const int gw = 35;
        /// <summary>
        /// 棋盘每一格的列坐标
        /// </summary>
        /// 
        public static readonly double[] QiPanGrid_X = new double[9]
        {
            75.0 + gw, 143.0 + gw, 211.0 + gw, 278.0 + gw, 346.0 + gw, 413.0 + gw, 480.0 + gw, 548.0 + gw, 616.0 + gw
        };
        /// <summary>
        /// 棋盘每一格的行坐标
        /// </summary>
        public static readonly double[] QiPanGrid_Y = new double[10]
        {
            61.0 + gw, 130.0 + gw, 197.0 + gw, 264.0 + gw, 332.0 + gw, 400.0 + gw, 467.0 + gw, 535.0 + gw, 603.0 + gw, 669.0 + gw
        };
        /// <summary>
        /// 阿拉伯数字0-9，对应的中文数字
        /// </summary>
        public static readonly string[] CnNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        #endregion


        /// <summary>
        /// 棋子移动的处理，如果棋子移动后配方被将军，则不能移动。
        /// </summary>
        /// <param name="QiZi">棋子编号</param>
        /// <param name="m">目的地的列</param>
        /// <param name="n">目的地的行</param>
        /// <param name="DieQz">所杀死的棋子的编号，-1表示没有杀死棋子</param>
        /// <param name="sound">是否打开声音效果</param>
        public static void QiZiMoveTo(int QiZi, int m, int n, int DieQz, bool sound)  // 运子
        {
            if (QiZi is < 0 or > 31) return;
            // 运子到(m,n)位置
            int x0 = QiZiArray[QiZi].Col;
            int y0 = QiZiArray[QiZi].Row;

            if (MoveCheck.AfterMoveWillJiangJun(QiZi, x0, y0, m, n, QiPan)) return; // 如果棋子移动后，本方处于将军状态，则不可以移动。
            _ = QiZiArray[QiZi].SetPosition(m, n);
            Arrows.HideAllPath();  // 隐藏提示箭头
            Qipu.AddItem(QiZi, x0, y0, m, n, DieQz); // 增加一行棋谱记录

            QiPuRecord QRecord = new();
            QRecord.SetRecordData(QiZi, x0, y0, m, n, DieQz);
            QiPuRecordRoot.Cursor = QiPuRecordRoot.Cursor.AddChild(QRecord);  // 棋谱增加新的节点，指针更新为该节点
            QiPuRecordRoot.Cursor.IsSelected = true;
            //TreeViewItem treeitem = QiPuRecordRoot.GetTree();
            //Window_QiPuKun.jsonTree.Items.Clear();
            //Window_QiPuKun.jsonTree.Items.Add(treeitem);

            QiPuSimpleRecordRoot = ConvertQiPuToSimple(QiPuRecordRoot);  // 更新简易棋谱记录
            Window_QiPuKu.memostr.Text = JsonConvert.SerializeObject(QiPuSimpleRecordRoot);


            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    PathPointImage[i, j].HasPoint = false; // 走棋后，隐藏走棋路径
                }
            }

            if (JiangJun.IsJueSha(QiZi)) // 检查是否绝杀
            {
                jueShaImage.ShowJueShaImage(); // 已绝杀时，显示绝杀图像
            }

            if (DieQz != -1) // 如果杀死了棋子
            {
                QiZiArray[DieQz].SetDied(); 
                if (sound)
                {
                    /*Form2.mp1.FileName := 'sounds/eat.mp3';
                    Form2.mp1.Open;
                    Form2.mp1.Play;*/
                }
            }
            else
            {
                if (sound)
                {
                    /*Form2.mp1.FileName := 'sounds/go.wav';
                    Form2.mp1.Open;
                    Form2.mp1.Play;*/
                }
            }

            SideTag = !SideTag;  // 变换走棋方
            if (SideTag==BLACKSIDE)
            {
                // 黑方走棋指示
                BlackSideRect.Fill=Brushes.LightGoldenrodYellow;
                RedSideRect.Fill=Brushes.Gray;
            }
            else
            {
                // 红方走棋指示
                BlackSideRect.Fill = Brushes.Gray;
                RedSideRect.Fill = Brushes.LightGoldenrodYellow;
            }
            CurrentQiZi = 100;  //  当前预选棋子设为无效棋子
            AnimationMove(QiZi, x0, y0, m, n); // 动画为异步运行，要注意系统数据的更新是否同步，因此将动画放在最后执行，避免所取数据出现错误。

        }
        /// <summary>
        /// 走棋动画
        /// </summary>
        /// <param name="qizi"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        private static void AnimationMove(int qizi, int x0, int y0, int x1, int y1)
        {
            #region 动画参数设置
            DoubleAnimation PAx = new()
            {
                From = QiPanGrid_X[x0] - GlobalValue.GRID_WIDTH / 2,
                To = QiPanGrid_X[x1] - GlobalValue.GRID_WIDTH / 2,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };
            DoubleAnimation PAy = new()
            {
                From = QiPanGrid_Y[y0] - GlobalValue.GRID_WIDTH / 2,
                To = QiPanGrid_Y[y1] - GlobalValue.GRID_WIDTH / 2,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            if (QiPanFanZhuan)
            {
                PAx.From = QiPanGrid_X[8 - x0] - GlobalValue.GRID_WIDTH / 2;
                PAx.To = QiPanGrid_X[8 - x1] - GlobalValue.GRID_WIDTH / 2;
                PAy.From = QiPanGrid_Y[9 - y0] - GlobalValue.GRID_WIDTH / 2;
                PAy.To = QiPanGrid_Y[9 - y1] - GlobalValue.GRID_WIDTH / 2;
            }
            #endregion
            QiZiArray[qizi].BeginAnimation(Canvas.LeftProperty, PAx);
            QiZiArray[qizi].BeginAnimation(Canvas.TopProperty, PAy);

            DoubleAnimation DAscale = new()
            {
                From = 1,
                To = 1.5,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };
            ScaleTransform scale = new();
            if (SideTag == REDSIDE)
            {
                RedSideRect.RenderTransform = scale;
                RedSideRect.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            if (SideTag == BLACKSIDE)
            {
                BlackSideRect.RenderTransform = scale;
                BlackSideRect.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, DAscale);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, DAscale);
        }
        /// <summary>
        /// 初始化界面，棋盘设置为开局状态，但棋盘翻转状态不会重置
        /// </summary>
        public static void Reset()
        {
            foreach (QiZi item in QiZiArray)
            {
                item.SetInitPosition();
            }
            SideTag = REDSIDE;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    QiPan[i, j] = -1;
                    PathPointImage[i, j].HasPoint = false;
                }
            }
            for (int i = 0; i < 32; i++)
            {
                QiPan[QiZiArray[i].Col, QiZiArray[i].Row] = i;
            }
            YuanWeiZhi.HiddenYuanWeiZhiImage();
            Qipu.QiPuList.Clear();
            Window_QiPu.ReStart();
            Arrows.HideAllPath();  // 隐藏提示箭头

            QiPuRecordRoot.Cursor = QiPuRecordRoot;  // 回到根部
            QiPuRecordRoot.DeleteChildNode();
            
            BlackSideRect.Fill = Brushes.Gray;
            RedSideRect.Fill = Brushes.LightGoldenrodYellow;
            
        }
        /// <summary>
        /// 悔棋按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HuiQi()
        {
            if (Qipu.QiPuList.Count < 1)
            {
                return;
            }

            Qipu.StepCode step = Qipu.QiPuList[^1].StepRecode; // ^1：索引运算符，表示倒数第一个
            QiZiArray[step.QiZi].Select();  // 重新计算可移动路径
            _ = QiZiArray[step.QiZi].SetPosition(step.X0, step.Y0);
            AnimationMove(step.QiZi, step.X1, step.Y1, step.X0, step.Y0);
            QiZiArray[step.QiZi].Select();  // 重新计算可移动路径
            QiZiArray[step.QiZi].Deselect();

            if (step.DieQz > -1)
            {
                QiZiArray[step.DieQz].Setlived();
                _ = QiZiArray[step.DieQz].SetPosition(step.X1, step.Y1);
            }
            QiPan[step.X0, step.Y0] = step.QiZi;
            QiPan[step.X1, step.Y1] = step.DieQz;
            Qipu.QiPuList.RemoveAt(Qipu.QiPuList.Count - 1);
            SideTag = !SideTag;

            if (QiPuRecordRoot.Cursor.GetParent() != null)
            {
                QiPuRecordRoot.Cursor=QiPuRecordRoot.Cursor.GetParent();
                QiPuRecordRoot.Cursor.IsSelected = true;
            }
        }

        /// <summary>
        /// 将全记录棋谱转化为简易记录棋谱，经JsonConvert.SerializeObject,存入数据库。目的是压缩数据量。
        /// </summary>
        /// <param name="FullQipu">全局变量QiPuRecordRoot</param>
        /// <returns>简易记录棋谱</returns>
        public static QiPuSimpleRecord ConvertQiPuToSimple(QiPuRecord FullQipu)
        {
            QiPuSimpleRecord SimpleQipu = new();
            SimpleQipu.Memo = FullQipu.Memo;
            SimpleQipu.CopyDataFromStep(FullQipu.StepData);
            foreach (QiPuRecord Recode in FullQipu.ChildNode)
            {
                QiPuSimpleRecord childRecode=ConvertQiPuToSimple(Recode);
                SimpleQipu.Child.Add(childRecode);
            }
            return SimpleQipu;
        }

        /// <summary>
        /// 将简易记录棋谱转化为全记录棋谱。用于从数据库读取数据后，经JsonConvert.DeserializeObject，存入全局变量QiPuRecordRoot
        /// </summary>
        /// <param name="SimpleQipu">全局变量QiPuSimpleRecordRoot</param>
        /// <returns>全记录棋谱</returns>
        public static QiPuRecord ConvertQiPuToFull(QiPuSimpleRecord SimpleQipu)
        {
            QiPuRecord Qipu = new();
            Qipu.Memo=SimpleQipu.Memo;
            Qipu.SetRecordData(SimpleQipu.Data);
            foreach (QiPuSimpleRecord Recode in SimpleQipu.Child)
            {
                QiPuRecord childRecode = ConvertQiPuToFull(Recode);
                Qipu.ChildNode.Add(childRecode);
            }
            return Qipu;
        }
    }
}

