using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Chess.SuanFa
{
    class JiangJun
    {

        /// <summary>
        /// 检查本棋子是否对将帅构成将军，用于走棋之后判断
        /// </summary>
        /// <param name="JiangOrShuai"> 0：黑将，16：红帅 </param>
        /// <returns>一维数组，其中有三个数据
        /// int[0]==-1: 没有发生将军
        /// int[0]==0: 黑将被将军
        /// int[0]==16: 红帅被将军 
        /// int[1]: 对方将军的棋子编号
        /// int[2]: 发生双将时的对方将军的第二个棋子的编号
        /// </returns>
        public static int[] IsJiangJun(int JiangOrShuai)
        {

            int[] vs = { -1, -1, -1 };
            if (JiangOrShuai != 0 && JiangOrShuai != 16) return vs;
            int[,] myqipan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myqipan[i, j] = GlobalValue.QiPan[i, j];
                }

            bool[,] thispoints;

            if (JiangOrShuai == 16)
            {
                vs[0] = vs[1] = vs[2] = -1;
                for (int qizi = 5; qizi <= 15; qizi++) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    if (GlobalValue.QiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue;
                    thispoints = MoveCheck.GetPathPoints(qizi, myqipan);
                    int x = GlobalValue.QiZiArray[16].Col;
                    int y = GlobalValue.QiZiArray[16].Row;
                    if (thispoints[x, y] == true)
                    {
                        vs[0] = 16;
                        if (vs[1] == -1)
                        {
                            vs[1] = qizi;
                        }
                        else
                        {
                            vs[2] = qizi; // 双将
                            return vs;
                        }
                    }
                }
            }
            if (JiangOrShuai == 0)
            {
                vs[0] = vs[1] = vs[2] = -1;
                for (int qizi = 21; qizi <= 31; qizi++) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                {
                    if (GlobalValue.QiZiArray[qizi].Visibility != System.Windows.Visibility.Visible) continue;
                    thispoints = MoveCheck.GetPathPoints(qizi, myqipan);
                    int x = GlobalValue.QiZiArray[0].Col;
                    int y = GlobalValue.QiZiArray[0].Row;
                    if (thispoints[x, y] == true)
                    {
                        vs[0] = 0;
                        if (vs[1] == -1)
                        {
                            vs[1] = qizi;
                        }
                        else
                        {
                            vs[2] = qizi; // 双将
                            return vs;
                        }
                    }
                }
            }
            return vs;
        }
        /// <summary>
        /// 棋子移动后，判断对方是否被绝杀
        /// </summary>
        /// <param name="MoveQizi">最后移动的棋子</param>
        /// <returns></returns>
        public static bool IsJueSha(int MoveQizi)
        {

            int[] jiangjun = { -1, -1, -1 };
            if (MoveQizi < 16) jiangjun = IsJiangJun(16);
            if (MoveQizi >= 16) jiangjun = IsJiangJun(0);
            GlobalValue.JianJunTiShi.Content = "战况";
            string GJqizi1, GJqizi2 = "";
            if (jiangjun[1] != -1) GJqizi1 = GlobalValue.QiZiImageFileName[jiangjun[1]]; else GJqizi1 = "";
            if (jiangjun[2] != -1) GJqizi2 = "和" + GlobalValue.QiZiImageFileName[jiangjun[2]]; else GJqizi2 = "";
            if (jiangjun[0] == 0)
            {
                GlobalValue.JianJunTiShi.Content = "黑将----正在被将军！！";

                bool[,] points = MoveCheck.GetPathPoints(0, GlobalValue.QiPan);
                bool test = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 0; j <= 2; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(0, i, j, GlobalValue.QiPan))
                        {
                            test = true;
                            break;
                        }
                    }
                if (test)
                {
                    GlobalValue.JianJunTiShi.Content += "黑将----正在被" + GJqizi1 + "将军！！黑将可自己移动解杀。";
                }
                else
                {
                    if (jiangjun[2] != -1)
                    {
                        GlobalValue.JianJunTiShi.Content += "黑将----无处可逃，被" + GJqizi1 + GJqizi2 + "双将绝杀！！！";
                        return true;
                    }
                    else
                    {
                        GlobalValue.JianJunTiShi.Content += "黑将----正在被" + GJqizi1 + "将军，困于老巢，请求外援。";
                        if (!NoJieSha(jiangjun[1]))
                        {
                            GlobalValue.JianJunTiShi.Content += "黑将----被" + GJqizi1 + "绝杀！！！";
                            return true;
                        };
                    }
                }
            }
            if (jiangjun[0] == 16)
            {
                GlobalValue.JianJunTiShi.Content = "红帅----正在被" + GJqizi1 + "将军！！";

                bool[,] points = MoveCheck.GetPathPoints(16, GlobalValue.QiPan);
                bool test = false;
                for (int i = 3; i <= 5; i++)
                    for (int j = 7; j <= 9; j++)
                    {
                        if (points[i, j] == true && !MoveCheck.IsKilledPoint(16, i, j, GlobalValue.QiPan))
                        {
                            test = true;
                            break;
                        }
                    }
                if (test)
                {
                    GlobalValue.JianJunTiShi.Content = "红帅----正在被" + GJqizi1 + "将军！！红帅可自己移动解杀。";
                }
                else
                {
                    if (jiangjun[2] != -1)
                    {
                        GlobalValue.JianJunTiShi.Content = "红帅----无处可逃，被" + GJqizi1 + GJqizi2 + "双将绝杀！！！";
                        return true;
                    }
                    else
                    {
                        GlobalValue.JianJunTiShi.Content = "红帅----正在被" + GJqizi1 + "将军，困于老巢，请求外援。";
                        if (!NoJieSha(jiangjun[1]))
                        {
                            GlobalValue.JianJunTiShi.Content = "红帅----被" + GJqizi1 + "绝杀！！！";
                            return true;
                        };
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// 被将军是，判断是否已被绝杀
        /// </summary>
        /// <param name="GJqizi">发起将军的棋子</param>
        /// <returns>true=未被绝杀，false=被绝杀</returns>
        private static bool NoJieSha(int GJqizi)
        {
            //黑方：车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
            //红方：车(23,24)，马(21,22)，炮(25,26)，兵(27,28,29,30,31)
            if (GJqizi is >= 11 and <= 15) return false;  //  黑方：卒(11,12,13,14,15)
            if (GJqizi is >= 27 and <= 31) return false;  //  红方：兵(27,28,29,30,31)
            int GJqiziCol = GlobalValue.QiZiArray[GJqizi].Col;
            int GJqiziRow = GlobalValue.QiZiArray[GJqizi].Row;
            int HeiJiangCol = GlobalValue.QiZiArray[0].Col;
            int HeiJiangRow = GlobalValue.QiZiArray[0].Row;
            int HongShuaiCol = GlobalValue.QiZiArray[16].Col;
            int HongShuaiRow = GlobalValue.QiZiArray[16].Row;
            ArrayList JieShaPoints = new ArrayList(); // 可解除攻击的点位
            //int[] jsPoint = new int[2];
            switch (GJqizi) // 根据发起将军棋子的位置，以及被将军的将帅的位置，计算或解除攻击的所有点位，存放到数组列表中
            {
                case 5:
                case 6:     //  攻击棋子为黑方马(5,6)
                    if (GJqiziRow - HongShuaiRow == 2) // 马从上方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol;
                        jsPoint[1] = GJqiziRow + 1; //  别马腿位置
                        JieShaPoints.Add(jsPoint);
                    }
                    if (HongShuaiRow - GJqiziRow == 2) // 马从下方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol;
                        jsPoint[1] = GJqiziRow - 1;
                        JieShaPoints.Add(jsPoint);
                    }
                    if (GJqiziCol - HongShuaiCol == 2) // 马从右方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol - 1;
                        jsPoint[1] = GJqiziRow;
                        JieShaPoints.Add(jsPoint);
                    }
                    if (HongShuaiCol - GJqiziCol == 2) // 马从左方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol + 1;
                        jsPoint[1] = GJqiziRow;
                        JieShaPoints.Add(jsPoint);
                    }
                    break;
                case 21:
                case 22:    //  攻击棋子为红方马(21,22)
                    if (GJqiziRow - HeiJiangRow == 2) // 马从上方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol;
                        jsPoint[1] = GJqiziRow + 1; //  别马腿位置
                        JieShaPoints.Add(jsPoint);

                    }
                    if (HeiJiangRow - GJqiziRow == 2) // 马从下方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol;
                        jsPoint[1] = GJqiziRow - 1;
                        JieShaPoints.Add(jsPoint);

                    }
                    if (GJqiziCol - HeiJiangCol == 2) // 马从右方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol - 1;
                        jsPoint[1] = GJqiziRow;
                        JieShaPoints.Add(jsPoint);

                    }
                    if (HeiJiangCol - GJqiziCol == 2) // 马从左方攻击
                    {
                        int[] jsPoint = { -1, -1 };
                        jsPoint[0] = GJqiziCol + 1;
                        jsPoint[1] = GJqiziRow;
                        JieShaPoints.Add(jsPoint);

                    }
                    break;
                case 7:
                case 8:     //  攻击棋子为黑方车(7,8)
                case 9:
                case 10:    //  攻击棋子为黑方炮(9,10)
                    if (GJqiziCol == HongShuaiCol) // 攻击方向为纵向
                    {
                        if (GJqiziRow < HongShuaiRow) // 从上方攻击
                        {
                            for (int row = GJqiziRow; row < HongShuaiRow; row++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = GJqiziCol;
                                jsPoint[1] = row;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = HongShuaiRow + 1; row <= GJqiziRow; row++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = GJqiziCol;
                                jsPoint[1] = row;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                    }
                    if (GJqiziRow == HongShuaiRow) // 攻击方向为横向
                    {
                        if (GJqiziCol < HongShuaiCol) // 从左方攻击
                        {
                            for (int col = GJqiziCol; col < HongShuaiCol; col++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = col;
                                jsPoint[1] = GJqiziRow;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = HongShuaiCol + 1; col <= GJqiziCol; col++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = col;
                                jsPoint[1] = GJqiziRow;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                    }
                    break;
                case 23:
                case 24:    //  攻击棋子为红方车(23,24)
                case 25:
                case 26:    //  攻击棋子为红方炮(25,26)
                    if (GJqiziCol == HeiJiangCol) // 攻击方向为纵向
                    {
                        if (GJqiziRow < HeiJiangRow) // 从上方攻击
                        {
                            for (int row = GJqiziRow; row < HeiJiangRow; row++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = GJqiziCol;
                                jsPoint[1] = row;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                        else // 从下方攻击
                        {
                            for (int row = HeiJiangRow + 1; row <= GJqiziRow; row++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = GJqiziCol;
                                jsPoint[1] = row;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                    }
                    if (GJqiziRow == HeiJiangRow) // 攻击方向为横向
                    {
                        if (GJqiziCol < HeiJiangCol) // 从左方攻击
                        {
                            for (int col = GJqiziCol; col < HeiJiangCol; col++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = col;
                                jsPoint[1] = GJqiziRow;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                        else // 从右方攻击
                        {
                            for (int col = HeiJiangCol + 1; col <= GJqiziCol; col++)
                            {
                                int[] jsPoint = { -1, -1 };
                                jsPoint[0] = col;
                                jsPoint[1] = GJqiziRow;
                                JieShaPoints.Add(jsPoint);
                            }
                        }
                    }
                    break;
                default:
                    break;

            }


            if (JieShaPoints.Count == 0) return false;  // 没有可以解除攻击的点位，估计不存在这个情况。

            bool[,] thispoints;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    int qizi = GlobalValue.QiPan[i, j]; // 从棋盘上找到存活的本方棋子
                    if (GJqizi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in JieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;
                            }
                        }
                    }
                    if (GJqizi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
                    {
                        thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.QiPan); // 获得本方棋子的可移动路径
                        foreach (int[] point in JieShaPoints) // 逐个取出可解除将军的点位坐标
                        {
                            if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                            {
                                if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.QiPan))
                                    return true;
                            }
                        }
                    }
                }
            return false;  // false=被绝杀
        }
    }

}
