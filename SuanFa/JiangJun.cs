using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.SuanFa
{
    class JiangJun
    {
        public static int isJiangJun(int qizi)
        {
            MoveCheck.GetPathPoints(qizi);
            int x = GlobalValue.QiZiArray[0].Col;
            int y = GlobalValue.QiZiArray[0].Row;
            if (MoveCheck.PathBool[x,y]== true) return 0;
            x = GlobalValue.QiZiArray[16].Col;
            y = GlobalValue.QiZiArray[16].Row;
            if (MoveCheck.PathBool[x, y] == true) return 16;
            return -1;
        }
    }
}
