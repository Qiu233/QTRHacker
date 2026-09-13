# GameWikiResExporter

从 Terraria 导出 QTRHacker 使用的 wiki 数据、中英文本地化、物品和 NPC 图片。已在 Windows 上使用 Terraria 1.4.5.8 验证。

## 编译和运行

需要 .NET SDK、.NET Framework 4.8 开发包和 XNA Framework 4.0。Terraria 安装目录提供 `xnafx40_redist.msi`；XNA 引用默认取自系统 GAC，与游戏使用相同的 x86 运行环境。本项目可单独编译，不依赖 QTRHacker 或 C++ 项目。

在仓库根目录执行：

```powershell
dotnet build src/GameWikiResExporter/GameWikiResExporter.csproj -c Release
./src/GameWikiResExporter/bin/Release/net48/GameWikiResExporter.exe --game-dir "C:\Program Files (x86)\Steam\steamapps\common\Terraria"
```

默认输出到 **EXE 所在目录**（上述命令对应 `src/GameWikiResExporter/bin/Release/net48/`），与终端当前目录无关。程序不会自动替换项目资源。

| 参数 | 用途 |
| --- | --- |
| `--game-dir <目录>` | 从指定目录加载 `Terraria.exe`、内嵌依赖及 `Content/Images`。运行时不使用仓库中的 GameRefs。 |
| `--output <目录>` | 指定输出目录；相对路径按当前工作目录解析。 |
| `--no-images` | 只导出 wiki 数据和本地化，不读取 XNB 或创建图形设备。 |
| `--help` / `-h` | 显示用法。 |

无参数时，从 EXE 目录向上查找仓库的 `GameRefs/Terraria.exe`。若 GameRefs 没有 `Content/Images`，会明确提示仅导出数据和本地化。显式指定的游戏目录缺少图片时会报错，可加 `--no-images`。无需启动 Terraria。

编译时默认引用 GameRefs；也可通过 `-p:GameReferenceDirectory="C:\...\Terraria"` 改用安装目录的 `Terraria.exe`，再通过运行参数 `--game-dir` 指定同一目录。Newtonsoft.Json 的编译引用取自 GameRefs；运行时优先加载所选游戏内嵌的依赖版本。非默认的 XNA GAC 根目录可通过 `-p:XnaAssemblyDirectory=...` 指定。

## 输出格式

| 输出 | 项目中对应的资源 |
| --- | --- |
| `WikiRes.zip` | `src/QTRHacker/Assets/Game/WikiRes.zip` |
| `Localization.zip` | `src/QTRHacker/Assets/Game/Localization.zip` |
| `Items.bin` | `src/QTRHacker/Assets/GameImages/Items.bin` |
| `NPCs.bin` | `src/QTRHacker/Assets/GameImages/NPCs.bin` |
| `GameConstants.cs` | `src/QTRHacker.Core/GameConstants.cs` |

- `WikiRes.zip` 保留四张 ID 表，以及 `ItemInfo.json`、`NPCInfo.json`、`RecipeInfo.json`。物品和 NPC 数组涵盖从 0 到游戏 `Count - 1` 的每个 ID；已废弃物品会保留游戏 `SetDefaults` 的空物品/替代物品行为。物品 JSON 只写现有 `ItemData` 模型使用的字段。NPC 数据采用普通难度。
- 配方仅写游戏实际注册的条目。保留 `item`、`rItems`、`rTiles` 结构，过滤空材料，并把 1.4.5 的单个工作台 ID 转为列表；空工作台列表表示无需工作台。现有模型不包含配方组、环境等额外条件。
- `Localization.zip` 中的 `Content.en-US*.json`、`Content.zh-Hans*.json` **直接复制所选 Terraria.exe 的内嵌资源字节**，不生成翻译、不重新序列化 JSON。
- 图片从安装目录中的原始 XNB 转成完整 PNG，保留尺寸、透明度和 NPC 整张动画图。为每个物品解析 `ItemID.Sets.TextureCopyLoad`（含多级引用），并保留 `NPC_Head_*` 等同前缀图片。
- `.bin` 格式保持与 `BinLoader` 一致：依次写入 `BinaryWriter.Write(string)` 名称、`Int64` PNG 字节数、PNG 字节。
- `GameConstants.cs` 从游戏读取版本、物品/NPC 数量、玩家 buff 槽数量及完整 NPC 帧数表。更新资源时应同时手动复制这个文件，以免新增 NPC 的动画索引越界。它和其他输出一样，不会自动写入项目源码。

先在输出目录的临时子目录完成整次导出，成功后才替换该输出目录内的同名文件；导出失败返回非零退出码，不发布未完成的数据。ZIP 条目使用固定时间戳，便于重复导出比对。需要更新项目时，检查输出后手动复制所需文件。

## 实现来源

本地化和数据均来自本机游戏。XNB 转 PNG 沿用 [Athari/XnaConvert 的 XNA 4.0 实现](https://github.com/Athari/XnaConvert/tree/master/Alba.XnaConvert.Loader.Xna40) 使用的 `ContentManager` / `Texture2D.SaveAsPng` 方法，由 XNA 处理压缩及纹理格式。没有另写 XNB 解码器，也不需要安装独立转换工具。署名和许可见 [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md)。

导出器调用少量 Terraria 的内部数据初始化方法；升级到改变这些 API 的游戏版本时，需重新编译并调整初始化逻辑。
