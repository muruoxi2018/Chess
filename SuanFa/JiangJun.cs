using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.SuanFa
{
    class JiangJun
    {

        /// <summary>
        /// 检查本棋子是否对将帅构成将军
        /// </summary>
        /// <param name="qizi">棋子编号(0-31)</param>
        /// <returns> 0=黑将被将军，16=红帅被将军 </returns>
        public static int IsJiangJun(int thisQZ,int col,int row)
        {
            
            if (thisQZ is < 0 or > 31) return -1;
            int[,] myqipan = new int[9, 10]; // 制作棋盘副本，防止破坏原棋盘数据数组。
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 10; j++)
                {
                    myqipan[i, j] = GlobalValue.QiPan[i, j];
                }
            myqipan[col, row] = thisQZ;
            myqipan[GlobalValue.QiZiArray[thisQZ].Col, GlobalValue.QiZiArray[thisQZ].Row] = -1;

            bool[,] thispoints;

            if (thisQZ <= 15)
            {
                for (int qizi = 5; qizi <= 15; qizi++) //车(7,8)，马(5,6)，炮(9,10)，卒(11,12,13,14,15)
                {
                    thispoints =MoveCheck.GetPathPoints(qizi, myqipan);
                    int x = (thisQZ == 16) ? col : GlobalValue.QiZiArray[16].Col;
                    int y = (thisQZ == 16) ? row : GlobalValue.QiZiArray[16].Row;
                    if (thispoints[x, y] == true) return 16;
                }
            }
            if (thisQZ > 15)
            {
                for (int qizi = 21; qizi <= 31; qizi++) //车(23,24)，马(21,22)，炮(25,26)，卒(27,28,29,30,31)
                {
                    thispoints = MoveCheck.GetPathPoints(qizi, myqipan);
                    int x = (thisQZ == 0) ? col : GlobalValue.QiZiArray[0].Col;
                    int y = (thisQZ == 0) ? row : GlobalValue.QiZiArray[0].Row;
                    if (thispoints[x, y] == true) return 0;
                }
            }
            return -1;
        }
        /// <summary>
        /// 判断是否已绝杀
        /// </summary>
        /// <param name="qizi"></param>
        /// <returns></returns>
        public static int IsJueSha(int qizi)
        {
            
            return IsJiangJun(qizi,0,0);
        }
    }

}
