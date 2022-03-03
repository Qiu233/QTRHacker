





#pragma warning disable
using QHackLib;
using QTRHacker.Core.GameObjects.ValueTypeRedefs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects.Terraria
{
	partial class Item
	{
#region Virtual Properties
		public virtual ValueTypeRedefs.Xna.Color Color { get => InternalObject.color; set => InternalObject.color = value; }
		public virtual bool Accessory { get => InternalObject.accessory; set => InternalObject.accessory = value; }
		public virtual bool AutoReuse { get => InternalObject.autoReuse; set => InternalObject.autoReuse = value; }
		public virtual bool BeingGrabbed { get => InternalObject.beingGrabbed; set => InternalObject.beingGrabbed = value; }
		public virtual bool Buy { get => InternalObject.buy; set => InternalObject.buy = value; }
		public virtual bool BuyOnce { get => InternalObject.buyOnce; set => InternalObject.buyOnce = value; }
		public virtual bool CanBePlacedInVanityRegardlessOfConditions { get => InternalObject.canBePlacedInVanityRegardlessOfConditions; set => InternalObject.canBePlacedInVanityRegardlessOfConditions = value; }
		public virtual bool CartTrack { get => InternalObject.cartTrack; set => InternalObject.cartTrack = value; }
		public virtual bool Channel { get => InternalObject.channel; set => InternalObject.channel = value; }
		public virtual bool Consumable { get => InternalObject.consumable; set => InternalObject.consumable = value; }
		public virtual bool DD2Summon { get => InternalObject.DD2Summon; set => InternalObject.DD2Summon = value; }
		public virtual bool Expert { get => InternalObject.expert; set => InternalObject.expert = value; }
		public virtual bool ExpertOnly { get => InternalObject.expertOnly; set => InternalObject.expertOnly = value; }
		public virtual bool Favorited { get => InternalObject.favorited; set => InternalObject.favorited = value; }
		public virtual bool Flame { get => InternalObject.flame; set => InternalObject.flame = value; }
		public virtual bool Instanced { get => InternalObject.instanced; set => InternalObject.instanced = value; }
		public virtual bool IsAShopItem { get => InternalObject.isAShopItem; set => InternalObject.isAShopItem = value; }
		public virtual bool Magic { get => InternalObject.magic; set => InternalObject.magic = value; }
		public virtual bool Material { get => InternalObject.material; set => InternalObject.material = value; }
		public virtual bool Mech { get => InternalObject.mech; set => InternalObject.mech = value; }
		public virtual bool Melee { get => InternalObject.melee; set => InternalObject.melee = value; }
		public virtual bool NewAndShiny { get => InternalObject.newAndShiny; set => InternalObject.newAndShiny = value; }
		public virtual bool NoMelee { get => InternalObject.noMelee; set => InternalObject.noMelee = value; }
		public virtual bool NotAmmo { get => InternalObject.notAmmo; set => InternalObject.notAmmo = value; }
		public virtual bool NoUseGraphic { get => InternalObject.noUseGraphic; set => InternalObject.noUseGraphic = value; }
		public virtual bool NoWet { get => InternalObject.noWet; set => InternalObject.noWet = value; }
		public virtual bool Potion { get => InternalObject.potion; set => InternalObject.potion = value; }
		public virtual bool QuestItem { get => InternalObject.questItem; set => InternalObject.questItem = value; }
		public virtual bool Ranged { get => InternalObject.ranged; set => InternalObject.ranged = value; }
		public virtual bool Sentry { get => InternalObject.sentry; set => InternalObject.sentry = value; }
		public virtual bool Social { get => InternalObject.social; set => InternalObject.social = value; }
		public virtual bool Summon { get => InternalObject.summon; set => InternalObject.summon = value; }
		public virtual bool UniqueStack { get => InternalObject.uniqueStack; set => InternalObject.uniqueStack = value; }
		public virtual bool UseTurn { get => InternalObject.useTurn; set => InternalObject.useTurn = value; }
		public virtual bool Vanity { get => InternalObject.vanity; set => InternalObject.vanity = value; }
		public virtual bool WornArmor { get => InternalObject.wornArmor; set => InternalObject.wornArmor = value; }
		public virtual byte Dye { get => InternalObject.dye; set => InternalObject.dye = value; }
		public virtual byte Paint { get => InternalObject.paint; set => InternalObject.paint = value; }
		public virtual byte Prefix { get => InternalObject.prefix; set => InternalObject.prefix = value; }
		public virtual short GlowMask { get => InternalObject.glowMask; set => InternalObject.glowMask = value; }
		public virtual short HairDye { get => InternalObject.hairDye; set => InternalObject.hairDye = value; }
		public virtual short MakeNPC { get => InternalObject.makeNPC; set => InternalObject.makeNPC = value; }
		public virtual int Alpha { get => InternalObject.alpha; set => InternalObject.alpha = value; }
		public virtual int Ammo { get => InternalObject.ammo; set => InternalObject.ammo = value; }
		public virtual int Axe { get => InternalObject.axe; set => InternalObject.axe = value; }
		public virtual int Bait { get => InternalObject.bait; set => InternalObject.bait = value; }
		public virtual int BodySlot { get => InternalObject.bodySlot; set => InternalObject.bodySlot = value; }
		public virtual int BuffTime { get => InternalObject.buffTime; set => InternalObject.buffTime = value; }
		public virtual int BuffType { get => InternalObject.buffType; set => InternalObject.buffType = value; }
		public virtual int CreateTile { get => InternalObject.createTile; set => InternalObject.createTile = value; }
		public virtual int CreateWall { get => InternalObject.createWall; set => InternalObject.createWall = value; }
		public virtual int Crit { get => InternalObject.crit; set => InternalObject.crit = value; }
		public virtual int Damage { get => InternalObject.damage; set => InternalObject.damage = value; }
		public virtual int Defense { get => InternalObject.defense; set => InternalObject.defense = value; }
		public virtual int DungeonPrice { get => InternalObject.dungeonPrice; set => InternalObject.dungeonPrice = value; }
		public virtual int EclipseMothronPrice { get => InternalObject.eclipseMothronPrice; set => InternalObject.eclipseMothronPrice = value; }
		public virtual int EclipsePostPlanteraPrice { get => InternalObject.eclipsePostPlanteraPrice; set => InternalObject.eclipsePostPlanteraPrice = value; }
		public virtual int EclipsePrice { get => InternalObject.eclipsePrice; set => InternalObject.eclipsePrice = value; }
		public virtual int FishingPole { get => InternalObject.fishingPole; set => InternalObject.fishingPole = value; }
		public virtual int Hammer { get => InternalObject.hammer; set => InternalObject.hammer = value; }
		public virtual int HeadSlot { get => InternalObject.headSlot; set => InternalObject.headSlot = value; }
		public virtual int HealLife { get => InternalObject.healLife; set => InternalObject.healLife = value; }
		public virtual int HealMana { get => InternalObject.healMana; set => InternalObject.healMana = value; }
		public virtual int HellPrice { get => InternalObject.hellPrice; set => InternalObject.hellPrice = value; }
		public virtual int HoldStyle { get => InternalObject.holdStyle; set => InternalObject.holdStyle = value; }
		public virtual int KeepTime { get => InternalObject.keepTime; set => InternalObject.keepTime = value; }
		public virtual int LegSlot { get => InternalObject.legSlot; set => InternalObject.legSlot = value; }
		public virtual int LifeRegen { get => InternalObject.lifeRegen; set => InternalObject.lifeRegen = value; }
		public virtual int Mana { get => InternalObject.mana; set => InternalObject.mana = value; }
		public virtual int ManaIncrease { get => InternalObject.manaIncrease; set => InternalObject.manaIncrease = value; }
		public virtual int MaxStack { get => InternalObject.maxStack; set => InternalObject.maxStack = value; }
		public virtual int MountType { get => InternalObject.mountType; set => InternalObject.mountType = value; }
		public virtual int NetID { get => InternalObject.netID; set => InternalObject.netID = value; }
		public virtual int NoGrabDelay { get => InternalObject.noGrabDelay; set => InternalObject.noGrabDelay = value; }
		public virtual int OwnIgnore { get => InternalObject.ownIgnore; set => InternalObject.ownIgnore = value; }
		public virtual int OwnTime { get => InternalObject.ownTime; set => InternalObject.ownTime = value; }
		public virtual int Pick { get => InternalObject.pick; set => InternalObject.pick = value; }
		public virtual int PlaceStyle { get => InternalObject.placeStyle; set => InternalObject.placeStyle = value; }
		public virtual int PlayerIndexTheItemIsReservedFor { get => InternalObject.playerIndexTheItemIsReservedFor; set => InternalObject.playerIndexTheItemIsReservedFor = value; }
		public virtual int QueenBeePrice { get => InternalObject.queenBeePrice; set => InternalObject.queenBeePrice = value; }
		public virtual int Rare { get => InternalObject.rare; set => InternalObject.rare = value; }
		public virtual int ReuseDelay { get => InternalObject.reuseDelay; set => InternalObject.reuseDelay = value; }
		public virtual int ShadowOrbPrice { get => InternalObject.shadowOrbPrice; set => InternalObject.shadowOrbPrice = value; }
		public virtual int Shoot { get => InternalObject.shoot; set => InternalObject.shoot = value; }
		public virtual int ShopSpecialCurrency { get => InternalObject.shopSpecialCurrency; set => InternalObject.shopSpecialCurrency = value; }
		public virtual int Stack { get => InternalObject.stack; set => InternalObject.stack = value; }
		public virtual int StringColor { get => InternalObject.stringColor; set => InternalObject.stringColor = value; }
		public virtual int TileBoost { get => InternalObject.tileBoost; set => InternalObject.tileBoost = value; }
		public virtual int TileWand { get => InternalObject.tileWand; set => InternalObject.tileWand = value; }
		public virtual int TimeLeftInWhichTheItemCannotBeTakenByEnemies { get => InternalObject.timeLeftInWhichTheItemCannotBeTakenByEnemies; set => InternalObject.timeLeftInWhichTheItemCannotBeTakenByEnemies = value; }
		public virtual int TimeSinceItemSpawned { get => InternalObject.timeSinceItemSpawned; set => InternalObject.timeSinceItemSpawned = value; }
		public virtual int TimeSinceTheItemHasBeenReservedForSomeone { get => InternalObject.timeSinceTheItemHasBeenReservedForSomeone; set => InternalObject.timeSinceTheItemHasBeenReservedForSomeone = value; }
		public virtual int TooltipContext { get => InternalObject.tooltipContext; set => InternalObject.tooltipContext = value; }
		public virtual int Type { get => InternalObject.type; set => InternalObject.type = value; }
		public virtual int UseAmmo { get => InternalObject.useAmmo; set => InternalObject.useAmmo = value; }
		public virtual int UseAnimation { get => InternalObject.useAnimation; set => InternalObject.useAnimation = value; }
		public virtual int UseStyle { get => InternalObject.useStyle; set => InternalObject.useStyle = value; }
		public virtual int UseTime { get => InternalObject.useTime; set => InternalObject.useTime = value; }
		public virtual int Value { get => InternalObject.value; set => InternalObject.value = value; }
		public virtual System.Nullable<System.Int32> ShopCustomPrice { get => InternalObject.shopCustomPrice; set => InternalObject.shopCustomPrice = value; }
		public virtual sbyte BackSlot { get => InternalObject.backSlot; set => InternalObject.backSlot = value; }
		public virtual sbyte BalloonSlot { get => InternalObject.balloonSlot; set => InternalObject.balloonSlot = value; }
		public virtual sbyte BeardSlot { get => InternalObject.beardSlot; set => InternalObject.beardSlot = value; }
		public virtual sbyte FaceSlot { get => InternalObject.faceSlot; set => InternalObject.faceSlot = value; }
		public virtual sbyte FrontSlot { get => InternalObject.frontSlot; set => InternalObject.frontSlot = value; }
		public virtual sbyte HandOffSlot { get => InternalObject.handOffSlot; set => InternalObject.handOffSlot = value; }
		public virtual sbyte HandOnSlot { get => InternalObject.handOnSlot; set => InternalObject.handOnSlot = value; }
		public virtual sbyte NeckSlot { get => InternalObject.neckSlot; set => InternalObject.neckSlot = value; }
		public virtual sbyte ShieldSlot { get => InternalObject.shieldSlot; set => InternalObject.shieldSlot = value; }
		public virtual sbyte ShoeSlot { get => InternalObject.shoeSlot; set => InternalObject.shoeSlot = value; }
		public virtual sbyte WaistSlot { get => InternalObject.waistSlot; set => InternalObject.waistSlot = value; }
		public virtual sbyte WingSlot { get => InternalObject.wingSlot; set => InternalObject.wingSlot = value; }
		public virtual float KnockBack { get => InternalObject.knockBack; set => InternalObject.knockBack = value; }
		public virtual float Scale { get => InternalObject.scale; set => InternalObject.scale = value; }
		public virtual float ShootSpeed { get => InternalObject.shootSpeed; set => InternalObject.shootSpeed = value; }
		public virtual string NameOverride { get => InternalObject._nameOverride; set => InternalObject._nameOverride = value; }
		public virtual string BestiaryNotes { get => InternalObject.BestiaryNotes; set => InternalObject.BestiaryNotes = value; }
		[Obsolete] public virtual GameObject UseSound { get => InternalObject.UseSound; set => InternalObject.UseSound = value; }
		[Obsolete] public virtual GameObject ToolTip { get => InternalObject.ToolTip; set => InternalObject.ToolTip = value; }
#endregion
	}
}