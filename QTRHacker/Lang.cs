/*
 * 由SharpDevelop创建。
 * 用户： lopi2
 * 日期: 2017/4/14
 * 时间: 23:19
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of Lang.
	/// </summary>
	public class Lang
	{

#if ENG
		public static string infLife = "Infinite Life", infOxygen = "Infinite Oxygen", infSummon = "Infinite summon", infMana = "Infinite Mana", infItemAndAmmo = "Infinite Item and Ammo", superRange = "Super range";
		public static string infFly = "Infinite Fly", immuneStoned = "Immunise Debuffs", highLight = "High Light", ghostMode = "Ghost Mode", respawnAtOnce = "Respawn without Waiting", goldHoleDropBag = "Gold Hole Drop Treasure Bag", slimeGunBurn = "Slime Gun burns NPC";
		public static string attackThroughWalls = "Attack Ignoring Blocks", decreaseGravity = "Decrease Gravity", increaseSpeed = "Increase Movement Speed", killAllNPC = "Kill All NPC", projectileThroughWalls = "Projectiles Through Walls", superPick = "Pick Far Away", extraTwoSlots = "Another Two Slots";
		public static string toggleDay = "Toggle Day/Night", toggleSunDial = "Toggle Sundial", toggleBloodMoon = "Toggle Blood Moon", toggleEclipse = "Toggle Eclipse";
		public static string none = "None", Event = "Event", nonePlayerBase = "No player base address found", extra = "Extra", dragTip = "Drag the cross to Terraria window", off = "OFF";
		public static string sucToHack = "Success to Hack", faiToHack = "Failed to Hack", sucToCancel = "Success to Cancel", faiToCancel = "Failed to Cancel", faiToGetBase = "Failed to get base address", baseaddr = "BaseAddr";
		public static string maxLife = "Max Life", maxMana = "Max Mana", confirm = "Confirm";
		public static string slot = "Slot", itemID = "ItemID", damage = "Damage", number = "Number", knockBack = "Knockback", crit = "Crit", buff = "Buff", buffTime = "BuffTime";
		public static string manaInc = "ManaIncrease", lifeIncrease = "LifeIncrease", useCD = "Using CD", waveCD = "Waving CD", effCD = "Effect CD", scale = "Scale", defense = "Defense", projSpeed = "Proj speed";
		public static string projID = "Proj type", dig = "Dig", hag = "Hag", hammer = "Hammer", digRange = "Dig range", tileID = "Tile ID", prefix = "Prefix", autoReuse = "Auto Reuse", equippable = "Equippable";
		public static string refresh = "Refresh", confirmHack = "Confirm", addBuff = "Add Buff", addPet = "Add Pet", setMount = "Set Mount", more = "More";
		public static string addBuffWnd = "Add Buff", wndBuffID = "Buff ID", wndBuffTime = "Buff Time", addPetWnd = "Add Pet", setMountWnd = "Set Mount";
		public static string telePoint = "TP Point", descr = "Description", teleport = "TP", teleMessage = "Are you sure to teleport to", addText = "Add", nameText = "Name", exists = "is exists", delete = "Delete", deleteMessage = "Are you sure to delete";
		public static string rename = "Rename", newName = "New Name", checkInv = "Check Inventory", checkBuff = "Check Buffs", player = "Player", currentLife = "Current Life";
		public static string seniorMessage = "Only teleport point is available because of single play";
		public static string regTip = "Please enter the code to register", regWrong = "Unavailable code", sucToReg = "Successful";
		public static string builder = "Builder", fastTileSpeed = "Very fast tiling speed", rulerEffect = "Ruler effect", machinicalRulerEffect = "Machinical Ruler effect";
		public static string hackInv = "Inventory Editor";
		public static string eff = "Effect", infernoEffect = "Inferno potion effect(no damage)", shadowDodge = "Shadow dodge", script = "Script", showCircuit = "Machinical Glass effect";
		public static string fishOnlyCrates = "Fish only crates";
		public static string Wiki = "Wiki", fishingPower = "Fish Skill", baitPower = "BaitPower", noPotionDelay = "No Potion Delay", killAllScreen = "Weapon Waving Kills All On Screen";
		public static string placeStyle = "Data", allRecipe = "Enable all recipes", snowMoon = "Toggle Snow Moon", pumpkinMoon = "Toggle Pumpkin Moon", strengthen_Vampire_Knives = "Strengthen the Vampire Knives", burnAllNPC = "Burn all NPCs", randomUUID = "Random UUID";
		public static string burnAllPlayer = "Burn All Player(without TShock)", dropLava = "Drop Lava On Player(Wasted)", addItem = "Add Item", newNpc = "Summon NPC", save = "Save", load = "Load", init = "Initialize";
		public static string convertTo = "Convert to", blockAttacking = "Waving attack blocks";

#else
		public static string infLife="无限生命",infOxygen="无限氧气",infSummon="无限召唤",infMana="无限魔法",infItemAndAmmo="无限物品/弹药",superRange="超远距离";
		public static string infFly="无限飞",immuneStoned="免疫负面Buff",highLight="全地图高亮",ghostMode="幽灵模式",respawnAtOnce="复活无需等待",goldHoleDropBag="金洞掉落银财宝袋",slimeGunBurn="史莱姆枪燃烧NPC";
		public static string attackThroughWalls="穿墙攻击",decreaseGravity="减少重力",increaseSpeed="加快移动速度",killAllNPC="杀光NPC并且阻止生成",projectileThroughWalls="弹幕穿墙",superPick="远距离拾取",extraTwoSlots="额外两个饰品栏";
		public static string toggleDay="昼夜更替",toggleSunDial="开/关 日晷",toggleBloodMoon="开/关 血月",toggleEclipse="开/关 日食";
		public static string none="无",Event="事件",nonePlayerBase="当前无玩家基址",extra="扩展",dragTip="拖动左边准星至泰拉瑞亚窗口",off="关闭";
		public static string sucToHack="修改成功",faiToHack="修改失败",sucToCancel="关闭成功",faiToCancel="关闭失败",faiToGetBase="获取基址失败",baseaddr="基址";
		public static string maxLife="最大生命",maxMana="最大魔法",confirm="确定";
		public static string slot="物品格",itemID="物品ID",damage="攻击力",number="数量",knockBack="击退",crit="暴击",buff="Buff",buffTime="Buff时间";
		public static string manaInc="魔法恢复",lifeIncrease="生命恢复",useCD="使用CD",waveCD="挥动CD",effCD="特效CD",scale="缩放",defense="防御力",projSpeed="弹幕速度";
		public static string projID="弹幕ID",dig="挖掘能力",hag="砍伐能力",hammer="锤击能力",digRange="挖掘距离",tileID="放置ID",prefix="前缀",autoReuse="按住连续使用",equippable="可装备";
		public static string refresh="刷新",confirmHack="确定",addBuff="添加Buff",addPet="添加宠物",setMount="设置坐骑",senior="高级功能";
		public static string addBuffWnd="添加Buff",wndBuffID="Buff ID",wndBuffTime="Buff时间",addPetWnd="添加宠物",setMountWnd="设置坐骑", more = "高级功能";
		public static string telePoint="传送点",descr="描述",teleport="传送",teleMessage="确定要传送到",addText="添加",nameText="名称",exists="已存在",delete="删除",deleteMessage="确定要删除";
		public static string rename="重命名",newName="新名称",checkInv="查看背包",checkBuff="查看Buff",player="玩家",currentLife="当前生命值";
		public static string seniorMessage="单人模式下,只有传送点可用";
		public static string regTip="请输入注册码",regWrong="无效的注册码",sucToReg="注册成功";
		public static string builder="建筑师",fastTileSpeed="极快的放置方块速度",rulerEffect="标尺：显示玩家与玩家的相对坐标",machinicalRulerEffect="机械尺：在屏幕上面显示方块格子";
		public static string hackInv="修改背包";
		public static string eff="装逼",infernoEffect="地狱降临光环(无伤害)",shadowDodge="影分身",script="脚本",showCircuit="机械眼镜：在屏幕上显示电路";
		public static string fishOnlyCrates="只钓板条箱";
		public static string Wiki = "百科", fishingPower = "钓技", baitPower = "鱼饵力度", noPotionDelay = "药水无冷却", killAllScreen = "挥砍击杀屏幕上所有生物";
        public static string placeStyle = "特殊值", allRecipe = "全物品合成", snowMoon = "开/关 霜月", pumpkinMoon = "开/关 南瓜月", strengthen_Vampire_Knives = "加强吸血飞刀", burnAllNPC = "燃烧所有NPC", randomUUID = "随机UUID";
		public static string burnAllPlayer = "燃烧玩家(不支持TShock)",dropLava="给玩家倒岩浆(不可用)", addItem = "添加物品", newNpc = "召唤NPC", save = "保存", load = "加载", init = "初始化";
		public static string convertTo = "转换到", blockAttacking = "可攻击方块";
#endif
	}
}
