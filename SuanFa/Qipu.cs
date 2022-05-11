﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Drawing;
using System.ComponentModel;

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
        public class QiPuRecord: INotifyPropertyChanged
        {
            private QiPuRecord ParentNode { get; set; } // 父结点
            public ObservableCollection<QiPuRecord> ChildNode { get; set; }  // 子结点
            public QiPuRecord Cursor { get; set; }  // 当前结点游标指针，仅根结点游标有用
            public int Id { get; set; } // 步数
            public string Nm { get; set; } // 数字代码
            public string Cn { get; set; } // 中文代码
            public string Memo { get; set; } // 备注
            public StepCode StepData { get; set; } // 棋谱记录
            public string SideColor { get; set; }  // RED=红方，BLACK=黑方
            private bool _isSelected;
            public bool IsSelected { 
                get { return _isSelected; }
                set 
                {
                    _isSelected=value;
                    INotifyPropertyChanged("IsSelected");
                } }    // 是否选中

            private void INotifyPropertyChanged(string v)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(v));
            }

            public QiPuRecord()
            {
                ParentNode = null;
                ChildNode = new ObservableCollection<QiPuRecord>();
                Cursor = this;
                SideColor = "Red";
                IsSelected = false;
            }
            public QiPuRecord(QiPuRecord recordNode)
            {
                ParentNode = recordNode;
                recordNode.ChildNode.Add(this);
                ChildNode = new ObservableCollection<QiPuRecord>();
                Cursor = null;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public QiPuRecord AddChild(QiPuRecord child)
            {
                foreach (var item in ChildNode)
                {
                    if (string.Equals(item.Cn, child.Cn, StringComparison.OrdinalIgnoreCase))
                    {
                        return item;
                    }
                }
                //child.CurrentRecord = null;
                child.ParentNode = this;
                child.Id = getDepth();
                ChildNode.Add(child);
                return child;
            }
            public void DeleteChildNode()
            {
                ChildNode = new ObservableCollection<QiPuRecord>();
            }
            public bool IsRoot()
            {
                return ParentNode == null;
            }
            public bool IsLeaf()
            {
                return ChildNode.Count == 0;
            }
            public void SetParent(QiPuRecord parent)
            {
                this.ParentNode = parent;
            }
            public QiPuRecord GetParent()
            {
                return ParentNode;
            }
            public void SetRecordData(StepCode code)
            {
                if (code == null) return;
                SetRecordData(code.QiZi, code.X0, code.Y0, code.X1, code.Y1, code.DieQz);
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
            public void SetRecordData(int QiZi, int x0, int y0, int x1, int y1, int DieQz)
            {
                string char1 = GlobalValue.QiZiCnName[QiZi];
                string char2 = QiZi is > 0 and < 15 ? (x0 + 1).ToString() : GlobalValue.CnNumber[9 - x0];
                string char3 = "";
                string char4;
                #region 棋谱翻译为中文
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
                #endregion
                Nm = $"{QiZi:d2} {x0:d} {y0:d} {x1:d} {y1:d} {DieQz:d}";
                Cn = char1 + char2 + char3 + char4;
                Memo = "";
                StepData = new StepCode(QiZi, x0, y0, x1, y1, DieQz);
                SideColor = QiZi is > 0 and < 15 ? "Black" : "Red";
                //QiPuList.Add(this);
            }
            private int getDepth()
            {
                int depth = 1;
                QiPuRecord point = this;
                while (!point.IsRoot())
                {
                    depth++;
                    point = point.GetParent();
                }
                return depth;
            }
            public TreeViewItem GetTree()
            {
                var tree = new TreeViewItem();
                tree.Header = $"{Id}. {Cn}";
                tree.IsExpanded = true;
                if (!IsLeaf())
                {
                    foreach (var item in ChildNode)
                    {
                        tree.Items.Add(item.GetTree());
                    }
                }
                return tree;
            }
        }
        public class QiPuSimpleRecord
        {
            public List<QiPuSimpleRecord> Child { get; set; }  // 子结点
            public StepCode Data { get; set; } // 棋谱记录
            public QiPuSimpleRecord()
            {
                Child = new List<QiPuSimpleRecord>();
            }
            public void CopyDataFromStep(StepCode code)
            {
                if (code == null) return;
                Data = new StepCode(code.QiZi, code.X0, code.Y0, code.X1, code.Y1, code.DieQz);
            }
        }

        public class StepCode // 棋谱记录
        {

            public int QiZi { get; set; } // 棋子编号
            public int X0 { get; set; } // 移动前位置
            public int Y0 { get; set; }
            public int X1 { get; set; } // 移动后位置
            public int Y1 { get; set; }
            public int DieQz { get; set; } // 移动后杀死的棋子
            public StepCode(int qiZi, int x0, int y0, int x1, int y1, int dieQz)
            {
                QiZi = qiZi;
                X0 = x0;
                Y0 = y0;
                X1 = x1;
                Y1 = y1;
                DieQz = dieQz;
            }
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
            #region 棋谱翻译为中文
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
            #endregion
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
        public static List<List<System.Drawing.Point>> GetListPoint(QPStep qPStep)
        {
            List<List<System.Drawing.Point>> pp = new List<List<System.Drawing.Point>>();
            foreach (var lp in qPStep.ChildSteps)
            {
                QPStep qs = lp[0];
                List<System.Drawing.Point> pt = new List<System.Drawing.Point>();
                pt.Add(new System.Drawing.Point(qs.StepRecode.X0, qs.StepRecode.Y0));
                pt.Add(new System.Drawing.Point(qs.StepRecode.X1, qs.StepRecode.Y1));
                pp.Add(pt);
            }
            return pp;
        }


    }
}
