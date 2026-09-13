这里的 `Item.ps.tt`、`NPC.ps.tt`、`Player.ps.tt` 引用 `TTHeader` 和对应的 `*Properties`，生成已纳入版本控制的 `*.ps.cs`。更新字段时应修改 `*Properties`，再重新运行 T4；直接修改 `*.ps.cs` 会在下次生成时丢失。

`GameDataExporter` 的 `Types/Terraria/*.tt` 是原始字段清单，不能直接覆盖 Core 的模板。同步时保留以下处理：

| 游戏字段类型 | Core 的处理 |
| --- | --- |
| 基础值类型 | `PROPERTY_VIRTUAL`，保持原始类型宽度 |
| XNA `Color`、`Point`、`Rectangle`、`Vector2` 和 `Terraria.BitsByte` | 使用 `ValueTypeRedefs` 中的对应类型 |
| `string` | `PROPERTY_GO_VIRTUAL("GameString", ...)`；字符串数组使用 `PROPERTY_ARRAY_VIRTUAL("GameString", ...)` |
| 已包装的 `Chest`、`Item`、`Mount` | 引用字段使用 `PROPERTY_GO_VIRTUAL`，不能使用普通值类型宏 |
| `Item[]`、`EquipmentLoadout[]` 等对象数组 | `PROPERTY_ARRAY_VIRTUAL` |
| 值类型数组 | 按维数使用 `PROPERTY_ARRAYV_VIRTUAL`、`PROPERTY_ARRAY2DV_VIRTUAL` 或 `PROPERTY_ARRAYMDV_VIRTUAL` |

保留已有的属性命名、三参数宏中的别名及手写方法。导出清单中的泛型集合、`Nullable<T>`、未包装的类/结构体、枚举和 `<...>k__BackingField` 不应直接加入模板：需要先实现相应的远程访问/布局处理。尤其不能把引用类型当成值类型读写，也不能把可空结构体当成指针传参。`Entity` 的继承字段由基类封装，导出的 `.txt` 可用来检查继承关系和完整 CLR 方法签名。

本次对照 Terraria 1.4.5.8 删除的旧字段：

- `Player`：`controlQuickBuff`、`releaseQuickBuff`、`manaSickLessDmg`、`maxRegenDelay`、`setBonus`。
- `NPC`：`ignorePlayerInteractions`、`lifeRegenExpectedLossPerSecond`。

同时加入当前版本可直接包装的 77 个新字段。原来的字符串、对象引用转换全部保留。`Chest.cs` 中已注释掉的 `chestItemSpawn`、`chestItemSpawn2`、`dresserItemSpawn` 仍不能启用。

1.4.5 的 `Item` 已不再继承 `Entity`，因此 Core 也改为继承 `GameObject`。位置、速度等属于新的 `WorldItem`，不能通过背包里的 `Item` 读取。`Item.NewItem` 的旧 `noGrabDelay` 参数已换成 `NewItemOwnership`，可选速度按 12 字节 `Nullable<Vector2>` 传递；需要联机同步时使用 `Item.RequestNewItem`，由游戏处理同步和物品归属。

在 Visual Studio 中对三个 `.ps.tt` 执行“运行自定义工具”，或使用随 Visual Studio 安装的 `TextTransform.exe`：

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe' src/QTRHacker.Core/GameObjects/Terraria/Item.ps.tt
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe' src/QTRHacker.Core/GameObjects/Terraria/NPC.ps.tt
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe' src/QTRHacker.Core/GameObjects/Terraria/Player.ps.tt
```

编译解决方案后，启动 Terraria，在仓库根目录运行下面的只读检查。它核对字段、T4 类型、包装类继承和代码中声明的游戏方法签名；不会执行默认测试入口里的物品修改，也不安装游戏补丁：

```powershell
& 'C:\Program Files (x86)\dotnet\dotnet.exe' bin/Debug/QTRHacker.Functions.Test.dll --verify-game-compatibility .
```

该检查不覆盖机器码特征搜索、调用约定和实际游戏操作；这些仍需针对改动验证。
