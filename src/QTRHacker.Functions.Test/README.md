# 自动诊断与 VEH join 原型

无参数运行现在诊断“解锁所有研究”：附加并读取游戏状态，记录研究方法的实际签名，执行一次安装目录中 `UnlockAllDuplications.Enable`，并在返回后继续观测 30 秒。需要游戏已进入世界，关闭/折叠旅行模式菜单，期间不要同时操作其他修改器功能。

用户将诊断包覆盖到修改器目录，双击 QTRHacker.Functions.Test.exe，再发回同目录的 QTRHacker-Diagnostic-*.log。没有菜单或命令行操作。测试会改变研究进度；本次修复位于 `QTRHacker.dll`，发包时需包含修复后的 DLL，否则诊断仍执行旧版功能。

日志实时写入，包含步骤时间、异常树与 Win32 错误码、独立进程句柄的存活/退出状态、退出码、内存状况及组件版本/哈希。也记录研究对象地址和 `LastEditId`，观测期间沿当前引用读取，不发起 DAC 查询。研究数据并非一致快照，进度变化不代表全部成功。诊断进程的托管未处理异常和未观察任务异常会尽量写入日志；原生致命错误可能只能留下最后一条记录。

`LastEditId` 是修改计数，不是已解锁条目数；重复注册已完成的研究可能不会改变它。`result=3` 也包括游戏提前退出导致观测中止的情况，需结合游戏退出码和用户反馈判断，不能直接当作游戏崩溃。

观察线程不调用 DAC、不附加调试器；调用卡住或游戏退出时也会记录状态，120 秒后结束诊断，不强行释放仍在执行的远程调用内存。旧版入口可能在两秒后返回而后台仍在运行，因此诊断不会因入口返回立即关闭连接。进程状态是离散观测，不能保证捕获两次观测之间的精确先后关系。

开发用 --diagnose --once [--pid PID] 只附加读取，不执行解锁。原来的背包短代码移到显式 --manual-item。

`--verify-research-call` 在本机独立汇编接收器上执行正式功能生成的循环，检查全部物品 ID、`amount=9999`、`teammateName=null` 和栈平衡，不需要启动游戏。x86 托管参数按从左到右顺序压栈，见 [CLR ABI](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/botr/clr-abi.md#calling-convention-specifics-for-x86)。该测试验证传参，不代替游戏中的 GC、线程与研究效果验证。

`--verify-map-call` 执行正式揭示地图功能生成的汇编，验证最小有效范围和小/中/大世界的每个坐标、亮度、栈与寄存器恢复。扫描范围与游戏 `WorldMap.UnlockMapSection` 一致，避开四周 40 格；`MapHelper.GetBackgroundType` 会读取附近地图格，不能从世界边缘开始调用。此检查不启动游戏，不代替地图渲染与远程执行验证。

`--verify-clr-call-arguments` 验证 x86 的 `UInt32`、`UIntPtr`、`IntPtr` 参数在寄存器和栈中保留完整位模式，覆盖 `0x7FFFFFFF`、`0x80000000`、`0xFFFFFFFF`，以及 `this`、返回缓冲区和普通有符号参数。还按字符串构造器的传参形状读取实际分配在 2 GiB 以上的字符串缓冲区。使用启用 LAA 的 `QTRHacker.Functions.Test.exe` 运行；不连接游戏。

--verify-process-exit <QHackCLR.TestTarget.exe> 使用独立测试进程，对照存活进程、已释放地址、已关闭句柄及退出后保留的句柄。本机对照中，退出后保留句柄会同时产生分配错误 5 和读取错误 299；存活进程的失效地址也能产生 299，因此仅凭原截图不能排除游戏先退出。

QHackCLR 附加时的地址溢出回归另见 [DacAddressChecks.md](DacAddressChecks.md)。

`--verify-clr` 还验证按名称定位普通类型时，不依赖整个模块的类型列表：人为让完整枚举报错，单独查找远程线程所需的四个 BCL 类型仍应成功；目标查询自身的错误仍需抛出。也检查嵌套类型、未加载类型、不存在的类型与元数据接口引用释放。实现先用 [FindTypeDefByName](https://learn.microsoft.com/en-us/dotnet/core/unmanaged-api/metadata/interfaces/imetadataimport-findtypedefbyname-method) 定位定义，再用 SOS `GetMethodDescFromToken` 的 TypeDef 分支取已加载类型地址（见 [DAC 实现](https://github.com/dotnet/coreclr/blob/abbb8f685929c7aeaa087dae46fedc1bc2af4b17/src/debug/daccess/request.cpp)）。这是对故障传播路径的回归检查，不是用户机器上原始 DAC 失败的复现。

VEH 验证入口使用独立的 x86 测试进程；另有 `--stress-live-items` 对运行中的游戏测试旧 flag。常驻原生 VEH 在 [Native/VehJoin.h](Native/VehJoin.h)，只识别已登记地址、线程和执行状态，修改异常上下文的 EIP 后发布完成，再返回 `EXCEPTION_CONTINUE_EXECUTION`。

以下原型与回归测试均使用明确的命令行参数。

## 构建

需要 Visual Studio 2022 C++ 工具、.NET 8 x86、.NET Framework 4.8 和项目现有的 Terraria/XNA 引用。在仓库根目录的 VS Developer PowerShell 执行：

```powershell
# 首次使用先构建现有依赖。以后只改原型时可以跳过这一行。
msbuild QTRHacker.sln /restore /t:Build /p:Configuration=Release /p:Platform=x86 /m

msbuild src/QTRHacker.Functions.Test/Native/VehJoinTarget.vcxproj /p:Configuration=Release /p:Platform=Win32
msbuild src/QTRHacker.Functions.Test/Native/VehItemBridge.vcxproj /p:Configuration=Release /p:Platform=Win32
msbuild src/QTRHacker.Functions.Test/ItemTarget/VehItemTarget.csproj /restore /p:Configuration=Release
msbuild src/QTRHacker.Functions.Test/QTRHacker.Functions.Test.csproj /p:Configuration=Release /p:BuildProjectReferences=false /p:SolutionDir="$((Get-Location).Path)/"

$runner = 'C:/Program Files (x86)/dotnet/dotnet.exe'
$game = 'C:/Program Files (x86)/Steam/steamapps/common/Terraria/Terraria.exe'
```

原生项目与 ItemTarget 单独构建，不加入正式入口或主解决方案的构建依赖。

## 确定性回收验证

```powershell
& $runner bin/Release/QTRHacker.Functions.Test.dll --verify-veh-join
```

- 旧 flag 对照：发布完成后在临时代码内停留，控制端释放代码，要求子进程以 `0xC0000005` 退出。
- VEH：分别验证函数返回与搬迁函数序言后跳回原函数，两种地址分配（含 2 GB 以上）、强制窗口与普通轮询，共 320 次。
- 强制窗口中，VEH 发布完成后暂留在常驻处理器；控制端先 `VirtualFreeEx`，确认 `MEM_FREE`，再将原地址分配为 `PAGE_NOACCESS`，最后让处理器返回。返回路径仍正确，才算通过。
- 检查通用寄存器、ESP、算术/方向标志、x87 控制/状态及 ST(0)、MXCSR、XMM0–XMM7；检查其他异常及错误线程的断点仍到达 SEH。
- 另建控制进程，强制结束它，要求目标在控制端死亡后独立执行并回收 64 次请求。

处理器内的 `gate` 自旋**仅用于人为固定测试时序**。正常调用设为 1，不能把等待控制端的门闩带进正式实现。

## 真实 Item.SetDefaults A/B

```powershell
& $runner bin/Release/QTRHacker.Functions.Test.dll --verify-veh-items `
  bin/veh-join/Release/VehItemTarget.exe $game 20000 3
```

独立 .NET Framework 4.8 进程加载指定 Terraria 的真实游戏程序集，初始化物品数据，在 CLR 线程上反复调用一个可 Hook 的托管 `Tick`。

- A：使用当前 `InlineHook`，原样执行 `WaitToDetach` / `WaitToDispose`。
- B：同样生成原来的整个 trampoline，仅将末尾的清零 flag / JMP 改为 `int3`；常驻 VEH 恢复到相同的原函数继续位置，控制端按 VEH 完成记录释放代码。
- 两组调用相同的 `HackMethodCall` / x86 参数生成器，真实调用 `Terraria.Item.SetDefaults(Int32, ItemVariant)`，轮换 5000 个输入。目标检查 `Tick` 返回值及物品 type/damage 与直接托管调用的基准一致；旧物品 ID 可能映射到其他类型，因此不能直接断言 type 等于输入 ID。
- 请求只在安装完成后放行。确认 `Tick` 已返回发生在**释放代码之后**，用于防止下一次执行或复用元数据。它不参与判断本次何时可以释放代码。
- 普通组与每 32 次调用显式 GC 的组分别运行，逐次重新读取物品引用，避免把跨 GC 保存对象地址的问题混进回收对比。每轮交换 A/B 顺序。

`20000 3` 表示每个组合 2 万次、重复 3 轮；A、B 各计 12 万次。报告写到运行器旁的 `veh-items-results.json`，包括完成数、异常、最后一块代码地址、退出码。记录的时间包含启动和构建 trampoline 的开销，不是 VEH 性能基准。

另有真实物品调用的确定性窗口对照：

```powershell
& $runner bin/Release/QTRHacker.Functions.Test.dll --verify-veh-items-window `
  bin/veh-join/Release/VehItemTarget.exe $game
```

两组完成同一个真实 `SetDefaults` 后，人为停在发布完成与恢复执行之间，先释放再恢复。A 的临时代码内增加测试门闩，预期访问冲突；B 的常驻 VEH 内增加门闩，预期恢复成功。报告为 `veh-items-window-results.json`。这证明特定回收窗口，不能把故意延长窗口的失败说成未修改旧版本在自然运行中崩溃。

## 运行中游戏的旧 flag 压力测试

```powershell
# PID 必须是已经进入世界的 Terraria，以下示例的 PID 需按实际进程替换。
& $runner bin/Release/QTRHacker.Functions.Test.dll --stress-live-items `
  54812 bin/veh-join/Release/VehItemTarget.exe 3000
```

循环位于修改器进程中的 `LiveItemChecks.Run`：每轮独立创建原来的 `InlineHook`，Hook 真实 `Terraria.Main.Update`，调用一次真实 `Item.SetDefaults`，然后原样 `WaitToDetach` / `WaitToDispose`。请求之间没有主动延时，也没有在游戏内注入循环。实际请求频率受游戏更新频率限制。

测试使用 `LiveItemProbe.Scratch` 私有物品，交替设为类型 1 和 3063。游戏内的辅助类型只保存该物品引用并提供初始化/清理；测试循环直接使用现有实例方法参数生成器调用 `Terraria.Item.SetDefaults`。每轮重新读取对象地址，保留现有实现从取地址到真正执行之间的时序。调用后的类型差异和前后对象地址会记录，但不因该差异提前停止压力测试。

辅助程序集通常由现有 `GameContext.LoadAssemblyAsBytes` 加载。如果实机的 DAC 类型遍历使该加载步骤失败，可在已经连接到目标游戏的 CE 中用 `inject_dotnet_dll` 做一次初始化：程序集为 `bin/veh-join/Release/VehItemTarget.exe`，类型 `VehItemTarget.LiveItemProbe`，方法 `Initialize`，参数为空字符串。随后重新运行上述入口。这里的辅助类型没有 VEH；实机调用仍使用旧 flag。

默认不附加调试器，也不启动异常收集器。在开始、每 250 次调用后及结束时用 `CheckRemoteDebuggerPresent` 查询目标的调试状态并记入日志；如果已有调试器或测试中检查到状态改变，则停止测试。日志位于运行器旁的 `live-oldflag-<PID>-<时间>/`。

仅在末尾显式添加 `--watch` 时启动外部 `VehJoinTarget.exe --watch`，收集访问冲突的寄存器、故障地址及最多两份 minidump。该观察器会附加原生调试器，关闭“调试器退出时终止目标”，消费附加时的初始断点，其余异常继续交给游戏处理。调试事件会影响目标的运行时序，因此该模式的结果需要与不附加调试器的测试分开记录。

在该日志目录创建 `stop.request` 可要求测试在本次调用完成后停止。测试结束释放私有物品引用；若启用了观察器，则将其脱离，不终止游戏。对于已经卡在旧 flag 等待中的调用，这个文件不会强制释放或取消它。

## 适用范围

原型假设单一执行者、入口安全关闭、没有遗留回调/返回地址再进入已释放块，并让处理器和记录保持常驻。它不解决入口补丁并发写入、同一 Hook 重入、跨调用对象指针失效、JIT/GC 栈信息等其他问题。

独立物品测试没有运行完整世界、渲染、网络或其他游戏线程。普通 A/B 如果都通过，只能报告本次未复现历史崩溃，不能据此认定旧 flag 安全或真实游戏的问题已修复。

## 本次验证结果

使用本机 Terraria **1.4.5.8**、x86 .NET Framework 4.8，在独立进程中验证：

| 用例 | 结果 |
| --- | --- |
| 普通高频物品 A/B（含显式 GC，3 轮） | 旧 flag、VEH 各完成 120,000 次，均未自然崩溃 |
| 强制窗口中的真实物品调用 | 旧 flag 在已释放的代码页内触发 `0xC0000005`，VEH 正常恢复 |
| 原生回收/现场保持测试 | Debug、Release 均通过，每次 320 次回收 |
| 控制端强制退出 | Debug、Release 均通过，目标在控制端退出后各完成 64 次调用 |

这验证了指定前提下的回收窗口修复；历史游戏崩溃原因仍未确定。

2026-09-14 对已进入单人世界的 Terraria（PID 54812），在修改器端逐次调用真实 `Item.SetDefaults`：

| 条件 | 完成数 | 耗时 | 游戏崩溃 | 调用后类型不符 |
| --- | ---: | ---: | --- | ---: |
| 附加原生异常收集器，第一轮 | 3,000 | 69.7 秒 | 未发生 | 2 |
| 附加原生异常收集器，第二轮 | 10,000 | 229.3 秒 | 未发生 | 2 |
| 收集器已退出，不附加调试器 | 10,000 | 223.2 秒 | 未发生 | 2 |

最后一轮使用同一个游戏进程，在先前观察器脱离后开始；不是从未被调试过的新进程。开始、每 250 次调用后及结束时的系统查询均为 `debuggerAttached=False`，CE 自身也未启用调试器。测试后游戏仍响应，`Main.Update` 的 32 个入口字节与测试前一致，本轮使用的 `0x3E8A0000`、`0x3E8E0000` 代码页均处于 `MEM_FREE`。日志及结束状态记录位于 `bin/Release/live-oldflag-54812-20260914-041417/`。

无调试器轮次中的两次类型不符分别出现在第 2、29 次调用，前后物品地址均发生变化。这个现象与 GC 移动物品后，控制端传入的地址过期相符，但还没有用 GC 事件或受控对照确认因果。它与代码页回收属于不同问题，VEH join 本身不能保证实例地址有效。这些实机结果仍未复现历史上的自然崩溃，也不能证明旧 flag 安全或调试器与崩溃概率无关。

接口语义参考：[VEH 回调](https://learn.microsoft.com/en-us/windows/win32/api/winnt/nc-winnt-pvectored_exception_handler)、[注册处理器及 DLL 生命周期](https://learn.microsoft.com/en-us/windows/win32/api/errhandlingapi/nf-errhandlingapi-addvectoredexceptionhandler)。

调试状态与时序参考：[CheckRemoteDebuggerPresent](https://learn.microsoft.com/en-us/windows/win32/api/debugapi/nf-debugapi-checkremotedebuggerpresent)、[调试事件发生时暂停目标线程](https://learn.microsoft.com/en-us/windows/win32/debug/debugging-events)。对象搬迁语义参考：[GC 的重定位与压缩阶段](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals#what-happens-during-a-garbage-collection)。
