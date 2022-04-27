# Chess

#### 介绍
喜欢象棋，也喜欢编程，所以，有了这个象棋软件。

##### 已具备如下功能
1.  棋盘可上下翻转，红方可在下面，也可以在上面。运行期间可随意翻转棋盘。
2.  走棋具备动画效果，有悔棋功能。
3.  可显示棋子移动的有效位置。
4.  将军时有提示，且下一步必须走解将的棋子，其他走棋无效。
5.  有绝杀判断功能。判断是否绝杀的算法比较复杂，费了不少脑细胞。
6.  有记谱功能，可在单独窗口同步显示。
7.  点“开局”按钮，可恢复到初始状态。
8.  仿QQ象棋界面，严格遵循象棋走棋规则。


##### 正在继续完善的功能：
1.  使用SQLite在本地保存棋谱。
2.  对已保存的棋谱增加修改、删除功能。
3.  开发变招数据存储结构。
4.  开发变招的箭头提示功能。

#### 软件架构

编程环境：Visual Studio 2019/2021
C#，NET5.0，WPF，SQLite3.0

#### 安装教程

通过NuGet安装如下包：
1.  Newtonsoft.Json
2.  System.Data.SQLite


#### 使用说明

1.  全部源码，开箱即用。
2.  代码中含有大量注释，能够快速理解程序流程。
3.  红方先走棋。非走棋方的棋子选不中。不会象棋的洗洗睡吧。


#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request


#### 特技

1.  感谢Gitee!
2.  感谢QQ象棋！如有版权问题，请留言，必改。
