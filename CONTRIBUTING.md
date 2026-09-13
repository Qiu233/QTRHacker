# 开发约定

## 项目职责

| 项目 | 职责 |
| --- | --- |
| `QTRHacker` | WPF 界面、ViewModel、用户配置、本地化和资源展示 |
| `QTRHacker.Core` | Terraria 对象封装、游戏操作、补丁调用和投射物工具 |
| `QHackLib` | 远程内存、汇编、线程和托管对象访问 |
| `QHackCLR` | C++/CLI 实现的 CLR 数据访问 |
| `QTRHacker.Patches` | 在游戏进程中运行的补丁，使用游戏对应的 .NET Framework |
| `Keystone.Net` | Keystone 汇编引擎的托管绑定 |
| `GameDataExporter` / `GameWikiResExporter` | 从游戏提取字段清单和 Wiki 资源 |
| `Launcher` | 启动随压缩包附带的 .NET 运行时 |
| `QTRHacker.Functions.Test` | 手动游戏操作实验及版本兼容性检查 |

修改尽量落在所属层。界面交互留在 WPF 项目，游戏相关行为放在 Core 或 Patches；底层库不依赖界面。先提取已经重复的逻辑，再根据实际调用需求增加抽象。

## 代码与生成文件

- 格式遵循根目录 `.editorconfig`。C# 使用制表符缩进、文件范围命名空间；XAML 使用空格缩进。修改已有文件时只整理相关代码，避免整库格式化淹没功能差异。
- 新增 C# 文件使用 UTF-8，脚本含中文时使用带 BOM 的 UTF-8，以兼容 Windows PowerShell 5.1。读写资源时显式指定编码，二进制资源保持字节内容。
- `*.ps.cs` 由 T4 生成。字段调整应修改 `*Properties` 和模板，再重新生成，具体规则见 [Core 游戏对象说明](src/QTRHacker.Core/GameObjects/Terraria/README.md)。
- `QHackCLR` 中的 CLR 接口头文件、Keystone 常量和游戏导出数据按各自来源维护。不要将命名、整数宽度、结构布局或游戏方法签名当成普通风格问题修改。
- 导出器的产物先检查再同步到项目资源；导出器默认输出到自己的运行目录。Wiki 资源流程见 [GameWikiResExporter](src/GameWikiResExporter/README.md)。

## WPF 与 MVVM

- 普通字段属性使用 `ViewModelBase.SetProperty`，由编译器提供属性名，并在值改变后通知。依赖属性的联动通知仍显式调用 `OnPropertyChanged(nameof(...))`。
- 写入游戏内存或需要强制刷新的属性应保留相应语义，不能机械替换为忽略相同值的 setter。
- 命令在构造时创建或缓存，getter 返回同一个实例。无执行条件时使用 `RelayCommand(Action<object>)`；需要游戏连接时使用 `HackCommand`。
- 批量修改先完成状态更新，再通知列表刷新。Wiki 的关键词、分类选择和筛选命令由 `WikiFilterViewModel` 管理，页面通过 `FilterChanged` 刷新数据。
- ViewModel 的异步方法优先返回 `Task`，由调用方等待并处理异常。`async void` 限于事件入口；不要在 UI 线程阻塞等待任务。
- 文件、线程、计时器和进程句柄需要明确的所有者与释放时机。异常处理应保留有用的上下文，避免用空 `catch` 隐藏失败。

## 构建、验证与发布

使用带有 .NET 桌面开发、C++/CLI 和相应 .NET Framework 目标包的 Visual Studio。桌面程序目标为 .NET 8 / x86；游戏内补丁与导出器的目标框架独立维护，不能全局统一成 .NET 8。

普通代码变更先验证 `Debug | x86` 构建，再检查受影响的操作。涉及本轮公共界面代码时，至少检查：属性通知名称、命令实例是否稳定、Wiki 搜索/反选/重置、中文和英文切换，以及先打开 Wiki 后连接游戏的命令状态。

`QTRHacker.Functions.Test` 的默认入口会修改游戏物品。版本核对使用明确的 `--verify-game-compatibility` 参数，命令及覆盖范围见 Core 游戏对象说明。

发布流程在 Visual Studio 中运行，输出到 `bin/Publish/Vanilla`。之后运行 `scripts/Pack.ps1` 或 `scripts/Pack-WithRuntime.ps1` 打包已有文件；打包脚本不负责构建。版本以主项目的 `Version` 为准，`bin`、`obj`、运行时缓存和压缩包不纳入版本控制。
