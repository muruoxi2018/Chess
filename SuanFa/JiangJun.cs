﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Chess.SuanFa // 算法
{
    class JiangJun  // 将军
    {


        /// <summary>
        /// 棋子移动后，判断对方是否被绝杀
        /// </summary>
        /// <param name="moveQiZi">最后移动的棋子</param>
        /// <returns>true=已被绝杀</returns>
        public static bool IsJueSha(int moveQiZi)
        {

            int[] jiangJun = { -1, -1, -1 };
            if (moveQiZi < 16) jiangJun = IsJiangJun(16); // 检查红帅是否被将军。
            if (moveQiZi >= 16) jiangJun = IsJiangJun(0); // 检查黑将是否被将军
            GlobalValue.jiangJunTiShi.Text = ""; // 在棋盘上部用文字显示棋局状态，主要用于调试，后期可优化为图像模式
            if (jiangJun[0] == -1) return false;  // 没有被将军时，则不需检测是否绝杀
            string gongJiQiZi1; // 第一个攻击棋子的名字
            if (jiangJun[1] != -1) gongJiQiZi1 = GlobalValue.qiZiImageFileName[jiangJun[1]]; else gongJiQiZi1 = "";
            string gongJiQiZi2; // 第二个攻击棋子的名字
            if (jiangJun[2] != -1) gongJiQiZi2 = "和" + GlobalValue.qiZiImageFileName[jiangJun[2]]; else gongJiQiZi2 = "";
            if (jiangJun[0] == 0) // 被将军的是黑将
            {
                GlobalValue.jiangJunTiShi.Text = "1、【黑将】正被将军！";

                bool[,] points = MoveCheck.GetPathPoints(0, GlobalValue.QiPan); // 获取黑将的可移动路径
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 0; j <= 2; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(0, i, j, GlobalValue.QiPan)) // 检查黑将可移动路径是否是对方的攻击点
                        {
                            selfCanMove = true; // 如果不是对方的攻击点，则可移动到请该点。
                            break;
                        }
                    }
                if (selfCanMove)  // 黑将可移动解杀时
                {
                    GlobalValue.jiangJunTiShi.Text += " 2、【黑将】被" + gongJiQiZi1 + "将军，可移动位置解杀。";
                    return false;
                }
                else  // 黑将不可移动时
                {
                    if (jiangJun[2] != -1) // 如果是双将
                    {
                        if ((jiangJun[1] is 21 or 22) || (jiangJun[2] is 21 or 22))
                        {
                            GlobalValue.jiangJunTiShi.Text += " 3、【黑将】不能移动，被" + gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }
                        GlobalValue.jiangJunTiShi.Text += " 4、【黑将】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，请求外援！";
                    }
                    else
                    {
                        GlobalValue.jiangJunTiShi.Text += " 5、【黑将】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }


                    if (!JieSha(jiangJun[1])) // 本方其他棋子解杀不成
                    {
                        GlobalValue.jiangJunTiShi.Text += " 6、【黑将】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    };
                }
            }
            if (jiangJun[0] == 16) // 被将军的是红帅
            {
                #region 被将军的是红帅
                GlobalValue.jiangJunTiShi.Text = " 2、【红帅】被" + gongJiQiZi1 + "将军！";

                bool[,] points = MoveCheck.GetPathPoints(16, GlobalValue.QiPan);
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 7; j <= 9; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(16, i, j, GlobalValue.QiPan))
                        {
                            selfCanMove = true;
                            break;
                        }
                    }
                if (selfCanMove)
                {
                    GlobalValue.jiangJunTiShi.Text = " 3、【红帅】被" + gongJiQiZi1 + "将军！！红帅可自己移动解杀。";
                }
                else
                {
                    if (jiangJun[2] != -1) // 双将
                    {
                        if ((jiangJun[1] is 5 or 6) || (jiangJun[2] is 5 or 6))
                        {
                            GlobalValue.jiangJunTiShi.Text += " 5、【红帅】不能移动，被" + gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }

                        GlobalValue.jiangJunTiShi.Text = " 4、【红帅】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，不能移动，请求外援！";
                    }
                    else // 单将
                    {
                        GlobalValue.jiangJunTiShi.Text = " 5、【红帅】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }
                    if (!JieSha(jiangJun[1]))  // 绝杀判断
                    {
                        GlobalValue.jiangJunTiShi.Text = " 6、【红帅】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    };

                }
                #endregion
            }
            return false;
        }
        /// <summary>
        /// 检查本棋子是否对将帅构成将军，在走棋之后判断
        /// </summary>
        /// <param name="jiangOrShuai"> 0：黑将，16：红帅 </param>
        /// <returns>
        /// 返回一维数组，其中有三个数据
        /// int[0]=-1: 没有发生将军
        /// int[0]=0: 黑将被将军
        /// int[0]=16: 红帅被将军 
        /// int[1]: 对方将军的棋子编号
        /// int[2]: 发生双将时的对方将军的第二个棋子的编号
        /// </returns>
        public static int[] IsJiangJun(int jiangOrShuai)
        {

            int[] jiangJunQiZi = { -1, -1, -1 }; // 保存发起将军的所有棋子，可能是一个，也可能是两个。
            if (jiangOrShuai != 0 && jiangOrShuai != 16) return jiangJunQiZi;
            int[,] myQiPan = new int[9, 10]; // 复制一份棋盘副本，防止破坏原棋盘数组的数据
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myQiPan[i, j] = GlobalValue.QiPan[i, j];
                }

            bool[,] thisPoints;

            if (jiangOrShuai == 16) // 被将军的是红帅
            {
                jiangJunQiZi[0] = jiangJunQiZi[1] = jiangJunQiZi[2] = -1;
                for (int qizi = 5; qizi <= 15; qizi++) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    if (GlobalValue.qiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue; // 已死的棋子排除
                    thisPoints = MoveCheck.GetPathPoints(qizi, myQiPan);
                    int x = GlobalValue.qiZiArray[16].Col;
                    int y = GlobalValue.qiZiArray[16].Row;
                    if (thisPoints[x, y] == true)
                    {
                        jiangJunQiZi[0] = 16;
                        if (jiangJunQiZi[1] == -1)
                        {
                            jiangJunQiZi[1] = qizi; // 第一个发起将军的棋子
                        }
                        else
                        {
                            jiangJunQiZi[2] = qizi; // 双将，保存第二个发起将军的棋子
                            return jiangJunQiZi;
                        }
                    }
                }
            }
            if (jiangOrShuai == 0) // 被将军的是黑将
            {
                jiangJunQiZi[0] = jiangJunQiZi[1] = jiangJunQiZi[2] = -1;
                for (int qizi = 21; qizi <= 31; qizi++) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                {
                    if (GlobalValue.qiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue; // 已死的棋子排除
                    thisPoints = MoveCheck.GetPathPoints(qizi, myQiPan);
                    int x = GlobalValue.qiZiArray[0].Col;
                    int y = GlobalValue.qiZiArray[0].Row;
                    if (thisPoints[x, y] == true)
                    {
                        jiangJunQiZi[0] = 0;
                        if (jiangJunQiZi[1] == -1)
                        {
                            jiangJunQiZi[1] = qizi; // 第一个发起将军的棋子
                        }
                        else
                        {
                            jiangJunQiZi[2] = qizi; // 双将，保存第二个发起将军的棋子
                            return jiangJunQiZi;
                        }
                    }
                }
            }
            return jiangJunQiZi;
        }
        /// <summary>
        /// 被将军时，在老将不能动的情况下，判断本方其他棋子能否解杀
        /// </summary>
        /// <param name="gongJiQiZi">发起将军的棋子</param>
        /// <returns>true=能解杀，false=不能解杀</returns>
        private static bool JieSha(int gongJiQiZi)
        {
            //黑方：车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
            //红方：车(23,24)，马(21,22)，炮(25,26)，兵(27,28,29,30,31)
            if (gongJiQiZi is >= 11 and <= 15) return false;  //  黑方：卒(11,12,13,14,15)
            if (gongJiQiZi is >= 27 and <= 31) return false;  //  红方：兵(27,28,29,30,31)
            int gongJiQiZiCol = GlobalValue.qiZiArray[gongJiQiZi].Col;
            int gongJiQiZiRow = GlobalValue.qiZiArray[gongJiQiZi].Row;
            int blackJiangCol = GlobalValue.qiZiArray[0].Col;
            int blackJiangRow = GlobalValue.qiZiArray[0].Row;
            int redShuaiCol = GlobalValue.qiZiArray[16].Col;
            int redShuaiRow = GlobalValue.qiZiArray[16].Row;

            bool[,] points;

            #region 移子解杀。如果是炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，则解杀
            switch (gongJiQiZi)  // 如果是炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，则解杀
            {
                case 9:
                case 10: // 攻击棋子为黑方炮(9,10)，查找黑炮与红帅之间的红方棋子，如可移开，则解杀
                    if (IsOtherPaoAtBack(gongJiQiZi)) return false;
                    int findCol = -1;
                    int findRow = -1;
                    if (gongJiQiZiCol == redShuaiCol) // 黑炮与红帅在同一列时，攻击方向为纵向
                    {
                        if (gongJiQiZiRow < redShuaiRow) // 从上方攻击
                        {
                            // 在黑炮和红帅之间寻找红方棋子
                            for (int row = gongJiQiZiRow + 1; row < redShuaiRow; row++)
                            {
                                // 如果在黑炮和红帅之间找到了红方棋子，则记录该棋子位置，并停止查找
                                if (GlobalValue.QiPan[gongJiQiZiCol, row] is > 16 and < 32)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = redShuaiRow + 1; row < gongJiQiZiRow; row++)
                            {
                                if (GlobalValue.QiPan[gongJiQiZiCol, row] is > 16 and < 32)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                    }
                    if (gongJiQiZiRow == redShuaiRow) // 黑炮与红帅在同一行时，攻击方向为横向
                    {
                        if (gongJiQiZiCol < redShuaiCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < redShuaiCol; col++)
                            {
                                if (GlobalValue.QiPan[col, gongJiQiZiRow] is > 16 and < 32)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = redShuaiCol + 1; col < gongJiQiZiCol; col++)
                            {
                                if (GlobalValue.QiPan[col, gongJiQiZiRow] is > 16 and < 32)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                    }
                    // 如果没有找到可移动的棋子，则跳过。
                    if (findCol == -1 || findRow == -1) break;
                    // 否则，获取所找到棋子的可移动路径
                    points = MoveCheck.GetPathPoints(GlobalValue.QiPan[findCol, findRow], GlobalValue.QiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            // 逐个判断棋子的可移动路径，如果此路径点的行列位置与炮的行列均不相同，则可解杀成功。
                            if (points[i, j] == true && i != gongJiQiZiCol && j != gongJiQiZiRow) return true;
                        }
                    }
                    break;

                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)，查找红炮与黑将之间的黑方棋子，如可移开，则解杀
                    if (IsOtherPaoAtBack(gongJiQiZi)) return false;
                    findCol = -1;
                    findRow = -1;
                    if (gongJiQiZiCol == blackJiangCol) // 红炮与黑将在同一列时，攻击方向为纵向
                    {
                        if (gongJiQiZiRow < blackJiangRow) // 从上方攻击
                        {
                            for (int row = gongJiQiZiRow + 1; row < blackJiangRow; row++)
                            {
                                if (GlobalValue.QiPan[gongJiQiZiCol, row] is > 0 and < 16)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = blackJiangRow + 1; row < gongJiQiZiRow; row++)
                            {
                                if (GlobalValue.QiPan[gongJiQiZiCol, row] is > 0 and < 16)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                    }
                    if (gongJiQiZiRow == blackJiangRow) // 红炮与黑将在同一行时，攻击方向为横向
                    {
                        if (gongJiQiZiCol < blackJiangCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < blackJiangCol; col++)
                            {
                                if (GlobalValue.QiPan[col, gongJiQiZiRow] is > 0 and < 16)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = blackJiangCol + 1; col < gongJiQiZiCol; col++)
                            {
                                if (GlobalValue.QiPan[col, gongJiQiZiRow] is > 0 and < 16)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                    }
                    if (findCol == -1 || findRow == -1) break;
                    points = MoveCheck.GetPathPoints(GlobalValue.QiPan[findCol, findRow], GlobalValue.QiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {

                            if (points[i, j] == true && i != gongJiQiZiCol && j != gongJiQiZiRow) return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            ArrayList jieShaPoints = new(); // 可解除攻击的点位

            #region  填子解杀。根据发起将军棋子的位置，以及被将军的将帅的位置，计算所有可解除将军的点位，存放到数组列表JieShaPoints中，以备进一步分析
            jieShaPoints.Add(new int[] { gongJiQiZiCol, gongJiQiZiRow }); // 把攻击棋子的位置先加进去
            //int[] jsPoint = new int[2];
            switch (gongJiQiZi) // 根据发起将军棋子的位置，以及被将军的将帅的位置，计算或解除将军的所有点位，存放到数组列表中
            {
                case 5:
                case 6:     //  攻击棋子为黑方马(5,6)

                    if (gongJiQiZiRow - redShuaiRow == 2) // 马从上方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol, gongJiQiZiRow + 1 }); //  别马腿位置
                    }
                    if (redShuaiRow - gongJiQiZiRow == 2) // 马从下方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol, gongJiQiZiRow - 1 }); //  别马腿位置
                    }
                    if (gongJiQiZiCol - redShuaiCol == 2) // 马从右方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol - 1, gongJiQiZiRow }); //  别马腿位置
                    }
                    if (redShuaiCol - gongJiQiZiCol == 2) // 马从左方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol + 1, gongJiQiZiRow }); //  别马腿位置
                    }
                    break;
                case 21:
                case 22:    //  攻击棋子为红方马(21,22)
                    if (gongJiQiZiRow - blackJiangRow == 2) // 马从上方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol, gongJiQiZiRow + 1 }); //  别马腿位置
                    }
                    if (blackJiangRow - gongJiQiZiRow == 2) // 马从下方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol, gongJiQiZiRow - 1 }); //  别马腿位置
                    }
                    if (gongJiQiZiCol - blackJiangCol == 2) // 马从右方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol - 1, gongJiQiZiRow }); //  别马腿位置
                    }
                    if (blackJiangCol - gongJiQiZiCol == 2) // 马从左方攻击
                    {
                        jieShaPoints.Add(new int[] { gongJiQiZiCol + 1, gongJiQiZiRow }); //  别马腿位置
                    }
                    break;

                case 7:
                case 8:     //  攻击棋子为黑方车(7,8)
                case 9:
                case 10:    //  攻击棋子为黑方炮(9,10)
                    //if ((gongJiQiZi is 9 or 10) && IsOtherPaoInFront(gongJiQiZi)) break;
                    if (gongJiQiZiCol == redShuaiCol) // 攻击方向为纵向
                    {
                        if (gongJiQiZiRow < redShuaiRow) // 从上方攻击
                        {
                            for (int row = gongJiQiZiRow + 1; row < redShuaiRow; row++)
                            {
                                jieShaPoints.Add(new int[] { gongJiQiZiCol, row });
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = redShuaiRow + 1; row < gongJiQiZiRow; row++)
                            {
                                jieShaPoints.Add(new int[] { gongJiQiZiCol, row });
                            }
                        }
                    }
                    if (gongJiQiZiRow == redShuaiRow) // 攻击方向为横向
                    {
                        if (gongJiQiZiCol < redShuaiCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < redShuaiCol; col++)
                            {
                                jieShaPoints.Add(new int[] { col, gongJiQiZiRow });
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = redShuaiCol + 1; col < gongJiQiZiCol; col++)
                            {
                                jieShaPoints.Add(new int[] { col, gongJiQiZiRow });
                            }
                        }
                    }
                    break;
                case 23:
                case 24:    //  攻击棋子为红方车(23,24)
                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)
                    //if ((gongJiQiZi is 25 or 26) && IsOtherPaoInFront(gongJiQiZi)) break;
                    if (gongJiQiZiCol == blackJiangCol) // 攻击方向为纵向
                    {
                        if (gongJiQiZiRow < blackJiangRow) // 从上方攻击
                        {
                            for (int row = gongJiQiZiRow + 1; row < blackJiangRow; row++)
                            {
                                jieShaPoints.Add(new int[] { gongJiQiZiCol, row });
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = blackJiangRow + 1; row < gongJiQiZiRow; row++)
                            {
                                jieShaPoints.Add(new int[] { gongJiQiZiCol, row });
                            }
                        }
                    }
                    if (gongJiQiZiRow == blackJiangRow) // 攻击方向为横向
                    {
                        if (gongJiQiZiCol < blackJiangCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < blackJiangCol; col++)
                            {
                                jieShaPoints.Add(new int[] { col, gongJiQiZiRow });
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = blackJiangCol + 1; col < gongJiQiZiCol; col++)
                            {
                                jieShaPoints.Add(new int[] { col, gongJiQiZiRow });
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            //if (JieShaPoints.Count == 0) return false;  // 不存在可以解除攻击的点位，则不能解杀。估计不存在这个情况。
            bool[,] thispoints;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    int qizi = GlobalValue.QiPan[i, j]; // 从棋盘上找到存活的本方棋子
                    if (gongJiQiZi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                    if (gongJiQiZi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                }
            return false;  // false=不能解杀
        }

        /// <summary>
        /// 是否还有一个炮在这个炮的背后
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private static bool IsOtherPaoAtBack(int pao1)
        {
            int jiangOrShuai = (pao1 < 16) ? 16 : 0;
            int pao2 = (pao1 == 9) ? 10 : 9;
            if (pao1 > 16) pao2 = (pao1 == 25) ? 25 : 25;
            // 如果两个炮与对方将帅在同一列
            if (GlobalValue.qiZiArray[jiangOrShuai].Col == GlobalValue.qiZiArray[pao1].Col
                && GlobalValue.qiZiArray[jiangOrShuai].Col == GlobalValue.qiZiArray[pao2].Col)
            {
                int col = GlobalValue.qiZiArray[pao1].Col;
                int startRow = Math.Min(GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[pao2].Row) + 1;
                int endRow = Math.Max(GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[pao2].Row) - 1;
                for (int row = startRow; row <= endRow; row++)
                {
                    // 两个炮之间如果没有棋子，则是重炮
                    if (GlobalValue.QiPan[col, row] != -1) return false;
                }
                // 如果另一个炮不在这个炮和将帅之间，则返回true
                if (!IsBetween(GlobalValue.qiZiArray[pao2].Row, GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[jiangOrShuai].Row))
                    return true;
            }
            // 如果两个炮与对方将帅在同一行
            if (GlobalValue.qiZiArray[jiangOrShuai].Row == GlobalValue.qiZiArray[pao1].Row
                && GlobalValue.qiZiArray[jiangOrShuai].Row == GlobalValue.qiZiArray[pao2].Row)
            {
                int row = GlobalValue.qiZiArray[pao1].Row;
                int startCol = Math.Min(GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[pao2].Col) + 1;
                int endCol = Math.Max(GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[pao2].Col) - 1;
                for (int col = startCol; row <= endCol; row++)
                {
                    // 两个炮之间如果没有棋子，则是重炮
                    if (GlobalValue.QiPan[col, row] != -1) return false;
                }
                // 如果另一个炮在这个炮和将帅之间，则返回true
                if (!IsBetween(GlobalValue.qiZiArray[pao2].Col, GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[jiangOrShuai].Col))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 是否还有一个炮在这个炮的前面
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private static bool IsOtherPaoInFront(int pao1)
        {
            int jiangOrShuai = (pao1 < 16) ? 16 : 0;
            int pao2 = (pao1 == 9) ? 10 : 9;
            if (pao1 > 16) pao2 = (pao1 == 25) ? 25 : 25;
            // 如果两个炮与对方将帅在同一列
            if (GlobalValue.qiZiArray[jiangOrShuai].Col == GlobalValue.qiZiArray[pao1].Col
                && GlobalValue.qiZiArray[jiangOrShuai].Col == GlobalValue.qiZiArray[pao2].Col)
            {
                int col = GlobalValue.qiZiArray[pao1].Col;
                int startRow = Math.Min(GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[pao2].Row) + 1;
                int endRow = Math.Max(GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[pao2].Row) - 1;
                for (int row = startRow; row <= endRow; row++)
                {
                    // 两个炮之间如果没有棋子，则是重炮
                    if (GlobalValue.QiPan[col, row] != -1) return false;
                }
                // 如果另一个炮在这个炮和将帅之间，则返回true
                if (IsBetween(GlobalValue.qiZiArray[pao2].Row, GlobalValue.qiZiArray[pao1].Row, GlobalValue.qiZiArray[jiangOrShuai].Row))
                    return true;
            }
            // 如果两个炮与对方将帅在同一行
            if (GlobalValue.qiZiArray[jiangOrShuai].Row == GlobalValue.qiZiArray[pao1].Row
                && GlobalValue.qiZiArray[jiangOrShuai].Row == GlobalValue.qiZiArray[pao2].Row)
            {
                int row = GlobalValue.qiZiArray[pao1].Row;
                int startCol = Math.Min(GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[pao2].Col) + 1;
                int endCol = Math.Max(GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[pao2].Col) - 1;
                for (int col = startCol; col <= endCol; col++)
                {
                    // 两个炮之间如果没有棋子，则是重炮
                    if (GlobalValue.QiPan[col, row] != -1) return false;
                }
                // 如果另一个炮在这个炮和将帅之间，则返回true
                if (IsBetween(GlobalValue.qiZiArray[pao2].Col, GlobalValue.qiZiArray[pao1].Col, GlobalValue.qiZiArray[jiangOrShuai].Col))
                    return true;
            }
            return false;
        }
        public static bool IsBetween(int num,int start,int end)
        {
            if (num >= Math.Min(start, end) && num <= Math.Max(start, end)) return true;
            return false;
        }
    }

}
