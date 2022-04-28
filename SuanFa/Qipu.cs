using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;

namespace Chess.SuanFa
{
    public static class Qipu  // 棋谱类
    {
        public class QPStep // 棋谱步骤
        {
            public int id { get; set; } // 步数
            public string Nm { get; set; } // 数字代码
            public string Cn { get; set; } // 中文代码
            public Step StepRecode { get; set; } // 棋谱记录
            public List<QPStep> qPSteps { get; set; } = new List<QPStep>();   // 棋谱变化

        }
        public class Step // 棋谱记录
        {
            public int QiZi { get; set; } // 棋子编号
            public int x0 { get; set; } // 移动前位置
            public int y0 { get; set; }
            public int x1 { get; set; } // 移动后位置
            public int y1 { get; set; }
            public int DieQz { get; set; } // 移动后杀死的棋子

        }

        public static ObservableCollection<QPStep> QiPuList = new(); // 棋谱步骤列表

        /// <summary>
        /// 添加一条棋谱记录
        /// </summary>
        /// <param name="QiZi"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="DieQz"></param>
        public static void AddItem(int QiZi, int x0, int y0, int x1, int y1, int DieQz)
        {
            string char1 = GlobalValue.QiZiCnName[QiZi];
            string char2 = QiZi is > 0 and < 15 ? (x0 + 1).ToString() : GlobalValue.CnNumber[9 - x0];
            string char3 = "";
            string char4;

            int m = Math.Abs(y1 - y0);
            // 进退平
            if (y0 == y1)
            {
                char3 = "平";
                char4 = QiZi is >= 0 and <= 15 ? (x1 + 1).ToString() : GlobalValue.CnNumber[9 - x1];
            }
            else
            {
                if (QiZi is >= 0 and <= 15)
                {
                    char3 = y1 > y0 ? "进" : "退";
                }
                if (QiZi is >= 16 and <= 31)
                {
                    char3 = y1 > y0 ? "退" : "进";
                }

                char4 = QiZi switch
                {
                    1 or 2 or 3 or 4 or 5 or 6 => (x1 + 1).ToString(),
                    17 or 18 or 19 or 20 or 21 or 22 => GlobalValue.CnNumber[9 - x1],
                    // 其他所有可以直走的棋子
                    _ => QiZi is > 0 and < 15 ? m.ToString() : GlobalValue.CnNumber[m],
                };

            }
            QiPuList.Add(new QPStep()
            {
                id = QiPuList.Count() + 1,
                Nm = string.Format("{0:d2} {1:d} {2:d} {3:d} {4:d} {5:d}", QiZi, x0, y0, x1, y1, DieQz),
                Cn = char1 + char2 + char3 + char4,
                StepRecode = new Step() { QiZi = QiZi, DieQz = DieQz, x0 = x0, y0 = y0, x1 = x1, y1 = y1, }
            });

        }
        /// <summary>
        /// 将棋谱数字代码转化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public static string NmToJson()
        {
            ArrayList recode=new();
            foreach(QPStep p in QiPuList)
            {
                recode.Add(p.Nm);
            }
            return JsonConvert.SerializeObject(recode);
        }
        /// <summary>
        /// 将棋谱中文代码转化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public static string CnToJson()
        {
            ArrayList recode = new();
            foreach (QPStep p in QiPuList)
            {
                recode.Add(p.Cn);
            }
            return JsonConvert.SerializeObject(recode);
        }
        /// <summary>
        /// 将棋谱中文代码转化为长字符串
        /// </summary>
        /// <returns></returns>
        public static string CnToString()
        {
            string recode="";
            foreach (QPStep p in QiPuList)
            {
                recode += p.Cn+" ";
            }
            return recode;
        }
    }
}
