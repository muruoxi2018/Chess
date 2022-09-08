# Chess 中国象棋软件设计

#### 介绍
喜欢象棋，也喜欢编程，所以，有了这个象棋软件。
本软件在设计上，借鉴了国内知名象棋软件通行的设计思想，具有友好的操作界面，符合大众使用习惯。

#### 主要功能如下：
1. 人机对战，测试自己的象棋水平。
2. 电脑对战，观看电脑控制红黑双方棋子如何攻杀。
3. 自由打谱，练习各种变化，添加着法注释，并能够全部保存。
4. 具有复盘功能，所有保存的棋谱及其着法变化，都可以随时打开进行温习。
5. 残局破解，测试残局能力。系统自带30个残局，有视频破解教程。
6. 残局设计，可不断收集、扩展残局库。

[![主菜单界面](https://s1.328888.xyz/2022/09/08/9EVxw.png)](https://imgloc.com/i/9EVxw)
[![主菜单界面](https://s1.328888.xyz/2022/09/08/9EzaF.png)](https://imgloc.com/i/9EzaF)
[![用户设置窗口](https://s1.328888.xyz/2022/09/08/9EUjs.png)](https://imgloc.com/i/9EUjs)
[![着法提示与打分](https://s1.328888.xyz/2022/09/08/9Exc0.png)](https://imgloc.com/i/9Exc0)
[![棋谱库](https://s1.328888.xyz/2022/09/08/9EJQp.png)](https://imgloc.com/i/9EJQp)
[![残局设计](https://s1.328888.xyz/2022/09/08/9EyNo.png)](https://imgloc.com/i/9EyNo)

#### 已具备的其他功能
* 棋盘可上下翻转，红方可在下方，也可以在上方。运行期间可随意翻转棋盘。
* 走棋具备动画效果，有悔棋功能。
* 可显示棋子移动的有效位置，非法目标位置将不可走到。
* 红方先走棋，非走棋方的棋子选不中。
* 将军时有提示，且下一步必须走解将的棋子，其他走棋无效。
* 走棋错误时，自动取消走棋，棋子返回到走棋前位置。
* 有绝杀判断功能。判断是否绝杀的算法比较复杂，费了不少脑细胞。
* 有记谱功能，可在单独窗口同步显示。
* 点“开局”按钮，可恢复到初始状态。
* 仿天天象棋界面，严格遵循象棋走棋规则。
* 使用SQLite在本地保存棋谱，具体增加、删除、修改功能。
* 完善的变招数据存储结构，可保存所有变化。
* 遇到变招时，显示箭头提示。显示箭头数量可进行设置，以便保存界面清洁。
* 电脑提示下一步最佳着法，显示局面分。
* 电脑走棋速度可人为设置。
* 窗口可任意缩放，棋盘、棋子、按钮等同步缩放。
* 窗口背景可任意更换。
* 具备界面主题选择功能，可选择个人喜好的主题。
* 自动保存用户设置，下次打开软件时，自动使用上次保存的设置。

#### 软件架构

编程环境：Visual Studio 2019/2022
C#，NET5.0/6.0，WPF，SQLite3.0

#### 安装教程

使用源码时，通过NuGet安装如下包：
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

#### 明清棋谱大致分为两大类

1.  一类是少林派，以橘中秘，金鹏十八变等等，简称用炮局。所谓少林派，节奏明快，直来直往，势大力沉。
2.  一类是武当派，很简单，以梅花谱为代表，简称用马局。 所谓武当派，一波三折，曲径通幽，绵里藏针。
#### 特技

1.  感谢Gitee!
