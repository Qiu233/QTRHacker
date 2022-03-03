using QHackLib;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects.Terraria
{
	public class Chest : GameObject
	{
		public Chest(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public virtual bool BankChest { get => InternalObject.bankChest; set => InternalObject.bankChest = value; }
		public virtual int Frame { get => InternalObject.frame; set => InternalObject.frame = value; }
		public virtual int FrameCounter { get => InternalObject.frameCounter; set => InternalObject.frameCounter = value; }
		public virtual int X { get => InternalObject.x; set => InternalObject.x = value; }
		public virtual int Y { get => InternalObject.y; set => InternalObject.y = value; }

		public virtual GameObjectArrayV<int> ChestItemSpawn
		{
			get => new(Context, InternalObject.chestItemSpawn);
			set => InternalObject.chestItemSpawn = value.InternalObject;
		}
		public virtual GameObjectArrayV<int> ChestItemSpawn2
		{
			get => new(Context, InternalObject.chestItemSpawn2);
			set => InternalObject.chestItemSpawn2 = value.InternalObject;
		}
		public virtual GameObjectArrayV<int> ChestTypeToIcon
		{
			get => new(Context, InternalObject.chestTypeToIcon);
			set => InternalObject.chestTypeToIcon = value.InternalObject;
		}
		public virtual GameObjectArrayV<int> ChestTypeToIcon2
		{
			get => new(Context, InternalObject.chestTypeToIcon2);
			set => InternalObject.chestTypeToIcon2 = value.InternalObject;
		}
		public virtual GameObjectArrayV<int> DresserItemSpawn
		{
			get => new(Context, InternalObject.dresserItemSpawn);
			set => InternalObject.dresserItemSpawn = value.InternalObject;
		}
		public virtual GameObjectArrayV<int> DresserTypeToIcon
		{
			get => new(Context, InternalObject.dresserTypeToIcon);
			set => InternalObject.dresserTypeToIcon = value.InternalObject;
		}
		public virtual GameObjectArray<Item> Item
		{
			get => new(Context, InternalObject.item);
			set => InternalObject.item = value.InternalObject;
		}
		public virtual GameString Name
		{
			get => new(Context, InternalObject.name);
			set => InternalObject.name = value.InternalObject;
		}
	}
}
