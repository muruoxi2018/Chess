using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Engine
{
    public class XQEngine
    {
        public static UcciInfoClass ucciInfo = new();   // 哈希表

        /// <summary>
        /// 获取最佳走棋招数
        /// </summary>
        /// <param name="fen">局面代码</param>
        /// <param name="thinkTime">思考限制时间</param>
        /// <param name="depth">计算深度</param>
        /// <returns></returns>
        public static string BestStep(int thinkTime, int depth)
        {
            string fenstr = QiPanDataToFenStr();
            Process proEngine = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    FileName = "Engine/eleeye.exe"
                }
            };
            proEngine.Start();
            proEngine.StandardInput.WriteLine("ucci");
            proEngine.StandardInput.WriteLine("setoption batch on");
            proEngine.StandardInput.WriteLine("position fen " + fenstr);
            proEngine.StandardInput.WriteLine("go time " + thinkTime.ToString());
            proEngine.StandardInput.WriteLine("go depth " + depth.ToString());
            proEngine.StandardInput.WriteLine("quit");
            string output = proEngine.StandardOutput.ReadToEnd();
            proEngine.Close();
            if (!output.Contains("nobestmove"))
            {
                // 裁剪掉无用信息
                int start = output.IndexOf("info depth");
                output = output[start..];
                output = output.Replace("bye" + Environment.NewLine, "");
                //output = UcciToCn(output);
                output = output.Replace(Environment.NewLine, "；");
                //start = output.IndexOf("bestmove");
                //return output.Substring(start);
            }
            else
            {
                output = "No BestMove!";
            }
            ucciInfo.InfoSource = output;
            return output;
        }

        // UCCI标准棋子名称，小写为黑方，对应0-15，大写为红方，对应16-31，见GlobalValue.qiZiCnName棋子名称数组
        private static readonly string[] Fen = {
            "k","a","a","b","b","n","n","r","r","c","c","p","p","p","p","p",
            "K","A","A","B","B","N","N","R","R","C","C","P","P","P","P","P"
        };

        /// <summary>
        /// 根据棋盘数据，生成FEN字符串
        /// </summary>
        /// <returns></returns>
        public static string QiPanDataToFenStr()
        {
            #region 棋盘数据生成FEN字符串
            string output = "";
            for (int i = 0; i < 10; i++) // 行
            {
                string lineString = "";
                int spaceNum = 0;
                for (int j = 0; j < 9; j++) // 列
                {
                    int qz = GlobalValue.qiPan[j, i];
                    if (qz == -1)
                    {
                        spaceNum++;
                    }
                    if (qz > -1)
                    {
                        if (spaceNum > 0)
                        {
                            lineString += spaceNum.ToString();
                            spaceNum = 0;
                        }
                        lineString += Fen[qz];
                    }
                }
                if (spaceNum > 0)
                {
                    lineString += spaceNum.ToString();
                }
                output += lineString;
                if (i < 9)
                {
                    output += "/";
                }
            }
            if (GlobalValue.sideTag == GlobalValue.BLACKSIDE)
            {
                output += " b";
            }
            else
            {
                output += " w";
            }
            output += " - - 0 1";
            return output;
            #endregion
        }

        public class PvClass
        {
            public string InfoStrLine { get; set; }
            public string Depth { get; set; }
            public string Score { get; set; }
            public List<string> UcciStr { get; set; }
            public List<string> CnStr { get; set; }
            public PvClass()
            {
                UcciStr = new List<string>();
                CnStr = new List<string>();
            }
            public void UcciToCn()
            {
                /*foreach (string str in bufArr) // 逐行解析
                {
                    string[] strarr = str.Trim().Split(" "); // 每一行字符串，删除前后空白字符，删除换行符后，再以空格为分隔符，分隔为单词数组
                    if (strarr.Length > 0)
                    {
                        pvc.Depth = strarr[Array.IndexOf(strarr, "depth") + 1]; // 搜索深度
                        pvc.Score = strarr[Array.IndexOf(strarr, "score") + 1]; // 分值

                        start = Array.IndexOf(strarr, "pv") + 1; // “pv”后边是着法，着法可能仅有一个，也可能是一串。
                        for (int i = start; i < strarr.Length; i++)
                        {
                            pvc.UcciStr.Add(strarr[i].Trim());
                        }
                        pvc.CnStr = UcciStrToCnStr(pvc.UcciStr, GlobalValue.qiPan);

                        string strs = $"着法（{pvc.Score}分）: ";
                        foreach (string stri in pvc.CnStr)
                        {
                            strs += stri + " ";
                        }
                        htb.Add("pv", strs);
                        pvc.htarr.Add(htb);
                    }
                    ucciInfo.InfoList.Add(pvc);
                }*/
            }
        }

        /// <summary>
        /// 着法类
        /// </summary>
        public class UcciInfoClass
        {
            public string InfoSource;
            public string Bestmove { get; set; }
            public string Ponder { get; set; }
            public List<PvClass> InfoList { get; set; }
            public UcciInfoClass()
            {
                InfoList = new List<PvClass>();
            }
            private void UcciToCn(string fenStr)
            {
                string keystr = "bestmove"; // 最佳着法
                int start = InfoSource.IndexOf(keystr);
                if (start > -1)
                {
                    string uccistr = InfoSource.Substring(start + keystr.Length, 5).Trim();
                    string cnstr = UcciStrToCnStr(uccistr, GlobalValue.qiPan, false);
                    Bestmove = cnstr;
                }

                keystr = "ponder"; // 后续的最佳着法
                start = InfoSource.IndexOf(keystr);
                if (start > -1)
                {
                    string uccistr = InfoSource.Substring(start + keystr.Length, 5).Trim();
                    string cnstr = UcciStrToCnStr(uccistr, GlobalValue.qiPan, false);
                    Ponder = cnstr;
                }
                int end = InfoSource.IndexOf("bestmove"); // 删除bestmove所在行
                string InfoStr = InfoSource[..end];
                InfoStr = InfoStr.Trim();
                string[] bufArr = InfoStr.Split(Environment.NewLine); // 按行分割字符串为数组
                foreach (string str in bufArr)
                {
                    PvClass pvc = new PvClass();
                    pvc.InfoStrLine=str;
                    pvc.UcciToCn();
                    InfoList.Add(pvc);
                }
            }
        }

        /// <summary>
        /// 将单个UCCI着法指令，转换为中文着法指令
        /// </summary>
        /// <param name="fenStr">UCCI着法指令，例如"h9g8"</param>
        /// <param name="curQiPan">棋盘数据数组</param>
        /// <returns>中文着法指令,例如“马八进七”</returns>
        private static List<string> UcciStrToCnStr(List<string> strlist, int[,] curQiPan)
        {
            int[,] qipan = new int[9, 10];
            for (int i = 0; i <= 8; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    qipan[i, j] = curQiPan[i, j];
                }
            }

            List<string> returnlist = new();
            foreach (string str in strlist)
            {
                string workStr = UcciStrToCnStr(str, qipan, true);
                returnlist.Add(workStr);
            }
            return returnlist;
        }

        /// <summary>
        /// 将单个UCCI着法，转换为中文着法
        /// </summary>
        /// <param name="ucciStr">UCCI着法</param>
        /// <param name="qipan">棋盘数据</param>
        /// <param name="writeQipan">是否有后续着法，用于避免改动系统现棋盘数据，导致数据错误</param>
        /// <returns></returns>
        private static string UcciStrToCnStr(string ucciStr, int[,] qipan, bool writeQipan)
        {
            ucciStr = ucciStr.Trim();
            if (ucciStr.Length != 4)
            {
                return "";
            }
            string cols = "abcdefghi";
            string rows = "9876543210";
            int x0 = cols.IndexOf(ucciStr[0]);  // 现位置
            int y0 = rows.IndexOf(ucciStr[1]);
            int x1 = cols.IndexOf(ucciStr[2]);  // 目标位置
            int y1 = rows.IndexOf(ucciStr[3]);
            string resultstr = "";
            int qizi = qipan[x0, y0];
            if (qizi > -1)
            {
                resultstr = GlobalValue.TranslateToCN(qizi, x0, y0, x1, y1);
                if (writeQipan)
                {
                    qipan[x0, y0] = -1;
                    qipan[x1, y1] = qizi;
                }
            }
            return resultstr;
        }
    }
}
