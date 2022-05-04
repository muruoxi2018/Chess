﻿using Chess.SuanFa;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static Chess.SuanFa.Qipu;

namespace Chess
{
    public static class GlobalValue
    {
        public static bool QiPanFanZhuan; // 棋盘上下翻转，默认值为false，下红上黑，设为true后，翻转后为下黑上红
        public static int[,] QiPan = new int[9, 10]; // 棋盘坐标，记录棋子位置，如果为-1，则表示该位置没有棋子。
        public static PathPoint[,] PathPointImage = new PathPoint[9, 10];
        public static QiZi[] QiZiArray = new QiZi[32];
        public static QiZi YuanWeiZhi;
        public static string qipustr;
        public static int CurrentQiZi;
        public const bool BLACKSIDE = false;
        public const bool REDSIDE = true;
        public static bool SideTag, GameOver;
        public const float GRID_WIDTH = 67.5f;   //棋盘格为 67.5*67.5

        public static ObservableCollection<QPStep> QiPuFuPanList = new(); // 复盘棋谱步骤列表

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
        /// <summary>
        /// 棋盘每一格的列坐标
        /// </summary>
        public static readonly double[] QiPanGrid_X = new double[9]
        {
            75.0, 143.0, 211.0, 278.0, 346.0, 413.0, 480.0, 548.0, 616.0
        };
        /// <summary>
        /// 棋盘每一格的行坐标
        /// </summary>
        public static readonly double[] QiPanGrid_Y = new double[10]
        {
            61.0, 130.0, 197.0, 264.0, 332.0, 400.0, 467.0, 535.0, 603.0, 669.0
        };
        /// <summary>
        /// 阿拉伯数字0-9，对应的中文数字
        /// </summary>
        public static readonly string[] CnNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        public static List<Qipu.QPStep> CnNumberList = new();

        public static Label JiangJunTiShi;
        public static CustomClass.JueSha jueShaImage;

        public static Window_QiPu Window_Qi; // 棋谱库窗口

        /// <summary>
        /// 棋子移动的处理
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
            // 动画为异步运行，要注意系统数据的更新是否同步，可考虑将动画放在最后执行，避免所取数据出现错误。

            Qipu.AddItem(QiZi, x0, y0, m, n, DieQz); // 棋谱记录

            if (JiangJun.IsJueSha(QiZi)) // 检查是否绝杀
            {
                jueShaImage.SetJueSha();
            }

            if (DieQz != -1)
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
            AnimationMove(QiZi, x0, y0, m, n);
            SideTag = !SideTag;  // 变换走棋方
            CurrentQiZi = 100;
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    PathPointImage[i, j].HasPoint = false;
                }
            }

        }

        private static void AnimationMove(int qizi, int x0, int y0, int x1, int y1)
        {
            DoubleAnimation PAx = new()
            {
                From = QiPanGrid_X[x0],
                To = QiPanGrid_X[x1],
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };
            DoubleAnimation PAy = new()
            {
                From = QiPanGrid_Y[y0],
                To = QiPanGrid_Y[y1],
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(0.15))
            };

            if (QiPanFanZhuan)
            {
                PAx.From = QiPanGrid_X[8 - x0];
                PAx.To = QiPanGrid_X[8 - x1];
                PAy.From = QiPanGrid_Y[9 - y0];
                PAy.To = QiPanGrid_Y[9 - y1];
            }
            QiZiArray[qizi].BeginAnimation(Canvas.LeftProperty, PAx);
            QiZiArray[qizi].BeginAnimation(Canvas.TopProperty, PAy);
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

        }
    }
}

