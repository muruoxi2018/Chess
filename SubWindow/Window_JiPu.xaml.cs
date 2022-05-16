using Chess.SuanFa;
using Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chess
{
    /// <summary>
    /// Window_JiPu.xaml 的交互逻辑
    /// </summary>
    public partial class Window_JiPu : Window
    {
        public Window_JiPu()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, RoutedEventArgs e)
        {
            JiPuDataGrid.ItemsSource = Qipu.QiPuList;

        }
        /// <summary>
        /// 将新谱保存到老谱中，变招作为分支保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save_jipu();
        }
        public void Save_jipu()
        {
            List<List<Qipu.QPStep>> listlistQipu = new();
            listlistQipu.Add(GlobalValue.FuPanDataList.ToList());
            ReBuildQipuList(listlistQipu, Qipu.QiPuList.ToList());
            GlobalValue.FuPanDataList.Clear();
            foreach (Qipu.QPStep qp in listlistQipu[0])
            {
                GlobalValue.FuPanDataList.Add(qp);
            }
            GlobalValue.Window_QiPuKu.GetMemo(GlobalValue.FuPanDataList);
        }
        /// <summary>
        /// 递归查找变招位置，并将变招存入相应分支
        /// </summary>
        /// <param name="OldQiPu">老谱</param>
        /// <param name="NewQiPu">新谱</param>
        private void ReBuildQipuList(List<List<Qipu.QPStep>> OldQiPu, List<Qipu.QPStep> NewQiPu)
        {
            bool findExist = false;
            foreach (List<Qipu.QPStep> oldqp in OldQiPu)
            {
                if (string.Equals(NewQiPu[0].Cn, oldqp[0].Cn, StringComparison.Ordinal))
                {
                    findExist = true; // 查找是否有第一步相同的棋谱
                }
            }
            if (findExist == false)
            {
                OldQiPu.Add(NewQiPu);
                return;
            }
            else // 存在第一步相同的棋谱
            {
                for (int listIndex = 0; listIndex < OldQiPu.Count; listIndex++)
                {
                    if (string.Equals(NewQiPu[0].Cn, OldQiPu[listIndex][0].Cn, StringComparison.Ordinal)) // 定位到第一步相同的棋谱
                    {
                        for (int i = 1; i < OldQiPu[listIndex].Count; i++) // 逐项对比
                        {
                            if (i > NewQiPu.Count - 1)
                            {
                                return; // 如果的步数少于老谱，且新棋谱与老谱完全重合，则不作处理，直接退出
                            }
                            if (i == OldQiPu[listIndex].Count - 1 && OldQiPu[listIndex].Count < NewQiPu.Count) // 老谱已到末尾，且新谱还没结束时
                            {
                                for (int j = OldQiPu[listIndex].Count; j < NewQiPu.Count; j++)
                                {
                                    OldQiPu[listIndex].Add(NewQiPu[j]); // 将新谱剩余的步数追加到老谱上
                                }
                                return;
                            }
                            if (!string.Equals(NewQiPu[i].Cn, OldQiPu[listIndex][i].Cn, StringComparison.Ordinal))
                            {
                                // 找到变招位置后
                                List<Qipu.QPStep> subNew = new();
                                for (int j = i; j < NewQiPu.Count; j++)
                                {
                                    subNew.Add(NewQiPu[j]); // 删除相同的招数
                                }
                                ReBuildQipuList(OldQiPu[listIndex][i - 1].ChildSteps, subNew); // 将变化招数存入子分支
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
