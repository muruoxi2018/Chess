using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Engine
{
    public class XQEngine
    {

        public static string BestStep(string fen, int thinkTime)
        {
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
            proEngine.StandardInput.WriteLine("position fen " + fen);
            proEngine.StandardInput.WriteLine("go time " + thinkTime.ToString());
            proEngine.StandardInput.WriteLine("quit");
            string output = proEngine.StandardOutput.ReadToEnd();
            proEngine.Close();
            if (!output.Contains("nobestmove"))
            {
                // 裁剪掉无用信息
                int start = output.IndexOf("info depth");
                output = output.Substring(start);
                output = output.Replace("info depth", "info_depth");
                output = output.Replace("bye" + Environment.NewLine, "");
                output = fenToCn(output);
                output = output.Replace(Environment.NewLine, "；");
                //start = output.IndexOf("bestmove");
                //return output.Substring(start);
            }
            else
            {
                output = "No BestMove!";
            }
            return output;
        }

        // UCCI标准
        private static readonly string[] Fen = {
            "k","a","a","b","b","n","n","r","r","c","c","p","p","p","p","p",
            "K","A","A","B","B","N","N","R","R","C","C","P","P","P","P","P"
        };

        /// <summary>
        /// 根据棋盘数据，生成FEN字符串
        /// </summary>
        /// <returns></returns>
        public static string getFenString()
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

        private static string fenToCn(string fenStr)
        {
            string bufferStr = fenStr.Replace(Environment.NewLine, " ");
            string[] arrStr = bufferStr.Split(" ");
            string cols = "abcdefghi";
            string rows = "9876543210";

            for (int i = 0; i < arrStr.Length; i++)
            {
                if (arrStr[i] == "pv" || arrStr[i] == "bestmove" || arrStr[i] == "ponder")
                {
                    string fens = arrStr[i + 1];
                    int x0 = cols.IndexOf(fens[0]);
                    int y0 = rows.IndexOf(fens[1]);
                    int x1 = cols.IndexOf(fens[2]);
                    int y1 = rows.IndexOf(fens[3]);
                    int qizi = GlobalValue.qiPan[x0, y0];
                    if (qizi > -1)
                    {
                        string cnstr = GlobalValue.TranslateToCN(qizi, x0, y0, x1, y1);
                        fenStr = fenStr.Replace(fens, cnstr);
                    }

                }
            }
            return fenStr;
        }
    }
}
