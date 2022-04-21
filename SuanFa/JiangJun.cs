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

            bool[,] thispoints=MoveCheck.GetPathPoints(thisQZ,myqipan);



            if (thisQZ >= 16)
            {
                int x = GlobalValue.QiZiArray[0].Col;
                int y = GlobalValue.QiZiArray[0].Row;
                if (thispoints[x, y] == true) return 0;
            }
            if (thisQZ < 16)
            {
                int x = GlobalValue.QiZiArray[16].Col;
                int y = GlobalValue.QiZiArray[16].Row;
                if (thispoints[x, y] == true) return 16;
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
