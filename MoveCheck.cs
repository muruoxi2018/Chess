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

                }
            }
            _ = QzMoveCheck(thisqz);
            GlobalValue.pathImage[GlobalValue.myqz[thisqz].Col, GlobalValue.myqz[thisqz].Row].hasPoint = false;
            return 0;
        }

        // 检查棋子移动目标位置的有效性
        private static bool QzMoveCheck(int MoveQiZi)
        {
            if (MoveQiZi > 31)  // 如果没有预选棋子
            {
                return false;
            }
            if (MoveQiZi < 0)
            {
                return false;
            }
            int mypx = GlobalValue.myqz[MoveQiZi].Col;
            int mypy = GlobalValue.myqz[MoveQiZi].Row;
            int m = 0, n = 0;
            int side = 0;

            switch (MoveQiZi)
            {
                case 7:
                case 8:
                case 23:
                case 24:    // 车的移动  ================================================
                    if (!JustOneIsThis(MoveQiZi))
                    {
                        for (int i = mypx - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (GlobalValue.qipan[i, mypy] == -1)
                            {
                                GlobalValue.pathImage[i, mypy].hasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.qipan[i, mypy]))
                                {
                                    GlobalValue.pathImage[i, mypy].hasPoint = true;
                                }
                                break;
                            }
                        }
                        for (int i = mypx + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (GlobalValue.qipan[i, mypy] == -1)
                            {
                                GlobalValue.pathImage[i, mypy].hasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.qipan[i, mypy]))
                                {
                                    GlobalValue.pathImage[i, mypy].hasPoint = true;
                                }
                                break;
                            }
                        }
                    }
                    for (int i = mypy - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (GlobalValue.qipan[mypx, i] == -1)
                        {
                            GlobalValue.pathImage[mypx, i].hasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, i]))
                            {
                                GlobalValue.pathImage[mypx, i].hasPoint = true;
                            }
                            break;
                        }
                    }
                    for (int i = mypy + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (GlobalValue.qipan[mypx, i] == -1)
                        {
                            GlobalValue.pathImage[mypx, i].hasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, i]))
                            {
                                GlobalValue.pathImage[mypx, i].hasPoint = true;
                            }
                            break;
                        }
                    }
                    break;

                case 5:
                case 6:
                case 21:
                case 22:
                    // 马的移动 ================================================
                    if (JustOneIsThis(MoveQiZi))
                    {
                        break;
                    }
                    // 八个方向，逐个检查
                    if ((mypy > 1) && (GlobalValue.qipan[mypx, mypy - 1] == -1)) // 检查上方，如不在边上，且没蹩马腿
                    {
                        if ((mypx > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy - 2])))
                        {
                            GlobalValue.pathImage[mypx - 1, mypy - 2].hasPoint = true;
                        }
                        if ((mypx < 8) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy - 2])))
                        {
                            GlobalValue.pathImage[mypx + 1, mypy - 2].hasPoint = true;
                        }
                    }

                    if ((mypy < 8) && (GlobalValue.qipan[mypx, mypy + 1] == -1)) // 检查下方，如不在边上，且没蹩马腿
                    {
                        if ((mypx > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy + 2])))
                        {
                            GlobalValue.pathImage[mypx - 1, mypy + 2].hasPoint = true;
                        }
                        if ((mypx < 8) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy + 2])))
                        {
                            GlobalValue.pathImage[mypx + 1, mypy + 2].hasPoint = true;
                        }
                    }

                    if ((mypx > 1) && (GlobalValue.qipan[mypx - 1, mypy] == -1)) // 检查左方，如不在边上，且没蹩马腿
                    {
                        if ((mypy > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 2, mypy - 1])))
                        {
                            GlobalValue.pathImage[mypx - 2, mypy - 1].hasPoint = true;
                        }
                        if ((mypy < 9) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 2, mypy + 1])))
                        {
                            GlobalValue.pathImage[mypx - 2, mypy + 1].hasPoint = true;
                        }
                    }

                    if ((mypx < 7) && (GlobalValue.qipan[mypx + 1, mypy] == -1)) // 检查右方，如不在边上，且没蹩马腿
                    {
                        if ((mypy > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 2, mypy - 1])))
                        {
                            GlobalValue.pathImage[mypx + 2, mypy - 1].hasPoint = true;
                        }
                        if ((mypy < 9) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 2, mypy + 1])))
                        {
                            GlobalValue.pathImage[mypx + 2, mypy + 1].hasPoint = true;
                        }
                    }
                    break;

                case 3:
                case 4:
                case 19:
                case 20: // 相的移动 ================================================
                    if (mypy <= 4) // 如果是上方相，则上下边界设定为0--4，下方相的边界设定为5--9
                    {
                        side = 5;
                    }
                    if (JustOneIsThis(MoveQiZi))
                    {
                        break;
                    }
                    if (mypy != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (mypx > 0)
                        {
                            if ((GlobalValue.qipan[mypx - 1, mypy + 1] == -1) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 2, mypy + 2]))) // 左下方
                            {
                                GlobalValue.pathImage[mypx - 2, mypy + 2].hasPoint = true;
                            }
                        }
                        if (mypx < 8)
                        {
                            if ((GlobalValue.qipan[mypx + 1, mypy + 1] == -1) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 2, mypy + 2]))) // 右下方
                            {
                                GlobalValue.pathImage[mypx + 2, mypy + 2].hasPoint = true;
                            }
                        }
                    }
                    if (mypy != 5 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (mypx > 0)
                        {
                            if ((GlobalValue.qipan[mypx - 1, mypy - 1] == -1) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 2, mypy - 2]))) // 左上方
                            {
                                GlobalValue.pathImage[mypx - 2, mypy - 2].hasPoint = true;
                            }
                        }
                        if (mypx < 8)
                        {
                            if ((GlobalValue.qipan[mypx + 1, mypy - 1] == -1) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 2, mypy - 2]))) // 右上方
                            {
                                GlobalValue.pathImage[mypx + 2, mypy - 2].hasPoint = true;
                            }
                        }
                    }
                    break;
                case 1:
                case 2:
                case 17:
                case 18: // 士的移动 ================================================
                    side = 0;
                    if (mypy <= 4) // 如果是上方棋子，则上下边界设定为0--2，下方相的边界设定为7--9
                    {
                        side = 7;
                    }
                    if (JustOneIsThis(MoveQiZi))
                    {
                        break;
                    }
                    if (mypy != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (mypx > 3)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy + 1])) // 左下方
                            {
                                GlobalValue.pathImage[mypx - 1, mypy + 1].hasPoint = true;
                            }
                        }
                        if (mypx < 5)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy + 1])) // 右下方
                            {
                                GlobalValue.pathImage[mypx + 1, mypy + 1].hasPoint = true;
                            }
                        }
                    }
                    if (mypy != 7 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (mypx > 3)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy - 1]))// 左上方
                            {
                                GlobalValue.pathImage[mypx - 1, mypy - 1].hasPoint = true;
                            }
                        }
                        if (mypx < 5)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy - 1])) // 右上方
                            {
                                GlobalValue.pathImage[mypx + 1, mypy - 1].hasPoint = true;
                            }
                        }
                    }
                    break;
                case 0:
                case 16: // 将帅的移动 ================================================
                    side = 0;
                    if (mypy <= 4) // 如果是上方棋子，则上下边界设定为0--2，下方相的边界设定为7--9
                    {
                        side = 7;
                    }
                    if (mypy != (9 - side))  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, mypy + 1])) // 下方移一格
                        {
                            if (GlobalValue.myqz[0].Col != GlobalValue.myqz[16].Col)    // 如果将帅横向不同线
                            {
                                GlobalValue.pathImage[mypx, mypy + 1].hasPoint = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    for (int i = GlobalValue.myqz[0].Row + 2; i < GlobalValue.myqz[16].Row; i++)
                                    {
                                        if (GlobalValue.qipan[mypx, i] != -1)
                                        {
                                            GlobalValue.pathImage[mypx, mypy + 1].hasPoint = true;
                                            break;
                                        }
                                    }
                                }
                                if (MoveQiZi == 16)
                                {
                                    GlobalValue.pathImage[mypx, mypy + 1].hasPoint = true;
                                }
                            }
                        }
                    }
                    if (mypy != (7 - side))  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, mypy - 1]))// 上方移一格
                        {
                            if (GlobalValue.myqz[0].Col != GlobalValue.myqz[16].Col)    // 如果将帅横向不同线
                            {
                                GlobalValue.pathImage[mypx, mypy - 1].hasPoint = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    GlobalValue.pathImage[mypx, mypy - 1].hasPoint = true;
                                }
                                if (MoveQiZi == 16)
                                {
                                    for (int i = GlobalValue.myqz[0].Row + 1; i < GlobalValue.myqz[16].Row - 1; i++)
                                    {
                                        if (GlobalValue.qipan[mypx, i] != -1)
                                        {
                                            GlobalValue.pathImage[mypx, mypy - 1].hasPoint = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (mypx > 3)  // 如果不在左边界上，则探查左方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy])) // 左方移一格
                        {
                            if (((mypx - 1) == GlobalValue.myqz[0].Col) || ((mypx - 1) == GlobalValue.myqz[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.myqz[0].Row + 1; i < GlobalValue.myqz[16].Row; i++)
                                {
                                    if (GlobalValue.qipan[mypx - 1, i] != -1)
                                    {
                                        GlobalValue.pathImage[mypx - 1, mypy].hasPoint = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                GlobalValue.pathImage[mypx - 1, mypy].hasPoint = true;
                            }
                        }

                    }
                    if (mypx < 5)  // 如果不在右边界上，则探查右方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy])) // 右方移一格
                        {
                            if (((mypx + 1) == GlobalValue.myqz[0].Col) || ((mypx + 1) == GlobalValue.myqz[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.myqz[0].Row + 1; i < GlobalValue.myqz[16].Row; i++)
                                {
                                    if (GlobalValue.qipan[mypx + 1, i] != -1)
                                    {
                                        GlobalValue.pathImage[mypx + 1, mypy].hasPoint = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                GlobalValue.pathImage[mypx + 1, mypy].hasPoint = true;
                            }
                        }
                    }

                    int j;
                    if ((GlobalValue.myqz[0].Col == GlobalValue.myqz[16].Col) && (mypx == m))      //  将帅前进吃子后不能见面
                    {
                        j = 0;
                        for (int i = Math.Min(GlobalValue.myqz[0].Row, GlobalValue.myqz[16].Row) + 1; i < Math.Max(GlobalValue.myqz[0].Row, GlobalValue.myqz[16].Row) - 1; i++)
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
                        if ((MoveQiZi == 4) && (Math.Abs(GlobalValue.myqz[0].Row - n) == 1))
                        {
                            j = 0;
                            for (int i = Math.Min(n, GlobalValue.myqz[16].Row) + 1; i < Math.Max(n, GlobalValue.myqz[16].Row) - 1; i++)
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
                    int gezi = 0; // 隔子计数
                    if (!JustOneIsThis(MoveQiZi))
                    {
                        for (int i = mypx - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (GlobalValue.qipan[i, mypy] == -1)
                            {
                                if (gezi == 0)
                                    GlobalValue.pathImage[i, mypy].hasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.qipan[i, mypy]))
                                {
                                    if (gezi == 1) GlobalValue.pathImage[i, mypy].hasPoint = true;
                                }
                                gezi++;
                            }
                        }
                        gezi = 0; // 隔子计数
                        for (int i = mypx + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (GlobalValue.qipan[i, mypy] == -1)
                            {
                                if (gezi == 0) GlobalValue.pathImage[i, mypy].hasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.qipan[i, mypy]))
                                {
                                    if (gezi == 1) GlobalValue.pathImage[i, mypy].hasPoint = true;
                                }
                                gezi++;
                            }
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = mypy - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (GlobalValue.qipan[mypx, i] == -1)
                        {
                            if (gezi == 0) GlobalValue.pathImage[mypx, i].hasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, i]))
                            {
                                if (gezi == 1) GlobalValue.pathImage[mypx, i].hasPoint = true;
                            }
                            gezi++;
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = mypy + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (GlobalValue.qipan[mypx, i] == -1)
                        {
                            if (gezi == 0) GlobalValue.pathImage[mypx, i].hasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, i]))
                            {
                                if (gezi == 1) GlobalValue.pathImage[mypx, i].hasPoint = true;
                            }
                            gezi++;
                        }
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
                        if ((mypy < 9) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, mypy + 1]))) // 下方一格
                        {
                            GlobalValue.pathImage[mypx, mypy + 1].hasPoint = true;
                        }
                        if (!JustOneIsThis(MoveQiZi) && (mypy <= 9) && (mypy > 4)) // 水平一格
                        {
                            if ((mypx > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy])))
                                GlobalValue.pathImage[mypx - 1, mypy].hasPoint = true;
                            if ((mypx < 8) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy])))
                                GlobalValue.pathImage[mypx + 1, mypy].hasPoint = true;
                        }
                    }
                    if (GlobalValue.myqz[MoveQiZi].Pcolor != GlobalValue.qipanfanzhuan)  // 上方兵卒的移动
                    {
                        if ((mypy > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx, mypy - 1]))) // 上方一格
                        {
                            GlobalValue.pathImage[mypx, mypy - 1].hasPoint = true;
                        }
                        if (!JustOneIsThis(MoveQiZi) && (mypy >= 0) && (mypy < 5)) // 水平一格
                        {
                            if ((mypx > 0) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx - 1, mypy])))
                                GlobalValue.pathImage[mypx - 1, mypy].hasPoint = true;
                            if ((mypx < 8) && (!IsTongBang(MoveQiZi, GlobalValue.qipan[mypx + 1, mypy])))
                                GlobalValue.pathImage[mypx + 1, mypy].hasPoint = true;
                        }
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

        /// <summary>
        /// 将帅在同一列时，检查本棋子是否是将帅之间的唯一棋子。
        /// </summary>
        /// <param name="qz"></param>
        /// <returns>将帅同列时，如果本棋子是他们之间的唯一棋子，则返回ture。</returns>
        private static bool JustOneIsThis(int qz)
        {
            if (GlobalValue.myqz[0].Col != GlobalValue.myqz[16].Col)    // 如果将帅不在同一列
            {
                return false;
            }
            if (GlobalValue.myqz[qz].Col != GlobalValue.myqz[0].Col)    // 如果棋子不与将帅同列
            {
                return false;
            }
            int count = 0;
            for (int i = GlobalValue.myqz[0].Row + 1; i < GlobalValue.myqz[16].Row; i++)
            {
                if (GlobalValue.qipan[GlobalValue.myqz[0].Col, i] != -1)
                {
                    count++;
                }
            }
            if (count == 1)
            {
                return true;
            }
            return false;

        }

    }
}
