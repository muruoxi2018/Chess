using System;
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
        /// 检查本棋子是否对将帅构成将军，在走棋之后判断
        /// </summary>
        /// <param name="jiangOrShuai"> 0：黑将，16：红帅 </param>
        /// <returns>
        /// 返回一维数组，其中有三个数据
        /// int[0]==-1: 没有发生将军
        /// int[0]==0: 黑将被将军
        /// int[0]==16: 红帅被将军 
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
                    myQiPan[i, j] = GlobalValue.qiPan[i, j];
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
        /// 棋子移动后，判断对方是否被绝杀
        /// </summary>
        /// <param name="moveQiZi">最后移动的棋子</param>
        /// <returns>true=已被绝杀</returns>
        public static bool IsJueSha(int moveQiZi)
        {

            int[] jiangJun = { -1, -1, -1 };
            if (moveQiZi < 16) jiangJun = IsJiangJun(16); // 检查红帅是否被将军。
            if (moveQiZi >= 16) jiangJun = IsJiangJun(0); // 检查黑将是否被将军
            GlobalValue.jiangJunTiShi.Content = ""; // 在棋盘上部用文字显示棋局状态，主要用于调试，后期可优化为图像模式
            if (jiangJun[0] == -1) return false;  // 没有被将军时，则不需检测是否绝杀
            string gongJiQiZi1; // 第一个攻击棋子的名字
            if (jiangJun[1] != -1) gongJiQiZi1 = GlobalValue.qiZiImageFileName[jiangJun[1]]; else gongJiQiZi1 = "";
            string gongJiQiZi2; // 第二个攻击棋子的名字
            if (jiangJun[2] != -1) gongJiQiZi2 = "和" + GlobalValue.qiZiImageFileName[jiangJun[2]]; else gongJiQiZi2 = "";
            if (jiangJun[0] == 0) // 被将军的是黑将
            {
                GlobalValue.jiangJunTiShi.Content = "1、【黑将】正被将军！";

                bool[,] points = MoveCheck.GetPathPoints(0, GlobalValue.qiPan); // 获取黑将的可移动路径
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 0; j <= 2; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(0, i, j, GlobalValue.qiPan)) // 检查可移动路径是否是对方的攻击点
                        {
                            selfCanMove = true; // 如果不是对方的攻击点，则可移动到请该点。
                            break;
                        }
                    }
                if (selfCanMove)
                {
                    GlobalValue.jiangJunTiShi.Content += " 2、【黑将】被" + gongJiQiZi1 + "将军，可移动位置解杀。";
                }
                else
                {
                    if (jiangJun[2] != -1) // 如果是双将
                    {
                        if ((jiangJun[1] is 21 or 22) || (jiangJun[2] is 21 or 22))
                        {
                            GlobalValue.jiangJunTiShi.Content += " 3、【黑将】不能移动，被" +gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }
                        GlobalValue.jiangJunTiShi.Content += " 4、【黑将】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，请求外援！";
                    }
                    else
                    {
                        GlobalValue.jiangJunTiShi.Content += " 5、【黑将】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }


                    if (!JieSha(jiangJun[1])) // 本方其他棋子解杀不成
                    {
                        GlobalValue.jiangJunTiShi.Content += " 6、【黑将】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    };
                }
            }
            if (jiangJun[0] == 16) // 被将军的是红帅
            {
                GlobalValue.jiangJunTiShi.Content = " 2、【红帅】被" + gongJiQiZi1 + "将军！";

                bool[,] points = MoveCheck.GetPathPoints(16, GlobalValue.qiPan);
                bool selfCanMove = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 7; j <= 9; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(16, i, j, GlobalValue.qiPan))
                        {
                            selfCanMove = true;
                            break;
                        }
                    }
                if (selfCanMove)
                {
                    GlobalValue.jiangJunTiShi.Content = " 3、【红帅】被" + gongJiQiZi1 + "将军！！红帅可自己移动解杀。";
                }
                else
                {
                    if (jiangJun[2] != -1) // 双将
                    {
                        if ((jiangJun[1] is 5 or 6) || (jiangJun[2] is 5 or 6))
                        {
                            GlobalValue.jiangJunTiShi.Content += " 5、【红帅】不能移动，被" + gongJiQiZi1 + gongJiQiZi2 + "双将绝杀！";
                            return true;
                        }

                        GlobalValue.jiangJunTiShi.Content = " 4、【红帅】被" + gongJiQiZi1 + gongJiQiZi2 + "双将，不能移动，请求外援！";
                    }
                    else // 单将
                    {
                        GlobalValue.jiangJunTiShi.Content = " 5、【红帅】被" + gongJiQiZi1 + "将军，不能移动，请求外援。";
                    }
                    if (!JieSha(jiangJun[1]))  // 绝杀判断
                    {
                        GlobalValue.jiangJunTiShi.Content = " 6、【红帅】被" + gongJiQiZi1 + "绝杀！";
                        return true;
                    };

                }
            }
            return false;
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

            bool[,] points = new bool[9, 10];

            #region 如果是炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，则解杀
            switch (gongJiQiZi)  // 如果是炮将军时，查找炮与将帅之间的被将军方的棋子，如可移开，则解杀
            {
                case 9:
                case 10: // 攻击棋子为黑方炮(9,10)，查找黑炮与红帅之间的红方棋子，如可移开，则解杀
                    int findCol = -1;
                    int findRow = -1;
                    if (gongJiQiZiCol == redShuaiCol) // 攻击方向为纵向
                    {
                        if (gongJiQiZiRow < redShuaiRow) // 从上方攻击
                        {
                            for (int row = gongJiQiZiRow + 1; row < redShuaiRow; row++)
                            {
                                if (GlobalValue.qiPan[gongJiQiZiCol, row] is > 16 and < 32)
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
                                if (GlobalValue.qiPan[gongJiQiZiCol, row] is > 16 and < 32)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                    }
                    if (gongJiQiZiRow == redShuaiRow) // 攻击方向为横向
                    {
                        if (gongJiQiZiCol < redShuaiCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < redShuaiCol; col++)
                            {
                                if (GlobalValue.qiPan[col, gongJiQiZiRow] is > 16 and < 32)
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
                                if (GlobalValue.qiPan[col, gongJiQiZiRow] is > 16 and < 32)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                    }
                    if (findCol == -1 || findRow == -1) break;
                    points = MoveCheck.GetPathPoints(GlobalValue.qiPan[findCol, findRow], GlobalValue.qiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (points[i, j] == true && j != gongJiQiZiRow) return true;
                        }
                    }
                    break;

                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)，查找红炮与黑将之间的黑方棋子，如可移开，则解杀
                    findCol = -1;
                    findRow = -1;
                    if (gongJiQiZiCol == blackJiangCol) // 攻击方向为纵向
                    {
                        if (gongJiQiZiRow < blackJiangRow) // 从上方攻击
                        {
                            for (int row = gongJiQiZiRow + 1; row < blackJiangRow; row++)
                            {
                                if (GlobalValue.qiPan[gongJiQiZiCol, row] is > 0 and < 16)
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
                                if (GlobalValue.qiPan[gongJiQiZiCol, row] is > 0 and < 16)
                                {
                                    findCol = gongJiQiZiCol;
                                    findRow = row;
                                    break;
                                }
                            }
                        }
                    }
                    if (gongJiQiZiRow == blackJiangRow) // 攻击方向为横向
                    {
                        if (gongJiQiZiCol < blackJiangCol) // 从左方攻击
                        {
                            for (int col = gongJiQiZiCol + 1; col < blackJiangCol; col++)
                            {
                                if (GlobalValue.qiPan[col, gongJiQiZiRow] is > 0 and < 16)
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
                                if (GlobalValue.qiPan[col, gongJiQiZiRow] is > 0 and < 16)
                                {
                                    findCol = col;
                                    findRow = gongJiQiZiRow;
                                    break;
                                }
                            }
                        }
                    }
                    if (findCol == -1 || findRow == -1) break;
                    points = MoveCheck.GetPathPoints(GlobalValue.qiPan[findCol, findRow], GlobalValue.qiPan);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (points[i, j] == true && j != gongJiQiZiRow) return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            ArrayList jieShaPoints = new(); // 可解除攻击的点位

            #region  根据发起将军棋子的位置，以及被将军的将帅的位置，计算所有可解除将军的点位，存放到数组列表JieShaPoints中，以备进一步分析
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
                    int qizi = GlobalValue.qiPan[i, j]; // 从棋盘上找到存活的本方棋子
                    if (gongJiQiZi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                    if (gongJiQiZi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                                    return true;  // true=能够解杀
                            }
                        }
                    }
                }
            return false;  // false=不能解杀
        }
    }

}
