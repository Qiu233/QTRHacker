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
	partial class Player
	{
#region Virtual Properties
		public virtual ValueTypeRedefs.Xna.Color EyeColor
{
	get => InternalObject.eyeColor;
	set => InternalObject.eyeColor = value;
}		public virtual ValueTypeRedefs.Xna.Color HairColor
{
	get => InternalObject.hairColor;
	set => InternalObject.hairColor = value;
}		public virtual ValueTypeRedefs.Xna.Color HairDyeColor
{
	get => InternalObject.hairDyeColor;
	set => InternalObject.hairDyeColor = value;
}		public virtual ValueTypeRedefs.Xna.Color PantsColor
{
	get => InternalObject.pantsColor;
	set => InternalObject.pantsColor = value;
}		public virtual ValueTypeRedefs.Xna.Color ShirtColor
{
	get => InternalObject.shirtColor;
	set => InternalObject.shirtColor = value;
}		public virtual ValueTypeRedefs.Xna.Color ShoeColor
{
	get => InternalObject.shoeColor;
	set => InternalObject.shoeColor = value;
}		public virtual ValueTypeRedefs.Xna.Color SkinColor
{
	get => InternalObject.skinColor;
	set => InternalObject.skinColor = value;
}		public virtual ValueTypeRedefs.Xna.Color UnderShirtColor
{
	get => InternalObject.underShirtColor;
	set => InternalObject.underShirtColor = value;
}		public virtual ValueTypeRedefs.Xna.Point InputMouseCoordsForward
{
	get => InternalObject._inputMouseCoordsForward;
	set => InternalObject._inputMouseCoordsForward = value;
}		public virtual ValueTypeRedefs.Xna.Point InputMouseCoordsSmartSelect
{
	get => InternalObject._inputMouseCoordsSmartSelect;
	set => InternalObject._inputMouseCoordsSmartSelect = value;
}		public virtual ValueTypeRedefs.Xna.Point MainMouseCoordsForward
{
	get => InternalObject._mainMouseCoordsForward;
	set => InternalObject._mainMouseCoordsForward = value;
}		public virtual ValueTypeRedefs.Xna.Point MainMouseCoordsSmartSelect
{
	get => InternalObject._mainMouseCoordsSmartSelect;
	set => InternalObject._mainMouseCoordsSmartSelect = value;
}		public virtual ValueTypeRedefs.Xna.Point TileTargetSmartSelect
{
	get => InternalObject._tileTargetSmartSelect;
	set => InternalObject._tileTargetSmartSelect = value;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Point> TentacleSpikesMax5
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Point>(Context, InternalObject._tentacleSpikesMax5);
	set => InternalObject._tentacleSpikesMax5 = value.InternalObject;
}		public virtual ValueTypeRedefs.Xna.Rectangle BodyFrame
{
	get => InternalObject.bodyFrame;
	set => InternalObject.bodyFrame = value;
}		public virtual ValueTypeRedefs.Xna.Rectangle HairFrame
{
	get => InternalObject.hairFrame;
	set => InternalObject.hairFrame = value;
}		public virtual ValueTypeRedefs.Xna.Rectangle HeadFrame
{
	get => InternalObject.headFrame;
	set => InternalObject.headFrame = value;
}		public virtual ValueTypeRedefs.Xna.Rectangle LegFrame
{
	get => InternalObject.legFrame;
	set => InternalObject.legFrame = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 NextTorchLuckCheckCenter
{
	get => InternalObject._nextTorchLuckCheckCenter;
	set => InternalObject._nextTorchLuckCheckCenter = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 BodyPosition
{
	get => InternalObject.bodyPosition;
	set => InternalObject.bodyPosition = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 BodyVelocity
{
	get => InternalObject.bodyVelocity;
	set => InternalObject.bodyVelocity = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 FullRotationOrigin
{
	get => InternalObject.fullRotationOrigin;
	set => InternalObject.fullRotationOrigin = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 HeadPosition
{
	get => InternalObject.headPosition;
	set => InternalObject.headPosition = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 HeadVelocity
{
	get => InternalObject.headVelocity;
	set => InternalObject.headVelocity = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 InstantMovementAccumulatedThisFrame
{
	get => InternalObject.instantMovementAccumulatedThisFrame;
	set => InternalObject.instantMovementAccumulatedThisFrame = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 ItemLocation
{
	get => InternalObject.itemLocation;
	set => InternalObject.itemLocation = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 LastBoost
{
	get => InternalObject.lastBoost;
	set => InternalObject.lastBoost = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 LastDeathPostion
{
	get => InternalObject.lastDeathPostion;
	set => InternalObject.lastDeathPostion = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 LegPosition
{
	get => InternalObject.legPosition;
	set => InternalObject.legPosition = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 LegVelocity
{
	get => InternalObject.legVelocity;
	set => InternalObject.legVelocity = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 MinionRestTargetPoint
{
	get => InternalObject.MinionRestTargetPoint;
	set => InternalObject.MinionRestTargetPoint = value;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> BeetlePos
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.beetlePos);
	set => InternalObject.beetlePos = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> BeetleVel
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.beetleVel);
	set => InternalObject.beetleVel = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> ItemFlamePos
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.itemFlamePos);
	set => InternalObject.itemFlamePos = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> ShadowOrigin
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.shadowOrigin);
	set => InternalObject.shadowOrigin = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> ShadowPos
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.shadowPos);
	set => InternalObject.shadowPos = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> SolarShieldPos
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.solarShieldPos);
	set => InternalObject.solarShieldPos = value.InternalObject;
}		public virtual GameObjectArrayV<ValueTypeRedefs.Xna.Vector2> SolarShieldVel
{
	get => new GameObjectArrayV<ValueTypeRedefs.Xna.Vector2>(Context, InternalObject.solarShieldVel);
	set => InternalObject.solarShieldVel = value.InternalObject;
}		public virtual bool BatbatCanHeal
{
	get => InternalObject._batbatCanHeal;
	set => InternalObject._batbatCanHeal = value;
}		public virtual bool ForceForwardCursor
{
	get => InternalObject._forceForwardCursor;
	set => InternalObject._forceForwardCursor = value;
}		public virtual bool ForceSmartSelectCursor
{
	get => InternalObject._forceSmartSelectCursor;
	set => InternalObject._forceSmartSelectCursor = value;
}		public virtual bool SpawnTentacleSpikes
{
	get => InternalObject._spawnTentacleSpikes;
	set => InternalObject._spawnTentacleSpikes = value;
}		public virtual bool AbigailMinion
{
	get => InternalObject.abigailMinion;
	set => InternalObject.abigailMinion = value;
}		public virtual bool AccCalendar
{
	get => InternalObject.accCalendar;
	set => InternalObject.accCalendar = value;
}		public virtual bool AccCritterGuide
{
	get => InternalObject.accCritterGuide;
	set => InternalObject.accCritterGuide = value;
}		public virtual bool AccDivingHelm
{
	get => InternalObject.accDivingHelm;
	set => InternalObject.accDivingHelm = value;
}		public virtual bool AccDreamCatcher
{
	get => InternalObject.accDreamCatcher;
	set => InternalObject.accDreamCatcher = value;
}		public virtual bool AccFishFinder
{
	get => InternalObject.accFishFinder;
	set => InternalObject.accFishFinder = value;
}		public virtual bool AccFishingLine
{
	get => InternalObject.accFishingLine;
	set => InternalObject.accFishingLine = value;
}		public virtual bool AccFlipper
{
	get => InternalObject.accFlipper;
	set => InternalObject.accFlipper = value;
}		public virtual bool AccJarOfSouls
{
	get => InternalObject.accJarOfSouls;
	set => InternalObject.accJarOfSouls = value;
}		public virtual bool AccLavaFishing
{
	get => InternalObject.accLavaFishing;
	set => InternalObject.accLavaFishing = value;
}		public virtual bool AccMerman
{
	get => InternalObject.accMerman;
	set => InternalObject.accMerman = value;
}		public virtual bool AccOreFinder
{
	get => InternalObject.accOreFinder;
	set => InternalObject.accOreFinder = value;
}		public virtual bool AccStopwatch
{
	get => InternalObject.accStopwatch;
	set => InternalObject.accStopwatch = value;
}		public virtual bool AccTackleBox
{
	get => InternalObject.accTackleBox;
	set => InternalObject.accTackleBox = value;
}		public virtual bool AccThirdEye
{
	get => InternalObject.accThirdEye;
	set => InternalObject.accThirdEye = value;
}		public virtual bool AccWeatherRadio
{
	get => InternalObject.accWeatherRadio;
	set => InternalObject.accWeatherRadio = value;
}		public virtual bool ActuationRodLock
{
	get => InternalObject.ActuationRodLock;
	set => InternalObject.ActuationRodLock = value;
}		public virtual bool ActuationRodLockSetting
{
	get => InternalObject.ActuationRodLockSetting;
	set => InternalObject.ActuationRodLockSetting = value;
}		public virtual bool AdjHoney
{
	get => InternalObject.adjHoney;
	set => InternalObject.adjHoney = value;
}		public virtual bool AdjLava
{
	get => InternalObject.adjLava;
	set => InternalObject.adjLava = value;
}		public virtual bool AdjWater
{
	get => InternalObject.adjWater;
	set => InternalObject.adjWater = value;
}		public virtual bool AlchemyTable
{
	get => InternalObject.alchemyTable;
	set => InternalObject.alchemyTable = value;
}		public virtual bool AmmoBox
{
	get => InternalObject.ammoBox;
	set => InternalObject.ammoBox = value;
}		public virtual bool AmmoCost75
{
	get => InternalObject.ammoCost75;
	set => InternalObject.ammoCost75 = value;
}		public virtual bool AmmoCost80
{
	get => InternalObject.ammoCost80;
	set => InternalObject.ammoCost80 = value;
}		public virtual bool AmmoPotion
{
	get => InternalObject.ammoPotion;
	set => InternalObject.ammoPotion = value;
}		public virtual bool Archery
{
	get => InternalObject.archery;
	set => InternalObject.archery = value;
}		public virtual bool ArcticDivingGear
{
	get => InternalObject.arcticDivingGear;
	set => InternalObject.arcticDivingGear = value;
}		public virtual bool ArmorEffectDrawOutlines
{
	get => InternalObject.armorEffectDrawOutlines;
	set => InternalObject.armorEffectDrawOutlines = value;
}		public virtual bool ArmorEffectDrawOutlinesForbidden
{
	get => InternalObject.armorEffectDrawOutlinesForbidden;
	set => InternalObject.armorEffectDrawOutlinesForbidden = value;
}		public virtual bool ArmorEffectDrawShadow
{
	get => InternalObject.armorEffectDrawShadow;
	set => InternalObject.armorEffectDrawShadow = value;
}		public virtual bool ArmorEffectDrawShadowBasilisk
{
	get => InternalObject.armorEffectDrawShadowBasilisk;
	set => InternalObject.armorEffectDrawShadowBasilisk = value;
}		public virtual bool ArmorEffectDrawShadowEOCShield
{
	get => InternalObject.armorEffectDrawShadowEOCShield;
	set => InternalObject.armorEffectDrawShadowEOCShield = value;
}		public virtual bool ArmorEffectDrawShadowLokis
{
	get => InternalObject.armorEffectDrawShadowLokis;
	set => InternalObject.armorEffectDrawShadowLokis = value;
}		public virtual bool ArmorEffectDrawShadowSubtle
{
	get => InternalObject.armorEffectDrawShadowSubtle;
	set => InternalObject.armorEffectDrawShadowSubtle = value;
}		public virtual bool AutoActuator
{
	get => InternalObject.autoActuator;
	set => InternalObject.autoActuator = value;
}		public virtual bool AutoJump
{
	get => InternalObject.autoJump;
	set => InternalObject.autoJump = value;
}		public virtual bool AutoPaint
{
	get => InternalObject.autoPaint;
	set => InternalObject.autoPaint = value;
}		public virtual bool AutoReuseGlove
{
	get => InternalObject.autoReuseGlove;
	set => InternalObject.autoReuseGlove = value;
}		public virtual bool BabyBird
{
	get => InternalObject.babyBird;
	set => InternalObject.babyBird = value;
}		public virtual bool BabyFaceMonster
{
	get => InternalObject.babyFaceMonster;
	set => InternalObject.babyFaceMonster = value;
}		public virtual bool BallistaPanic
{
	get => InternalObject.ballistaPanic;
	set => InternalObject.ballistaPanic = value;
}		public virtual bool BatsOfLight
{
	get => InternalObject.batsOfLight;
	set => InternalObject.batsOfLight = value;
}		public virtual bool BeetleBuff
{
	get => InternalObject.beetleBuff;
	set => InternalObject.beetleBuff = value;
}		public virtual bool BeetleDefense
{
	get => InternalObject.beetleDefense;
	set => InternalObject.beetleDefense = value;
}		public virtual bool BeetleOffense
{
	get => InternalObject.beetleOffense;
	set => InternalObject.beetleOffense = value;
}		public virtual bool BehindBackWall
{
	get => InternalObject.behindBackWall;
	set => InternalObject.behindBackWall = value;
}		public virtual bool BlackBelt
{
	get => InternalObject.blackBelt;
	set => InternalObject.blackBelt = value;
}		public virtual bool BlackCat
{
	get => InternalObject.blackCat;
	set => InternalObject.blackCat = value;
}		public virtual bool Blackout
{
	get => InternalObject.blackout;
	set => InternalObject.blackout = value;
}		public virtual bool Bleed
{
	get => InternalObject.bleed;
	set => InternalObject.bleed = value;
}		public virtual bool Blind
{
	get => InternalObject.blind;
	set => InternalObject.blind = value;
}		public virtual bool BlueFairy
{
	get => InternalObject.blueFairy;
	set => InternalObject.blueFairy = value;
}		public virtual bool BoneArmor
{
	get => InternalObject.boneArmor;
	set => InternalObject.boneArmor = value;
}		public virtual bool BotherWithUnaimedMinecartTracks
{
	get => InternalObject.botherWithUnaimedMinecartTracks;
	set => InternalObject.botherWithUnaimedMinecartTracks = value;
}		public virtual bool BrokenArmor
{
	get => InternalObject.brokenArmor;
	set => InternalObject.brokenArmor = value;
}		public virtual bool Bunny
{
	get => InternalObject.bunny;
	set => InternalObject.bunny = value;
}		public virtual bool Burned
{
	get => InternalObject.burned;
	set => InternalObject.burned = value;
}		public virtual bool CactusThorns
{
	get => InternalObject.cactusThorns;
	set => InternalObject.cactusThorns = value;
}		public virtual bool Calmed
{
	get => InternalObject.calmed;
	set => InternalObject.calmed = value;
}		public virtual bool CanCarpet
{
	get => InternalObject.canCarpet;
	set => InternalObject.canCarpet = value;
}		public virtual bool CanFloatInWater
{
	get => InternalObject.canFloatInWater;
	set => InternalObject.canFloatInWater = value;
}		public virtual bool CanJumpAgain_Basilisk
{
	get => InternalObject.canJumpAgain_Basilisk;
	set => InternalObject.canJumpAgain_Basilisk = value;
}		public virtual bool CanJumpAgain_Blizzard
{
	get => InternalObject.canJumpAgain_Blizzard;
	set => InternalObject.canJumpAgain_Blizzard = value;
}		public virtual bool CanJumpAgain_Cloud
{
	get => InternalObject.canJumpAgain_Cloud;
	set => InternalObject.canJumpAgain_Cloud = value;
}		public virtual bool CanJumpAgain_Fart
{
	get => InternalObject.canJumpAgain_Fart;
	set => InternalObject.canJumpAgain_Fart = value;
}		public virtual bool CanJumpAgain_Sail
{
	get => InternalObject.canJumpAgain_Sail;
	set => InternalObject.canJumpAgain_Sail = value;
}		public virtual bool CanJumpAgain_Sandstorm
{
	get => InternalObject.canJumpAgain_Sandstorm;
	set => InternalObject.canJumpAgain_Sandstorm = value;
}		public virtual bool CanJumpAgain_Santank
{
	get => InternalObject.canJumpAgain_Santank;
	set => InternalObject.canJumpAgain_Santank = value;
}		public virtual bool CanJumpAgain_Unicorn
{
	get => InternalObject.canJumpAgain_Unicorn;
	set => InternalObject.canJumpAgain_Unicorn = value;
}		public virtual bool CanJumpAgain_WallOfFleshGoat
{
	get => InternalObject.canJumpAgain_WallOfFleshGoat;
	set => InternalObject.canJumpAgain_WallOfFleshGoat = value;
}		public virtual bool CanRocket
{
	get => InternalObject.canRocket;
	set => InternalObject.canRocket = value;
}		public virtual bool CanSeeInvisibleBlocks
{
	get => InternalObject.CanSeeInvisibleBlocks;
	set => InternalObject.CanSeeInvisibleBlocks = value;
}		public virtual bool Carpet
{
	get => InternalObject.carpet;
	set => InternalObject.carpet = value;
}		public virtual bool CartFlip
{
	get => InternalObject.cartFlip;
	set => InternalObject.cartFlip = value;
}		public virtual bool Channel
{
	get => InternalObject.channel;
	set => InternalObject.channel = value;
}		public virtual bool ChaosState
{
	get => InternalObject.chaosState;
	set => InternalObject.chaosState = value;
}		public virtual bool Chilled
{
	get => InternalObject.chilled;
	set => InternalObject.chilled = value;
}		public virtual bool ChloroAmmoCost80
{
	get => InternalObject.chloroAmmoCost80;
	set => InternalObject.chloroAmmoCost80 = value;
}		public virtual bool ColdDash
{
	get => InternalObject.coldDash;
	set => InternalObject.coldDash = value;
}		public virtual bool CompanionCube
{
	get => InternalObject.companionCube;
	set => InternalObject.companionCube = value;
}		public virtual bool Confused
{
	get => InternalObject.confused;
	set => InternalObject.confused = value;
}		public virtual bool ControlCreativeMenu
{
	get => InternalObject.controlCreativeMenu;
	set => InternalObject.controlCreativeMenu = value;
}		public virtual bool ControlDown
{
	get => InternalObject.controlDown;
	set => InternalObject.controlDown = value;
}		public virtual bool ControlHook
{
	get => InternalObject.controlHook;
	set => InternalObject.controlHook = value;
}		public virtual bool ControlInv
{
	get => InternalObject.controlInv;
	set => InternalObject.controlInv = value;
}		public virtual bool ControlJump
{
	get => InternalObject.controlJump;
	set => InternalObject.controlJump = value;
}		public virtual bool ControlLeft
{
	get => InternalObject.controlLeft;
	set => InternalObject.controlLeft = value;
}		public virtual bool ControlMap
{
	get => InternalObject.controlMap;
	set => InternalObject.controlMap = value;
}		public virtual bool ControlMount
{
	get => InternalObject.controlMount;
	set => InternalObject.controlMount = value;
}		public virtual bool ControlQuickHeal
{
	get => InternalObject.controlQuickHeal;
	set => InternalObject.controlQuickHeal = value;
}		public virtual bool ControlQuickMana
{
	get => InternalObject.controlQuickMana;
	set => InternalObject.controlQuickMana = value;
}		public virtual bool ControlRight
{
	get => InternalObject.controlRight;
	set => InternalObject.controlRight = value;
}		public virtual bool ControlSmart
{
	get => InternalObject.controlSmart;
	set => InternalObject.controlSmart = value;
}		public virtual bool ControlThrow
{
	get => InternalObject.controlThrow;
	set => InternalObject.controlThrow = value;
}		public virtual bool ControlTorch
{
	get => InternalObject.controlTorch;
	set => InternalObject.controlTorch = value;
}		public virtual bool ControlUp
{
	get => InternalObject.controlUp;
	set => InternalObject.controlUp = value;
}		public virtual bool ControlUseItem
{
	get => InternalObject.controlUseItem;
	set => InternalObject.controlUseItem = value;
}		public virtual bool ControlUseTile
{
	get => InternalObject.controlUseTile;
	set => InternalObject.controlUseTile = value;
}		public virtual bool CoolWhipBuff
{
	get => InternalObject.coolWhipBuff;
	set => InternalObject.coolWhipBuff = value;
}		public virtual bool Cordage
{
	get => InternalObject.cordage;
	set => InternalObject.cordage = value;
}		public virtual bool CratePotion
{
	get => InternalObject.cratePotion;
	set => InternalObject.cratePotion = value;
}		public virtual bool CreativeGodMode
{
	get => InternalObject.creativeGodMode;
	set => InternalObject.creativeGodMode = value;
}		public virtual bool CreativeInterface
{
	get => InternalObject.creativeInterface;
	set => InternalObject.creativeInterface = value;
}		public virtual bool CrimsonHeart
{
	get => InternalObject.crimsonHeart;
	set => InternalObject.crimsonHeart = value;
}		public virtual bool CrimsonRegen
{
	get => InternalObject.crimsonRegen;
	set => InternalObject.crimsonRegen = value;
}		public virtual bool CrystalLeaf
{
	get => InternalObject.crystalLeaf;
	set => InternalObject.crystalLeaf = value;
}		public virtual bool CSapling
{
	get => InternalObject.cSapling;
	set => InternalObject.cSapling = value;
}		public virtual bool Cursed
{
	get => InternalObject.cursed;
	set => InternalObject.cursed = value;
}		public virtual bool CursorItemIconEnabled
{
	get => InternalObject.cursorItemIconEnabled;
	set => InternalObject.cursorItemIconEnabled = value;
}		public virtual bool CursorItemIconReversed
{
	get => InternalObject.cursorItemIconReversed;
	set => InternalObject.cursorItemIconReversed = value;
}		public virtual bool DangerSense
{
	get => InternalObject.dangerSense;
	set => InternalObject.dangerSense = value;
}		public virtual bool Dazed
{
	get => InternalObject.dazed;
	set => InternalObject.dazed = value;
}		public virtual bool Dd2Accessory
{
	get => InternalObject.dd2Accessory;
	set => InternalObject.dd2Accessory = value;
}		public virtual bool Dead
{
	get => InternalObject.dead;
	set => InternalObject.dead = value;
}		public virtual bool DeadForGood
{
	get => InternalObject.deadForGood;
	set => InternalObject.deadForGood = value;
}		public virtual bool DeadlySphereMinion
{
	get => InternalObject.DeadlySphereMinion;
	set => InternalObject.DeadlySphereMinion = value;
}		public virtual bool DefendedByPaladin
{
	get => InternalObject.defendedByPaladin;
	set => InternalObject.defendedByPaladin = value;
}		public virtual bool DelayUseItem
{
	get => InternalObject.delayUseItem;
	set => InternalObject.delayUseItem = value;
}		public virtual bool DesertBoots
{
	get => InternalObject.desertBoots;
	set => InternalObject.desertBoots = value;
}		public virtual bool DesertDash
{
	get => InternalObject.desertDash;
	set => InternalObject.desertDash = value;
}		public virtual bool DetectCreature
{
	get => InternalObject.detectCreature;
	set => InternalObject.detectCreature = value;
}		public virtual bool Dino
{
	get => InternalObject.dino;
	set => InternalObject.dino = value;
}		public virtual bool DisabledBlizzardGraphic
{
	get => InternalObject.disabledBlizzardGraphic;
	set => InternalObject.disabledBlizzardGraphic = value;
}		public virtual bool DisabledBlizzardSound
{
	get => InternalObject.disabledBlizzardSound;
	set => InternalObject.disabledBlizzardSound = value;
}		public virtual bool Discount
{
	get => InternalObject.discount;
	set => InternalObject.discount = value;
}		public virtual bool DontConsumeWand
{
	get => InternalObject.dontConsumeWand;
	set => InternalObject.dontConsumeWand = value;
}		public virtual bool DontHurtCritters
{
	get => InternalObject.dontHurtCritters;
	set => InternalObject.dontHurtCritters = value;
}		public virtual bool DontStarveShader
{
	get => InternalObject.dontStarveShader;
	set => InternalObject.dontStarveShader = value;
}		public virtual bool DownedDD2EventAnyDifficulty
{
	get => InternalObject.downedDD2EventAnyDifficulty;
	set => InternalObject.downedDD2EventAnyDifficulty = value;
}		public virtual bool DpsStarted
{
	get => InternalObject.dpsStarted;
	set => InternalObject.dpsStarted = value;
}		public virtual bool DrawingFootball
{
	get => InternalObject.drawingFootball;
	set => InternalObject.drawingFootball = value;
}		public virtual bool Dripping
{
	get => InternalObject.dripping;
	set => InternalObject.dripping = value;
}		public virtual bool DrippingSlime
{
	get => InternalObject.drippingSlime;
	set => InternalObject.drippingSlime = value;
}		public virtual bool DrippingSparkleSlime
{
	get => InternalObject.drippingSparkleSlime;
	set => InternalObject.drippingSparkleSlime = value;
}		public virtual bool DryadWard
{
	get => InternalObject.dryadWard;
	set => InternalObject.dryadWard = value;
}		public virtual bool DryCoralTorch
{
	get => InternalObject.dryCoralTorch;
	set => InternalObject.dryCoralTorch = value;
}		public virtual bool Eater
{
	get => InternalObject.eater;
	set => InternalObject.eater = value;
}		public virtual bool EditedChestName
{
	get => InternalObject.editedChestName;
	set => InternalObject.editedChestName = value;
}		public virtual bool Electrified
{
	get => InternalObject.electrified;
	set => InternalObject.electrified = value;
}		public virtual bool EmpressBlade
{
	get => InternalObject.empressBlade;
	set => InternalObject.empressBlade = value;
}		public virtual bool EmpressBrooch
{
	get => InternalObject.empressBrooch;
	set => InternalObject.empressBrooch = value;
}		public virtual bool EnemySpawns
{
	get => InternalObject.enemySpawns;
	set => InternalObject.enemySpawns = value;
}		public virtual bool EquippedAnyTileRangeAcc
{
	get => InternalObject.equippedAnyTileRangeAcc;
	set => InternalObject.equippedAnyTileRangeAcc = value;
}		public virtual bool EquippedAnyTileSpeedAcc
{
	get => InternalObject.equippedAnyTileSpeedAcc;
	set => InternalObject.equippedAnyTileSpeedAcc = value;
}		public virtual bool EquippedAnyWallSpeedAcc
{
	get => InternalObject.equippedAnyWallSpeedAcc;
	set => InternalObject.equippedAnyWallSpeedAcc = value;
}		public virtual bool ExtraAccessory
{
	get => InternalObject.extraAccessory;
	set => InternalObject.extraAccessory = value;
}		public virtual bool EyebrellaCloud
{
	get => InternalObject.eyebrellaCloud;
	set => InternalObject.eyebrellaCloud = value;
}		public virtual bool EyeSpring
{
	get => InternalObject.eyeSpring;
	set => InternalObject.eyeSpring = value;
}		public virtual bool FairyBoots
{
	get => InternalObject.fairyBoots;
	set => InternalObject.fairyBoots = value;
}		public virtual bool FindTreasure
{
	get => InternalObject.findTreasure;
	set => InternalObject.findTreasure = value;
}		public virtual bool FireWalk
{
	get => InternalObject.fireWalk;
	set => InternalObject.fireWalk = value;
}		public virtual bool FlapSound
{
	get => InternalObject.flapSound;
	set => InternalObject.flapSound = value;
}		public virtual bool FlinxMinion
{
	get => InternalObject.flinxMinion;
	set => InternalObject.flinxMinion = value;
}		public virtual bool FlowerBoots
{
	get => InternalObject.flowerBoots;
	set => InternalObject.flowerBoots = value;
}		public virtual bool ForceMerman
{
	get => InternalObject.forceMerman;
	set => InternalObject.forceMerman = value;
}		public virtual bool ForceWerewolf
{
	get => InternalObject.forceWerewolf;
	set => InternalObject.forceWerewolf = value;
}		public virtual bool FrogLegJumpBoost
{
	get => InternalObject.frogLegJumpBoost;
	set => InternalObject.frogLegJumpBoost = value;
}		public virtual bool FrostArmor
{
	get => InternalObject.frostArmor;
	set => InternalObject.frostArmor = value;
}		public virtual bool FrostBurn
{
	get => InternalObject.frostBurn;
	set => InternalObject.frostBurn = value;
}		public virtual bool Frozen
{
	get => InternalObject.frozen;
	set => InternalObject.frozen = value;
}		public virtual bool Ghost
{
	get => InternalObject.ghost;
	set => InternalObject.ghost = value;
}		public virtual bool GhostHeal
{
	get => InternalObject.ghostHeal;
	set => InternalObject.ghostHeal = value;
}		public virtual bool GhostHurt
{
	get => InternalObject.ghostHurt;
	set => InternalObject.ghostHurt = value;
}		public virtual bool Gills
{
	get => InternalObject.gills;
	set => InternalObject.gills = value;
}		public virtual bool GoingDownWithGrapple
{
	get => InternalObject.GoingDownWithGrapple;
	set => InternalObject.GoingDownWithGrapple = value;
}		public virtual bool GoldRing
{
	get => InternalObject.goldRing;
	set => InternalObject.goldRing = value;
}		public virtual bool GravControl
{
	get => InternalObject.gravControl;
	set => InternalObject.gravControl = value;
}		public virtual bool GravControl2
{
	get => InternalObject.gravControl2;
	set => InternalObject.gravControl2 = value;
}		public virtual bool GreenFairy
{
	get => InternalObject.greenFairy;
	set => InternalObject.greenFairy = value;
}		public virtual bool Grinch
{
	get => InternalObject.grinch;
	set => InternalObject.grinch = value;
}		public virtual bool Gross
{
	get => InternalObject.gross;
	set => InternalObject.gross = value;
}		public virtual bool HappyFunTorchTime
{
	get => InternalObject.happyFunTorchTime;
	set => InternalObject.happyFunTorchTime = value;
}		public virtual bool HasAngelHalo
{
	get => InternalObject.hasAngelHalo;
	set => InternalObject.hasAngelHalo = value;
}		public virtual bool HasCreditsSceneMusicBox
{
	get => InternalObject.hasCreditsSceneMusicBox;
	set => InternalObject.hasCreditsSceneMusicBox = value;
}		public virtual bool HasFloatingTube
{
	get => InternalObject.hasFloatingTube;
	set => InternalObject.hasFloatingTube = value;
}		public virtual bool HasFootball
{
	get => InternalObject.hasFootball;
	set => InternalObject.hasFootball = value;
}		public virtual bool HasGardenGnomeNearby
{
	get => InternalObject.HasGardenGnomeNearby;
	set => InternalObject.HasGardenGnomeNearby = value;
}		public virtual bool HasJumpOption_Basilisk
{
	get => InternalObject.hasJumpOption_Basilisk;
	set => InternalObject.hasJumpOption_Basilisk = value;
}		public virtual bool HasJumpOption_Blizzard
{
	get => InternalObject.hasJumpOption_Blizzard;
	set => InternalObject.hasJumpOption_Blizzard = value;
}		public virtual bool HasJumpOption_Cloud
{
	get => InternalObject.hasJumpOption_Cloud;
	set => InternalObject.hasJumpOption_Cloud = value;
}		public virtual bool HasJumpOption_Fart
{
	get => InternalObject.hasJumpOption_Fart;
	set => InternalObject.hasJumpOption_Fart = value;
}		public virtual bool HasJumpOption_Sail
{
	get => InternalObject.hasJumpOption_Sail;
	set => InternalObject.hasJumpOption_Sail = value;
}		public virtual bool HasJumpOption_Sandstorm
{
	get => InternalObject.hasJumpOption_Sandstorm;
	set => InternalObject.hasJumpOption_Sandstorm = value;
}		public virtual bool HasJumpOption_Santank
{
	get => InternalObject.hasJumpOption_Santank;
	set => InternalObject.hasJumpOption_Santank = value;
}		public virtual bool HasJumpOption_Unicorn
{
	get => InternalObject.hasJumpOption_Unicorn;
	set => InternalObject.hasJumpOption_Unicorn = value;
}		public virtual bool HasJumpOption_WallOfFleshGoat
{
	get => InternalObject.hasJumpOption_WallOfFleshGoat;
	set => InternalObject.hasJumpOption_WallOfFleshGoat = value;
}		public virtual bool HasLuckyCoin
{
	get => InternalObject.hasLuckyCoin;
	set => InternalObject.hasLuckyCoin = value;
}		public virtual bool HasLucyTheAxe
{
	get => InternalObject.hasLucyTheAxe;
	set => InternalObject.hasLucyTheAxe = value;
}		public virtual bool HasMagiluminescence
{
	get => InternalObject.hasMagiluminescence;
	set => InternalObject.hasMagiluminescence = value;
}		public virtual bool HasMoltenQuiver
{
	get => InternalObject.hasMoltenQuiver;
	set => InternalObject.hasMoltenQuiver = value;
}		public virtual bool HasPaladinShield
{
	get => InternalObject.hasPaladinShield;
	set => InternalObject.hasPaladinShield = value;
}		public virtual bool HasRainbowCursor
{
	get => InternalObject.hasRainbowCursor;
	set => InternalObject.hasRainbowCursor = value;
}		public virtual bool HasRaisableShield
{
	get => InternalObject.hasRaisableShield;
	set => InternalObject.hasRaisableShield = value;
}		public virtual bool HasTitaniumStormBuff
{
	get => InternalObject.hasTitaniumStormBuff;
	set => InternalObject.hasTitaniumStormBuff = value;
}		public virtual bool HasUnicornHorn
{
	get => InternalObject.hasUnicornHorn;
	set => InternalObject.hasUnicornHorn = value;
}		public virtual bool HbLocked
{
	get => InternalObject.hbLocked;
	set => InternalObject.hbLocked = value;
}		public virtual bool Headcovered
{
	get => InternalObject.headcovered;
	set => InternalObject.headcovered = value;
}		public virtual bool HeartyMeal
{
	get => InternalObject.heartyMeal;
	set => InternalObject.heartyMeal = value;
}		public virtual bool HideMerman
{
	get => InternalObject.hideMerman;
	set => InternalObject.hideMerman = value;
}		public virtual bool HideWolf
{
	get => InternalObject.hideWolf;
	set => InternalObject.hideWolf = value;
}		public virtual bool Honey
{
	get => InternalObject.honey;
	set => InternalObject.honey = value;
}		public virtual bool Hornet
{
	get => InternalObject.hornet;
	set => InternalObject.hornet = value;
}		public virtual bool HornetMinion
{
	get => InternalObject.hornetMinion;
	set => InternalObject.hornetMinion = value;
}		public virtual bool Hostile
{
	get => InternalObject.hostile;
	set => InternalObject.hostile = value;
}		public virtual bool Hungry
{
	get => InternalObject.hungry;
	set => InternalObject.hungry = value;
}		public virtual bool HuntressAmmoCost90
{
	get => InternalObject.huntressAmmoCost90;
	set => InternalObject.huntressAmmoCost90 = value;
}		public virtual bool IceBarrier
{
	get => InternalObject.iceBarrier;
	set => InternalObject.iceBarrier = value;
}		public virtual bool IceSkate
{
	get => InternalObject.iceSkate;
	set => InternalObject.iceSkate = value;
}		public virtual bool Ichor
{
	get => InternalObject.ichor;
	set => InternalObject.ichor = value;
}		public virtual bool IgnoreWater
{
	get => InternalObject.ignoreWater;
	set => InternalObject.ignoreWater = value;
}		public virtual bool Immune
{
	get => InternalObject.immune;
	set => InternalObject.immune = value;
}		public virtual bool ImmuneNoBlink
{
	get => InternalObject.immuneNoBlink;
	set => InternalObject.immuneNoBlink = value;
}		public virtual bool ImpMinion
{
	get => InternalObject.impMinion;
	set => InternalObject.impMinion = value;
}		public virtual bool Inferno
{
	get => InternalObject.inferno;
	set => InternalObject.inferno = value;
}		public virtual bool InfoAccMechShowWires
{
	get => InternalObject.InfoAccMechShowWires;
	set => InternalObject.InfoAccMechShowWires = value;
}		public virtual bool Invis
{
	get => InternalObject.invis;
	set => InternalObject.invis = value;
}		public virtual bool IsControlledByFilm
{
	get => InternalObject.isControlledByFilm;
	set => InternalObject.isControlledByFilm = value;
}		public virtual bool IsDisplayDollOrInanimate
{
	get => InternalObject.isDisplayDollOrInanimate;
	set => InternalObject.isDisplayDollOrInanimate = value;
}		public virtual bool IsFirstFractalAfterImage
{
	get => InternalObject.isFirstFractalAfterImage;
	set => InternalObject.isFirstFractalAfterImage = value;
}		public virtual bool IsHatRackDoll
{
	get => InternalObject.isHatRackDoll;
	set => InternalObject.isHatRackDoll = value;
}		public virtual bool IsPerformingJump_Basilisk
{
	get => InternalObject.isPerformingJump_Basilisk;
	set => InternalObject.isPerformingJump_Basilisk = value;
}		public virtual bool IsPerformingJump_Blizzard
{
	get => InternalObject.isPerformingJump_Blizzard;
	set => InternalObject.isPerformingJump_Blizzard = value;
}		public virtual bool IsPerformingJump_Cloud
{
	get => InternalObject.isPerformingJump_Cloud;
	set => InternalObject.isPerformingJump_Cloud = value;
}		public virtual bool IsPerformingJump_Fart
{
	get => InternalObject.isPerformingJump_Fart;
	set => InternalObject.isPerformingJump_Fart = value;
}		public virtual bool IsPerformingJump_Sail
{
	get => InternalObject.isPerformingJump_Sail;
	set => InternalObject.isPerformingJump_Sail = value;
}		public virtual bool IsPerformingJump_Sandstorm
{
	get => InternalObject.isPerformingJump_Sandstorm;
	set => InternalObject.isPerformingJump_Sandstorm = value;
}		public virtual bool IsPerformingJump_Santank
{
	get => InternalObject.isPerformingJump_Santank;
	set => InternalObject.isPerformingJump_Santank = value;
}		public virtual bool IsPerformingJump_Unicorn
{
	get => InternalObject.isPerformingJump_Unicorn;
	set => InternalObject.isPerformingJump_Unicorn = value;
}		public virtual bool IsPerformingJump_WallOfFleshGoat
{
	get => InternalObject.isPerformingJump_WallOfFleshGoat;
	set => InternalObject.isPerformingJump_WallOfFleshGoat = value;
}		public virtual bool IsPerformingPogostickTricks
{
	get => InternalObject.isPerformingPogostickTricks;
	set => InternalObject.isPerformingPogostickTricks = value;
}		public virtual bool IsPettingAnimal
{
	get => InternalObject.isPettingAnimal;
	set => InternalObject.isPettingAnimal = value;
}		public virtual bool IsTheAnimalBeingPetSmall
{
	get => InternalObject.isTheAnimalBeingPetSmall;
	set => InternalObject.isTheAnimalBeingPetSmall = value;
}		public virtual bool JumpBoost
{
	get => InternalObject.jumpBoost;
	set => InternalObject.jumpBoost = value;
}		public virtual bool JustDroppedAnItem
{
	get => InternalObject.JustDroppedAnItem;
	set => InternalObject.JustDroppedAnItem = value;
}		public virtual bool JustJumped
{
	get => InternalObject.justJumped;
	set => InternalObject.justJumped = value;
}		public virtual bool KbBuff
{
	get => InternalObject.kbBuff;
	set => InternalObject.kbBuff = value;
}		public virtual bool KbGlove
{
	get => InternalObject.kbGlove;
	set => InternalObject.kbGlove = value;
}		public virtual bool KillClothier
{
	get => InternalObject.killClothier;
	set => InternalObject.killClothier = value;
}		public virtual bool KillGuide
{
	get => InternalObject.killGuide;
	set => InternalObject.killGuide = value;
}		public virtual bool LastMouseInterface
{
	get => InternalObject.lastMouseInterface;
	set => InternalObject.lastMouseInterface = value;
}		public virtual bool LastPound
{
	get => InternalObject.lastPound;
	set => InternalObject.lastPound = value;
}		public virtual bool LastStoned
{
	get => InternalObject.lastStoned;
	set => InternalObject.lastStoned = value;
}		public virtual bool LavaImmune
{
	get => InternalObject.lavaImmune;
	set => InternalObject.lavaImmune = value;
}		public virtual bool LavaRose
{
	get => InternalObject.lavaRose;
	set => InternalObject.lavaRose = value;
}		public virtual bool LeinforsHair
{
	get => InternalObject.leinforsHair;
	set => InternalObject.leinforsHair = value;
}		public virtual bool LifeForce
{
	get => InternalObject.lifeForce;
	set => InternalObject.lifeForce = value;
}		public virtual bool LifeMagnet
{
	get => InternalObject.lifeMagnet;
	set => InternalObject.lifeMagnet = value;
}		public virtual bool LightOrb
{
	get => InternalObject.lightOrb;
	set => InternalObject.lightOrb = value;
}		public virtual bool Lizard
{
	get => InternalObject.lizard;
	set => InternalObject.lizard = value;
}		public virtual bool LongInvince
{
	get => InternalObject.longInvince;
	set => InternalObject.longInvince = value;
}		public virtual bool LoveStruck
{
	get => InternalObject.loveStruck;
	set => InternalObject.loveStruck = value;
}		public virtual bool LuckNeedsSync
{
	get => InternalObject.luckNeedsSync;
	set => InternalObject.luckNeedsSync = value;
}		public virtual bool MagicCuffs
{
	get => InternalObject.magicCuffs;
	set => InternalObject.magicCuffs = value;
}		public virtual bool MagicLantern
{
	get => InternalObject.magicLantern;
	set => InternalObject.magicLantern = value;
}		public virtual bool MagicQuiver
{
	get => InternalObject.magicQuiver;
	set => InternalObject.magicQuiver = value;
}		public virtual bool MagmaStone
{
	get => InternalObject.magmaStone;
	set => InternalObject.magmaStone = value;
}		public virtual bool MakeStrongBee
{
	get => InternalObject.makeStrongBee;
	set => InternalObject.makeStrongBee = value;
}		public virtual bool ManaFlower
{
	get => InternalObject.manaFlower;
	set => InternalObject.manaFlower = value;
}		public virtual bool ManaMagnet
{
	get => InternalObject.manaMagnet;
	set => InternalObject.manaMagnet = value;
}		public virtual bool ManaRegenBuff
{
	get => InternalObject.manaRegenBuff;
	set => InternalObject.manaRegenBuff = value;
}		public virtual bool ManaSick
{
	get => InternalObject.manaSick;
	set => InternalObject.manaSick = value;
}		public virtual bool MapAlphaDown
{
	get => InternalObject.mapAlphaDown;
	set => InternalObject.mapAlphaDown = value;
}		public virtual bool MapAlphaUp
{
	get => InternalObject.mapAlphaUp;
	set => InternalObject.mapAlphaUp = value;
}		public virtual bool MapFullScreen
{
	get => InternalObject.mapFullScreen;
	set => InternalObject.mapFullScreen = value;
}		public virtual bool MapStyle
{
	get => InternalObject.mapStyle;
	set => InternalObject.mapStyle = value;
}		public virtual bool MapZoomIn
{
	get => InternalObject.mapZoomIn;
	set => InternalObject.mapZoomIn = value;
}		public virtual bool MapZoomOut
{
	get => InternalObject.mapZoomOut;
	set => InternalObject.mapZoomOut = value;
}		public virtual bool MeleeScaleGlove
{
	get => InternalObject.meleeScaleGlove;
	set => InternalObject.meleeScaleGlove = value;
}		public virtual bool Merman
{
	get => InternalObject.merman;
	set => InternalObject.merman = value;
}		public virtual bool MinecartLeft
{
	get => InternalObject.minecartLeft;
	set => InternalObject.minecartLeft = value;
}		public virtual bool MiniMinotaur
{
	get => InternalObject.miniMinotaur;
	set => InternalObject.miniMinotaur = value;
}		public virtual bool MoonLeech
{
	get => InternalObject.moonLeech;
	set => InternalObject.moonLeech = value;
}		public virtual bool MoonLordLegs
{
	get => InternalObject.moonLordLegs;
	set => InternalObject.moonLordLegs = value;
}		public virtual bool MouseInterface
{
	get => InternalObject.mouseInterface;
	set => InternalObject.mouseInterface = value;
}		public virtual bool NetLife
{
	get => InternalObject.netLife;
	set => InternalObject.netLife = value;
}		public virtual bool NetMana
{
	get => InternalObject.netMana;
	set => InternalObject.netMana = value;
}		public virtual bool NightVision
{
	get => InternalObject.nightVision;
	set => InternalObject.nightVision = value;
}		public virtual bool NoBuilding
{
	get => InternalObject.noBuilding;
	set => InternalObject.noBuilding = value;
}		public virtual bool NoFallDmg
{
	get => InternalObject.noFallDmg;
	set => InternalObject.noFallDmg = value;
}		public virtual bool NoItems
{
	get => InternalObject.noItems;
	set => InternalObject.noItems = value;
}		public virtual bool NoKnockback
{
	get => InternalObject.noKnockback;
	set => InternalObject.noKnockback = value;
}		public virtual bool OldAdjHoney
{
	get => InternalObject.oldAdjHoney;
	set => InternalObject.oldAdjHoney = value;
}		public virtual bool OldAdjLava
{
	get => InternalObject.oldAdjLava;
	set => InternalObject.oldAdjLava = value;
}		public virtual bool OldAdjWater
{
	get => InternalObject.oldAdjWater;
	set => InternalObject.oldAdjWater = value;
}		public virtual bool OnFire
{
	get => InternalObject.onFire;
	set => InternalObject.onFire = value;
}		public virtual bool OnFire2
{
	get => InternalObject.onFire2;
	set => InternalObject.onFire2 = value;
}		public virtual bool OnFire3
{
	get => InternalObject.onFire3;
	set => InternalObject.onFire3 = value;
}		public virtual bool OnFrostBurn
{
	get => InternalObject.onFrostBurn;
	set => InternalObject.onFrostBurn = value;
}		public virtual bool OnFrostBurn2
{
	get => InternalObject.onFrostBurn2;
	set => InternalObject.onFrostBurn2 = value;
}		public virtual bool OnHitDodge
{
	get => InternalObject.onHitDodge;
	set => InternalObject.onHitDodge = value;
}		public virtual bool OnHitPetal
{
	get => InternalObject.onHitPetal;
	set => InternalObject.onHitPetal = value;
}		public virtual bool OnHitRegen
{
	get => InternalObject.onHitRegen;
	set => InternalObject.onHitRegen = value;
}		public virtual bool OnHitTitaniumStorm
{
	get => InternalObject.onHitTitaniumStorm;
	set => InternalObject.onHitTitaniumStorm = value;
}		public virtual bool OnTrack
{
	get => InternalObject.onTrack;
	set => InternalObject.onTrack = value;
}		public virtual bool OnWrongGround
{
	get => InternalObject.onWrongGround;
	set => InternalObject.onWrongGround = value;
}		public virtual bool OutOfRange
{
	get => InternalObject.outOfRange;
	set => InternalObject.outOfRange = value;
}		public virtual bool PalladiumRegen
{
	get => InternalObject.palladiumRegen;
	set => InternalObject.palladiumRegen = value;
}		public virtual bool Panic
{
	get => InternalObject.panic;
	set => InternalObject.panic = value;
}		public virtual bool Parrot
{
	get => InternalObject.parrot;
	set => InternalObject.parrot = value;
}		public virtual bool ParryDamageBuff
{
	get => InternalObject.parryDamageBuff;
	set => InternalObject.parryDamageBuff = value;
}		public virtual bool Penguin
{
	get => InternalObject.penguin;
	set => InternalObject.penguin = value;
}		public virtual bool PetFlagBabyImp
{
	get => InternalObject.petFlagBabyImp;
	set => InternalObject.petFlagBabyImp = value;
}		public virtual bool PetFlagBabyRedPanda
{
	get => InternalObject.petFlagBabyRedPanda;
	set => InternalObject.petFlagBabyRedPanda = value;
}		public virtual bool PetFlagBabyShark
{
	get => InternalObject.petFlagBabyShark;
	set => InternalObject.petFlagBabyShark = value;
}		public virtual bool PetFlagBabyWerewolf
{
	get => InternalObject.petFlagBabyWerewolf;
	set => InternalObject.petFlagBabyWerewolf = value;
}		public virtual bool PetFlagBerniePet
{
	get => InternalObject.petFlagBerniePet;
	set => InternalObject.petFlagBerniePet = value;
}		public virtual bool PetFlagBrainOfCthulhuPet
{
	get => InternalObject.petFlagBrainOfCthulhuPet;
	set => InternalObject.petFlagBrainOfCthulhuPet = value;
}		public virtual bool PetFlagChesterPet
{
	get => InternalObject.petFlagChesterPet;
	set => InternalObject.petFlagChesterPet = value;
}		public virtual bool PetFlagDD2BetsyPet
{
	get => InternalObject.petFlagDD2BetsyPet;
	set => InternalObject.petFlagDD2BetsyPet = value;
}		public virtual bool PetFlagDD2Dragon
{
	get => InternalObject.petFlagDD2Dragon;
	set => InternalObject.petFlagDD2Dragon = value;
}		public virtual bool PetFlagDD2Gato
{
	get => InternalObject.petFlagDD2Gato;
	set => InternalObject.petFlagDD2Gato = value;
}		public virtual bool PetFlagDD2Ghost
{
	get => InternalObject.petFlagDD2Ghost;
	set => InternalObject.petFlagDD2Ghost = value;
}		public virtual bool PetFlagDD2OgrePet
{
	get => InternalObject.petFlagDD2OgrePet;
	set => InternalObject.petFlagDD2OgrePet = value;
}		public virtual bool PetFlagDeerclopsPet
{
	get => InternalObject.petFlagDeerclopsPet;
	set => InternalObject.petFlagDeerclopsPet = value;
}		public virtual bool PetFlagDestroyerPet
{
	get => InternalObject.petFlagDestroyerPet;
	set => InternalObject.petFlagDestroyerPet = value;
}		public virtual bool PetFlagDukeFishronPet
{
	get => InternalObject.petFlagDukeFishronPet;
	set => InternalObject.petFlagDukeFishronPet = value;
}		public virtual bool PetFlagDynamiteKitten
{
	get => InternalObject.petFlagDynamiteKitten;
	set => InternalObject.petFlagDynamiteKitten = value;
}		public virtual bool PetFlagEaterOfWorldsPet
{
	get => InternalObject.petFlagEaterOfWorldsPet;
	set => InternalObject.petFlagEaterOfWorldsPet = value;
}		public virtual bool PetFlagEverscreamPet
{
	get => InternalObject.petFlagEverscreamPet;
	set => InternalObject.petFlagEverscreamPet = value;
}		public virtual bool PetFlagEyeOfCthulhuPet
{
	get => InternalObject.petFlagEyeOfCthulhuPet;
	set => InternalObject.petFlagEyeOfCthulhuPet = value;
}		public virtual bool PetFlagFairyQueenPet
{
	get => InternalObject.petFlagFairyQueenPet;
	set => InternalObject.petFlagFairyQueenPet = value;
}		public virtual bool PetFlagFennecFox
{
	get => InternalObject.petFlagFennecFox;
	set => InternalObject.petFlagFennecFox = value;
}		public virtual bool PetFlagGlitteryButterfly
{
	get => InternalObject.petFlagGlitteryButterfly;
	set => InternalObject.petFlagGlitteryButterfly = value;
}		public virtual bool PetFlagGlommerPet
{
	get => InternalObject.petFlagGlommerPet;
	set => InternalObject.petFlagGlommerPet = value;
}		public virtual bool PetFlagGolemPet
{
	get => InternalObject.petFlagGolemPet;
	set => InternalObject.petFlagGolemPet = value;
}		public virtual bool PetFlagIceQueenPet
{
	get => InternalObject.petFlagIceQueenPet;
	set => InternalObject.petFlagIceQueenPet = value;
}		public virtual bool PetFlagKingSlimePet
{
	get => InternalObject.petFlagKingSlimePet;
	set => InternalObject.petFlagKingSlimePet = value;
}		public virtual bool PetFlagLilHarpy
{
	get => InternalObject.petFlagLilHarpy;
	set => InternalObject.petFlagLilHarpy = value;
}		public virtual bool PetFlagLunaticCultistPet
{
	get => InternalObject.petFlagLunaticCultistPet;
	set => InternalObject.petFlagLunaticCultistPet = value;
}		public virtual bool PetFlagMartianPet
{
	get => InternalObject.petFlagMartianPet;
	set => InternalObject.petFlagMartianPet = value;
}		public virtual bool PetFlagMoonLordPet
{
	get => InternalObject.petFlagMoonLordPet;
	set => InternalObject.petFlagMoonLordPet = value;
}		public virtual bool PetFlagPigPet
{
	get => InternalObject.petFlagPigPet;
	set => InternalObject.petFlagPigPet = value;
}		public virtual bool PetFlagPlanteraPet
{
	get => InternalObject.petFlagPlanteraPet;
	set => InternalObject.petFlagPlanteraPet = value;
}		public virtual bool PetFlagPlantero
{
	get => InternalObject.petFlagPlantero;
	set => InternalObject.petFlagPlantero = value;
}		public virtual bool PetFlagPumpkingPet
{
	get => InternalObject.petFlagPumpkingPet;
	set => InternalObject.petFlagPumpkingPet = value;
}		public virtual bool PetFlagQueenBeePet
{
	get => InternalObject.petFlagQueenBeePet;
	set => InternalObject.petFlagQueenBeePet = value;
}		public virtual bool PetFlagQueenSlimePet
{
	get => InternalObject.petFlagQueenSlimePet;
	set => InternalObject.petFlagQueenSlimePet = value;
}		public virtual bool PetFlagShadowMimic
{
	get => InternalObject.petFlagShadowMimic;
	set => InternalObject.petFlagShadowMimic = value;
}		public virtual bool PetFlagSkeletronPet
{
	get => InternalObject.petFlagSkeletronPet;
	set => InternalObject.petFlagSkeletronPet = value;
}		public virtual bool PetFlagSkeletronPrimePet
{
	get => InternalObject.petFlagSkeletronPrimePet;
	set => InternalObject.petFlagSkeletronPrimePet = value;
}		public virtual bool PetFlagSugarGlider
{
	get => InternalObject.petFlagSugarGlider;
	set => InternalObject.petFlagSugarGlider = value;
}		public virtual bool PetFlagTwinsPet
{
	get => InternalObject.petFlagTwinsPet;
	set => InternalObject.petFlagTwinsPet = value;
}		public virtual bool PetFlagUpbeatStar
{
	get => InternalObject.petFlagUpbeatStar;
	set => InternalObject.petFlagUpbeatStar = value;
}		public virtual bool PetFlagVoltBunny
{
	get => InternalObject.petFlagVoltBunny;
	set => InternalObject.petFlagVoltBunny = value;
}		public virtual bool PirateMinion
{
	get => InternalObject.pirateMinion;
	set => InternalObject.pirateMinion = value;
}		public virtual bool Poisoned
{
	get => InternalObject.poisoned;
	set => InternalObject.poisoned = value;
}		public virtual bool PortalPhysicsFlag
{
	get => InternalObject.portalPhysicsFlag;
	set => InternalObject.portalPhysicsFlag = value;
}		public virtual bool PoundRelease
{
	get => InternalObject.poundRelease;
	set => InternalObject.poundRelease = value;
}		public virtual bool Powerrun
{
	get => InternalObject.powerrun;
	set => InternalObject.powerrun = value;
}		public virtual bool PreventAllItemPickups
{
	get => InternalObject.preventAllItemPickups;
	set => InternalObject.preventAllItemPickups = value;
}		public virtual bool PStone
{
	get => InternalObject.pStone;
	set => InternalObject.pStone = value;
}		public virtual bool Pulley
{
	get => InternalObject.pulley;
	set => InternalObject.pulley = value;
}		public virtual bool Puppy
{
	get => InternalObject.puppy;
	set => InternalObject.puppy = value;
}		public virtual bool PvpDeath
{
	get => InternalObject.pvpDeath;
	set => InternalObject.pvpDeath = value;
}		public virtual bool Pygmy
{
	get => InternalObject.pygmy;
	set => InternalObject.pygmy = value;
}		public virtual bool Rabid
{
	get => InternalObject.rabid;
	set => InternalObject.rabid = value;
}		public virtual bool Raven
{
	get => InternalObject.raven;
	set => InternalObject.raven = value;
}		public virtual bool RedFairy
{
	get => InternalObject.redFairy;
	set => InternalObject.redFairy = value;
}		public virtual bool ReleaseCreativeMenu
{
	get => InternalObject.releaseCreativeMenu;
	set => InternalObject.releaseCreativeMenu = value;
}		public virtual bool ReleaseDown
{
	get => InternalObject.releaseDown;
	set => InternalObject.releaseDown = value;
}		public virtual bool ReleaseHook
{
	get => InternalObject.releaseHook;
	set => InternalObject.releaseHook = value;
}		public virtual bool ReleaseInventory
{
	get => InternalObject.releaseInventory;
	set => InternalObject.releaseInventory = value;
}		public virtual bool ReleaseJump
{
	get => InternalObject.releaseJump;
	set => InternalObject.releaseJump = value;
}		public virtual bool ReleaseLeft
{
	get => InternalObject.releaseLeft;
	set => InternalObject.releaseLeft = value;
}		public virtual bool ReleaseMapFullscreen
{
	get => InternalObject.releaseMapFullscreen;
	set => InternalObject.releaseMapFullscreen = value;
}		public virtual bool ReleaseMapStyle
{
	get => InternalObject.releaseMapStyle;
	set => InternalObject.releaseMapStyle = value;
}		public virtual bool ReleaseMount
{
	get => InternalObject.releaseMount;
	set => InternalObject.releaseMount = value;
}		public virtual bool ReleaseQuickHeal
{
	get => InternalObject.releaseQuickHeal;
	set => InternalObject.releaseQuickHeal = value;
}		public virtual bool ReleaseQuickMana
{
	get => InternalObject.releaseQuickMana;
	set => InternalObject.releaseQuickMana = value;
}		public virtual bool ReleaseRight
{
	get => InternalObject.releaseRight;
	set => InternalObject.releaseRight = value;
}		public virtual bool ReleaseSmart
{
	get => InternalObject.releaseSmart;
	set => InternalObject.releaseSmart = value;
}		public virtual bool ReleaseThrow
{
	get => InternalObject.releaseThrow;
	set => InternalObject.releaseThrow = value;
}		public virtual bool ReleaseUp
{
	get => InternalObject.releaseUp;
	set => InternalObject.releaseUp = value;
}		public virtual bool ReleaseUseItem
{
	get => InternalObject.releaseUseItem;
	set => InternalObject.releaseUseItem = value;
}		public virtual bool ReleaseUseTile
{
	get => InternalObject.releaseUseTile;
	set => InternalObject.releaseUseTile = value;
}		public virtual bool ResistCold
{
	get => InternalObject.resistCold;
	set => InternalObject.resistCold = value;
}		public virtual bool RocketFrame
{
	get => InternalObject.rocketFrame;
	set => InternalObject.rocketFrame = value;
}		public virtual bool RocketRelease
{
	get => InternalObject.rocketRelease;
	set => InternalObject.rocketRelease = value;
}		public virtual bool RulerGrid
{
	get => InternalObject.rulerGrid;
	set => InternalObject.rulerGrid = value;
}		public virtual bool RulerLine
{
	get => InternalObject.rulerLine;
	set => InternalObject.rulerLine = value;
}		public virtual bool RunningOnSand
{
	get => InternalObject.runningOnSand;
	set => InternalObject.runningOnSand = value;
}		public virtual bool SailDash
{
	get => InternalObject.sailDash;
	set => InternalObject.sailDash = value;
}		public virtual bool SandStorm
{
	get => InternalObject.sandStorm;
	set => InternalObject.sandStorm = value;
}		public virtual bool Sapling
{
	get => InternalObject.sapling;
	set => InternalObject.sapling = value;
}		public virtual bool Scope
{
	get => InternalObject.scope;
	set => InternalObject.scope = value;
}		public virtual bool SetApprenticeT2
{
	get => InternalObject.setApprenticeT2;
	set => InternalObject.setApprenticeT2 = value;
}		public virtual bool SetApprenticeT3
{
	get => InternalObject.setApprenticeT3;
	set => InternalObject.setApprenticeT3 = value;
}		public virtual bool SetForbidden
{
	get => InternalObject.setForbidden;
	set => InternalObject.setForbidden = value;
}		public virtual bool SetForbiddenCooldownLocked
{
	get => InternalObject.setForbiddenCooldownLocked;
	set => InternalObject.setForbiddenCooldownLocked = value;
}		public virtual bool SetHuntressT2
{
	get => InternalObject.setHuntressT2;
	set => InternalObject.setHuntressT2 = value;
}		public virtual bool SetHuntressT3
{
	get => InternalObject.setHuntressT3;
	set => InternalObject.setHuntressT3 = value;
}		public virtual bool SetMonkT2
{
	get => InternalObject.setMonkT2;
	set => InternalObject.setMonkT2 = value;
}		public virtual bool SetMonkT3
{
	get => InternalObject.setMonkT3;
	set => InternalObject.setMonkT3 = value;
}		public virtual bool SetNebula
{
	get => InternalObject.setNebula;
	set => InternalObject.setNebula = value;
}		public virtual bool SetSolar
{
	get => InternalObject.setSolar;
	set => InternalObject.setSolar = value;
}		public virtual bool SetSquireT2
{
	get => InternalObject.setSquireT2;
	set => InternalObject.setSquireT2 = value;
}		public virtual bool SetSquireT3
{
	get => InternalObject.setSquireT3;
	set => InternalObject.setSquireT3 = value;
}		public virtual bool SetStardust
{
	get => InternalObject.setStardust;
	set => InternalObject.setStardust = value;
}		public virtual bool SetVortex
{
	get => InternalObject.setVortex;
	set => InternalObject.setVortex = value;
}		public virtual bool ShadowDodge
{
	get => InternalObject.shadowDodge;
	set => InternalObject.shadowDodge = value;
}		public virtual bool SharknadoMinion
{
	get => InternalObject.sharknadoMinion;
	set => InternalObject.sharknadoMinion = value;
}		public virtual bool ShieldRaised
{
	get => InternalObject.shieldRaised;
	set => InternalObject.shieldRaised = value;
}		public virtual bool ShinyStone
{
	get => InternalObject.shinyStone;
	set => InternalObject.shinyStone = value;
}		public virtual bool ShowLastDeath
{
	get => InternalObject.showLastDeath;
	set => InternalObject.showLastDeath = value;
}		public virtual bool ShroomiteStealth
{
	get => InternalObject.shroomiteStealth;
	set => InternalObject.shroomiteStealth = value;
}		public virtual bool Silence
{
	get => InternalObject.silence;
	set => InternalObject.silence = value;
}		public virtual bool Skeletron
{
	get => InternalObject.skeletron;
	set => InternalObject.skeletron = value;
}		public virtual bool SkipAnimatingValuesInPlayerFrame
{
	get => InternalObject.skipAnimatingValuesInPlayerFrame;
	set => InternalObject.skipAnimatingValuesInPlayerFrame = value;
}		public virtual bool SkyStoneEffects
{
	get => InternalObject.skyStoneEffects;
	set => InternalObject.skyStoneEffects = value;
}		public virtual bool Sliding
{
	get => InternalObject.sliding;
	set => InternalObject.sliding = value;
}		public virtual bool Slime
{
	get => InternalObject.slime;
	set => InternalObject.slime = value;
}		public virtual bool Slippy
{
	get => InternalObject.slippy;
	set => InternalObject.slippy = value;
}		public virtual bool Slippy2
{
	get => InternalObject.slippy2;
	set => InternalObject.slippy2 = value;
}		public virtual bool Sloping
{
	get => InternalObject.sloping;
	set => InternalObject.sloping = value;
}		public virtual bool Slow
{
	get => InternalObject.slow;
	set => InternalObject.slow = value;
}		public virtual bool SlowFall
{
	get => InternalObject.slowFall;
	set => InternalObject.slowFall = value;
}		public virtual bool SlowOgreSpit
{
	get => InternalObject.slowOgreSpit;
	set => InternalObject.slowOgreSpit = value;
}		public virtual bool Smolstar
{
	get => InternalObject.smolstar;
	set => InternalObject.smolstar = value;
}		public virtual bool Snowman
{
	get => InternalObject.snowman;
	set => InternalObject.snowman = value;
}		public virtual bool SocialGhost
{
	get => InternalObject.socialGhost;
	set => InternalObject.socialGhost = value;
}		public virtual bool SocialIgnoreLight
{
	get => InternalObject.socialIgnoreLight;
	set => InternalObject.socialIgnoreLight = value;
}		public virtual bool SocialShadowRocketBoots
{
	get => InternalObject.socialShadowRocketBoots;
	set => InternalObject.socialShadowRocketBoots = value;
}		public virtual bool SolarDashConsumedFlare
{
	get => InternalObject.solarDashConsumedFlare;
	set => InternalObject.solarDashConsumedFlare = value;
}		public virtual bool SolarDashing
{
	get => InternalObject.solarDashing;
	set => InternalObject.solarDashing = value;
}		public virtual bool SonarPotion
{
	get => InternalObject.sonarPotion;
	set => InternalObject.sonarPotion = value;
}		public virtual bool SpaceGun
{
	get => InternalObject.spaceGun;
	set => InternalObject.spaceGun = value;
}		public virtual bool SpawnMax
{
	get => InternalObject.spawnMax;
	set => InternalObject.spawnMax = value;
}		public virtual bool Spider
{
	get => InternalObject.spider;
	set => InternalObject.spider = value;
}		public virtual bool SpiderArmor
{
	get => InternalObject.spiderArmor;
	set => InternalObject.spiderArmor = value;
}		public virtual bool SpiderMinion
{
	get => InternalObject.spiderMinion;
	set => InternalObject.spiderMinion = value;
}		public virtual bool SporeSac
{
	get => InternalObject.sporeSac;
	set => InternalObject.sporeSac = value;
}		public virtual bool Squashling
{
	get => InternalObject.squashling;
	set => InternalObject.squashling = value;
}		public virtual bool StairFall
{
	get => InternalObject.stairFall;
	set => InternalObject.stairFall = value;
}		public virtual bool StardustDragon
{
	get => InternalObject.stardustDragon;
	set => InternalObject.stardustDragon = value;
}		public virtual bool StardustGuardian
{
	get => InternalObject.stardustGuardian;
	set => InternalObject.stardustGuardian = value;
}		public virtual bool StardustMinion
{
	get => InternalObject.stardustMinion;
	set => InternalObject.stardustMinion = value;
}		public virtual bool Starving
{
	get => InternalObject.starving;
	set => InternalObject.starving = value;
}		public virtual bool Sticky
{
	get => InternalObject.sticky;
	set => InternalObject.sticky = value;
}		public virtual bool Stinky
{
	get => InternalObject.stinky;
	set => InternalObject.stinky = value;
}		public virtual bool Stoned
{
	get => InternalObject.stoned;
	set => InternalObject.stoned = value;
}		public virtual bool StormTiger
{
	get => InternalObject.stormTiger;
	set => InternalObject.stormTiger = value;
}		public virtual bool StrongBees
{
	get => InternalObject.strongBees;
	set => InternalObject.strongBees = value;
}		public virtual bool Suffocating
{
	get => InternalObject.suffocating;
	set => InternalObject.suffocating = value;
}		public virtual bool Sunflower
{
	get => InternalObject.sunflower;
	set => InternalObject.sunflower = value;
}		public virtual bool SuspiciouslookingTentacle
{
	get => InternalObject.suspiciouslookingTentacle;
	set => InternalObject.suspiciouslookingTentacle = value;
}		public virtual bool TankPetReset
{
	get => InternalObject.tankPetReset;
	set => InternalObject.tankPetReset = value;
}		public virtual bool Teleporting
{
	get => InternalObject.teleporting;
	set => InternalObject.teleporting = value;
}		public virtual bool Tiki
{
	get => InternalObject.tiki;
	set => InternalObject.tiki = value;
}		public virtual bool TileInteractAttempted
{
	get => InternalObject.tileInteractAttempted;
	set => InternalObject.tileInteractAttempted = value;
}		public virtual bool TileInteractionHappened
{
	get => InternalObject.tileInteractionHappened;
	set => InternalObject.tileInteractionHappened = value;
}		public virtual bool Tipsy
{
	get => InternalObject.tipsy;
	set => InternalObject.tipsy = value;
}		public virtual bool Tongued
{
	get => InternalObject.tongued;
	set => InternalObject.tongued = value;
}		public virtual bool TrapDebuffSource
{
	get => InternalObject.trapDebuffSource;
	set => InternalObject.trapDebuffSource = value;
}		public virtual bool TreasureMagnet
{
	get => InternalObject.treasureMagnet;
	set => InternalObject.treasureMagnet = value;
}		public virtual bool Trident
{
	get => InternalObject.trident;
	set => InternalObject.trident = value;
}		public virtual bool Truffle
{
	get => InternalObject.truffle;
	set => InternalObject.truffle = value;
}		public virtual bool TryKeepingHoveringDown
{
	get => InternalObject.tryKeepingHoveringDown;
	set => InternalObject.tryKeepingHoveringDown = value;
}		public virtual bool TryKeepingHoveringUp
{
	get => InternalObject.tryKeepingHoveringUp;
	set => InternalObject.tryKeepingHoveringUp = value;
}		public virtual bool Turtle
{
	get => InternalObject.turtle;
	set => InternalObject.turtle = value;
}		public virtual bool TurtleArmor
{
	get => InternalObject.turtleArmor;
	set => InternalObject.turtleArmor = value;
}		public virtual bool TurtleThorns
{
	get => InternalObject.turtleThorns;
	set => InternalObject.turtleThorns = value;
}		public virtual bool TwinsMinion
{
	get => InternalObject.twinsMinion;
	set => InternalObject.twinsMinion = value;
}		public virtual bool UFOMinion
{
	get => InternalObject.UFOMinion;
	set => InternalObject.UFOMinion = value;
}		public virtual bool UnlockedBiomeTorches
{
	get => InternalObject.unlockedBiomeTorches;
	set => InternalObject.unlockedBiomeTorches = value;
}		public virtual bool VampireFrog
{
	get => InternalObject.vampireFrog;
	set => InternalObject.vampireFrog = value;
}		public virtual bool Venom
{
	get => InternalObject.venom;
	set => InternalObject.venom = value;
}		public virtual bool VolatileGelatin
{
	get => InternalObject.volatileGelatin;
	set => InternalObject.volatileGelatin = value;
}		public virtual bool VortexDebuff
{
	get => InternalObject.vortexDebuff;
	set => InternalObject.vortexDebuff = value;
}		public virtual bool VortexStealthActive
{
	get => InternalObject.vortexStealthActive;
	set => InternalObject.vortexStealthActive = value;
}		public virtual bool WaterWalk
{
	get => InternalObject.waterWalk;
	set => InternalObject.waterWalk = value;
}		public virtual bool WaterWalk2
{
	get => InternalObject.waterWalk2;
	set => InternalObject.waterWalk2 = value;
}		public virtual bool WearsRobe
{
	get => InternalObject.wearsRobe;
	set => InternalObject.wearsRobe = value;
}		public virtual bool Webbed
{
	get => InternalObject.webbed;
	set => InternalObject.webbed = value;
}		public virtual bool WellFed
{
	get => InternalObject.wellFed;
	set => InternalObject.wellFed = value;
}		public virtual bool WereWolf
{
	get => InternalObject.wereWolf;
	set => InternalObject.wereWolf = value;
}		public virtual bool WindPushed
{
	get => InternalObject.windPushed;
	set => InternalObject.windPushed = value;
}		public virtual bool Wisp
{
	get => InternalObject.wisp;
	set => InternalObject.wisp = value;
}		public virtual bool WitheredArmor
{
	get => InternalObject.witheredArmor;
	set => InternalObject.witheredArmor = value;
}		public virtual bool WitheredWeapon
{
	get => InternalObject.witheredWeapon;
	set => InternalObject.witheredWeapon = value;
}		public virtual bool WolfAcc
{
	get => InternalObject.wolfAcc;
	set => InternalObject.wolfAcc = value;
}		public virtual bool Yoraiz0rDarkness
{
	get => InternalObject.yoraiz0rDarkness;
	set => InternalObject.yoraiz0rDarkness = value;
}		public virtual bool YoyoGlove
{
	get => InternalObject.yoyoGlove;
	set => InternalObject.yoyoGlove = value;
}		public virtual bool YoyoString
{
	get => InternalObject.yoyoString;
	set => InternalObject.yoyoString = value;
}		public virtual bool Zephyrfish
{
	get => InternalObject.zephyrfish;
	set => InternalObject.zephyrfish = value;
}		public virtual GameObjectArrayV<bool> AdjTile
{
	get => new GameObjectArrayV<bool>(Context, InternalObject._adjTile);
	set => InternalObject._adjTile = value.InternalObject;
}		public virtual GameObjectArrayV<bool> OldAdjTile
{
	get => new GameObjectArrayV<bool>(Context, InternalObject._oldAdjTile);
	set => InternalObject._oldAdjTile = value.InternalObject;
}		public virtual GameObjectArrayV<bool> BuffImmune
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.buffImmune);
	set => InternalObject.buffImmune = value.InternalObject;
}		public virtual GameObjectArrayV<bool> HideInfo
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.hideInfo);
	set => InternalObject.hideInfo = value.InternalObject;
}		public virtual GameObjectArrayV<bool> HideVisibleAccessory
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.hideVisibleAccessory);
	set => InternalObject.hideVisibleAccessory = value.InternalObject;
}		public virtual GameObjectArrayV<bool> InventoryChestStack
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.inventoryChestStack);
	set => InternalObject.inventoryChestStack = value.InternalObject;
}		public virtual GameObjectArrayV<bool> NearbyTorch
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.nearbyTorch);
	set => InternalObject.nearbyTorch = value.InternalObject;
}		public virtual GameObjectArrayV<bool> NpcTypeNoAggro
{
	get => new GameObjectArrayV<bool>(Context, InternalObject.npcTypeNoAggro);
	set => InternalObject.npcTypeNoAggro = value.InternalObject;
}		public virtual byte AccCritterGuideCounter
{
	get => InternalObject.accCritterGuideCounter;
	set => InternalObject.accCritterGuideCounter = value;
}		public virtual byte AccCritterGuideNumber
{
	get => InternalObject.accCritterGuideNumber;
	set => InternalObject.accCritterGuideNumber = value;
}		public virtual byte AccThirdEyeCounter
{
	get => InternalObject.accThirdEyeCounter;
	set => InternalObject.accThirdEyeCounter = value;
}		public virtual byte AccThirdEyeNumber
{
	get => InternalObject.accThirdEyeNumber;
	set => InternalObject.accThirdEyeNumber = value;
}		public virtual byte Difficulty
{
	get => InternalObject.difficulty;
	set => InternalObject.difficulty = value;
}		public virtual byte FlameRingAlpha
{
	get => InternalObject.flameRingAlpha;
	set => InternalObject.flameRingAlpha = value;
}		public virtual byte FlameRingFrame
{
	get => InternalObject.flameRingFrame;
	set => InternalObject.flameRingFrame = value;
}		public virtual byte IceBarrierFrame
{
	get => InternalObject.iceBarrierFrame;
	set => InternalObject.iceBarrierFrame = value;
}		public virtual byte IceBarrierFrameCounter
{
	get => InternalObject.iceBarrierFrameCounter;
	set => InternalObject.iceBarrierFrameCounter = value;
}		public virtual byte LuckPotion
{
	get => InternalObject.luckPotion;
	set => InternalObject.luckPotion = value;
}		public virtual byte MeleeEnchant
{
	get => InternalObject.meleeEnchant;
	set => InternalObject.meleeEnchant = value;
}		public virtual byte OldLuckPotion
{
	get => InternalObject.oldLuckPotion;
	set => InternalObject.oldLuckPotion = value;
}		public virtual byte PulleyDir
{
	get => InternalObject.pulleyDir;
	set => InternalObject.pulleyDir = value;
}		public virtual byte SpelunkerTimer
{
	get => InternalObject.spelunkerTimer;
	set => InternalObject.spelunkerTimer = value;
}		public virtual byte SuffocateDelay
{
	get => InternalObject.suffocateDelay;
	set => InternalObject.suffocateDelay = value;
}		public virtual byte WetSlime
{
	get => InternalObject.wetSlime;
	set => InternalObject.wetSlime = value;
}		public virtual GameObjectArrayV<byte> ENCRYPTION_KEY
{
	get => new GameObjectArrayV<byte>(Context, InternalObject.ENCRYPTION_KEY);
	set => InternalObject.ENCRYPTION_KEY = value.InternalObject;
}		public virtual System.DateTime DpsEnd
{
	get => InternalObject.dpsEnd;
	set => InternalObject.dpsEnd = value;
}		public virtual System.DateTime DpsLastHit
{
	get => InternalObject.dpsLastHit;
	set => InternalObject.dpsLastHit = value;
}		public virtual System.DateTime DpsStart
{
	get => InternalObject.dpsStart;
	set => InternalObject.dpsStart = value;
}		public virtual System.DateTime LastDeathTime
{
	get => InternalObject.lastDeathTime;
	set => InternalObject.lastDeathTime = value;
}		public virtual double BodyFrameCounter
{
	get => InternalObject.bodyFrameCounter;
	set => InternalObject.bodyFrameCounter = value;
}		public virtual double HeadFrameCounter
{
	get => InternalObject.headFrameCounter;
	set => InternalObject.headFrameCounter = value;
}		public virtual double LadyBugLuckTimeLeft
{
	get => InternalObject.ladyBugLuckTimeLeft;
	set => InternalObject.ladyBugLuckTimeLeft = value;
}		public virtual double LegFrameCounter
{
	get => InternalObject.legFrameCounter;
	set => InternalObject.legFrameCounter = value;
}		public virtual double TaxRate
{
	get => InternalObject.taxRate;
	set => InternalObject.taxRate = value;
}		public virtual double TaxTimer
{
	get => InternalObject.taxTimer;
	set => InternalObject.taxTimer = value;
}		public virtual int FramesLeftEligibleForDeadmansChestDeathAchievement
{
	get => InternalObject._framesLeftEligibleForDeadmansChestDeathAchievement;
	set => InternalObject._framesLeftEligibleForDeadmansChestDeathAchievement = value;
}		public virtual int FunkytownAchievementCheckCooldown
{
	get => InternalObject._funkytownAchievementCheckCooldown;
	set => InternalObject._funkytownAchievementCheckCooldown = value;
}		public virtual int ImmuneStrikes
{
	get => InternalObject._immuneStrikes;
	set => InternalObject._immuneStrikes = value;
}		public virtual int LastAddedAvancedShadow
{
	get => InternalObject._lastAddedAvancedShadow;
	set => InternalObject._lastAddedAvancedShadow = value;
}		public virtual int LastSmartCursorToolStrategy
{
	get => InternalObject._lastSmartCursorToolStrategy;
	set => InternalObject._lastSmartCursorToolStrategy = value;
}		public virtual int LockTileInteractionsTimer
{
	get => InternalObject._lockTileInteractionsTimer;
	set => InternalObject._lockTileInteractionsTimer = value;
}		public virtual int PortalPhysicsTime
{
	get => InternalObject._portalPhysicsTime;
	set => InternalObject._portalPhysicsTime = value;
}		public virtual int QuickGrappleCooldown
{
	get => InternalObject._quickGrappleCooldown;
	set => InternalObject._quickGrappleCooldown = value;
}		public virtual int TimeSinceLastImmuneGet
{
	get => InternalObject._timeSinceLastImmuneGet;
	set => InternalObject._timeSinceLastImmuneGet = value;
}		public virtual int AccCompass
{
	get => InternalObject.accCompass;
	set => InternalObject.accCompass = value;
}		public virtual int AccDepthMeter
{
	get => InternalObject.accDepthMeter;
	set => InternalObject.accDepthMeter = value;
}		public virtual int AccWatch
{
	get => InternalObject.accWatch;
	set => InternalObject.accWatch = value;
}		public virtual int AfkCounter
{
	get => InternalObject.afkCounter;
	set => InternalObject.afkCounter = value;
}		public virtual int Aggro
{
	get => InternalObject.aggro;
	set => InternalObject.aggro = value;
}		public virtual int AltFunctionUse
{
	get => InternalObject.altFunctionUse;
	set => InternalObject.altFunctionUse = value;
}		public virtual int AnglerQuestsFinished
{
	get => InternalObject.anglerQuestsFinished;
	set => InternalObject.anglerQuestsFinished = value;
}		public virtual int AttackCD
{
	get => InternalObject.attackCD;
	set => InternalObject.attackCD = value;
}		public virtual int AvailableAdvancedShadowsCount
{
	get => InternalObject.availableAdvancedShadowsCount;
	set => InternalObject.availableAdvancedShadowsCount = value;
}		public virtual int BartenderQuestLog
{
	get => InternalObject.bartenderQuestLog;
	set => InternalObject.bartenderQuestLog = value;
}		public virtual int BeardGrowthTimer
{
	get => InternalObject.beardGrowthTimer;
	set => InternalObject.beardGrowthTimer = value;
}		public virtual int BeetleCountdown
{
	get => InternalObject.beetleCountdown;
	set => InternalObject.beetleCountdown = value;
}		public virtual int BeetleFrame
{
	get => InternalObject.beetleFrame;
	set => InternalObject.beetleFrame = value;
}		public virtual int BeetleFrameCounter
{
	get => InternalObject.beetleFrameCounter;
	set => InternalObject.beetleFrameCounter = value;
}		public virtual int BeetleOrbs
{
	get => InternalObject.beetleOrbs;
	set => InternalObject.beetleOrbs = value;
}		public virtual int BlockInteractionWithProjectiles
{
	get => InternalObject.BlockInteractionWithProjectiles;
	set => InternalObject.BlockInteractionWithProjectiles = value;
}		public virtual int BlockRange
{
	get => InternalObject.blockRange;
	set => InternalObject.blockRange = value;
}		public virtual int Body
{
	get => InternalObject.body;
	set => InternalObject.body = value;
}		public virtual int BoneGloveTimer
{
	get => InternalObject.boneGloveTimer;
	set => InternalObject.boneGloveTimer = value;
}		public virtual int BrainOfConfusionDodgeAnimationCounter
{
	get => InternalObject.brainOfConfusionDodgeAnimationCounter;
	set => InternalObject.brainOfConfusionDodgeAnimationCounter = value;
}		public virtual int Breath
{
	get => InternalObject.breath;
	set => InternalObject.breath = value;
}		public virtual int BreathCD
{
	get => InternalObject.breathCD;
	set => InternalObject.breathCD = value;
}		public virtual int BreathMax
{
	get => InternalObject.breathMax;
	set => InternalObject.breathMax = value;
}		public virtual int CAngelHalo
{
	get => InternalObject.cAngelHalo;
	set => InternalObject.cAngelHalo = value;
}		public virtual int CarpetFrame
{
	get => InternalObject.carpetFrame;
	set => InternalObject.carpetFrame = value;
}		public virtual int CarpetTime
{
	get => InternalObject.carpetTime;
	set => InternalObject.carpetTime = value;
}		public virtual int CartRampTime
{
	get => InternalObject.cartRampTime;
	set => InternalObject.cartRampTime = value;
}		public virtual int CBack
{
	get => InternalObject.cBack;
	set => InternalObject.cBack = value;
}		public virtual int CBackpack
{
	get => InternalObject.cBackpack;
	set => InternalObject.cBackpack = value;
}		public virtual int CBalloon
{
	get => InternalObject.cBalloon;
	set => InternalObject.cBalloon = value;
}		public virtual int CBalloonFront
{
	get => InternalObject.cBalloonFront;
	set => InternalObject.cBalloonFront = value;
}		public virtual int CBeard
{
	get => InternalObject.cBeard;
	set => InternalObject.cBeard = value;
}		public virtual int CBody
{
	get => InternalObject.cBody;
	set => InternalObject.cBody = value;
}		public virtual int CCarpet
{
	get => InternalObject.cCarpet;
	set => InternalObject.cCarpet = value;
}		public virtual int CFace
{
	get => InternalObject.cFace;
	set => InternalObject.cFace = value;
}		public virtual int CFaceFlower
{
	get => InternalObject.cFaceFlower;
	set => InternalObject.cFaceFlower = value;
}		public virtual int CFaceHead
{
	get => InternalObject.cFaceHead;
	set => InternalObject.cFaceHead = value;
}		public virtual int CFloatingTube
{
	get => InternalObject.cFloatingTube;
	set => InternalObject.cFloatingTube = value;
}		public virtual int CFront
{
	get => InternalObject.cFront;
	set => InternalObject.cFront = value;
}		public virtual int CGrapple
{
	get => InternalObject.cGrapple;
	set => InternalObject.cGrapple = value;
}		public virtual int CHandOff
{
	get => InternalObject.cHandOff;
	set => InternalObject.cHandOff = value;
}		public virtual int CHandOn
{
	get => InternalObject.cHandOn;
	set => InternalObject.cHandOn = value;
}		public virtual int ChangeItem
{
	get => InternalObject.changeItem;
	set => InternalObject.changeItem = value;
}		public virtual int CHead
{
	get => InternalObject.cHead;
	set => InternalObject.cHead = value;
}		public virtual int Chest
{
	get => InternalObject.chest;
	set => InternalObject.chest = value;
}		public virtual int ChestX
{
	get => InternalObject.chestX;
	set => InternalObject.chestX = value;
}		public virtual int ChestY
{
	get => InternalObject.chestY;
	set => InternalObject.chestY = value;
}		public virtual int CLegs
{
	get => InternalObject.cLegs;
	set => InternalObject.cLegs = value;
}		public virtual int CLeinShampoo
{
	get => InternalObject.cLeinShampoo;
	set => InternalObject.cLeinShampoo = value;
}		public virtual int CLight
{
	get => InternalObject.cLight;
	set => InternalObject.cLight = value;
}		public virtual int CMinecart
{
	get => InternalObject.cMinecart;
	set => InternalObject.cMinecart = value;
}		public virtual int CMinion
{
	get => InternalObject.cMinion;
	set => InternalObject.cMinion = value;
}		public virtual int CMount
{
	get => InternalObject.cMount;
	set => InternalObject.cMount = value;
}		public virtual int CNeck
{
	get => InternalObject.cNeck;
	set => InternalObject.cNeck = value;
}		public virtual int CounterWeight
{
	get => InternalObject.counterWeight;
	set => InternalObject.counterWeight = value;
}		public virtual int CPet
{
	get => InternalObject.cPet;
	set => InternalObject.cPet = value;
}		public virtual int CPortalbeStool
{
	get => InternalObject.cPortalbeStool;
	set => InternalObject.cPortalbeStool = value;
}		public virtual int CrystalLeafDamage
{
	get => InternalObject.crystalLeafDamage;
	set => InternalObject.crystalLeafDamage = value;
}		public virtual int CrystalLeafKB
{
	get => InternalObject.crystalLeafKB;
	set => InternalObject.crystalLeafKB = value;
}		public virtual int CShield
{
	get => InternalObject.cShield;
	set => InternalObject.cShield = value;
}		public virtual int CShieldFallback
{
	get => InternalObject.cShieldFallback;
	set => InternalObject.cShieldFallback = value;
}		public virtual int CShoe
{
	get => InternalObject.cShoe;
	set => InternalObject.cShoe = value;
}		public virtual int CTail
{
	get => InternalObject.cTail;
	set => InternalObject.cTail = value;
}		public virtual int CUnicornHorn
{
	get => InternalObject.cUnicornHorn;
	set => InternalObject.cUnicornHorn = value;
}		public virtual int CursorItemIconID
{
	get => InternalObject.cursorItemIconID;
	set => InternalObject.cursorItemIconID = value;
}		public virtual int CWaist
{
	get => InternalObject.cWaist;
	set => InternalObject.cWaist = value;
}		public virtual int CWings
{
	get => InternalObject.cWings;
	set => InternalObject.cWings = value;
}		public virtual int CYorai
{
	get => InternalObject.cYorai;
	set => InternalObject.cYorai = value;
}		public virtual int Dash
{
	get => InternalObject.dash;
	set => InternalObject.dash = value;
}		public virtual int DashDelay
{
	get => InternalObject.dashDelay;
	set => InternalObject.dashDelay = value;
}		public virtual int DashTime
{
	get => InternalObject.dashTime;
	set => InternalObject.dashTime = value;
}		public virtual int DashType
{
	get => InternalObject.dashType;
	set => InternalObject.dashType = value;
}		public virtual int DefaultItemGrabRange
{
	get => InternalObject.defaultItemGrabRange;
	set => InternalObject.defaultItemGrabRange = value;
}		public virtual int DpsDamage
{
	get => InternalObject.dpsDamage;
	set => InternalObject.dpsDamage = value;
}		public virtual int EmoteTime
{
	get => InternalObject.emoteTime;
	set => InternalObject.emoteTime = value;
}		public virtual int EnvironmentBuffImmunityTimer
{
	get => InternalObject.environmentBuffImmunityTimer;
	set => InternalObject.environmentBuffImmunityTimer = value;
}		public virtual int EocDash
{
	get => InternalObject.eocDash;
	set => InternalObject.eocDash = value;
}		public virtual int EocHit
{
	get => InternalObject.eocHit;
	set => InternalObject.eocHit = value;
}		public virtual int ExtraAccessorySlots
{
	get => InternalObject.extraAccessorySlots;
	set => InternalObject.extraAccessorySlots = value;
}		public virtual int ExtraFall
{
	get => InternalObject.extraFall;
	set => InternalObject.extraFall = value;
}		public virtual int FallStart
{
	get => InternalObject.fallStart;
	set => InternalObject.fallStart = value;
}		public virtual int FallStart2
{
	get => InternalObject.fallStart2;
	set => InternalObject.fallStart2 = value;
}		public virtual int FishingSkill
{
	get => InternalObject.fishingSkill;
	set => InternalObject.fishingSkill = value;
}		public virtual int Gem
{
	get => InternalObject.gem;
	set => InternalObject.gem = value;
}		public virtual int GemCount
{
	get => InternalObject.gemCount;
	set => InternalObject.gemCount = value;
}		public virtual int GhostFrame
{
	get => InternalObject.ghostFrame;
	set => InternalObject.ghostFrame = value;
}		public virtual int GhostFrameCounter
{
	get => InternalObject.ghostFrameCounter;
	set => InternalObject.ghostFrameCounter = value;
}		public virtual int GolferScoreAccumulated
{
	get => InternalObject.golferScoreAccumulated;
	set => InternalObject.golferScoreAccumulated = value;
}		public virtual int GrapCount
{
	get => InternalObject.grapCount;
	set => InternalObject.grapCount = value;
}		public virtual int GraveImmediateTime
{
	get => InternalObject.graveImmediateTime;
	set => InternalObject.graveImmediateTime = value;
}		public virtual int Hair
{
	get => InternalObject.hair;
	set => InternalObject.hair = value;
}		public virtual int HairDye
{
	get => InternalObject.hairDye;
	set => InternalObject.hairDye = value;
}		public virtual int Head
{
	get => InternalObject.head;
	set => InternalObject.head = value;
}		public virtual int HeldProj
{
	get => InternalObject.heldProj;
	set => InternalObject.heldProj = value;
}		public virtual int HighestAbigailCounterOriginalDamage
{
	get => InternalObject.highestAbigailCounterOriginalDamage;
	set => InternalObject.highestAbigailCounterOriginalDamage = value;
}		public virtual int HighestStormTigerGemOriginalDamage
{
	get => InternalObject.highestStormTigerGemOriginalDamage;
	set => InternalObject.highestStormTigerGemOriginalDamage = value;
}		public virtual int HotbarOffset
{
	get => InternalObject.HotbarOffset;
	set => InternalObject.HotbarOffset = value;
}		public virtual int ImmuneAlpha
{
	get => InternalObject.immuneAlpha;
	set => InternalObject.immuneAlpha = value;
}		public virtual int ImmuneAlphaDirection
{
	get => InternalObject.immuneAlphaDirection;
	set => InternalObject.immuneAlphaDirection = value;
}		public virtual int ImmuneTime
{
	get => InternalObject.immuneTime;
	set => InternalObject.immuneTime = value;
}		public virtual int InfernoCounter
{
	get => InternalObject.infernoCounter;
	set => InternalObject.infernoCounter = value;
}		public virtual int InsanityShadowCooldown
{
	get => InternalObject.insanityShadowCooldown;
	set => InternalObject.insanityShadowCooldown = value;
}		public virtual int ItemAnimation
{
	get => InternalObject.itemAnimation;
	set => InternalObject.itemAnimation = value;
}		public virtual int ItemAnimationMax
{
	get => InternalObject.itemAnimationMax;
	set => InternalObject.itemAnimationMax = value;
}		public virtual int ItemFlameCount
{
	get => InternalObject.itemFlameCount;
	set => InternalObject.itemFlameCount = value;
}		public virtual int ItemHeight
{
	get => InternalObject.itemHeight;
	set => InternalObject.itemHeight = value;
}		public virtual int ItemTime
{
	get => InternalObject.itemTime;
	set => InternalObject.itemTime = value;
}		public virtual int ItemTimeMax
{
	get => InternalObject.itemTimeMax;
	set => InternalObject.itemTimeMax = value;
}		public virtual int ItemWidth
{
	get => InternalObject.itemWidth;
	set => InternalObject.itemWidth = value;
}		public virtual int Jump
{
	get => InternalObject.jump;
	set => InternalObject.jump = value;
}		public virtual int JumpHeight
{
	get => InternalObject.jumpHeight;
	set => InternalObject.jumpHeight = value;
}		public virtual int LastChest
{
	get => InternalObject.lastChest;
	set => InternalObject.lastChest = value;
}		public virtual int LastCreatureHit
{
	get => InternalObject.lastCreatureHit;
	set => InternalObject.lastCreatureHit = value;
}		public virtual int LastPortalColorIndex
{
	get => InternalObject.lastPortalColorIndex;
	set => InternalObject.lastPortalColorIndex = value;
}		public virtual int LastTeleportPylonStyleUsed
{
	get => InternalObject.lastTeleportPylonStyleUsed;
	set => InternalObject.lastTeleportPylonStyleUsed = value;
}		public virtual int LastTileRangeX
{
	get => InternalObject.lastTileRangeX;
	set => InternalObject.lastTileRangeX = value;
}		public virtual int LastTileRangeY
{
	get => InternalObject.lastTileRangeY;
	set => InternalObject.lastTileRangeY = value;
}		public virtual int LavaCD
{
	get => InternalObject.lavaCD;
	set => InternalObject.lavaCD = value;
}		public virtual int LavaMax
{
	get => InternalObject.lavaMax;
	set => InternalObject.lavaMax = value;
}		public virtual int LavaTime
{
	get => InternalObject.lavaTime;
	set => InternalObject.lavaTime = value;
}		public virtual int LeftTimer
{
	get => InternalObject.leftTimer;
	set => InternalObject.leftTimer = value;
}		public virtual int Legs
{
	get => InternalObject.legs;
	set => InternalObject.legs = value;
}		public virtual int LifeRegen
{
	get => InternalObject.lifeRegen;
	set => InternalObject.lifeRegen = value;
}		public virtual int LifeRegenCount
{
	get => InternalObject.lifeRegenCount;
	set => InternalObject.lifeRegenCount = value;
}		public virtual int LifeRegenTime
{
	get => InternalObject.lifeRegenTime;
	set => InternalObject.lifeRegenTime = value;
}		public virtual int LoadStatus
{
	get => InternalObject.loadStatus;
	set => InternalObject.loadStatus = value;
}		public virtual int LostCoins
{
	get => InternalObject.lostCoins;
	set => InternalObject.lostCoins = value;
}		public virtual int LuckyTorchCounter
{
	get => InternalObject.luckyTorchCounter;
	set => InternalObject.luckyTorchCounter = value;
}		public virtual int ManaRegen
{
	get => InternalObject.manaRegen;
	set => InternalObject.manaRegen = value;
}		public virtual int ManaRegenBonus
{
	get => InternalObject.manaRegenBonus;
	set => InternalObject.manaRegenBonus = value;
}		public virtual int ManaRegenCount
{
	get => InternalObject.manaRegenCount;
	set => InternalObject.manaRegenCount = value;
}		public virtual int ManaRegenDelay
{
	get => InternalObject.manaRegenDelay;
	set => InternalObject.manaRegenDelay = value;
}		public virtual int ManaRegenDelayBonus
{
	get => InternalObject.manaRegenDelayBonus;
	set => InternalObject.manaRegenDelayBonus = value;
}		public virtual int ManaSickTime
{
	get => InternalObject.manaSickTime;
	set => InternalObject.manaSickTime = value;
}		public virtual int ManaSickTimeMax
{
	get => InternalObject.manaSickTimeMax;
	set => InternalObject.manaSickTimeMax = value;
}		public virtual int MaxMinions
{
	get => InternalObject.maxMinions;
	set => InternalObject.maxMinions = value;
}		public virtual int MaxTorchAttacks
{
	get => InternalObject.maxTorchAttacks;
	set => InternalObject.maxTorchAttacks = value;
}		public virtual int MaxTurrets
{
	get => InternalObject.maxTurrets;
	set => InternalObject.maxTurrets = value;
}		public virtual int MaxTurretsOld
{
	get => InternalObject.maxTurretsOld;
	set => InternalObject.maxTurretsOld = value;
}		public virtual int MinionAttackTargetNPC
{
	get => InternalObject.MinionAttackTargetNPC;
	set => InternalObject.MinionAttackTargetNPC = value;
}		public virtual int MiscCounter
{
	get => InternalObject.miscCounter;
	set => InternalObject.miscCounter = value;
}		public virtual int MiscTimer
{
	get => InternalObject.miscTimer;
	set => InternalObject.miscTimer = value;
}		public virtual int MushroomDelayTime
{
	get => InternalObject.mushroomDelayTime;
	set => InternalObject.mushroomDelayTime = value;
}		public virtual int MusicNotes
{
	get => InternalObject.musicNotes;
	set => InternalObject.musicNotes = value;
}		public virtual int NameLen
{
	get => InternalObject.nameLen;
	set => InternalObject.nameLen = value;
}		public virtual int NearbyTorches
{
	get => InternalObject.nearbyTorches;
	set => InternalObject.nearbyTorches = value;
}		public virtual int NebulaCD
{
	get => InternalObject.nebulaCD;
	set => InternalObject.nebulaCD = value;
}		public virtual int NebulaLevelDamage
{
	get => InternalObject.nebulaLevelDamage;
	set => InternalObject.nebulaLevelDamage = value;
}		public virtual int NebulaLevelLife
{
	get => InternalObject.nebulaLevelLife;
	set => InternalObject.nebulaLevelLife = value;
}		public virtual int NebulaLevelMana
{
	get => InternalObject.nebulaLevelMana;
	set => InternalObject.nebulaLevelMana = value;
}		public virtual int NebulaManaCounter
{
	get => InternalObject.nebulaManaCounter;
	set => InternalObject.nebulaManaCounter = value;
}		public virtual int NetLifeTime
{
	get => InternalObject.netLifeTime;
	set => InternalObject.netLifeTime = value;
}		public virtual int NetManaTime
{
	get => InternalObject.netManaTime;
	set => InternalObject.netManaTime = value;
}		public virtual int NetSkip
{
	get => InternalObject.netSkip;
	set => InternalObject.netSkip = value;
}		public virtual int NextCycledSpiderMinionType
{
	get => InternalObject.nextCycledSpiderMinionType;
	set => InternalObject.nextCycledSpiderMinionType = value;
}		public virtual int NonTorch
{
	get => InternalObject.nonTorch;
	set => InternalObject.nonTorch = value;
}		public virtual int NoThrow
{
	get => InternalObject.noThrow;
	set => InternalObject.noThrow = value;
}		public virtual int NumberOfTorchAttacksMade
{
	get => InternalObject.numberOfTorchAttacksMade;
	set => InternalObject.numberOfTorchAttacksMade = value;
}		public virtual int NumMinions
{
	get => InternalObject.numMinions;
	set => InternalObject.numMinions = value;
}		public virtual int OldSelectItem
{
	get => InternalObject.oldSelectItem;
	set => InternalObject.oldSelectItem = value;
}		public virtual int PetalTimer
{
	get => InternalObject.petalTimer;
	set => InternalObject.petalTimer = value;
}		public virtual int PhantasmTime
{
	get => InternalObject.phantasmTime;
	set => InternalObject.phantasmTime = value;
}		public virtual int PhantomPhoneixCounter
{
	get => InternalObject.phantomPhoneixCounter;
	set => InternalObject.phantomPhoneixCounter = value;
}		public virtual int PotionDelay
{
	get => InternalObject.potionDelay;
	set => InternalObject.potionDelay = value;
}		public virtual int PotionDelayTime
{
	get => InternalObject.potionDelayTime;
	set => InternalObject.potionDelayTime = value;
}		public virtual int PulleyFrame
{
	get => InternalObject.pulleyFrame;
	set => InternalObject.pulleyFrame = value;
}		public virtual int RespawnTimer
{
	get => InternalObject.respawnTimer;
	set => InternalObject.respawnTimer = value;
}		public virtual int RestorationDelayTime
{
	get => InternalObject.restorationDelayTime;
	set => InternalObject.restorationDelayTime = value;
}		public virtual int ReuseDelay
{
	get => InternalObject.reuseDelay;
	set => InternalObject.reuseDelay = value;
}		public virtual int RightTimer
{
	get => InternalObject.rightTimer;
	set => InternalObject.rightTimer = value;
}		public virtual int RocketBoots
{
	get => InternalObject.rocketBoots;
	set => InternalObject.rocketBoots = value;
}		public virtual int RocketDelay
{
	get => InternalObject.rocketDelay;
	set => InternalObject.rocketDelay = value;
}		public virtual int RocketDelay2
{
	get => InternalObject.rocketDelay2;
	set => InternalObject.rocketDelay2 = value;
}		public virtual int RocketSoundDelay
{
	get => InternalObject.rocketSoundDelay;
	set => InternalObject.rocketSoundDelay = value;
}		public virtual int RocketTime
{
	get => InternalObject.rocketTime;
	set => InternalObject.rocketTime = value;
}		public virtual int RocketTimeMax
{
	get => InternalObject.rocketTimeMax;
	set => InternalObject.rocketTimeMax = value;
}		public virtual int RopeCount
{
	get => InternalObject.ropeCount;
	set => InternalObject.ropeCount = value;
}		public virtual int RunSoundDelay
{
	get => InternalObject.runSoundDelay;
	set => InternalObject.runSoundDelay = value;
}		public virtual int SelectedItem
{
	get => InternalObject.selectedItem;
	set => InternalObject.selectedItem = value;
}		public virtual int ShadowCount
{
	get => InternalObject.shadowCount;
	set => InternalObject.shadowCount = value;
}		public virtual int ShadowDodgeTimer
{
	get => InternalObject.shadowDodgeTimer;
	set => InternalObject.shadowDodgeTimer = value;
}		public virtual int Shield_parry_cooldown
{
	get => InternalObject.shield_parry_cooldown;
	set => InternalObject.shield_parry_cooldown = value;
}		public virtual int ShieldParryTimeLeft
{
	get => InternalObject.shieldParryTimeLeft;
	set => InternalObject.shieldParryTimeLeft = value;
}		public virtual int Sign
{
	get => InternalObject.sign;
	set => InternalObject.sign = value;
}		public virtual int SkinDyePacked
{
	get => InternalObject.skinDyePacked;
	set => InternalObject.skinDyePacked = value;
}		public virtual int SkinVariant
{
	get => InternalObject.skinVariant;
	set => InternalObject.skinVariant = value;
}		public virtual int SlideDir
{
	get => InternalObject.slideDir;
	set => InternalObject.slideDir = value;
}		public virtual int SnowBallLauncherInteractionCooldown
{
	get => InternalObject.snowBallLauncherInteractionCooldown;
	set => InternalObject.snowBallLauncherInteractionCooldown = value;
}		public virtual int SolarCounter
{
	get => InternalObject.solarCounter;
	set => InternalObject.solarCounter = value;
}		public virtual int SolarShields
{
	get => InternalObject.solarShields;
	set => InternalObject.solarShields = value;
}		public virtual int SoulDrain
{
	get => InternalObject.soulDrain;
	set => InternalObject.soulDrain = value;
}		public virtual int SpawnX
{
	get => InternalObject.SpawnX;
	set => InternalObject.SpawnX = value;
}		public virtual int SpawnY
{
	get => InternalObject.SpawnY;
	set => InternalObject.SpawnY = value;
}		public virtual int SpikedBoots
{
	get => InternalObject.spikedBoots;
	set => InternalObject.spikedBoots = value;
}		public virtual int StarCloakCooldown
{
	get => InternalObject.starCloakCooldown;
	set => InternalObject.starCloakCooldown = value;
}		public virtual int StatDefense
{
	get => InternalObject.statDefense;
	set => InternalObject.statDefense = value;
}		public virtual int StatLife
{
	get => InternalObject.statLife;
	set => InternalObject.statLife = value;
}		public virtual int StatLifeMax
{
	get => InternalObject.statLifeMax;
	set => InternalObject.statLifeMax = value;
}		public virtual int StatLifeMax2
{
	get => InternalObject.statLifeMax2;
	set => InternalObject.statLifeMax2 = value;
}		public virtual int StatMana
{
	get => InternalObject.statMana;
	set => InternalObject.statMana = value;
}		public virtual int StatManaMax
{
	get => InternalObject.statManaMax;
	set => InternalObject.statManaMax = value;
}		public virtual int StatManaMax2
{
	get => InternalObject.statManaMax2;
	set => InternalObject.statManaMax2 = value;
}		public virtual int StealthTimer
{
	get => InternalObject.stealthTimer;
	set => InternalObject.stealthTimer = value;
}		public virtual int Step
{
	get => InternalObject.step;
	set => InternalObject.step = value;
}		public virtual int StickyBreak
{
	get => InternalObject.stickyBreak;
	set => InternalObject.stickyBreak = value;
}		public virtual int StringColor
{
	get => InternalObject.stringColor;
	set => InternalObject.stringColor = value;
}		public virtual int SwimTime
{
	get => InternalObject.swimTime;
	set => InternalObject.swimTime = value;
}		public virtual int TankPet
{
	get => InternalObject.tankPet;
	set => InternalObject.tankPet = value;
}		public virtual int TaxMoney
{
	get => InternalObject.taxMoney;
	set => InternalObject.taxMoney = value;
}		public virtual int Team
{
	get => InternalObject.team;
	set => InternalObject.team = value;
}		public virtual int TeleportStyle
{
	get => InternalObject.teleportStyle;
	set => InternalObject.teleportStyle = value;
}		public virtual int TileRangeX
{
	get => InternalObject.tileRangeX;
	set => InternalObject.tileRangeX = value;
}		public virtual int TileRangeY
{
	get => InternalObject.tileRangeY;
	set => InternalObject.tileRangeY = value;
}		public virtual int TileTargetX
{
	get => InternalObject.tileTargetX;
	set => InternalObject.tileTargetX = value;
}		public virtual int TileTargetY
{
	get => InternalObject.tileTargetY;
	set => InternalObject.tileTargetY = value;
}		public virtual int TimeSinceLastDashStarted
{
	get => InternalObject.timeSinceLastDashStarted;
	set => InternalObject.timeSinceLastDashStarted = value;
}		public virtual int ToolTime
{
	get => InternalObject.toolTime;
	set => InternalObject.toolTime = value;
}		public virtual int TorchFunTimer
{
	get => InternalObject.torchFunTimer;
	set => InternalObject.torchFunTimer = value;
}		public virtual int TorchGodCooldown
{
	get => InternalObject.torchGodCooldown;
	set => InternalObject.torchGodCooldown = value;
}		public virtual int VanityRocketBoots
{
	get => InternalObject.vanityRocketBoots;
	set => InternalObject.vanityRocketBoots = value;
}		public virtual int VolatileGelatinCounter
{
	get => InternalObject.volatileGelatinCounter;
	set => InternalObject.volatileGelatinCounter = value;
}		public virtual int WingFrame
{
	get => InternalObject.wingFrame;
	set => InternalObject.wingFrame = value;
}		public virtual int WingFrameCounter
{
	get => InternalObject.wingFrameCounter;
	set => InternalObject.wingFrameCounter = value;
}		public virtual int Wings
{
	get => InternalObject.wings;
	set => InternalObject.wings = value;
}		public virtual int WingsLogic
{
	get => InternalObject.wingsLogic;
	set => InternalObject.wingsLogic = value;
}		public virtual int WingTimeMax
{
	get => InternalObject.wingTimeMax;
	set => InternalObject.wingTimeMax = value;
}		public virtual int WireOperationsCooldown
{
	get => InternalObject.wireOperationsCooldown;
	set => InternalObject.wireOperationsCooldown = value;
}		public virtual int Yoraiz0rEye
{
	get => InternalObject.yoraiz0rEye;
	set => InternalObject.yoraiz0rEye = value;
}		public virtual GameObjectArrayV<int> TorchAttackPosX
{
	get => new GameObjectArrayV<int>(Context, InternalObject._torchAttackPosX);
	set => InternalObject._torchAttackPosX = value.InternalObject;
}		public virtual GameObjectArrayV<int> TorchAttackPosY
{
	get => new GameObjectArrayV<int>(Context, InternalObject._torchAttackPosY);
	set => InternalObject._torchAttackPosY = value.InternalObject;
}		public virtual GameObjectArrayV<int> BuffTime
{
	get => new GameObjectArrayV<int>(Context, InternalObject.buffTime);
	set => InternalObject.buffTime = value.InternalObject;
}		public virtual GameObjectArrayV<int> BuffType
{
	get => new GameObjectArrayV<int>(Context, InternalObject.buffType);
	set => InternalObject.buffType = value.InternalObject;
}		public virtual GameObjectArrayV<int> BuilderAccStatus
{
	get => new GameObjectArrayV<int>(Context, InternalObject.builderAccStatus);
	set => InternalObject.builderAccStatus = value.InternalObject;
}		public virtual GameObjectArrayV<int> DoubleTapCardinalTimer
{
	get => new GameObjectArrayV<int>(Context, InternalObject.doubleTapCardinalTimer);
	set => InternalObject.doubleTapCardinalTimer = value.InternalObject;
}		public virtual GameObjectArrayV<int> Grappling
{
	get => new GameObjectArrayV<int>(Context, InternalObject.grappling);
	set => InternalObject.grappling = value.InternalObject;
}		public virtual GameObjectArrayV<int> HoldDownCardinalTimer
{
	get => new GameObjectArrayV<int>(Context, InternalObject.holdDownCardinalTimer);
	set => InternalObject.holdDownCardinalTimer = value.InternalObject;
}		public virtual GameObjectArrayV<int> HurtCooldowns
{
	get => new GameObjectArrayV<int>(Context, InternalObject.hurtCooldowns);
	set => InternalObject.hurtCooldowns = value.InternalObject;
}		public virtual GameObjectArrayV<int> OwnedProjectileCounts
{
	get => new GameObjectArrayV<int>(Context, InternalObject.ownedProjectileCounts);
	set => InternalObject.ownedProjectileCounts = value.InternalObject;
}		public virtual GameObjectArrayV<int> ShadowDirection
{
	get => new GameObjectArrayV<int>(Context, InternalObject.shadowDirection);
	set => InternalObject.shadowDirection = value.InternalObject;
}		public virtual GameObjectArrayV<int> SpI
{
	get => new GameObjectArrayV<int>(Context, InternalObject.spI);
	set => InternalObject.spI = value.InternalObject;
}		public virtual GameObjectArrayV<int> SpX
{
	get => new GameObjectArrayV<int>(Context, InternalObject.spX);
	set => InternalObject.spX = value.InternalObject;
}		public virtual GameObjectArrayV<int> SpY
{
	get => new GameObjectArrayV<int>(Context, InternalObject.spY);
	set => InternalObject.spY = value.InternalObject;
}		public virtual GameObjectArrayV<int> UnlitTorchX
{
	get => new GameObjectArrayV<int>(Context, InternalObject.unlitTorchX);
	set => InternalObject.unlitTorchX = value.InternalObject;
}		public virtual GameObjectArrayV<int> UnlitTorchY
{
	get => new GameObjectArrayV<int>(Context, InternalObject.unlitTorchY);
	set => InternalObject.unlitTorchY = value.InternalObject;
}		public virtual long LastTimePlayerWasSaved
{
	get => InternalObject.lastTimePlayerWasSaved;
	set => InternalObject.lastTimePlayerWasSaved = value;
}		public virtual sbyte Back
{
	get => InternalObject.back;
	set => InternalObject.back = value;
}		public virtual sbyte Backpack
{
	get => InternalObject.backpack;
	set => InternalObject.backpack = value;
}		public virtual sbyte Balloon
{
	get => InternalObject.balloon;
	set => InternalObject.balloon = value;
}		public virtual sbyte BalloonFront
{
	get => InternalObject.balloonFront;
	set => InternalObject.balloonFront = value;
}		public virtual sbyte Beard
{
	get => InternalObject.beard;
	set => InternalObject.beard = value;
}		public virtual sbyte Face
{
	get => InternalObject.face;
	set => InternalObject.face = value;
}		public virtual sbyte FaceFlower
{
	get => InternalObject.faceFlower;
	set => InternalObject.faceFlower = value;
}		public virtual sbyte FaceHead
{
	get => InternalObject.faceHead;
	set => InternalObject.faceHead = value;
}		public virtual sbyte Front
{
	get => InternalObject.front;
	set => InternalObject.front = value;
}		public virtual sbyte Handoff
{
	get => InternalObject.handoff;
	set => InternalObject.handoff = value;
}		public virtual sbyte Handon
{
	get => InternalObject.handon;
	set => InternalObject.handon = value;
}		public virtual sbyte Neck
{
	get => InternalObject.neck;
	set => InternalObject.neck = value;
}		public virtual sbyte Shield
{
	get => InternalObject.shield;
	set => InternalObject.shield = value;
}		public virtual sbyte Shoe
{
	get => InternalObject.shoe;
	set => InternalObject.shoe = value;
}		public virtual sbyte Tail
{
	get => InternalObject.tail;
	set => InternalObject.tail = value;
}		public virtual sbyte Waist
{
	get => InternalObject.waist;
	set => InternalObject.waist = value;
}		public virtual float BlizzardSoundVolume
{
	get => InternalObject._blizzardSoundVolume;
	set => InternalObject._blizzardSoundVolume = value;
}		public virtual float DeerclopsBlizzardSmoothedEffect
{
	get => InternalObject._deerclopsBlizzardSmoothedEffect;
	set => InternalObject._deerclopsBlizzardSmoothedEffect = value;
}		public virtual float ShaderObstructionInternalValue
{
	get => InternalObject._shaderObstructionInternalValue;
	set => InternalObject._shaderObstructionInternalValue = value;
}		public virtual float StormShaderObstruction
{
	get => InternalObject._stormShaderObstruction;
	set => InternalObject._stormShaderObstruction = value;
}		public virtual float AccRunSpeed
{
	get => InternalObject.accRunSpeed;
	set => InternalObject.accRunSpeed = value;
}		public virtual float BasiliskCharge
{
	get => InternalObject.basiliskCharge;
	set => InternalObject.basiliskCharge = value;
}		public virtual float BeetleCounter
{
	get => InternalObject.beetleCounter;
	set => InternalObject.beetleCounter = value;
}		public virtual float BodyRotation
{
	get => InternalObject.bodyRotation;
	set => InternalObject.bodyRotation = value;
}		public virtual float CarpetFrameCounter
{
	get => InternalObject.carpetFrameCounter;
	set => InternalObject.carpetFrameCounter = value;
}		public virtual float DefaultGravity
{
	get => InternalObject.defaultGravity;
	set => InternalObject.defaultGravity = value;
}		public virtual float DrainBoost
{
	get => InternalObject.drainBoost;
	set => InternalObject.drainBoost = value;
}		public virtual float Endurance
{
	get => InternalObject.endurance;
	set => InternalObject.endurance = value;
}		public virtual float FirstFractalAfterImageOpacity
{
	get => InternalObject.firstFractalAfterImageOpacity;
	set => InternalObject.firstFractalAfterImageOpacity = value;
}		public virtual float FlameRingRot
{
	get => InternalObject.flameRingRot;
	set => InternalObject.flameRingRot = value;
}		public virtual float FlameRingScale
{
	get => InternalObject.flameRingScale;
	set => InternalObject.flameRingScale = value;
}		public virtual float FullRotation
{
	get => InternalObject.fullRotation;
	set => InternalObject.fullRotation = value;
}		public virtual float GfxOffY
{
	get => InternalObject.gfxOffY;
	set => InternalObject.gfxOffY = value;
}		public virtual float GhostDir
{
	get => InternalObject.ghostDir;
	set => InternalObject.ghostDir = value;
}		public virtual float GhostDmg
{
	get => InternalObject.ghostDmg;
	set => InternalObject.ghostDmg = value;
}		public virtual float GhostFade
{
	get => InternalObject.ghostFade;
	set => InternalObject.ghostFade = value;
}		public virtual float GravDir
{
	get => InternalObject.gravDir;
	set => InternalObject.gravDir = value;
}		public virtual float Gravity
{
	get => InternalObject.gravity;
	set => InternalObject.gravity = value;
}		public virtual float HairDyeVar
{
	get => InternalObject.hairDyeVar;
	set => InternalObject.hairDyeVar = value;
}		public virtual float HeadRotation
{
	get => InternalObject.headRotation;
	set => InternalObject.headRotation = value;
}		public virtual float ItemGrabSpeed
{
	get => InternalObject.itemGrabSpeed;
	set => InternalObject.itemGrabSpeed = value;
}		public virtual float ItemGrabSpeedMax
{
	get => InternalObject.itemGrabSpeedMax;
	set => InternalObject.itemGrabSpeedMax = value;
}		public virtual float ItemRotation
{
	get => InternalObject.itemRotation;
	set => InternalObject.itemRotation = value;
}		public virtual float JumpSpeed
{
	get => InternalObject.jumpSpeed;
	set => InternalObject.jumpSpeed = value;
}		public virtual float JumpSpeedBoost
{
	get => InternalObject.jumpSpeedBoost;
	set => InternalObject.jumpSpeedBoost = value;
}		public virtual float LegRotation
{
	get => InternalObject.legRotation;
	set => InternalObject.legRotation = value;
}		public virtual float LifeSteal
{
	get => InternalObject.lifeSteal;
	set => InternalObject.lifeSteal = value;
}		public virtual float Luck
{
	get => InternalObject.luck;
	set => InternalObject.luck = value;
}		public virtual float LuckMaximumCap
{
	get => InternalObject.luckMaximumCap;
	set => InternalObject.luckMaximumCap = value;
}		public virtual float LuckMinimumCap
{
	get => InternalObject.luckMinimumCap;
	set => InternalObject.luckMinimumCap = value;
}		public virtual float ManaCost
{
	get => InternalObject.manaCost;
	set => InternalObject.manaCost = value;
}		public virtual float ManaSickLessDmg
{
	get => InternalObject.manaSickLessDmg;
	set => InternalObject.manaSickLessDmg = value;
}		public virtual float ManaSickReduction
{
	get => InternalObject.manaSickReduction;
	set => InternalObject.manaSickReduction = value;
}		public virtual float MaxFallSpeed
{
	get => InternalObject.maxFallSpeed;
	set => InternalObject.maxFallSpeed = value;
}		public virtual float MaxRegenDelay
{
	get => InternalObject.maxRegenDelay;
	set => InternalObject.maxRegenDelay = value;
}		public virtual float MaxRunSpeed
{
	get => InternalObject.maxRunSpeed;
	set => InternalObject.maxRunSpeed = value;
}		public virtual float MountFishronSpecialCounter
{
	get => InternalObject.MountFishronSpecialCounter;
	set => InternalObject.MountFishronSpecialCounter = value;
}		public virtual float MoveSpeed
{
	get => InternalObject.moveSpeed;
	set => InternalObject.moveSpeed = value;
}		public virtual float NearbyActiveNPCs
{
	get => InternalObject.nearbyActiveNPCs;
	set => InternalObject.nearbyActiveNPCs = value;
}		public virtual float OpacityForCreditsRoll
{
	get => InternalObject.opacityForCreditsRoll;
	set => InternalObject.opacityForCreditsRoll = value;
}		public virtual float PickSpeed
{
	get => InternalObject.pickSpeed;
	set => InternalObject.pickSpeed = value;
}		public virtual float PulleyFrameCounter
{
	get => InternalObject.pulleyFrameCounter;
	set => InternalObject.pulleyFrameCounter = value;
}		public virtual float RunAcceleration
{
	get => InternalObject.runAcceleration;
	set => InternalObject.runAcceleration = value;
}		public virtual float RunSlowdown
{
	get => InternalObject.runSlowdown;
	set => InternalObject.runSlowdown = value;
}		public virtual float ShadowDodgeCount
{
	get => InternalObject.shadowDodgeCount;
	set => InternalObject.shadowDodgeCount = value;
}		public virtual float SlotsMinions
{
	get => InternalObject.slotsMinions;
	set => InternalObject.slotsMinions = value;
}		public virtual float Stealth
{
	get => InternalObject.stealth;
	set => InternalObject.stealth = value;
}		public virtual float StepSpeed
{
	get => InternalObject.stepSpeed;
	set => InternalObject.stepSpeed = value;
}		public virtual float TeleportTime
{
	get => InternalObject.teleportTime;
	set => InternalObject.teleportTime = value;
}		public virtual float Thorns
{
	get => InternalObject.thorns;
	set => InternalObject.thorns = value;
}		public virtual float TileSpeed
{
	get => InternalObject.tileSpeed;
	set => InternalObject.tileSpeed = value;
}		public virtual float TorchLuck
{
	get => InternalObject.torchLuck;
	set => InternalObject.torchLuck = value;
}		public virtual float TownNPCs
{
	get => InternalObject.townNPCs;
	set => InternalObject.townNPCs = value;
}		public virtual float TrackBoost
{
	get => InternalObject.trackBoost;
	set => InternalObject.trackBoost = value;
}		public virtual float WallSpeed
{
	get => InternalObject.wallSpeed;
	set => InternalObject.wallSpeed = value;
}		public virtual float WhipRangeMultiplier
{
	get => InternalObject.whipRangeMultiplier;
	set => InternalObject.whipRangeMultiplier = value;
}		public virtual float WingAccRunSpeed
{
	get => InternalObject.wingAccRunSpeed;
	set => InternalObject.wingAccRunSpeed = value;
}		public virtual float WingRunAccelerationMult
{
	get => InternalObject.wingRunAccelerationMult;
	set => InternalObject.wingRunAccelerationMult = value;
}		public virtual float WingTime
{
	get => InternalObject.wingTime;
	set => InternalObject.wingTime = value;
}		public virtual GameObjectArrayV<float> ShadowRotation
{
	get => new GameObjectArrayV<float>(Context, InternalObject.shadowRotation);
	set => InternalObject.shadowRotation = value.InternalObject;
}		public virtual GameObjectArrayV<float> SpeedSlice
{
	get => new GameObjectArrayV<float>(Context, InternalObject.speedSlice);
	set => InternalObject.speedSlice = value.InternalObject;
}		public virtual string CursorItemIconText
{
	get => new GameString(Context, InternalObject.cursorItemIconText);
	set => InternalObject.cursorItemIconText = GameObjects.GameString.New(Context, value).TypedInternalObject;
}		public virtual string DisplayedFishingInfo
{
	get => new GameString(Context, InternalObject.displayedFishingInfo);
	set => InternalObject.displayedFishingInfo = GameObjects.GameString.New(Context, value).TypedInternalObject;
}		public virtual string LostCoinString
{
	get => new GameString(Context, InternalObject.lostCoinString);
	set => InternalObject.lostCoinString = GameObjects.GameString.New(Context, value).TypedInternalObject;
}		public virtual string Name
{
	get => new GameString(Context, InternalObject.name);
	set => InternalObject.name = GameObjects.GameString.New(Context, value).TypedInternalObject;
}		public virtual string SetBonus
{
	get => new GameString(Context, InternalObject.setBonus);
	set => InternalObject.setBonus = GameObjects.GameString.New(Context, value).TypedInternalObject;
}		public virtual GameObjectArray<GameString> SpN
{
	get => new GameObjectArray<GameString>(Context, InternalObject.spN); 
	set => InternalObject.spN = value.InternalObject;
}		public virtual ValueTypeRedefs.Terraria.BitsByte HideMisc
{
	get => InternalObject.hideMisc;
	set => InternalObject.hideMisc = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte OwnedLargeGems
{
	get => InternalObject.ownedLargeGems;
	set => InternalObject.ownedLargeGems = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte VoidVaultInfo
{
	get => InternalObject.voidVaultInfo;
	set => InternalObject.voidVaultInfo = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte Zone1
{
	get => InternalObject.zone1;
	set => InternalObject.zone1 = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte Zone2
{
	get => InternalObject.zone2;
	set => InternalObject.zone2 = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte Zone3
{
	get => InternalObject.zone3;
	set => InternalObject.zone3 = value;
}		public virtual ValueTypeRedefs.Terraria.BitsByte Zone4
{
	get => InternalObject.zone4;
	set => InternalObject.zone4 = value;
}		public virtual Chest Bank
{
	get => new(Context, InternalObject.bank); 
	set => InternalObject.bank = value.InternalObject; 
}		public virtual Chest Bank2
{
	get => new(Context, InternalObject.bank2); 
	set => InternalObject.bank2 = value.InternalObject; 
}		public virtual Chest Bank3
{
	get => new(Context, InternalObject.bank3); 
	set => InternalObject.bank3 = value.InternalObject; 
}		public virtual Chest Bank4
{
	get => new(Context, InternalObject.bank4); 
	set => InternalObject.bank4 = value.InternalObject; 
}		public virtual Item BoneGloveItem
{
	get => new(Context, InternalObject.boneGloveItem); 
	set => InternalObject.boneGloveItem = value.InternalObject; 
}		public virtual Item BrainOfConfusionItem
{
	get => new(Context, InternalObject.brainOfConfusionItem); 
	set => InternalObject.brainOfConfusionItem = value.InternalObject; 
}		public virtual Item EquippedWings
{
	get => new(Context, InternalObject.equippedWings); 
	set => InternalObject.equippedWings = value.InternalObject; 
}		public virtual Item HoneyCombItem
{
	get => new(Context, InternalObject.honeyCombItem); 
	set => InternalObject.honeyCombItem = value.InternalObject; 
}		public virtual Item LastVisualizedSelectedItem
{
	get => new(Context, InternalObject.lastVisualizedSelectedItem); 
	set => InternalObject.lastVisualizedSelectedItem = value.InternalObject; 
}		public virtual Item StarCloakItem
{
	get => new(Context, InternalObject.starCloakItem); 
	set => InternalObject.starCloakItem = value.InternalObject; 
}		public virtual Item StarCloakItem_beeCloakOverrideItem
{
	get => new(Context, InternalObject.starCloakItem_beeCloakOverrideItem); 
	set => InternalObject.starCloakItem_beeCloakOverrideItem = value.InternalObject; 
}		public virtual Item StarCloakItem_manaCloakOverrideItem
{
	get => new(Context, InternalObject.starCloakItem_manaCloakOverrideItem); 
	set => InternalObject.starCloakItem_manaCloakOverrideItem = value.InternalObject; 
}		public virtual Item StarCloakItem_starVeilOverrideItem
{
	get => new(Context, InternalObject.starCloakItem_starVeilOverrideItem); 
	set => InternalObject.starCloakItem_starVeilOverrideItem = value.InternalObject; 
}		public virtual Item TrashItem
{
	get => new(Context, InternalObject.trashItem); 
	set => InternalObject.trashItem = value.InternalObject; 
}		public virtual GameObjectArray<Item> TemporaryItemSlots
{
	get => new GameObjectArray<Item>(Context, InternalObject._temporaryItemSlots); 
	set => InternalObject._temporaryItemSlots = value.InternalObject;
}		public virtual GameObjectArray<Item> Armor
{
	get => new GameObjectArray<Item>(Context, InternalObject.armor); 
	set => InternalObject.armor = value.InternalObject;
}		public virtual GameObjectArray<Item> Dye
{
	get => new GameObjectArray<Item>(Context, InternalObject.dye); 
	set => InternalObject.dye = value.InternalObject;
}		public virtual GameObjectArray<Item> Inventory
{
	get => new GameObjectArray<Item>(Context, InternalObject.inventory); 
	set => InternalObject.inventory = value.InternalObject;
}		public virtual GameObjectArray<Item> MiscDyes
{
	get => new GameObjectArray<Item>(Context, InternalObject.miscDyes); 
	set => InternalObject.miscDyes = value.InternalObject;
}		public virtual GameObjectArray<Item> MiscEquips
{
	get => new GameObjectArray<Item>(Context, InternalObject.miscEquips); 
	set => InternalObject.miscEquips = value.InternalObject;
}		public virtual Mount Mount
{
	get => new(Context, InternalObject.mount); 
	set => InternalObject.mount = value.InternalObject; 
}
#endregion
	}
}