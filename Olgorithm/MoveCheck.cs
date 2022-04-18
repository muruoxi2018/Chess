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
                    GlobalValue.PathPointImage[i, j].HasPoint = false;
                    GlobalValue.PathPointImage[i, j].SetHidden();

                }
            }
            if (thisqz > -1)
            {
                _ = QzMoveCheck(thisqz);
                GlobalValue.PathPointImage[GlobalValue.QiZiArray[thisqz].Col, GlobalValue.QiZiArray[thisqz].Row].HasPoint = false;
            }
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
            int MoveQiZi_Col = GlobalValue.QiZiArray[MoveQiZi].Col;
            int MoveQiZi_Row = GlobalValue.QiZiArray[MoveQiZi].Row;
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
                        for (int i = MoveQiZi_Col - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (GlobalValue.QiPan[i, MoveQiZi_Row] == -1)
                            {
                                GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[i, MoveQiZi_Row]))
                                {
                                    GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                }
                                break;
                            }
                        }
                        for (int i = MoveQiZi_Col + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (GlobalValue.QiPan[i, MoveQiZi_Row] == -1)
                            {
                                GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[i, MoveQiZi_Row]))
                                {
                                    GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                }
                                break;
                            }
                        }
                    }
                    for (int i = MoveQiZi_Row - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (GlobalValue.QiPan[MoveQiZi_Col, i] == -1)
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, i]))
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                            }
                            break;
                        }
                    }
                    for (int i = MoveQiZi_Row + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (GlobalValue.QiPan[MoveQiZi_Col, i] == -1)
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, i]))
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
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
                    if (MoveQiZi_Row > 1 && GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row - 1] == -1) // 检查上方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row - 2]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row - 2].HasPoint = true;
                        }
                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row - 2]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row - 2].HasPoint = true;
                        }
                    }

                    if (MoveQiZi_Row < 8 && GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row + 1] == -1) // 检查下方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row + 2]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row + 2].HasPoint = true;
                        }
                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row + 2]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row + 2].HasPoint = true;
                        }
                    }

                    if (MoveQiZi_Col > 1 && GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row] == -1) // 检查左方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 2, MoveQiZi_Row - 1]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 2, MoveQiZi_Row - 1].HasPoint = true;
                        }
                        if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 2, MoveQiZi_Row + 1]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 2, MoveQiZi_Row + 1].HasPoint = true;
                        }
                    }

                    if (MoveQiZi_Col < 7 && GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row] == -1) // 检查右方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 2, MoveQiZi_Row - 1]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 2, MoveQiZi_Row - 1].HasPoint = true;
                        }
                        if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 2, MoveQiZi_Row + 1]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 2, MoveQiZi_Row + 1].HasPoint = true;
                        }
                    }
                    break;

                case 3:
                case 4:
                case 19:
                case 20: // 相的移动 ================================================
                    if (MoveQiZi_Row <= 4) // 如果是上方相，则上下边界设定为0--4，下方相的边界设定为5--9
                    {
                        side = 5;
                    }
                    if (JustOneIsThis(MoveQiZi))
                    {
                        break;
                    }
                    if (MoveQiZi_Row != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (MoveQiZi_Col > 0)
                        {
                            if (GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row + 1] == -1 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 2, MoveQiZi_Row + 2])) // 左下方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col - 2, MoveQiZi_Row + 2].HasPoint = true;
                            }
                        }
                        if (MoveQiZi_Col < 8)
                        {
                            if (GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row + 1] == -1 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 2, MoveQiZi_Row + 2])) // 右下方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col + 2, MoveQiZi_Row + 2].HasPoint = true;
                            }
                        }
                    }
                    if (MoveQiZi_Row != 5 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (MoveQiZi_Col > 0)
                        {
                            if (GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row - 1] == -1 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 2, MoveQiZi_Row - 2])) // 左上方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col - 2, MoveQiZi_Row - 2].HasPoint = true;
                            }
                        }
                        if (MoveQiZi_Col < 8)
                        {
                            if (GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row - 1] == -1 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 2, MoveQiZi_Row - 2])) // 右上方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col + 2, MoveQiZi_Row - 2].HasPoint = true;
                            }
                        }
                    }
                    break;
                case 1:
                case 2:
                case 17:
                case 18: // 士的移动 ================================================
                    side = 0;
                    if (MoveQiZi_Row <= 4) // 如果是上方棋子，则上下边界设定为0--2，下方相的边界设定为7--9
                    {
                        side = 7;
                    }
                    if (JustOneIsThis(MoveQiZi))
                    {
                        break;
                    }
                    if (MoveQiZi_Row != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (MoveQiZi_Col > 3)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row + 1])) // 左下方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row + 1].HasPoint = true;
                            }
                        }
                        if (MoveQiZi_Col < 5)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row + 1])) // 右下方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row + 1].HasPoint = true;
                            }
                        }
                    }
                    if (MoveQiZi_Row != 7 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (MoveQiZi_Col > 3)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row - 1]))// 左上方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row - 1].HasPoint = true;
                            }
                        }
                        if (MoveQiZi_Col < 5)
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row - 1])) // 右上方
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row - 1].HasPoint = true;
                            }
                        }
                    }
                    break;
                case 0:
                case 16: // 将帅的移动 ================================================
                    side = 0;
                    if (MoveQiZi_Row <= 4) // 如果是上方棋子，则上下边界设定为0--2，下方相的边界设定为7--9
                    {
                        side = 7;
                    }
                    if (MoveQiZi_Row != (9 - side))  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row + 1])) // 下方移一格
                        {
                            if (GlobalValue.QiZiArray[0].Col != GlobalValue.QiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row + 1].HasPoint = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    for (int i = GlobalValue.QiZiArray[0].Row + 2; i < GlobalValue.QiZiArray[16].Row; i++)
                                    {
                                        if (GlobalValue.QiPan[MoveQiZi_Col, i] != -1)
                                        {
                                            GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row + 1].HasPoint = true;
                                            break;
                                        }
                                    }
                                }
                                if (MoveQiZi == 16)
                                {
                                    GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row + 1].HasPoint = true;
                                }
                            }
                        }
                    }
                    if (MoveQiZi_Row != (7 - side))  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row - 1]))// 上方移一格
                        {
                            if (GlobalValue.QiZiArray[0].Col != GlobalValue.QiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row - 1].HasPoint = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row - 1].HasPoint = true;
                                }
                                if (MoveQiZi == 16)
                                {
                                    for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row - 1; i++)
                                    {
                                        if (GlobalValue.QiPan[MoveQiZi_Col, i] != -1)
                                        {
                                            GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row - 1].HasPoint = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (MoveQiZi_Col > 3)  // 如果不在左边界上，则探查左方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row])) // 左方移一格
                        {
                            if (((MoveQiZi_Col - 1) == GlobalValue.QiZiArray[0].Col) || ((MoveQiZi_Col - 1) == GlobalValue.QiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row; i++)
                                {
                                    if (GlobalValue.QiPan[MoveQiZi_Col - 1, i] != -1)
                                    {
                                        GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row].HasPoint = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row].HasPoint = true;
                            }
                        }

                    }
                    if (MoveQiZi_Col < 5)  // 如果不在右边界上，则探查右方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row])) // 右方移一格
                        {
                            if (((MoveQiZi_Col + 1) == GlobalValue.QiZiArray[0].Col) || ((MoveQiZi_Col + 1) == GlobalValue.QiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row; i++)
                                {
                                    if (GlobalValue.QiPan[MoveQiZi_Col + 1, i] != -1)
                                    {
                                        GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row].HasPoint = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row].HasPoint = true;
                            }
                        }
                    }

                    int j;
                    if (GlobalValue.QiZiArray[0].Col == GlobalValue.QiZiArray[16].Col && MoveQiZi_Col == m)      //  将帅前进吃子后不能见面
                    {
                        j = 0;
                        for (int i = Math.Min(GlobalValue.QiZiArray[0].Row, GlobalValue.QiZiArray[16].Row) + 1; i < Math.Max(GlobalValue.QiZiArray[0].Row, GlobalValue.QiZiArray[16].Row) - 1; i++)
                        {
                            if (GlobalValue.QiPan[MoveQiZi_Col, i] != -1)
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            return false;
                        }
                        if (MoveQiZi == 4 && Math.Abs(GlobalValue.QiZiArray[0].Row - n) == 1)
                        {
                            j = 0;
                            for (int i = Math.Min(n, GlobalValue.QiZiArray[16].Row) + 1; i < Math.Max(n, GlobalValue.QiZiArray[16].Row) - 1; i++)
                            {
                                if (GlobalValue.QiPan[MoveQiZi_Col, i] != -1)
                                {
                                    j++;
                                }
                            }
                            if (j == 0)
                            {
                                return false;
                            }
                        }
                        if (MoveQiZi == 20 && Math.Abs(GlobalValue.QiZiArray[20].Row - n) == 1)
                        {
                            j = 0;
                            for (int i = Math.Min(n, GlobalValue.QiZiArray[4].Row) + 1; i < Math.Max(n, GlobalValue.QiZiArray[4].Row) - 1; i++)
                            {
                                if (GlobalValue.QiPan[MoveQiZi_Col, i] != -1)
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
                        for (int i = MoveQiZi_Col - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (GlobalValue.QiPan[i, MoveQiZi_Row] == -1)
                            {
                                if (gezi == 0)
                                {
                                    GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[i, MoveQiZi_Row]))
                                {
                                    if (gezi == 1)
                                    {
                                        GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                        gezi = 0; // 隔子计数
                        for (int i = MoveQiZi_Col + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (GlobalValue.QiPan[i, MoveQiZi_Row] == -1)
                            {
                                if (gezi == 0)
                                {
                                    GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[i, MoveQiZi_Row]))
                                {
                                    if (gezi == 1)
                                    {
                                        GlobalValue.PathPointImage[i, MoveQiZi_Row].HasPoint = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = MoveQiZi_Row - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (GlobalValue.QiPan[MoveQiZi_Col, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, i]))
                            {
                                if (gezi == 1)
                                {
                                    GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                                }
                            }
                            gezi++;
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = MoveQiZi_Row + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (GlobalValue.QiPan[MoveQiZi_Col, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, i]))
                            {
                                if (gezi == 1)
                                {
                                    GlobalValue.PathPointImage[MoveQiZi_Col, i].HasPoint = true;
                                }
                            }
                            gezi++;
                        }
                    }
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15: // 黑方卒的移动
                    if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row + 1])) // 下方一格
                    {
                        GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row + 1].HasPoint = true;
                    }
                    if (!JustOneIsThis(MoveQiZi) && MoveQiZi_Row <= 9 && MoveQiZi_Row > 4) // 水平一格
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row].HasPoint = true;
                        }

                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row].HasPoint = true;
                        }
                    }
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:// 红方兵的移动
                    if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col, MoveQiZi_Row - 1])) // 上方一格
                    {
                        GlobalValue.PathPointImage[MoveQiZi_Col, MoveQiZi_Row - 1].HasPoint = true;
                    }
                    if (!JustOneIsThis(MoveQiZi) && MoveQiZi_Row >= 0 && MoveQiZi_Row < 5) // 水平一格
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col - 1, MoveQiZi_Row]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col - 1, MoveQiZi_Row].HasPoint = true;
                        }

                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, GlobalValue.QiPan[MoveQiZi_Col + 1, MoveQiZi_Row]))
                        {
                            GlobalValue.PathPointImage[MoveQiZi_Col + 1, MoveQiZi_Row].HasPoint = true;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        // 判断是不是同一方棋子
        public static bool IsTongBang(int qz1, int qz2)
        {
            return qz1 >= 0 && qz2 >= 0 && ((qz1 <= 15 && qz2 <= 15) || (qz1 >= 16 && qz2 >= 16));
        }

        /// <summary>
        /// 将帅在同一列时，检查本棋子是否是将帅之间的唯一棋子。
        /// </summary>
        /// <param name="qz"></param>
        /// <returns>将帅同列时，如果本棋子是他们之间的唯一棋子，则返回ture。</returns>
        private static bool JustOneIsThis(int qz)
        {
            if (GlobalValue.QiZiArray[0].Col != GlobalValue.QiZiArray[16].Col)    // 如果将帅不在同一列
            {
                return false;
            }
            if (GlobalValue.QiZiArray[qz].Col != GlobalValue.QiZiArray[0].Col)    // 如果棋子不与将帅同列
            {
                return false;
            }
            int count = 0;
            for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row; i++)
            {
                if (GlobalValue.QiPan[GlobalValue.QiZiArray[0].Col, i] != -1)
                {
                    count++;
                }
            }
            return count == 1;
        }
    }
}
