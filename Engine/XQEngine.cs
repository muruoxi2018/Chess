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
            int start = output.IndexOf("bestmove");
            return output.Substring(start);
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
            string output = "";
            for(int i = 0; i < 10; i++) // 行
            {
                string lineString = "";
                int spaceNum=0;
                for (int j = 0; j < 9; j++) // 列
                {
                    int qz=GlobalValue.qiPan[j,i];
                    if (qz == -1)
                    {
                        spaceNum++;
                    }
                    if (qz > -1)
                    {
                        if (spaceNum > 0)
                        {
                            lineString+=spaceNum.ToString();
                            spaceNum = 0;
                        }
                        lineString+=Fen[qz];
                    }
                }
                if (spaceNum > 0)
                {
                    lineString += spaceNum.ToString();
                }
                output +=lineString;
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
        }
    }
}
