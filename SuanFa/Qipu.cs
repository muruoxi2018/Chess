using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Chess.SuanFa
{
    public static class Qipu  // 棋谱类
    {

        public static ObservableCollection<QPStep> QiPuList = new(); // 棋谱步骤列表
        public class QPStep // 棋谱步骤
        {
            public int Id { get; set; } // 步数
            public string Nm { get; set; } // 数字代码
            public string Cn { get; set; } // 中文代码
            public string Memo { get; set; } // 备注
            public StepCode StepRecode { get; set; } // 棋谱记录
            public List<List<QPStep>> ChildSteps { get; set; }   // 棋谱变化
            public QPStep()
            {
                ChildSteps = new List<List<QPStep>>();
            }
            public TreeViewItem ToTreeNode()
            {
                string childSTR = ChildSteps.Count < 1 ? "" : $" -->({ChildSteps.Count})";

                TreeViewItem tree = new()
                {
                    Header = $"{Id:D2} {Cn}{childSTR}"
                };
                /*_ = tree.Items.Add(new TreeViewItem()
                {
                    Header = "数字编码: " + Nm
                });
                _ = tree.Items.Add(new TreeViewItem()
                {
                    Header = "备注: " + Memo
                });*/
                List<TreeViewItem> list = StepRecode.TreeViewItem();
                foreach (TreeViewItem item in list)
                {
                    //_ = tree.Items.Add(item);
                }
                if (ChildSteps != null)
                {
                    foreach (List<QPStep> qps in ChildSteps)
                    {
                        TreeViewItem childItem = new();
                        childItem.Header = $"变招: {ChildSteps.IndexOf(qps) + 1}";
                        foreach (QPStep qp in qps)
                        {
                            childItem.Items.Add(qp.ToTreeNode());
                        }
                        _ = tree.Items.Add(childItem);
                    }
                }
                return tree;
            }

        }
        public class StepCode // 棋谱记录
        {
            public StepCode(int qiZi, int x0, int y0, int x1, int y1, int dieQz)
            {
                QiZi = qiZi;
                X0 = x0;
                Y0 = y0;
                X1 = x1;
                Y1 = y1;
                DieQz = dieQz;
            }

            public int QiZi { get; set; } // 棋子编号
            public int X0 { get; set; } // 移动前位置
            public int Y0 { get; set; }
            public int X1 { get; set; } // 移动后位置
            public int Y1 { get; set; }
            public int DieQz { get; set; } // 移动后杀死的棋子
            public List<TreeViewItem> TreeViewItem()
            {
                List<TreeViewItem> tree = new();
                tree.Add(new TreeViewItem()
                {
                    Header = "棋子编号: " + QiZi
                });

                tree.Add(new TreeViewItem()
                {
                    Header = $"从({X0:D},{Y0:D})走到({X1:D},{Y1:D})"
                });
                tree.Add(new TreeViewItem()
                {
                    Header = "杀死棋子: " + ((DieQz > -1) ? DieQz : "无")
                });

                return tree;
            }

        }

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
                Id = QiPuList.Count + 1,
                Nm = $"{QiZi:d2} {x0:d} {y0:d} {x1:d} {y1:d} {DieQz:d}",
                Cn = char1 + char2 + char3 + char4,
                Memo = "",
                StepRecode = new StepCode(QiZi, x0, y0, x1, y1, DieQz)
            });

        }
        /// <summary>
        /// 将棋谱数字代码转化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public static string NmToJson()
        {
            ArrayList recode = new();
            foreach (QPStep p in QiPuList)
            {
                _ = recode.Add(p.Nm);
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
                _ = recode.Add(p.Cn);
            }
            return JsonConvert.SerializeObject(recode);
        }
        /// <summary>
        /// 将棋谱中文代码转化为长字符串
        /// </summary>
        /// <returns></returns>
        public static string CnToString()
        {
            int maxLen = 20;
            string recode = "";
            foreach (QPStep p in QiPuList)
            {
                recode += p.Cn + " ";
            }
            string substr = recode;
            if (recode.Length > maxLen)
            {
                substr = recode.Substring(0, maxLen) + " ...";
            }
            return $"{substr} (共{QiPuList.Count}步)";
        }


    }
}
