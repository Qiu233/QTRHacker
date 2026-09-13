# QHackCLR 检查与回归

公共 `DataAccess.Read/Write` 在参数非法时抛出参数异常，远程内存访问失败或传输不完整时抛出 `IOException`，包含地址、长度、Win32 错误码和 HRESULT。原有布尔返回值保留以兼容调用处，成功返回 `true`。零长度操作允许使用空数组；包含托管引用的值类型不能用于原始内存读写。

DAC 查询失败抛出带操作名和 HRESULT 的 `COMException`。DAC 的 `ReadVirtual` 使用内部 `TryRead`，继续以 HRESULT 表达探测性读取失败；`Flush` 使用的特殊读取回调保持原有协议。空引用仍可构造 `ClrObject` 并通过 `IsNullPtr` 判断。

`ClrType.GetLength(objRef)` 同时支持数组和字符串：`GameString` 通过 `GameObjectArrayV<char>.Length` / `HackObject.GetArrayLength` 读取字符串长度，玩家名称、游戏版本及属性编辑器都依赖这条路径。带维度的长度读取及数组索引接口仍只用于数组。

字段枚举按 DAC 提供的字段数量结束，并扣除基类的实例字段数量。`NextField` 是下一个字段描述符的地址，不是以零结束的链表。

## 独立回归

在仓库根目录使用 Visual Studio MSBuild 构建（需要 C++/CLI、.NET 8 x86 和 .NET Framework 4.8 targeting pack）：

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' src/QTRHacker.Functions.Test/QTRHacker.Functions.Test.csproj /restore /t:Build /p:Configuration=Debug /p:Platform=x86 "/p:SolutionDir=$((Get-Location).Path)\"
& 'C:\Program Files (x86)\dotnet\dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-clr src/QHackCLR.TestTarget/bin/x86/Debug/net48/QHackCLR.TestTarget.exe
```

检查启动独立的 .NET Framework 进程，不需要游戏。覆盖：正常及失败的内存读写、缓冲区边界、含引用的值类型、普通数组及带下界的多维/一维数组、空引用、未 JIT 方法、继承及线程静态字段、非法 DAC 句柄、元数据查询前后的 DAC 引用计数、缓存刷新和目标退出。字符串回归使用真实 `GameString`，验证空串、中文、内嵌零字符、代理对，以及 `GetValue`、`ToString`、隐式转换和属性编辑器使用的静态读取方法。

`QHackLib.InlineHook` 的释放流程也使用这个独立目标进程验证：

```powershell
& 'C:\Program Files (x86)\dotnet\dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-inline-hook src/QHackCLR.TestTarget/bin/x86/Debug/net48/QHackCLR.TestTarget.exe
```

连续执行 20 次一次性钩子生命周期，核对只执行一次、返回值和原指令保持正确、解除钩子后仍能读取自有分配区的头部，以及最终释放内存。`WaitToDetach` 后目标地址已恢复原指令，`WaitToDispose` 必须从钩子的分配地址读取信息，不能再将目标指令解释为跳转地址。通过解决方案构建时，测试目标位于 `src/QHackCLR.TestTarget/bin/Debug/net48/`。

## 连接后的界面读取

游戏进入世界后运行：

```powershell
& 'C:\Program Files (x86)\dotnet\dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-game-attach
```

此检查调用十字连接使用的 `HackGlobal.Initialize`，读取实际游戏版本和属性编辑器字符串，并让真实 `PlayersListViewViewModel` 的 `DispatcherTimer` 连续刷新 20 次。随后初始化玩家编辑窗口，选择全部 340 个物品格，核对三套配装的装备/染料数组映射，并切换全部 10 个页签，验证刷新绑定及实际定时刷新。配装页面的染料格编号 20–29 应映射到 `Dye[0..9]`，不能直接作为染料数组下标。

检查不注入补丁、不修改游戏数据，不显示窗口或模拟鼠标操作；测试日志和配置写到独立临时目录。控制台测试进程单独设置 WPF 资源所属程序集，以加载主程序的真实 XAML 和图片资源。

游戏兼容性使用 [Core 中的只读检查](../QTRHacker.Core/GameObjects/Terraria/README.md)。不要运行 Functions.Test 的默认入口，它会修改游戏物品。

## 本轮审查中的生命周期结论

- `GetModule` 创建引用计数为 1 的 `ClrDataModule`，它自身持有 DAC 引用；`QueryInterface(IMetaDataImport)` 经 `GetMdInterface` 对返回接口额外 `AddRef`。两份引用各自需要 `Release`。已修复模块名称读取和字段元数据查询的局部释放，包括失败路径；独立回归确认重复查询不再增加 DAC 引用计数。[官方 GetModule 实现](https://github.com/dotnet/coreclr/blob/master/src/debug/daccess/request.cpp)、[官方 ClrDataModule/GetMdInterface 实现](https://github.com/dotnet/coreclr/blob/master/src/debug/daccess/task.cpp)
- `ClrType` 等包装类只有析构函数，没有终结器；`RuntimeBuilder.Flush` 清空缓存时也不调用其析构函数。C++/CLI 的 GC 不会自动执行 `IDisposable.Dispose`，所以普通 `new Dacp...` 快照仍存在生命周期问题。不能在 `Flush` 中直接销毁所有包装对象，因为上层仍可能持有这些对象。[C++/CLI 析构与终结器语义](https://learn.microsoft.com/en-us/cpp/dotnet/how-to-define-and-consume-classes-and-structs-cpp-cli#destructors-and-finalizers)
- `QHackContext.Dispose` 只释放 `DataTarget`；`ClrAppDomain` 持有的 DAC 接口、`DacLibrary` 及 DLL 的整体释放策略本轮保持原样。它们涉及存活包装对象和 DAC 的释放顺序，不能通过在断开连接时补一个 `FreeLibrary` 解决。

类型属性的 `HasFlag` 判断在当前仓库未找到调用处，保持原样；`DacDataTargetImpl.QueryInterface` 和 `WriteVirtual` 按要求保持原样。
