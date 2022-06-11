# Chess 中国象棋软件设计

#### 介绍
喜欢象棋，也喜欢编程，所以，有了这个象棋软件。
![4uBF3.png](https://s1.328888.xyz/2022/05/07/4uBF3.png)
##### 已具备如下功能
1.  棋盘可上下翻转，红方可在下面，也可以在上面。运行期间可随意翻转棋盘。
2.  走棋具备动画效果，有悔棋功能。
3.  可显示棋子移动的有效位置。
4.  红方先走棋，非走棋方的棋子选不中。
4.  将军时有提示，且下一步必须走解将的棋子，其他走棋无效。
5.  走棋错误时，自动取消走棋。
5.  有绝杀判断功能。判断是否绝杀的算法比较复杂，费了不少脑细胞。
6.  有记谱功能，可在单独窗口同步显示。
7.  点“开局”按钮，可恢复到初始状态。
8.  仿QQ象棋界面，严格遵循象棋走棋规则。
9.  SQLite在本地保存棋谱，具体增加、删除、修改功能。
10. 完善的变招数据存储结构。
11. 遇到变招时，显示箭头提示。

#### 软件架构

编程环境：Visual Studio 2019/2022
C#，NET5.0/6.0，WPF，SQLite3.0

#### 安装教程

通过NuGet安装如下包：
1.  Newtonsoft.Json
2.  System.Data.SQLite


#### 使用说明

1.  全部源码，开箱即用。
2.  代码中含有大量注释，能够快速理解程序流程。
3.  修改数据库地址为你磁盘的真实地址，修改位置如下：
``` c#
//  OpenSource\SqliteHelper.cs
//  数据库文件路径。调试期间使用绝对路径，发布时改为相对路径。
private static string DbFile = @"D:\CSHARP\Chess\DB\KaiJuKu.db";

//  软件发布时，数据库改为如下相对路径。
//  private static readonly string DbFile = System.Environment.CurrentDirectory + @"\DB\KaiJuKu.db";
```  

#### 代码示例

``` c#
for (int i = 0; i < 9; i++)
    for (int j = 0; j < 10; j++)
    {
        int qizi = GlobalValue.qiPan[i, j]; // 从棋盘上找到存活的本方棋子
        if (gongJiQiZi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
        {
            thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
            foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
            {
                if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                {
                    if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                        return true;  // true=能够解杀
                }
            }
        }
        if (gongJiQiZi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
        {
            thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
            foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
            {
                if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                {
                    if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                        return true;  // true=能够解杀
                }
            }
        }
    }
```


#### 绝杀算法流程图
![5PaWA.png](https://s1.328888.xyz/2022/06/02/5PaWA.png)
#### 参与贡献

1.  Fork 本仓库：暂无
2.  新建 Feat_xxx 分支：暂无
3.  提交代码：暂无
4.  新建 Pull Request：暂无

#### 参考资料

1.  [opencv识别象棋棋子_中国象棋电脑应用规范——棋盘棋子的格式坐标与着法表示](https://blog.csdn.net/weixin_28681719/article/details/113090094?utm_medium=distribute.pc_relevant.none-task-blog-2~default~baidujs_title~default-4-113090094-blog-87528438.pc_relevant_paycolumn_v3&spm=1001.2101.3001.4242.3&utm_relevant_index=6)
2.  [qq象棋棋谱格式详解及其解析](https://blog.csdn.net/qq_43668159/article/details/87528438)
3.  [中国象棋棋谱棋书链接](https://blog.csdn.net/hbuxiaofei/article/details/50686325?utm_medium=distribute.pc_relevant.none-task-blog-2~default~baidujs_title~default-0-50686325-blog-87528438.pc_relevant_paycolumn_v3&spm=1001.2101.3001.4242.1&utm_relevant_index=2)
4.  [谈谈象棋的基本功《三》棋谱篇](https://blog.csdn.net/l970090853/article/details/89036756?spm=1001.2101.3001.6650.3&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-89036756-blog-87528438.pc_relevant_paycolumn_v3&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-89036756-blog-87528438.pc_relevant_paycolumn_v3&utm_relevant_index=5)


#### 特技

1.  感谢Gitee!
