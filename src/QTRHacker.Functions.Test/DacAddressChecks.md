# QHackCLR 高地址回归

`CLRDATA_ADDRESS` 始终为 64 位，较小的目标指针会按符号扩展。例如 x86 地址 `0x80000000` 可以由 DAC 返回为 `0xFFFFFFFF80000000`。直接调用 `UIntPtr(UInt64)` 会在 x86 主机抛出溢出异常。接口契约见 [.NET 的 clrdata.idl](https://github.com/dotnet/runtime/blob/main/src/coreclr/inc/clrdata.idl#L29-L34)。

原有 `RuntimeBuilder.EnumerateModules` 在这种场景下会沿 `QHackContext.InitHelpers` 抛出 `Arithmetic operation resulted in an overflow`。已在独立 .NET Framework 进程复现与用户截图一致的异常和调用栈；没有取得反馈者的进程快照。

现在 DAC 返回的模块、类型、字段、静态存储和方法地址统一经 `GlobalHelpers.ToNativeAddress` 转换。x86 接受合法的零扩展和符号扩展形式，也保留 `-1` 方法地址哨兵；其他超出目标指针范围的值仍抛出包含原始地址的异常。64 位路径保留完整地址。

反方向也必须遵守同一契约：原有 `ClrEntity.NativeHandle` 和 `ClrObject` 把 x86 高地址零扩展后传回 DAC，部分接口会返回 `E_INVALIDARG`。现在统一用 `ToDacAddress` 符号扩展。独立测试进程中，旧代码对高地址对象调用 `GetObjectData` 返回 `0x80070057`，修改后通过。该复现不能直接认定为用户 `GetMethodTableName` 报错的原因；后者还需要原始调用栈及失败地址。

## 运行

先按项目现有方式构建 x86 Debug 或 Release，再运行：

```powershell
& 'C:/Program Files (x86)/dotnet/dotnet.exe' bin/Release/QTRHacker.Functions.Test.dll `
  --verify-clr-high-addresses src/QHackCLR.TestTarget/bin/Debug/net48/QHackCLR.TestTarget.exe
```

测试程序路径可替换为单独构建的 `src/QHackCLR.TestTarget/bin/x86/Release/net48/QHackCLR.TestTarget.exe`。

运行器复制测试 EXE，在副本上设置 `LARGE_ADDRESS_AWARE`，然后传入 `--high-addresses`。子进程只保留自身 2 GB 以下的空闲虚拟地址，不提交对应物理内存；从字节加载 512 份测试程序集并初始化静态字段、对象和 JIT 方法，使真实 CLR 的新模块和方法落入高地址。游戏无需启动。

检查覆盖：

- 附加及 Flush 后的模块和类型枚举。
- 类型所属模块、方法所属类型和原生 JIT 地址。
- 基元/引用静态字段、实例类型及实例字段值。
- 位于 2 GB 以上的真实托管数组：`GetObjectData`、长度以及首尾元素；测试进程额外分配并固定一个 32 MB 数组。
- 正确处理 2 GB 边界、零地址和 `-1`，拒绝不能表示为 x86 地址的 64 位数值。
- 传回 DAC 时恢复正确的符号扩展形式。
- 子进程正常退出。

Debug、Release 的高地址测试及原有 `--verify-clr` 回归均已通过。本次 Release 运行枚举到 513 个测试模块，其中 494 个模块、495 个类型及 197 个 JIT 方法位于 2 GB 以上；具体数量和地址会随进程布局变化。该测试验证地址转换修复，反馈者仍需用修复版确认原现场。

补充验证：反向地址转换修复后的 Release 回归中，数组地址为 `0x82DF1010`，附加及 Flush 后的对象查询与首尾元素读取均通过。`--verify-clr` 还检查了堆的内置类型、补丁加载使用的 `System.Byte` 查询，以及补丁初始化失败保留原始异常、重复调用不重复加载。
