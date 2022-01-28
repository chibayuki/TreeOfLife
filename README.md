[![GitHub stars](https://img.shields.io/github/stars/chibayuki/TreeOfLife.svg?style=social&label=Stars)](https://github.com/chibayuki/TreeOfLife/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/chibayuki/TreeOfLife.svg?style=social&label=Fork)](https://github.com/chibayuki/TreeOfLife/network/members)
[![GitHub watchers](https://img.shields.io/github/watchers/chibayuki/TreeOfLife.svg?style=social&label=Watch)](https://github.com/chibayuki/TreeOfLife/watchers)
[![GitHub followers](https://img.shields.io/github/followers/chibayuki.svg?style=social&label=Follow)](https://github.com/chibayuki?tab=followers)

[![GitHub issues](https://img.shields.io/github/issues/chibayuki/TreeOfLife.svg)](https://github.com/chibayuki/TreeOfLife/issues)
[![GitHub last commit](https://img.shields.io/github/last-commit/chibayuki/TreeOfLife.svg)](https://github.com/chibayuki/TreeOfLife/commits)
[![GitHub release](https://img.shields.io/github/release/chibayuki/TreeOfLife.svg)](https://github.com/chibayuki/TreeOfLife/releases)
[![GitHub repo size in bytes](https://img.shields.io/github/repo-size/chibayuki/TreeOfLife.svg)](https://github.com/chibayuki/TreeOfLife)
[![Language](https://img.shields.io/badge/language-C%23-green.svg)](https://github.com/chibayuki/TreeOfLife)

# Tree of Life
基于现代分类学的生物演化树编辑程序。

![ScreenShot](ScreenShot.png)

## 技术要点
* 树与图的增删改查及其序列化/反序列化
* 文件封装与版本控制
* 语义分析与检索

## 版本跟踪
* 后续版本
> 将在[TreeOfLife.Release](https://github.com/chibayuki/TreeOfLife.Release)发布。
* 里程碑 14 (版本 1.0.1470.1000.M14.211205-1900)
> 新增"统计"与"校对"页面，编辑状态下"分类学"页面新增不合规提示；"检索"页面新增范围选择，每个分组新增显示结果数目，支持折叠与展开；改进控件外观、颜色与UI布局，提高UI整体对比度，修复部分控件内容模糊问题；删除部分从未使用的分级；地质年表更新至v2021/10版。
* 里程碑 13 (版本 1.0.1322.1000.M13.210925-1400)
> 改进UI元素的主题控制，实现全局主题颜色与浅色/深色主题热更新；UI进一步与后端分离，耗时操作将在后台运行；位于顶级类群时已支持插入下级类群。
* 里程碑 12 (版本 1.0.1240.1000.M12.210718-2000)
> 改进搜索，搜索结果按相关性分组显示，搜索结果较多时将默认折叠相关性较低的分组，支持对并系群、复系群的搜索，支持Esc键清空搜索关键字和搜索结果，搜索完成后自动全选搜索关键字；改进“分类学”页面，"科学分类"支持切换详细程度，"下属类群"不再显示并系群排除的类群，调整标签和异名的布局；新增诞生和灭绝年代。
* 里程碑 11 (版本 1.0.1134.1000.M11.210518-2200)
> 新增Logo与“关于”页面；改进主要UI元素的样式、颜色、交互效果等；部分控件显示"并系群"、"复系群"字样，编辑状态上级类群改为只显示到最近的一个具名类群；改进添加下级类群时"种"和"亚种"的匹配；改进搜索，支持不连续文本匹配，支持Enter键触发搜索。
* 里程碑 10 (版本 1.0.1030.1000.M10.210405-1400)
> UI实现图形化的演化树，实现直接在演化树上进行编辑，演化树规模较小时位置改为居中。
* 里程碑 9 (版本 1.0.915.1000.M9.210129-2000)
> 底层数据结构由N叉树改为加权有向图；实现并系群、复系群的编辑与存储；版本控制首次得到验证；删除的类群将自动从搜索结果中去除；更新中文名由自动改为提示。
* 里程碑 8 (版本 1.0.812.1000.M8.210108-2100)
> 实现模糊搜索功能；纯文本演化树改为只显示当前类群附近的部分；选择类群和编辑类群时将同步更新演化树；编辑类群分级时将自动更新中文名。
* 里程碑 7 (版本 1.0.708.1000.M7.201230-2100)
> 实现基本的搜索功能和UI。
* 里程碑 6 (版本 1.0.617.1000.M6.201226-1000)
> 移植到.NET 5和WPF；将文件、版本等抽象为包、包内容等数据结构；“文件”页面增加文件信息。
* 里程碑 5 (版本 1.0.415.1000.M5.201204-2200)
> 文件格式变更为易于版本兼容的容器式结构；UI实现类群的继承变更、排序、删除等功能；新增临时的纯文本演化树。
* 里程碑 4 (版本 1.0.323.1000.M4.201128-1700)
> UI封装用于选择分类级别的多级动态控件；实现类群的查看、跳转、基本信息编辑功能。
* 里程碑 3 (版本 1.0.209.1000.M3.201119-1900, 未发布)
> 后端实现基于Json的文件打开、保存等功能。
* 里程碑 2 (版本 1.0.112.1000.M2.201110-2050)
> UI封装表示类群、上下级类群组的控件。
* 里程碑 1 (版本 1.0.16.1000.M1.201110-2050, 未发布)
> 初步定义类群、演化树等概念的数据结构；后端实现类群的增删改查等基本功能。

#### 说明
###### 1、Tree of Life使用了Com的未开源版本，如有运行/调试需求，请自行解决。
###### 2、截图所示的生物分类数据主要来自维基百科，数据文件尚未开源。
###### 3、如有合作意向，需要未开源可执行文件、数据文件的，请联系本人。
