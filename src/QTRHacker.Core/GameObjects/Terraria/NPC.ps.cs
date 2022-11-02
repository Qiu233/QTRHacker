﻿#pragma warning disable
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
	partial class NPC
	{
#region Virtual Properties
		public virtual ValueTypeRedefs.Xna.Color Color
{
	get => InternalObject.color;
	set => InternalObject.color = value;
}		public virtual ValueTypeRedefs.Xna.Rectangle Frame
{
	get => InternalObject.frame;
	set => InternalObject.frame = value;
}		public virtual ValueTypeRedefs.Xna.Rectangle TargetRect
{
	get => InternalObject.targetRect;
	set => InternalObject.targetRect = value;
}		public virtual ValueTypeRedefs.Xna.Vector2 NetOffset
{
	get => InternalObject.netOffset;
	set => InternalObject.netOffset = value;
}		public virtual ValueTypeRedefs.Xna.Vector2[] OldPos
{
	get => InternalObject.oldPos;
	set => InternalObject.oldPos = value;
}		public virtual bool BehindTiles
{
	get => InternalObject.behindTiles;
	set => InternalObject.behindTiles = value;
}		public virtual bool BetsysCurse
{
	get => InternalObject.betsysCurse;
	set => InternalObject.betsysCurse = value;
}		public virtual bool Boss
{
	get => InternalObject.boss;
	set => InternalObject.boss = value;
}		public virtual bool BoughtBunny
{
	get => InternalObject.boughtBunny;
	set => InternalObject.boughtBunny = value;
}		public virtual bool BoughtCat
{
	get => InternalObject.boughtCat;
	set => InternalObject.boughtCat = value;
}		public virtual bool BoughtDog
{
	get => InternalObject.boughtDog;
	set => InternalObject.boughtDog = value;
}		public virtual bool CanGhostHeal
{
	get => InternalObject.canGhostHeal;
	set => InternalObject.canGhostHeal = value;
}		public virtual bool Celled
{
	get => InternalObject.celled;
	set => InternalObject.celled = value;
}		public virtual bool Chaseable
{
	get => InternalObject.chaseable;
	set => InternalObject.chaseable = value;
}		public virtual bool CloseDoor
{
	get => InternalObject.closeDoor;
	set => InternalObject.closeDoor = value;
}		public virtual bool ColdDamage
{
	get => InternalObject.coldDamage;
	set => InternalObject.coldDamage = value;
}		public virtual bool CollideX
{
	get => InternalObject.collideX;
	set => InternalObject.collideX = value;
}		public virtual bool CollideY
{
	get => InternalObject.collideY;
	set => InternalObject.collideY = value;
}		public virtual bool CombatBookWasUsed
{
	get => InternalObject.combatBookWasUsed;
	set => InternalObject.combatBookWasUsed = value;
}		public virtual bool Confused
{
	get => InternalObject.confused;
	set => InternalObject.confused = value;
}		public virtual bool Daybreak
{
	get => InternalObject.daybreak;
	set => InternalObject.daybreak = value;
}		public virtual bool DespawnEncouraged
{
	get => InternalObject.despawnEncouraged;
	set => InternalObject.despawnEncouraged = value;
}		public virtual bool DontCountMe
{
	get => InternalObject.dontCountMe;
	set => InternalObject.dontCountMe = value;
}		public virtual bool DontTakeDamage
{
	get => InternalObject.dontTakeDamage;
	set => InternalObject.dontTakeDamage = value;
}		public virtual bool DontTakeDamageFromHostiles
{
	get => InternalObject.dontTakeDamageFromHostiles;
	set => InternalObject.dontTakeDamageFromHostiles = value;
}		public virtual bool DownedAncientCultist
{
	get => InternalObject.downedAncientCultist;
	set => InternalObject.downedAncientCultist = value;
}		public virtual bool DownedBoss1
{
	get => InternalObject.downedBoss1;
	set => InternalObject.downedBoss1 = value;
}		public virtual bool DownedBoss2
{
	get => InternalObject.downedBoss2;
	set => InternalObject.downedBoss2 = value;
}		public virtual bool DownedBoss3
{
	get => InternalObject.downedBoss3;
	set => InternalObject.downedBoss3 = value;
}		public virtual bool DownedChristmasIceQueen
{
	get => InternalObject.downedChristmasIceQueen;
	set => InternalObject.downedChristmasIceQueen = value;
}		public virtual bool DownedChristmasSantank
{
	get => InternalObject.downedChristmasSantank;
	set => InternalObject.downedChristmasSantank = value;
}		public virtual bool DownedChristmasTree
{
	get => InternalObject.downedChristmasTree;
	set => InternalObject.downedChristmasTree = value;
}		public virtual bool DownedClown
{
	get => InternalObject.downedClown;
	set => InternalObject.downedClown = value;
}		public virtual bool DownedDeerclops
{
	get => InternalObject.downedDeerclops;
	set => InternalObject.downedDeerclops = value;
}		public virtual bool DownedEmpressOfLight
{
	get => InternalObject.downedEmpressOfLight;
	set => InternalObject.downedEmpressOfLight = value;
}		public virtual bool DownedFishron
{
	get => InternalObject.downedFishron;
	set => InternalObject.downedFishron = value;
}		public virtual bool DownedFrost
{
	get => InternalObject.downedFrost;
	set => InternalObject.downedFrost = value;
}		public virtual bool DownedGoblins
{
	get => InternalObject.downedGoblins;
	set => InternalObject.downedGoblins = value;
}		public virtual bool DownedGolemBoss
{
	get => InternalObject.downedGolemBoss;
	set => InternalObject.downedGolemBoss = value;
}		public virtual bool DownedHalloweenKing
{
	get => InternalObject.downedHalloweenKing;
	set => InternalObject.downedHalloweenKing = value;
}		public virtual bool DownedHalloweenTree
{
	get => InternalObject.downedHalloweenTree;
	set => InternalObject.downedHalloweenTree = value;
}		public virtual bool DownedMartians
{
	get => InternalObject.downedMartians;
	set => InternalObject.downedMartians = value;
}		public virtual bool DownedMechBoss1
{
	get => InternalObject.downedMechBoss1;
	set => InternalObject.downedMechBoss1 = value;
}		public virtual bool DownedMechBoss2
{
	get => InternalObject.downedMechBoss2;
	set => InternalObject.downedMechBoss2 = value;
}		public virtual bool DownedMechBoss3
{
	get => InternalObject.downedMechBoss3;
	set => InternalObject.downedMechBoss3 = value;
}		public virtual bool DownedMechBossAny
{
	get => InternalObject.downedMechBossAny;
	set => InternalObject.downedMechBossAny = value;
}		public virtual bool DownedMoonlord
{
	get => InternalObject.downedMoonlord;
	set => InternalObject.downedMoonlord = value;
}		public virtual bool DownedPirates
{
	get => InternalObject.downedPirates;
	set => InternalObject.downedPirates = value;
}		public virtual bool DownedPlantBoss
{
	get => InternalObject.downedPlantBoss;
	set => InternalObject.downedPlantBoss = value;
}		public virtual bool DownedQueenBee
{
	get => InternalObject.downedQueenBee;
	set => InternalObject.downedQueenBee = value;
}		public virtual bool DownedQueenSlime
{
	get => InternalObject.downedQueenSlime;
	set => InternalObject.downedQueenSlime = value;
}		public virtual bool DownedSlimeKing
{
	get => InternalObject.downedSlimeKing;
	set => InternalObject.downedSlimeKing = value;
}		public virtual bool DownedTowerNebula
{
	get => InternalObject.downedTowerNebula;
	set => InternalObject.downedTowerNebula = value;
}		public virtual bool DownedTowerSolar
{
	get => InternalObject.downedTowerSolar;
	set => InternalObject.downedTowerSolar = value;
}		public virtual bool DownedTowerStardust
{
	get => InternalObject.downedTowerStardust;
	set => InternalObject.downedTowerStardust = value;
}		public virtual bool DownedTowerVortex
{
	get => InternalObject.downedTowerVortex;
	set => InternalObject.downedTowerVortex = value;
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
}		public virtual bool DryadBane
{
	get => InternalObject.dryadBane;
	set => InternalObject.dryadBane = value;
}		public virtual bool DryadWard
{
	get => InternalObject.dryadWard;
	set => InternalObject.dryadWard = value;
}		public virtual bool EoCKilledToday
{
	get => InternalObject.EoCKilledToday;
	set => InternalObject.EoCKilledToday = value;
}		public virtual bool FairyLog
{
	get => InternalObject.fairyLog;
	set => InternalObject.fairyLog = value;
}		public virtual bool ForcePartyHatOn
{
	get => InternalObject.ForcePartyHatOn;
	set => InternalObject.ForcePartyHatOn = value;
}		public virtual bool FreeCake
{
	get => InternalObject.freeCake;
	set => InternalObject.freeCake = value;
}		public virtual bool Friendly
{
	get => InternalObject.friendly;
	set => InternalObject.friendly = value;
}		public virtual bool Hide
{
	get => InternalObject.hide;
	set => InternalObject.hide = value;
}		public virtual bool Homeless
{
	get => InternalObject.homeless;
	set => InternalObject.homeless = value;
}		public virtual bool Ichor
{
	get => InternalObject.ichor;
	set => InternalObject.ichor = value;
}		public virtual bool Immortal
{
	get => InternalObject.immortal;
	set => InternalObject.immortal = value;
}		public virtual bool IsABestiaryIconDummy
{
	get => InternalObject.IsABestiaryIconDummy;
	set => InternalObject.IsABestiaryIconDummy = value;
}		public virtual bool Javelined
{
	get => InternalObject.javelined;
	set => InternalObject.javelined = value;
}		public virtual bool JustHit
{
	get => InternalObject.justHit;
	set => InternalObject.justHit = value;
}		public virtual bool LavaImmune
{
	get => InternalObject.lavaImmune;
	set => InternalObject.lavaImmune = value;
}		public virtual bool LoveStruck
{
	get => InternalObject.loveStruck;
	set => InternalObject.loveStruck = value;
}		public virtual bool LunarApocalypseIsUp
{
	get => InternalObject.LunarApocalypseIsUp;
	set => InternalObject.LunarApocalypseIsUp = value;
}		public virtual bool MarkedByScytheWhip
{
	get => InternalObject.markedByScytheWhip;
	set => InternalObject.markedByScytheWhip = value;
}		public virtual bool Midas
{
	get => InternalObject.midas;
	set => InternalObject.midas = value;
}		public virtual bool NeedsUniqueInfoUpdate
{
	get => InternalObject.needsUniqueInfoUpdate;
	set => InternalObject.needsUniqueInfoUpdate = value;
}		public virtual bool NetAlways
{
	get => InternalObject.netAlways;
	set => InternalObject.netAlways = value;
}		public virtual bool NetUpdate
{
	get => InternalObject.netUpdate;
	set => InternalObject.netUpdate = value;
}		public virtual bool NetUpdate2
{
	get => InternalObject.netUpdate2;
	set => InternalObject.netUpdate2 = value;
}		public virtual bool NoGravity
{
	get => InternalObject.noGravity;
	set => InternalObject.noGravity = value;
}		public virtual bool NoSpawnCycle
{
	get => InternalObject.noSpawnCycle;
	set => InternalObject.noSpawnCycle = value;
}		public virtual bool NoTileCollide
{
	get => InternalObject.noTileCollide;
	set => InternalObject.noTileCollide = value;
}		public virtual bool Oiled
{
	get => InternalObject.oiled;
	set => InternalObject.oiled = value;
}		public virtual bool OldHomeless
{
	get => InternalObject.oldHomeless;
	set => InternalObject.oldHomeless = value;
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
}		public virtual bool Poisoned
{
	get => InternalObject.poisoned;
	set => InternalObject.poisoned = value;
}		public virtual bool ReflectsProjectiles
{
	get => InternalObject.reflectsProjectiles;
	set => InternalObject.reflectsProjectiles = value;
}		public virtual bool SavedAngler
{
	get => InternalObject.savedAngler;
	set => InternalObject.savedAngler = value;
}		public virtual bool SavedBartender
{
	get => InternalObject.savedBartender;
	set => InternalObject.savedBartender = value;
}		public virtual bool SavedGoblin
{
	get => InternalObject.savedGoblin;
	set => InternalObject.savedGoblin = value;
}		public virtual bool SavedGolfer
{
	get => InternalObject.savedGolfer;
	set => InternalObject.savedGolfer = value;
}		public virtual bool SavedMech
{
	get => InternalObject.savedMech;
	set => InternalObject.savedMech = value;
}		public virtual bool SavedStylist
{
	get => InternalObject.savedStylist;
	set => InternalObject.savedStylist = value;
}		public virtual bool SavedTaxCollector
{
	get => InternalObject.savedTaxCollector;
	set => InternalObject.savedTaxCollector = value;
}		public virtual bool SavedWizard
{
	get => InternalObject.savedWizard;
	set => InternalObject.savedWizard = value;
}		public virtual bool SetFrameSize
{
	get => InternalObject.setFrameSize;
	set => InternalObject.setFrameSize = value;
}		public virtual bool ShadowFlame
{
	get => InternalObject.shadowFlame;
	set => InternalObject.shadowFlame = value;
}		public virtual bool SoulDrain
{
	get => InternalObject.soulDrain;
	set => InternalObject.soulDrain = value;
}		public virtual bool SpawnedFromStatue
{
	get => InternalObject.SpawnedFromStatue;
	set => InternalObject.SpawnedFromStatue = value;
}		public virtual bool StairFall
{
	get => InternalObject.stairFall;
	set => InternalObject.stairFall = value;
}		public virtual bool Stinky
{
	get => InternalObject.stinky;
	set => InternalObject.stinky = value;
}		public virtual bool TaxCollector
{
	get => InternalObject.taxCollector;
	set => InternalObject.taxCollector = value;
}		public virtual bool Teleporting
{
	get => InternalObject.teleporting;
	set => InternalObject.teleporting = value;
}		public virtual bool TentacleSpiked
{
	get => InternalObject.tentacleSpiked;
	set => InternalObject.tentacleSpiked = value;
}		public virtual bool TowerActiveNebula
{
	get => InternalObject.TowerActiveNebula;
	set => InternalObject.TowerActiveNebula = value;
}		public virtual bool TowerActiveSolar
{
	get => InternalObject.TowerActiveSolar;
	set => InternalObject.TowerActiveSolar = value;
}		public virtual bool TowerActiveStardust
{
	get => InternalObject.TowerActiveStardust;
	set => InternalObject.TowerActiveStardust = value;
}		public virtual bool TowerActiveVortex
{
	get => InternalObject.TowerActiveVortex;
	set => InternalObject.TowerActiveVortex = value;
}		public virtual bool TownNPC
{
	get => InternalObject.townNPC;
	set => InternalObject.townNPC = value;
}		public virtual bool TrapImmune
{
	get => InternalObject.trapImmune;
	set => InternalObject.trapImmune = value;
}		public virtual bool TravelNPC
{
	get => InternalObject.travelNPC;
	set => InternalObject.travelNPC = value;
}		public virtual bool Venom
{
	get => InternalObject.venom;
	set => InternalObject.venom = value;
}		public virtual bool WoFKilledToday
{
	get => InternalObject.WoFKilledToday;
	set => InternalObject.WoFKilledToday = value;
}		public virtual bool[] BuffImmune
{
	get => InternalObject.buffImmune;
	set => InternalObject.buffImmune = value;
}		public virtual bool[] NpcsFoundForCheckActive
{
	get => InternalObject.npcsFoundForCheckActive;
	set => InternalObject.npcsFoundForCheckActive = value;
}		public virtual bool[] PlayerInteraction
{
	get => InternalObject.playerInteraction;
	set => InternalObject.playerInteraction = value;
}		public virtual byte NetStream
{
	get => InternalObject.netStream;
	set => InternalObject.netStream = value;
}		public virtual byte[] StreamPlayer
{
	get => InternalObject.streamPlayer;
	set => InternalObject.streamPlayer = value;
}		public virtual double FrameCounter
{
	get => InternalObject.frameCounter;
	set => InternalObject.frameCounter = value;
}		public virtual short CatchItem
{
	get => InternalObject.catchItem;
	set => InternalObject.catchItem = value;
}		public virtual short ReleaseOwner
{
	get => InternalObject.releaseOwner;
	set => InternalObject.releaseOwner = value;
}		public virtual int ActiveRangeX
{
	get => InternalObject.activeRangeX;
	set => InternalObject.activeRangeX = value;
}		public virtual int ActiveRangeY
{
	get => InternalObject.activeRangeY;
	set => InternalObject.activeRangeY = value;
}		public virtual int ActiveTime
{
	get => InternalObject.activeTime;
	set => InternalObject.activeTime = value;
}		public virtual int AFKTimeNeededForNoWorms
{
	get => InternalObject.AFKTimeNeededForNoWorms;
	set => InternalObject.AFKTimeNeededForNoWorms = value;
}		public virtual int AiAction
{
	get => InternalObject.aiAction;
	set => InternalObject.aiAction = value;
}		public virtual int AiStyle
{
	get => InternalObject.aiStyle;
	set => InternalObject.aiStyle = value;
}		public virtual int Alpha
{
	get => InternalObject.alpha;
	set => InternalObject.alpha = value;
}		public virtual int AltTexture
{
	get => InternalObject.altTexture;
	set => InternalObject.altTexture = value;
}		public virtual int Breath
{
	get => InternalObject.breath;
	set => InternalObject.breath = value;
}		public virtual int BreathCounter
{
	get => InternalObject.breathCounter;
	set => InternalObject.breathCounter = value;
}		public virtual int ButterflyChance
{
	get => InternalObject.butterflyChance;
	set => InternalObject.butterflyChance = value;
}		public virtual int CrimsonBoss
{
	get => InternalObject.crimsonBoss;
	set => InternalObject.crimsonBoss = value;
}		public virtual int Damage
{
	get => InternalObject.damage;
	set => InternalObject.damage = value;
}		public virtual int DefaultMaxSpawns
{
	get => InternalObject.defaultMaxSpawns;
	set => InternalObject.defaultMaxSpawns = value;
}		public virtual int DefaultSpawnRate
{
	get => InternalObject.defaultSpawnRate;
	set => InternalObject.defaultSpawnRate = value;
}		public virtual int DefDamage
{
	get => InternalObject.defDamage;
	set => InternalObject.defDamage = value;
}		public virtual int DefDefense
{
	get => InternalObject.defDefense;
	set => InternalObject.defDefense = value;
}		public virtual int Defense
{
	get => InternalObject.defense;
	set => InternalObject.defense = value;
}		public virtual int DirectionY
{
	get => InternalObject.directionY;
	set => InternalObject.directionY = value;
}		public virtual int DoorX
{
	get => InternalObject.doorX;
	set => InternalObject.doorX = value;
}		public virtual int DoorY
{
	get => InternalObject.doorY;
	set => InternalObject.doorY = value;
}		public virtual int ExtraValue
{
	get => InternalObject.extraValue;
	set => InternalObject.extraValue = value;
}		public virtual int FireFlyChance
{
	get => InternalObject.fireFlyChance;
	set => InternalObject.fireFlyChance = value;
}		public virtual int FireFlyFriendly
{
	get => InternalObject.fireFlyFriendly;
	set => InternalObject.fireFlyFriendly = value;
}		public virtual int FireFlyMultiple
{
	get => InternalObject.fireFlyMultiple;
	set => InternalObject.fireFlyMultiple = value;
}		public virtual int FriendlyRegen
{
	get => InternalObject.friendlyRegen;
	set => InternalObject.friendlyRegen = value;
}		public virtual int GoldCritterChance
{
	get => InternalObject.goldCritterChance;
	set => InternalObject.goldCritterChance = value;
}		public virtual int GolemBoss
{
	get => InternalObject.golemBoss;
	set => InternalObject.golemBoss = value;
}		public virtual int HomeTileX
{
	get => InternalObject.homeTileX;
	set => InternalObject.homeTileX = value;
}		public virtual int HomeTileY
{
	get => InternalObject.homeTileY;
	set => InternalObject.homeTileY = value;
}		public virtual int HousingCategory
{
	get => InternalObject.housingCategory;
	set => InternalObject.housingCategory = value;
}		public virtual int IgnorePlayerInteractions
{
	get => InternalObject.ignorePlayerInteractions;
	set => InternalObject.ignorePlayerInteractions = value;
}		public virtual int ImmuneTime
{
	get => InternalObject.immuneTime;
	set => InternalObject.immuneTime = value;
}		public virtual int LadyBugBadLuckTime
{
	get => InternalObject.ladyBugBadLuckTime;
	set => InternalObject.ladyBugBadLuckTime = value;
}		public virtual int LadyBugGoodLuckTime
{
	get => InternalObject.ladyBugGoodLuckTime;
	set => InternalObject.ladyBugGoodLuckTime = value;
}		public virtual int LadyBugRainTime
{
	get => InternalObject.ladyBugRainTime;
	set => InternalObject.ladyBugRainTime = value;
}		public virtual int LastInteraction
{
	get => InternalObject.lastInteraction;
	set => InternalObject.lastInteraction = value;
}		public virtual int LastPortalColorIndex
{
	get => InternalObject.lastPortalColorIndex;
	set => InternalObject.lastPortalColorIndex = value;
}		public virtual int Life
{
	get => InternalObject.life;
	set => InternalObject.life = value;
}		public virtual int LifeMax
{
	get => InternalObject.lifeMax;
	set => InternalObject.lifeMax = value;
}		public virtual int LifeRegen
{
	get => InternalObject.lifeRegen;
	set => InternalObject.lifeRegen = value;
}		public virtual int LifeRegenCount
{
	get => InternalObject.lifeRegenCount;
	set => InternalObject.lifeRegenCount = value;
}		public virtual int LifeRegenExpectedLossPerSecond
{
	get => InternalObject.lifeRegenExpectedLossPerSecond;
	set => InternalObject.lifeRegenExpectedLossPerSecond = value;
}		public virtual int LunarShieldPowerExpert
{
	get => InternalObject.LunarShieldPowerExpert;
	set => InternalObject.LunarShieldPowerExpert = value;
}		public virtual int LunarShieldPowerNormal
{
	get => InternalObject.LunarShieldPowerNormal;
	set => InternalObject.LunarShieldPowerNormal = value;
}		public virtual int MaxAI
{
	get => InternalObject.maxAI;
	set => InternalObject.maxAI = value;
}		public virtual int MaxAttack
{
	get => InternalObject.maxAttack;
	set => InternalObject.maxAttack = value;
}		public virtual int MaximumAmountOfTimesLadyBugRainCanStack
{
	get => InternalObject.maximumAmountOfTimesLadyBugRainCanStack;
	set => InternalObject.maximumAmountOfTimesLadyBugRainCanStack = value;
}		public virtual int MaxSpawns
{
	get => InternalObject.maxSpawns;
	set => InternalObject.maxSpawns = value;
}		public virtual int MoonLordCountdown
{
	get => InternalObject.MoonLordCountdown;
	set => InternalObject.MoonLordCountdown = value;
}		public virtual int MoonLordFightingDistance
{
	get => InternalObject.MoonLordFightingDistance;
	set => InternalObject.MoonLordFightingDistance = value;
}		public virtual int NetID
{
	get => InternalObject.netID;
	set => InternalObject.netID = value;
}		public virtual int NetSkip
{
	get => InternalObject.netSkip;
	set => InternalObject.netSkip = value;
}		public virtual int NetSpam
{
	get => InternalObject.netSpam;
	set => InternalObject.netSpam = value;
}		public virtual int OffSetDelayTime
{
	get => InternalObject.offSetDelayTime;
	set => InternalObject.offSetDelayTime = value;
}		public virtual int OldDirectionY
{
	get => InternalObject.oldDirectionY;
	set => InternalObject.oldDirectionY = value;
}		public virtual int OldHomeTileX
{
	get => InternalObject.oldHomeTileX;
	set => InternalObject.oldHomeTileX = value;
}		public virtual int OldHomeTileY
{
	get => InternalObject.oldHomeTileY;
	set => InternalObject.oldHomeTileY = value;
}		public virtual int OldTarget
{
	get => InternalObject.oldTarget;
	set => InternalObject.oldTarget = value;
}		public virtual int PlantBoss
{
	get => InternalObject.plantBoss;
	set => InternalObject.plantBoss = value;
}		public virtual int Rarity
{
	get => InternalObject.rarity;
	set => InternalObject.rarity = value;
}		public virtual int RealLife
{
	get => InternalObject.realLife;
	set => InternalObject.realLife = value;
}		public virtual int SafeRangeX
{
	get => InternalObject.safeRangeX;
	set => InternalObject.safeRangeX = value;
}		public virtual int SafeRangeY
{
	get => InternalObject.safeRangeY;
	set => InternalObject.safeRangeY = value;
}		public virtual int SHeight
{
	get => InternalObject.sHeight;
	set => InternalObject.sHeight = value;
}		public virtual int ShieldStrengthTowerNebula
{
	get => InternalObject.ShieldStrengthTowerNebula;
	set => InternalObject.ShieldStrengthTowerNebula = value;
}		public virtual int ShieldStrengthTowerSolar
{
	get => InternalObject.ShieldStrengthTowerSolar;
	set => InternalObject.ShieldStrengthTowerSolar = value;
}		public virtual int ShieldStrengthTowerStardust
{
	get => InternalObject.ShieldStrengthTowerStardust;
	set => InternalObject.ShieldStrengthTowerStardust = value;
}		public virtual int ShieldStrengthTowerVortex
{
	get => InternalObject.ShieldStrengthTowerVortex;
	set => InternalObject.ShieldStrengthTowerVortex = value;
}		public virtual int SoundDelay
{
	get => InternalObject.soundDelay;
	set => InternalObject.soundDelay = value;
}		public virtual int SpawnRangeX
{
	get => InternalObject.spawnRangeX;
	set => InternalObject.spawnRangeX = value;
}		public virtual int SpawnRangeY
{
	get => InternalObject.spawnRangeY;
	set => InternalObject.spawnRangeY = value;
}		public virtual int SpawnRate
{
	get => InternalObject.spawnRate;
	set => InternalObject.spawnRate = value;
}		public virtual int SpawnSpaceX
{
	get => InternalObject.spawnSpaceX;
	set => InternalObject.spawnSpaceX = value;
}		public virtual int SpawnSpaceY
{
	get => InternalObject.spawnSpaceY;
	set => InternalObject.spawnSpaceY = value;
}		public virtual int SpriteDirection
{
	get => InternalObject.spriteDirection;
	set => InternalObject.spriteDirection = value;
}		public virtual int StatsAreScaledForThisManyPlayers
{
	get => InternalObject.statsAreScaledForThisManyPlayers;
	set => InternalObject.statsAreScaledForThisManyPlayers = value;
}		public virtual int SWidth
{
	get => InternalObject.sWidth;
	set => InternalObject.sWidth = value;
}		public virtual int Target
{
	get => InternalObject.target;
	set => InternalObject.target = value;
}		public virtual int TeleportStyle
{
	get => InternalObject.teleportStyle;
	set => InternalObject.teleportStyle = value;
}		public virtual int TimeLeft
{
	get => InternalObject.timeLeft;
	set => InternalObject.timeLeft = value;
}		public virtual int TownNpcVariationIndex
{
	get => InternalObject.townNpcVariationIndex;
	set => InternalObject.townNpcVariationIndex = value;
}		public virtual int TownRangeX
{
	get => InternalObject.townRangeX;
	set => InternalObject.townRangeX = value;
}		public virtual int TownRangeY
{
	get => InternalObject.townRangeY;
	set => InternalObject.townRangeY = value;
}		public virtual int Type
{
	get => InternalObject.type;
	set => InternalObject.type = value;
}		public virtual int WaveNumber
{
	get => InternalObject.waveNumber;
	set => InternalObject.waveNumber = value;
}		public virtual int[,,,] MoonLordAttacksArray
{
	get => InternalObject.MoonLordAttacksArray;
	set => InternalObject.MoonLordAttacksArray = value;
}		public virtual int[,] CavernMonsterType
{
	get => InternalObject.cavernMonsterType;
	set => InternalObject.cavernMonsterType = value;
}		public virtual int[,] MoonLordAttacksArray2
{
	get => InternalObject.MoonLordAttacksArray2;
	set => InternalObject.MoonLordAttacksArray2 = value;
}		public virtual int[] DeerclopsAttack1Frames
{
	get => InternalObject._deerclopsAttack1Frames;
	set => InternalObject._deerclopsAttack1Frames = value;
}		public virtual int[] DeerclopsAttack2Frames
{
	get => InternalObject._deerclopsAttack2Frames;
	set => InternalObject._deerclopsAttack2Frames = value;
}		public virtual int[] DeerclopsAttack3Frames
{
	get => InternalObject._deerclopsAttack3Frames;
	set => InternalObject._deerclopsAttack3Frames = value;
}		public virtual int[] AttackNPC
{
	get => InternalObject.attackNPC;
	set => InternalObject.attackNPC = value;
}		public virtual int[] BuffTime
{
	get => InternalObject.buffTime;
	set => InternalObject.buffTime = value;
}		public virtual int[] BuffType
{
	get => InternalObject.buffType;
	set => InternalObject.buffType = value;
}		public virtual int[] Immune
{
	get => InternalObject.immune;
	set => InternalObject.immune = value;
}		public virtual int[] KillCount
{
	get => InternalObject.killCount;
	set => InternalObject.killCount = value;
}		public virtual int[] LazyNPCOwnedProjectileSearchArray
{
	get => InternalObject.lazyNPCOwnedProjectileSearchArray;
	set => InternalObject.lazyNPCOwnedProjectileSearchArray = value;
}		public virtual float GfxOffY
{
	get => InternalObject.gfxOffY;
	set => InternalObject.gfxOffY = value;
}		public virtual float Gravity
{
	get => InternalObject.gravity;
	set => InternalObject.gravity = value;
}		public virtual float HoneyMovementSpeed
{
	get => InternalObject.honeyMovementSpeed;
	set => InternalObject.honeyMovementSpeed = value;
}		public virtual float KnockBackResist
{
	get => InternalObject.knockBackResist;
	set => InternalObject.knockBackResist = value;
}		public virtual float LavaMovementSpeed
{
	get => InternalObject.lavaMovementSpeed;
	set => InternalObject.lavaMovementSpeed = value;
}		public virtual float NameOver
{
	get => InternalObject.nameOver;
	set => InternalObject.nameOver = value;
}		public virtual float NpcSlots
{
	get => InternalObject.npcSlots;
	set => InternalObject.npcSlots = value;
}		public virtual float Rotation
{
	get => InternalObject.rotation;
	set => InternalObject.rotation = value;
}		public virtual float Scale
{
	get => InternalObject.scale;
	set => InternalObject.scale = value;
}		public virtual float StepSpeed
{
	get => InternalObject.stepSpeed;
	set => InternalObject.stepSpeed = value;
}		public virtual float StrengthMultiplier
{
	get => InternalObject.strengthMultiplier;
	set => InternalObject.strengthMultiplier = value;
}		public virtual float TakenDamageMultiplier
{
	get => InternalObject.takenDamageMultiplier;
	set => InternalObject.takenDamageMultiplier = value;
}		public virtual float TeleportTime
{
	get => InternalObject.teleportTime;
	set => InternalObject.teleportTime = value;
}		public virtual float Value
{
	get => InternalObject.value;
	set => InternalObject.value = value;
}		public virtual float WaterMovementSpeed
{
	get => InternalObject.waterMovementSpeed;
	set => InternalObject.waterMovementSpeed = value;
}		public virtual float WaveKills
{
	get => InternalObject.waveKills;
	set => InternalObject.waveKills = value;
}		public virtual float[] Ai
{
	get => InternalObject.ai;
	set => InternalObject.ai = value;
}		public virtual float[] LocalAI
{
	get => InternalObject.localAI;
	set => InternalObject.localAI = value;
}		public virtual float[] OldRot
{
	get => InternalObject.oldRot;
	set => InternalObject.oldRot = value;
}		public virtual string GivenName
{
	get => new GameString(Context, InternalObject._givenName);
	set => InternalObject._givenName = GameString.New(Context, value).TypedInternalObject;
}

#endregion
	}
}
