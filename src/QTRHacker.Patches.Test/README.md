# 游戏功能补丁回归检查

当前以 `gamerefs/Terraria.exe`（1.4.5.8）为基准。测试在独立的 x86 / .NET Framework 进程中初始化游戏数据，使用临时玩家、地图、物品和弹幕；不连接已打开的游戏，不读取或保存玩家存档。

从仓库根目录，在 Visual Studio Developer PowerShell 中运行：

```powershell
msbuild src/QTRHacker.Patches.Test/QTRHacker.Patches.Test.csproj /restore /p:Configuration=Debug /p:Platform=x86
& src/QTRHacker.Patches.Test/bin/x86/Debug/net48/QTRHacker.Patches.Test.exe
```

可将另一份游戏 EXE 的完整路径作为测试程序的第一个参数。需要 XNA 4 和 .NET Framework 4.8；使用 Visual Studio 的完整 MSBuild，`dotnet build` 无法加载当前的 ILRepack 构建任务。该项目独立构建，不在主解决方案中。

测试覆盖：

- 合并后的补丁 DLL 中，Harmony 特性和目标方法能否解析，语义 IL 匹配数量是否正确。
- 与游戏一致的 x86 大地址空间：先编译低地址的 `Projectile.AI`，暂时保留低地址空闲空间，迫使补丁 JIT 到 2 GB 以上，并验证跨界跳转及 GC 后的实际执行。
- 16 项功能开关、玩家隔离、关闭后的属性恢复，以及非法功能编号的错误回传。
- 生命伤害拦截、零魔力施法、氧气和飞行时间补充、移动及建筑属性。
- 旅行菜单的难度判断（不改变角色难度）、照明缓冲、钓鱼箱子概率。
- 金币洞实际生成月亮领主宝袋、增强吸血飞刀实际生成至少 100 枚弹幕；关闭后恢复金币和普通飞刀数量。

测试不创建图形设备，因此完整 `Boot` 初始化、实际画面、建筑操作和外部进程注入仍需实机验证。

## AOB 扫描及实机检查

先从仓库根目录构建主解决方案，再使用明确的测试参数：

```powershell
msbuild QTRHacker.sln /t:Build /p:Configuration=Debug /p:Platform=x86 /m
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-aobscan
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-game-compatibility .
```

`--verify-aobscan` 只扫描测试进程自己的内存，覆盖 RX/RWX 区域、跨区域/分块匹配、最后一个匹配位置、重叠匹配、不可访问页和通配符解析。兼容性检查需要 Terraria 已进入世界。

下列命令通过修改器的常规加载和调用流程操作正在运行的游戏，首次开启时会注入补丁：

```powershell
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --gameplay-feature inspect
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --gameplay-feature verify-loader-buffer
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --gameplay-feature MachanicalRuler on
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --gameplay-feature MachanicalRuler off
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --gameplay-feature verify-toggle-cycle
```

`verify-toggle-cycle` 要求功能全部关闭，并模拟同时点击不同按钮：并发请求开启全部 16 项，检查位掩码，再关闭全部功能。它也可以用于验证首次加载时的并发请求。

`verify-loader-buffer` 分配临时远程内存，将完整补丁 DLL 写入后读回逐字节比较，随后释放内存，不执行注入。加载器会检查分配及写入结果，并将 DLL 缓冲保留到远程调用结束；界面等待超时后再次请求会复用正在进行的加载。

功能名见 [GameplayFeature.cs](../QTRHacker.Core/GameplayFeature.cs)。游戏因失去焦点而暂停时，部分属性要等恢复运行后的下一帧才会更新。替换已注入的 DLL 后，需要重启游戏来测试新的加载流程。不要省略测试参数：`QTRHacker.Functions.Test` 的默认入口会修改背包物品。

## 后续游戏更新

原来的 16 项 AOB 内置功能现在共用 `GameplayFeatureFunction`。`PatchesManager` 写入请求参数并发布请求编号，游戏已有的托管更新回调再执行 `GameplayPatches.SetEnabled` 并回写完成编号。补丁根据托管方法、字段和 IL 常量定位，不依赖 JIT 选择的寄存器或原生字节码。原有脚本的 AOB API 仍保留。

补丁使用 NuGet 固定版本 `Lib.Harmony 2.4.2`，合并到 DLL 中。仓库原有的 `refdlls/0Harmony.dll` 不再参与构建；其旧版 MonoMod 在 x86 地址跨过 2 GB 时可能选择 x64 跳转，导致实际执行 `Projectile.AI` 时访问冲突。高地址回归用例用于覆盖这一问题。

IL 补丁检查预期匹配数量；安装失败时回滚本组 Harmony 补丁，并将异常回传给修改器，只有操作成功才更新界面开关。游戏更新后应先运行本测试，再验证实机效果；托管方法签名和游戏逻辑发生变化时仍需适配。
