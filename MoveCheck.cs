using System;

namespace Chess
{
    public class MoveCheck
    {
        public static int Getpath(int thisqz)   // 查找棋子可移动的路径，并标记

        {
            //GlobalValue.qzpath.Initialize();//  清除所有棋子路径数据
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.pathImage[i, j].hasPoint = false;
                    GlobalValue.pathImage[i, j].SetHidden();

                    if (QzMoveCheck(thisqz, i, j))
                    {
                        //GlobalValue.qzpath[i, j] = true;
                        GlobalValue.pathImage[i, j].hasPoint = true;
                    }
                }
            }

            //GlobalValue.qzpath[GlobalValue.myqz[thisqz].Col, GlobalValue.myqz[thisqz].Row] = false;
            GlobalValue.pathImage[GlobalValue.myqz[thisqz].Col, GlobalValue.myqz[thisqz].Row].hasPoint = false;
            return 0;
        }

        // 检查棋子移动目标位置的有效性
        public static bool QzMoveCheck(int MoveQiZi, int m, int n)
        {
            if (MoveQiZi > 31)  // 如果没有预选棋子
            {
                return false;
            }
            if (IsTongBang(MoveQiZi, GlobalValue.qipan[m, n]))
            {
                return false;
            }

            int mypx = GlobalValue.myqz[MoveQiZi].Col;
            int mypy = GlobalValue.myqz[MoveQiZi].Row;

            switch (MoveQiZi)
            {
                case 7:
                case 8:
                case 23:
                case 24:    // 车的移动  ================================================
                    if ((mypx != m) && (mypy != n))  // 车只能水平或垂直移动
                    {
                        return false;
                    }
                    if (mypx == m)  // 车的垂直方向
                    {
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) && (GlobalValue.myqz[i].Col == GlobalValue.myqz[MoveQiZi].Col))  // 查找同一条竖线上的棋子
                            {
                                if (((mypy > n) && (GlobalValue.myqz[i].Row >= (n + 1)) && (GlobalValue.myqz[i].Row <= (mypy - 1)))
                                    || ((mypy < n) && (GlobalValue.myqz[i].Row >= (mypy + 1)) && (GlobalValue.myqz[i].Row <= (n - 1))))
                                // 如果有棋子在原位置和目标位置之间，则无效
                                {
                                    return false;

                                }
                            }
                        }
                    }
                    if (mypy == n)  // 车的水平方向
                    {
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) && (GlobalValue.myqz[i].Row == GlobalValue.myqz[MoveQiZi].Row))  // 查找同一条水平线上的棋子
                            {
                                if (((mypx > m) && (GlobalValue.myqz[i].Col >= (m + 1)) && (GlobalValue.myqz[i].Col <= (mypx - 1))) ||
                                  ((mypx < m) && (GlobalValue.myqz[i].Col >= (mypx + 1)) && (GlobalValue.myqz[i].Col <= (m - 1))))
                                // 如果有棋子在原位置和目标位置之间，则无效
                                {
                                    return false;

                                }
                            }
                        }
                    }
                    break;

                case 5:
                case 6:
                case 21:
                case 22:
                    // 马的移动 ================================================
                    if ((mypx == m) || (mypy == n))  // 马只能斜向移动
                    {
                        return false;
                    }
                    if ((Math.Abs(mypx - m) + Math.Abs(mypy - n)) != 3)  // 马一次只能斜跳两步
                    {
                        return false;
                    }
                    if (Math.Abs(mypx - m) == 1)  // 60度跳马
                    {
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) &&
                              (GlobalValue.myqz[i].Col == GlobalValue.myqz[MoveQiZi].Col))  // 查找上下两侧有没有棋子，不能别马腿
                            {
                                if (((mypy > n) && (GlobalValue.myqz[i].Row == (mypy - 1))) ||
                                 ((mypy < n) && (GlobalValue.myqz[i].Row == (mypy + 1))))
                                // 如果上下两侧有棋子，则无效
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (Math.Abs(mypx - m) == 2)  // 30度跳马
                    {
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) &&
                              (GlobalValue.myqz[i].Row == GlobalValue.myqz[MoveQiZi].Row))  // 查找左右两侧有没有棋子，不能别马腿
                            {
                                if (((mypx > m) && (GlobalValue.myqz[i].Col == (mypx - 1))) ||
                                 ((mypx < m) && (GlobalValue.myqz[i].Col == (mypx + 1))))
                                // 如果左右两侧有棋子，则无效
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    break;

                case 3:
                case 4:
                case 19:
                case 20: // 相的移动 ================================================
                    if ((Math.Abs(mypx - m) != 2) || (Math.Abs(mypy - n) != 2))  // 相只能走方
                    {
                        return false;
                    }
                    if ((GlobalValue.myqz[MoveQiZi].Pcolor == GlobalValue.qipanfanzhuan) && (n > 4))  // 相不能过河
                    {
                        return false;
                    }
                    if ((GlobalValue.myqz[MoveQiZi].Pcolor == !GlobalValue.qipanfanzhuan) && (n < 5))  // 相不能过河
                    {
                        return false;
                    }
                    for (int i = 0; i <= 31; i++)
                    {
                        if ((GlobalValue.myqz[i].Col == ((mypx + m) / 2)) &&
                          (GlobalValue.myqz[i].Row == ((mypy + n) / 2)))  // 不能别相腿
                        {
                            return false;
                        }
                    }
                    break;
                case 1:
                case 2:
                case 17:
                case 18: // 士的移动 ================================================
                    if ((m < 3) || (m > 5) || ((n > 2) && (n < 7)))  // 士不能出将士区
                    {
                        return false;

                    }
                    if ((m == GlobalValue.myqz[MoveQiZi].Col) || (n == GlobalValue.myqz[MoveQiZi].Row))  // 士只能走斜
                    {
                        return false;

                    }
                    if ((Math.Abs(m - mypx) > 1) || (Math.Abs(n - mypy) > 1))  // 士一次只能飞一步
                    {
                        return false;

                    }
                    break;
                case 0:
                case 16: // 将帅的移动 ================================================
                    if ((m < 3) || (m > 5) || ((n > 2) && (n < 7)))  // 将帅不能出将士区
                    {
                        return false;
                    }
                    if ((m != GlobalValue.myqz[MoveQiZi].Col) && (n != GlobalValue.myqz[MoveQiZi].Row))  // 将帅只能直走
                    {
                        return false;
                    }
                    if ((Math.Abs(m - mypx) > 1) || (Math.Abs(n - mypy) > 1))  // 将帅一次只能移动一步
                    {
                        return false;
                    }
                    int j;
                    //if ((MoveQiZi=4) &&  (GlobalValue.myqz[20].col = m))  || ((MoveQiZi=20) &&  (GlobalValue.myqz[4].col = m))       // 将帅平移后不能见面
                    if ((Math.Abs(GlobalValue.myqz[20].Col - GlobalValue.myqz[4].Col) == 1) && ((m == GlobalValue.myqz[20].Col) || (m == GlobalValue.myqz[4].Col)))
                    {
                        j = 0;
                        for (int i = Math.Min(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) + 1; i < Math.Max(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) - 1; i++)
                        {
                            if (GlobalValue.qipan[m, i] != -1)
                            {
                                j++;
                            }
                        }
                        if ((j == 0) && (m != mypx))       //   将帅平移后，如果他们之间没有棋子，则不能不能平移
                        {
                            return false;
                        }
                    }
                    if ((GlobalValue.myqz[4].Col == GlobalValue.myqz[20].Col) && (mypx == m))      //  将帅前进吃子后不能见面
                    {
                        j = 0;
                        for (int i = Math.Min(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) + 1; i < Math.Max(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) - 1; i++)
                        {
                            if (GlobalValue.qipan[mypx, i] != -1)
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            return false;
                        }
                        if ((MoveQiZi == 4) && (Math.Abs(GlobalValue.myqz[4].Row - n) == 1))
                        {
                            j = 0;
                            for (int i = Math.Min(n, GlobalValue.myqz[20].Row) + 1; i < Math.Max(n, GlobalValue.myqz[20].Row) - 1; i++)
                            {
                                if (GlobalValue.qipan[mypx, i] != -1)
                                {
                                    j++;
                                }
                            }
                            if (j == 0)
                            {
                                return false;
                            }
                        }
                        if ((MoveQiZi == 20) && (Math.Abs(GlobalValue.myqz[20].Row - n) == 1))
                        {
                            j = 0;
                            for (int i = Math.Min(n, GlobalValue.myqz[4].Row) + 1; i < Math.Max(n, GlobalValue.myqz[4].Row) - 1; i++)
                            {
                                if (GlobalValue.qipan[mypx, i] != -1)
                                {
                                    j++;
                                }
                            }
                            if (j == 0)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case 9:
                case 10:
                case 25:
                case 26: // 炮的移动 ================================================
                    if ((mypx != m) && (mypy != n))  // 炮只能水平或垂直移动
                    {
                        return false;
                    }
                    j = 0;
                    if ((mypx == m) && (mypy != n))  // 炮垂直移动时
                    {
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) && (GlobalValue.myqz[i].Col == GlobalValue.myqz[MoveQiZi].Col))  // 查找同一条竖线上的棋子
                            {
                                if (((mypy > n) && (GlobalValue.myqz[i].Row >= (n + 1)) && (GlobalValue.myqz[i].Row <= (mypy - 1))) ||
                                        ((mypy < n) && (GlobalValue.myqz[i].Row >= (mypy + 1)) && (GlobalValue.myqz[i].Row <= (n - 1))))
                                {
                                    // 查找原位置和目标位置之间对方棋子的数量
                                    j++;
                                }
                            }
                        }
                    }
                    if ((mypx != m) && (mypy == n))  // 炮水平移动时
                    {
                        j = 0;
                        for (int i = 0; i <= 31; i++)
                        {
                            if ((GlobalValue.myqz[i].GetId() != GlobalValue.myqz[MoveQiZi].GetId()) && (GlobalValue.myqz[i].Row == GlobalValue.myqz[MoveQiZi].Row))  // 查找同一条水平线上的棋子
                            {
                                if (((mypx > m) && (GlobalValue.myqz[i].Col >= (m + 1)) && (GlobalValue.myqz[i].Col <= (mypx - 1))) ||
                                        ((mypx < m) && (GlobalValue.myqz[i].Col >= (mypx + 1)) && (GlobalValue.myqz[i].Col <= (m - 1))))
                                {
                                    // 如果有棋子在原位置和目标位置之间，则无效
                                    j++;
                                }
                            }
                        }
                    }
                    if ((j == 0) && (GlobalValue.qipan[m, n] != -1))  // 如果没有隔子，但目标位置有子，则无效
                    {
                        return false;
                    }
                    if ((j == 1) && (GlobalValue.qipan[m, n] == -1))  // 如果隔了一个子，但目标位置没有子，则无效
                    {
                        return false;
                    }
                    if (j > 1)  // 如果隔了两个子以上，则无效
                    {
                        return false;
                    }
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:

                    // 卒的移动 ================================================

                    if (GlobalValue.myqz[MoveQiZi].Pcolor == GlobalValue.qipanfanzhuan)  // 上方兵卒的移动
                    {
                        if (n < mypy)  // 兵卒不能后退
                        {
                            return false;

                        }
                        if (((mypy == 3) || (mypy == 4)) && (m != mypx))
                        // 未过河之前，不能水平移动
                        {
                            return false;

                        }
                    }
                    if (GlobalValue.myqz[MoveQiZi].Pcolor == !GlobalValue.qipanfanzhuan)  // 下方兵卒的移动
                    {
                        if (n > mypy)  // 兵卒不能后退
                        {
                            return false;

                        }
                        if (((mypy == 5) || (mypy == 6)) && (m != mypx))  // 未过河之前，不能水平移动
                        {
                            return false;

                        }
                    }
                    if ((Math.Abs(m - mypx) > 1) || (Math.Abs(n - mypy) > 1))  // 兵卒一次只能移动一步
                    {
                        return false;

                    }
                    if (Math.Abs(m - mypx) == Math.Abs(n - mypy))  // 兵卒不能斜着走
                    {
                        return false;

                    }
                    break;
                default:
                    return false;
            }

            if ((MoveQiZi != 4) && (MoveQiZi != 20) && (GlobalValue.myqz[4].Col == GlobalValue.myqz[20].Col) && (mypx == GlobalValue.myqz[4].Col))       // 将帅之间只有一个棋子时，该棋子不可以平移
            {
                int j = 0;
                for (int i = Math.Min(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) + 1; i < Math.Max(GlobalValue.myqz[4].Row, GlobalValue.myqz[20].Row) - 1; i++)
                {
                    if (GlobalValue.qipan[GlobalValue.myqz[4].Col, i] != -1)
                    {
                        j++;
                    }
                }
                if ((j == 1) && (m != mypx))       //   将帅之间只有一个棋子时，该棋子不可以平移
                {
                    return false;

                }

            }
            return true;
        }

        // 判断是不是同一方棋子
        public static bool IsTongBang(int qz1, int qz2)
        {
            if ((qz1 < 0) || (qz2 < 0)) return false;
            return ((qz1 <= 15) && (qz2 <= 15)) || ((qz1 >= 16) && (qz2 >= 16));
        }
        public static bool IsIsTongBang(int m0, int n0, int m1, int n1)
        {
            return GlobalValue.qipan[m1, n1] != -1 && (((GlobalValue.qipan[m0, n0] <= 15) && (GlobalValue.qipan[m1, n1] <= 15)) || ((GlobalValue.qipan[m0, n0] >= 16) && (GlobalValue.qipan[m1, n1] >= 16)));
        }

    }
}
