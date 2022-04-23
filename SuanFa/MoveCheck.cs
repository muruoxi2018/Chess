using System;

namespace Chess.SuanFa
{
    public class MoveCheck
    {
        public static bool[,] PathBool = new bool[9, 10];
        public static void GetAndShowPathPoints(int thisqz)   // 查找棋子可移动的路径，并标记

        {
            //GlobalValue.qzpath.Initialize();//  清除所有棋子路径数据
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    GlobalValue.PathPointImage[i, j].HasPoint = false;
                    PathBool[i, j] = false;
                }
            }
            if (thisqz > -1)
            {
                PathBool = GetPathPoints(thisqz, GlobalValue.QiPan);
                if (thisqz is 0 or 16)
                {
                    for (int i = 0; i <= 8; i++)
                    {
                        for (int j = 0; j <= 9; j++)
                        {
                            if (PathBool[i, j] == true && IsKilledPoint(thisqz, i, j, GlobalValue.QiPan) == true)
                            {
                                PathBool[i, j] = false;
                            }
                        }
                    }
                }
                else
                {
                    if (!CanMoveOutCol(thisqz, GlobalValue.QiPan))
                    {
                        int col = GlobalValue.QiZiArray[thisqz].Col;
                        for (int i = 0; i <= 8; i++)
                        {
                            for (int j = 0; j <= 9; j++)
                            {
                                if (i != col)
                                {
                                    PathBool[i, j] = false;
                                }
                            }
                        }
                    }
                    if (!CanMoveOutRow(thisqz, GlobalValue.QiPan))
                    {
                        int row = GlobalValue.QiZiArray[thisqz].Row;
                        for (int i = 0; i <= 8; i++)
                        {
                            for (int j = 0; j <= 9; j++)
                            {
                                if (j != row)
                                {
                                    PathBool[i, j] = false;
                                }
                            }
                        }
                    }
                }


                for (int i = 0; i <= 8; i++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        GlobalValue.PathPointImage[i, j].HasPoint = PathBool[i, j];
                    }
                }
                GlobalValue.PathPointImage[GlobalValue.QiZiArray[thisqz].Col, GlobalValue.QiZiArray[thisqz].Row].HasPoint = false;
            }
        }

        // 检查棋子移动目标位置的有效性
        public static bool[,] GetPathPoints(int MoveQiZi, int[,] qipan)
        {
            bool[,] points = new bool[9, 10];
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    points[i, j] = false;
                }
            }
            if (MoveQiZi > 31)  // 如果没有预选棋子
            {
                return points;
            }
            if (MoveQiZi < 0)
            {
                return points;
            }
            if (GlobalValue.QiZiArray[MoveQiZi].Visibility==System.Windows.Visibility.Hidden) return points;
            int MoveQiZi_Col = GlobalValue.QiZiArray[MoveQiZi].Col;
            int MoveQiZi_Row = GlobalValue.QiZiArray[MoveQiZi].Row;
            int side = 0;

            switch (MoveQiZi)
            {
                case 7:
                case 8:
                case 23:
                case 24:    // 车的移动  ================================================
                    if (!JustOneIsThis(MoveQiZi, qipan))
                    {
                        for (int i = MoveQiZi_Col - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (qipan[i, MoveQiZi_Row] == -1)
                            {
                                points[i, MoveQiZi_Row] = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, qipan[i, MoveQiZi_Row]))
                                {
                                    points[i, MoveQiZi_Row] = true;
                                }
                                break;
                            }
                        }
                        for (int i = MoveQiZi_Col + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (qipan[i, MoveQiZi_Row] == -1)
                            {
                                points[i, MoveQiZi_Row] = true;
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, qipan[i, MoveQiZi_Row]))
                                {
                                    points[i, MoveQiZi_Row] = true;
                                }
                                break;
                            }
                        }
                    }
                    for (int i = MoveQiZi_Row - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (qipan[MoveQiZi_Col, i] == -1)
                        {
                            points[MoveQiZi_Col, i] = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, i]))
                            {
                                points[MoveQiZi_Col, i] = true;
                            }
                            break;
                        }
                    }
                    for (int i = MoveQiZi_Row + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (qipan[MoveQiZi_Col, i] == -1)
                        {
                            points[MoveQiZi_Col, i] = true;
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, i]))
                            {
                                points[MoveQiZi_Col, i] = true;
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
                    if (JustOneIsThis(MoveQiZi, qipan))
                    {
                        break;
                    }
                    // 八个方向，逐个检查
                    if (MoveQiZi_Row > 1 && qipan[MoveQiZi_Col, MoveQiZi_Row - 1] == -1) // 检查上方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row - 2]))
                        {
                            points[MoveQiZi_Col - 1, MoveQiZi_Row - 2] = true;
                        }
                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row - 2]))
                        {
                            points[MoveQiZi_Col + 1, MoveQiZi_Row - 2] = true;
                        }
                    }

                    if (MoveQiZi_Row < 8 && qipan[MoveQiZi_Col, MoveQiZi_Row + 1] == -1) // 检查下方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row + 2]))
                        {
                            points[MoveQiZi_Col - 1, MoveQiZi_Row + 2] = true;
                        }
                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row + 2]))
                        {
                            points[MoveQiZi_Col + 1, MoveQiZi_Row + 2] = true;
                        }
                    }

                    if (MoveQiZi_Col > 1 && qipan[MoveQiZi_Col - 1, MoveQiZi_Row] == -1) // 检查左方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 2, MoveQiZi_Row - 1]))
                        {
                            points[MoveQiZi_Col - 2, MoveQiZi_Row - 1] = true;
                        }
                        if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 2, MoveQiZi_Row + 1]))
                        {
                            points[MoveQiZi_Col - 2, MoveQiZi_Row + 1] = true;
                        }
                    }

                    if (MoveQiZi_Col < 7 && qipan[MoveQiZi_Col + 1, MoveQiZi_Row] == -1) // 检查右方，如不在边上，且没蹩马腿
                    {
                        if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 2, MoveQiZi_Row - 1]))
                        {
                            points[MoveQiZi_Col + 2, MoveQiZi_Row - 1] = true;
                        }
                        if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 2, MoveQiZi_Row + 1]))
                        {
                            points[MoveQiZi_Col + 2, MoveQiZi_Row + 1] = true;
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
                    if (JustOneIsThis(MoveQiZi, qipan))
                    {
                        break;
                    }
                    if (MoveQiZi_Row != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (MoveQiZi_Col > 0)
                        {
                            if (qipan[MoveQiZi_Col - 1, MoveQiZi_Row + 1] == -1 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 2, MoveQiZi_Row + 2])) // 左下方
                            {
                                points[MoveQiZi_Col - 2, MoveQiZi_Row + 2] = true;
                            }
                        }
                        if (MoveQiZi_Col < 8)
                        {
                            if (qipan[MoveQiZi_Col + 1, MoveQiZi_Row + 1] == -1 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 2, MoveQiZi_Row + 2])) // 右下方
                            {
                                points[MoveQiZi_Col + 2, MoveQiZi_Row + 2] = true;
                            }
                        }
                    }
                    if (MoveQiZi_Row != 5 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (MoveQiZi_Col > 0)
                        {
                            if (qipan[MoveQiZi_Col - 1, MoveQiZi_Row - 1] == -1 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 2, MoveQiZi_Row - 2])) // 左上方
                            {
                                points[MoveQiZi_Col - 2, MoveQiZi_Row - 2] = true;
                            }
                        }
                        if (MoveQiZi_Col < 8)
                        {
                            if (qipan[MoveQiZi_Col + 1, MoveQiZi_Row - 1] == -1 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 2, MoveQiZi_Row - 2])) // 右上方
                            {
                                points[MoveQiZi_Col + 2, MoveQiZi_Row - 2] = true;
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
                    if (JustOneIsThis(MoveQiZi, qipan))
                    {
                        break;
                    }
                    if (MoveQiZi_Row != 9 - side)  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (MoveQiZi_Col > 3)
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row + 1])) // 左下方
                            {
                                points[MoveQiZi_Col - 1, MoveQiZi_Row + 1] = true;
                            }
                        }
                        if (MoveQiZi_Col < 5)
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row + 1])) // 右下方
                            {
                                points[MoveQiZi_Col + 1, MoveQiZi_Row + 1] = true;
                            }
                        }
                    }
                    if (MoveQiZi_Row != 7 - side)  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (MoveQiZi_Col > 3)
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row - 1]))// 左上方
                            {
                                points[MoveQiZi_Col - 1, MoveQiZi_Row - 1] = true;
                            }
                        }
                        if (MoveQiZi_Col < 5)
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row - 1])) // 右上方
                            {
                                points[MoveQiZi_Col + 1, MoveQiZi_Row - 1] = true;
                            }
                        }
                    }
                    break;
                case 0:
                case 16: // 将帅的移动 ================================================
                    side = 0;
                    if (MoveQiZi_Row <= 4) // 黑将的上下移动边界为0--2，红帅的上下移动边界为7--9
                    {
                        side = 7;
                    }
                    if (MoveQiZi_Row != (9 - side))  // 如果不在下边界上，则探查下方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, MoveQiZi_Row + 1])) // 下方移一格
                        {
                            if (GlobalValue.QiZiArray[0].Col != GlobalValue.QiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                points[MoveQiZi_Col, MoveQiZi_Row + 1] = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    for (int i = GlobalValue.QiZiArray[0].Row + 2; i < GlobalValue.QiZiArray[16].Row; i++)
                                    {
                                        if (qipan[MoveQiZi_Col, i] != -1)
                                        {
                                            points[MoveQiZi_Col, MoveQiZi_Row + 1] = true;
                                            break;
                                        }
                                    }
                                }
                                if (MoveQiZi == 16)
                                {
                                    points[MoveQiZi_Col, MoveQiZi_Row + 1] = true;
                                }
                            }
                        }
                    }
                    if (MoveQiZi_Row != (7 - side))  // 如果不在上边界上，则探查上方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, MoveQiZi_Row - 1]))// 上方移一格
                        {
                            if (GlobalValue.QiZiArray[0].Col != GlobalValue.QiZiArray[16].Col)    // 如果将帅横向不同线
                            {
                                points[MoveQiZi_Col, MoveQiZi_Row - 1] = true;
                            }
                            else
                            {
                                if (MoveQiZi == 0)
                                {
                                    points[MoveQiZi_Col, MoveQiZi_Row - 1] = true;
                                }
                                if (MoveQiZi == 16)
                                {
                                    for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row - 1; i++)
                                    {
                                        if (qipan[MoveQiZi_Col, i] != -1)
                                        {
                                            points[MoveQiZi_Col, MoveQiZi_Row - 1] = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (MoveQiZi_Col > 3)  // 如果不在左边界上，则探查左方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row])) // 左方移一格
                        {
                            if (((MoveQiZi_Col - 1) == GlobalValue.QiZiArray[0].Col) || ((MoveQiZi_Col - 1) == GlobalValue.QiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row; i++)
                                {
                                    if (qipan[MoveQiZi_Col - 1, i] != -1)
                                    {
                                        points[MoveQiZi_Col - 1, MoveQiZi_Row] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                points[MoveQiZi_Col - 1, MoveQiZi_Row] = true;
                            }
                        }

                    }
                    if (MoveQiZi_Col < 5)  // 如果不在右边界上，则探查右方的可移动路径
                    {
                        if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row])) // 右方移一格
                        {
                            if (((MoveQiZi_Col + 1) == GlobalValue.QiZiArray[0].Col) || ((MoveQiZi_Col + 1) == GlobalValue.QiZiArray[16].Col))    // 如果将帅横向移动一格
                            {
                                for (int i = GlobalValue.QiZiArray[0].Row + 1; i < GlobalValue.QiZiArray[16].Row; i++)
                                {
                                    if (qipan[MoveQiZi_Col + 1, i] != -1)
                                    {
                                        points[MoveQiZi_Col + 1, MoveQiZi_Row] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                points[MoveQiZi_Col + 1, MoveQiZi_Row] = true;
                            }
                        }
                    }


                    break;
                case 9:
                case 10:
                case 25:
                case 26: // 炮的移动 ================================================
                    int gezi = 0; // 隔子计数
                    if (!JustOneIsThis(MoveQiZi, qipan))
                    {
                        for (int i = MoveQiZi_Col - 1; i >= 0; i--) // 同一行向左找
                        {
                            if (qipan[i, MoveQiZi_Row] == -1)
                            {
                                if (gezi == 0)
                                {
                                    points[i, MoveQiZi_Row] = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, qipan[i, MoveQiZi_Row]))
                                {
                                    if (gezi == 1)
                                    {
                                        points[i, MoveQiZi_Row] = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                        gezi = 0; // 隔子计数
                        for (int i = MoveQiZi_Col + 1; i <= 8; i++) // 同一行向右找
                        {
                            if (qipan[i, MoveQiZi_Row] == -1)
                            {
                                if (gezi == 0)
                                {
                                    points[i, MoveQiZi_Row] = true;
                                }
                            }
                            else
                            {
                                if (!IsTongBang(MoveQiZi, qipan[i, MoveQiZi_Row]))
                                {
                                    if (gezi == 1)
                                    {
                                        points[i, MoveQiZi_Row] = true;
                                    }
                                }
                                gezi++;
                            }
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = MoveQiZi_Row - 1; i >= 0; i--) // 同一列向上找
                    {
                        if (qipan[MoveQiZi_Col, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                points[MoveQiZi_Col, i] = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, i]))
                            {
                                if (gezi == 1)
                                {
                                    points[MoveQiZi_Col, i] = true;
                                }
                            }
                            gezi++;
                        }
                    }
                    gezi = 0; // 隔子计数
                    for (int i = MoveQiZi_Row + 1; i <= 9; i++) // 同一列向下找
                    {
                        if (qipan[MoveQiZi_Col, i] == -1)
                        {
                            if (gezi == 0)
                            {
                                points[MoveQiZi_Col, i] = true;
                            }
                        }
                        else
                        {
                            if (!IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, i]))
                            {
                                if (gezi == 1)
                                {
                                    points[MoveQiZi_Col, i] = true;
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
                    if (MoveQiZi_Row < 9 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, MoveQiZi_Row + 1])) // 下方一格
                    {
                        points[MoveQiZi_Col, MoveQiZi_Row + 1] = true;
                    }
                    if (!JustOneIsThis(MoveQiZi, qipan) && MoveQiZi_Row <= 9 && MoveQiZi_Row > 4) // 水平一格
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row]))
                        {
                            points[MoveQiZi_Col - 1, MoveQiZi_Row] = true;
                        }

                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row]))
                        {
                            points[MoveQiZi_Col + 1, MoveQiZi_Row] = true;
                        }
                    }
                    break;
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:// 红方兵的移动
                    if (MoveQiZi_Row > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col, MoveQiZi_Row - 1])) // 上方一格
                    {
                        points[MoveQiZi_Col, MoveQiZi_Row - 1] = true;
                    }
                    if (!JustOneIsThis(MoveQiZi, qipan) && MoveQiZi_Row >= 0 && MoveQiZi_Row < 5) // 水平一格
                    {
                        if (MoveQiZi_Col > 0 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col - 1, MoveQiZi_Row]))
                        {
                            points[MoveQiZi_Col - 1, MoveQiZi_Row] = true;
                        }

                        if (MoveQiZi_Col < 8 && !IsTongBang(MoveQiZi, qipan[MoveQiZi_Col + 1, MoveQiZi_Row]))
                        {
                            points[MoveQiZi_Col + 1, MoveQiZi_Row] = true;
                        }
                    }
                    break;
                default:
                    return points;
            }
            return points;
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
        private static bool JustOneIsThis(int qz, int[,] qipan)
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
                if (qipan[GlobalValue.QiZiArray[0].Col, i] != -1)
                {
                    count++;
                }
            }
            return count == 1;
        }
        /// <summary>
        /// 在将帅的可移动路径中，排除对方车、马、炮、卒的可攻击点。
        /// </summary>
        /// <param name="QZJiangShuai">0=将，16=帅</param>
        /// <param name="col">可移动点的列位置</param>
        /// <param name="row">可移动点的行位置</param>
        /// <param name="qipan">当前棋盘数据</param>
        /// <returns></returns>
        private static bool IsKilledPoint(int QZJiangShuai, int col, int row, int[,] qipan)
        {
            // 注意：数组作为参数传递时，不是传递的副本，而是直接数组本身。
            int[,] myqipan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myqipan[i, j] = qipan[i, j];
                }
            myqipan[col, row] = QZJiangShuai;
            myqipan[GlobalValue.QiZiArray[QZJiangShuai].Col, GlobalValue.QiZiArray[QZJiangShuai].Row] = -1;

            bool[,] thispoints;
            if (QZJiangShuai == 16)
            {
                for (int qizi = 5; qizi <= 15; qizi++) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    thispoints = GetPathPoints(qizi, myqipan);
                    if (thispoints[col, row]) return true;
                }
            }
            if (QZJiangShuai == 0)
            {
                for (int qizi = 21; qizi <= 31; qizi++) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                {
                    thispoints = GetPathPoints(qizi, myqipan);
                    if (thispoints[col, row]) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断棋子是否可离开本列，如离开后本方将帅被将军，则不能离开。
        /// </summary>
        /// <param name="thisQZ"></param>
        /// <param name="qipan"></param>
        /// <returns></returns>
        private static bool CanMoveOutCol(int thisQZ, int[,] qipan)
        {
            if (thisQZ is 0 or 16) return true;
            if (thisQZ <= 15)
                if (GlobalValue.QiZiArray[thisQZ].Row != GlobalValue.QiZiArray[0].Row)
                {
                    return true; // 黑方棋子与黑将不在同一列时，无需处理。
                }
                else
                {
                    // 黑方棋子与黑将在同一列时，则要进一步判断
                }
            if (thisQZ > 15)
                if (GlobalValue.QiZiArray[thisQZ].Row != GlobalValue.QiZiArray[0].Row)
                {
                    return true; // 红方棋子与红帅不在同一列时，无需处理。
                }
                else
                {
                    // 红方棋子与红帅在同一列时，则要进一步判断
                }
            return true;

        }
        /// <summary>
        /// 判断棋子是否可离开本行，如离开后本方将帅被将军，则不能离开。
        /// </summary>
        /// <param name="thisQZ"></param>
        /// <param name="qipan"></param>
        /// <returns></returns>
        private static bool CanMoveOutRow(int thisQZ, int[,] qipan)
        {
            if (thisQZ is 0 or 16) return true;
            if (thisQZ <= 15)
                if (GlobalValue.QiZiArray[thisQZ].Row != GlobalValue.QiZiArray[0].Row)
                {
                    return true; // 黑方棋子与黑将不在同一行时
                }
                else
                {
                    // 黑方棋子与黑将在同一行时，则要进一步判断
                }
            if (thisQZ > 15)
                if (GlobalValue.QiZiArray[thisQZ].Row != GlobalValue.QiZiArray[0].Row)
                {
                    return true; // 红方棋子与红帅不在同一行时
                }
                else
                {
                    // 红方棋子与红帅在同一行时，则要进一步判断
                }
            return true;
        }

        /// <summary>
        /// 
        /// 检查棋子移动后，本方是否被将军。
        /// 用于棋子移动前的检测，如果是移动后被将军，则不能允许移动。
        /// 
        /// </summary>
        /// <param name="thisQz"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="qipan"></param>
        /// <returns> false=未将军，true=被将军 </returns>
        public static bool AfterMoveWillJiangJun(int thisQz, int x0, int y0, int x1, int y1, int[,] qipan)
        {
            // 注意：数组作为参数传递时，不是传递的副本，而是直接数组本身。
            int[,] myqipan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myqipan[i, j] = qipan[i, j];
                }
            myqipan[x1, y1] = thisQz;
            myqipan[x0, y0] = -1;

            bool[,] thispoints;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    int qizi = myqipan[i, j]; // 从棋盘副本上找棋子
                    if (thisQz > 15)
                    {
                        if (qizi is >=5 and <=15) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                            {
                                thispoints = GetPathPoints(qizi, myqipan);
                                int x = (thisQz == 16) ? x1 : GlobalValue.QiZiArray[16].Col;
                                int y = (thisQz == 16) ? y1 : GlobalValue.QiZiArray[16].Row;
                                if (thispoints[x, y] == true) return true;
                            }
                    }
                    if (thisQz < 15)
                    {
                        if (qizi is >=21 and <=31) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                         {
                            thispoints = GetPathPoints(qizi, myqipan);
                            int x = (thisQz == 0) ? x1 : GlobalValue.QiZiArray[0].Col;
                            int y = (thisQz == 0) ? y1 : GlobalValue.QiZiArray[0].Row;
                            if (thispoints[x, y] == true) return true;
                        }
                    }
                }
            return false;
        }
    }
}
