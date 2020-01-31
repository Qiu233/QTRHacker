using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.IO;
using QTRHacker.Functions.GameObjects.Map;
using QTRHacker.Functions.InjectHook;

namespace QTRHacker.Functions
{
	/// <summary>
	/// Terraria游戏上下文环境
	/// </summary>
	public class GameContext : IDisposable
	{
		public bool AssembliesLoaded
		{
			get;
			private set;
		}
		public static bool OffsetsInitialized
		{
			get;
		}
		public const int MaxItemTypes = 3930;
		public int My_Player_Address
		{
			get;
		}
		/// <summary>
		/// 相关的Context实例，不需要手动操作
		/// </summary>
		public Context HContext
		{
			get;
		}
		public int ArrayHeadLength
		{
			get;
		}

		public int Player_Array_Address
		{
			get;
		}
		public int Player_Array_Pointer
		{
			get;
		}
		public int NPC_Array_Address
		{
			get;
		}
		public int NPC_Array_Pointer
		{
			get;
		}
		public int Main_Map_Address
		{
			get;
		}
		public int Main_Map_Pointer
		{
			get;
		}
		public int Main_RefreshMap_Address
		{
			get;
		}
		public int MapFullScreen_Address
		{
			get;
		}
		public int MouseRight_Address
		{
			get;
		}
		public int MouseRightRelease_Address
		{
			get;
		}
		public int ScreenWidth_Address
		{
			get;
		}
		public int ScreenHeight_Address
		{
			get;
		}
		public int MapFullscreenPos_Address
		{
			get;
		}
		public int MapFullscreenPos_Pointer
		{
			get;
		}

		public int MouseX_Address
		{
			get;
		}
		public int MouseY_Address
		{
			get;
		}
		public int TileTargetX_Address
		{
			get;
		}
		public int TileTargetY_Address
		{
			get;
		}
		public int MapFullScreenScale_Address
		{
			get;
		}
		public int Tile_Address
		{
			get;
		}
		public int Tile_Pointer
		{
			get;
		}
		public int NetMode_Address
		{
			get;
		}
		public int ActiveWorldFileData_Address
		{
			get;
		}
		public int ActiveWorldFileData_Pointer
		{
			get;
		}

		public int[] Debuff
		{
			get;
		}

		public int Debuff_Address
		{
			get;
		}
		public int Debuff_Pointer
		{
			get;
		}

		public int NetMode
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, NetMode_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, NetMode_Address, ref value, 4, 0);
			}
		}

		public float MapFullScreenScale
		{
			get
			{
				float v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullScreenScale_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullScreenScale_Address, ref value, 4, 0);
			}
		}

		public int MouseX
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseX_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseX_Address, ref value, 4, 0);
			}
		}
		public int MouseY
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseY_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseY_Address, ref value, 4, 0);
			}
		}

		public int TileTargetX
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, TileTargetX_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, TileTargetX_Address, ref value, 4, 0);
			}
		}
		public int TileTargetY
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, TileTargetY_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, TileTargetY_Address, ref value, 4, 0);
			}
		}

		public float MapFullscreenPos_X
		{
			get
			{
				byte[] b = new byte[4];
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4, b, 4, 0);
				return BitConverter.ToSingle(b, 0);
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4, BitConverter.GetBytes(value), 4, 0);
			}
		}
		public float MapFullscreenPos_Y
		{
			get
			{
				byte[] b = new byte[4];
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4 + 4, b, 4, 0);
				return BitConverter.ToSingle(b, 0);
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4 + 4, BitConverter.GetBytes(value), 4, 0);
			}
		}

		public int ScreenWidth
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, ScreenWidth_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, ScreenWidth_Address, ref value, 4, 0);
			}
		}

		public int ScreenHeight
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, ScreenHeight_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, ScreenHeight_Address, ref value, 4, 0);
			}
		}

		public bool MouseRightRelease
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseRightRelease_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseRightRelease_Address, ref value, 1, 0);
			}
		}

		public bool MouseRight
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseRight_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseRight_Address, ref value, 1, 0);
			}
		}


		public bool MapFullScreen
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullScreen_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullScreen_Address, ref value, 1, 0);
			}
		}


		public bool RefreshMap
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, Main_RefreshMap_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, Main_RefreshMap_Address, ref value, 1, 0);
			}
		}


		public int MyPlayerIndex
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, My_Player_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, My_Player_Address, ref value, 4, 0);
			}
		}

		public GameObjectArray2D<Tile> Tile
		{
			get
			{
				return new GameObjectArray2D<Tile>(this, Tile_Address);
			}
		}


		public PlayerArray Players
		{
			get
			{
				return new PlayerArray(this, Player_Array_Address);
			}
		}

		public NPCArray NPC
		{
			get
			{
				return new NPCArray(this, NPC_Array_Address);
			}
		}

		public Player MyPlayer
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, Player_Array_Address + HContext.ArrayHeadLength + 0x04 * MyPlayerIndex, ref v, 4, 0);
				return new Player(this, v);
			}
		}

		public WorldMap Map
		{
			get
			{
				return new WorldMap(this, Main_Map_Address);
			}
		}

		/// <summary>
		/// 这里的8被替换成了自适应的长度，如果在低版本.NET中无效将会被修改
		/// </summary>
		public string UUID
		{
			get
			{
				int v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "clientUUID");
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				byte[] bs = new byte[(32 + 4) * 2];
				NativeFunctions.ReadProcessMemory(HContext.Handle, v + HContext.ArrayHeadLength, bs, bs.Length, 0);
				return Encoding.Unicode.GetString(bs);
			}
			set
			{
				int v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "clientUUID");
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				byte[] bs = Encoding.Unicode.GetBytes(value);
				NativeFunctions.WriteProcessMemory(HContext.Handle, v + HContext.ArrayHeadLength, bs, (32 + 4) * 2, 0);
			}
		}

		public int MaxTilesX
		{
			get;
		}

		public int MaxTilesY
		{
			get;
		}

		public bool DayTime
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "dayTime");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "dayTime");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool FastForwardTime
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "fastForwardTime");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "fastForwardTime");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool PumpkinMoon
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "pumpkinMoon");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "pumpkinMoon");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool BloodMoon
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "bloodMoon");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "bloodMoon");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}
		public bool SnowMoon
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "snowMoon");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "snowMoon");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}


		public bool Eclipse
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "eclipse");
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "eclipse");
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public double Time
		{
			get
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "time");
				var bs = new byte[8];
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, bs, 8, 0);
				return BitConverter.ToDouble(bs, 0);
			}
			set
			{
				int vvv = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "time");
				var bs = BitConverter.GetBytes(value);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, bs, 8, 0);
			}
		}

		public WorldFileData ActiveWorldFileData
		{
			get
			{
				return new WorldFileData(this, ActiveWorldFileData_Address);
			}
		}

		private void InitalizeOffsets()
		{
			if (OffsetsInitialized)
				return;
			foreach (var t in System.Reflection.Assembly.GetExecutingAssembly().DefinedTypes)
			{
				if (t.Namespace != "QTRHacker.Functions.GameObjects")
					continue;
				var typeNameAttr = t.GetCustomAttributes(typeof(GameFieldOffsetTypeNameAttribute), false);
				foreach (var f in t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
				{
					if (!f.IsStatic ||
						!(f.FieldType == typeof(int) || f.FieldType == typeof(uint) ||
						f.FieldType == typeof(long) || f.FieldType == typeof(ulong)))
						continue;
					var fieldNameAttr = f.GetCustomAttributes(typeof(GameFieldOffsetFieldNameAttribute), false);
					if (fieldNameAttr.Length == 0)
						continue;
					GameFieldOffsetFieldNameAttribute gfofna = fieldNameAttr[0] as GameFieldOffsetFieldNameAttribute;
					if (gfofna.TypeName == null && typeNameAttr.Length == 0)//信息不足
						continue;
					Microsoft.Diagnostics.Runtime.ClrType clrType = null;
					if (gfofna.TypeName != null)
						clrType = HContext.MainAddressHelper.Module.GetTypeByName(gfofna.TypeName);
					else
						clrType = HContext.MainAddressHelper.Module.GetTypeByName((typeNameAttr[0] as GameFieldOffsetTypeNameAttribute).TypeName);
					var field = clrType.GetFieldByName(gfofna.FieldName);
					if (field == null)
						throw new Exception("Field named " + gfofna.FieldName + " not found.(" + f.DeclaringType.FullName + "." + f.Name + ")");
					f.SetValue(null, field.Offset + 4);//需要+4，原因是Offset比真实的"偏移"要少了一个指针的位置
				}
			}
		}

		public static void NewText(GameContext Context, string Text, byte R = 255, byte G = 255, byte B = 255, bool force = false)
		{
			int addr = Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main",
				t => t.GetFullSignature() ==
				"Terraria.Main.NewText(System.String, Byte, Byte, Byte, Boolean)");
			CLRFunctionCaller.Call(Context, addr, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), $"@{Text}", R, G, B, force);
		}

		/// <summary>
		/// 如果你不知道这是干什么的
		/// 请不要调用
		/// </summary>
		/// <param name="pid"></param>
		private void LoadAllInjections(int pid)
		{
			if (AssembliesLoaded)
				return;
			using (Context tmpC = Context.Create(pid))
			{
				byte b = 0;
				NativeFunctions.ReadProcessMemory(tmpC.Handle, tmpC.MainAddressHelper["Terraria.Main", "Update"], ref b, 1, 0);
				if (b == 0xE9)//already loaded
				{
					AssembliesLoaded = true;
					return;
				}
			}

			Context.LoadAssembly(pid, Path.GetFullPath("QTRInjectionBase.dll"), "QTRInjectionBase.QTRInjectionBase");
			Context.LoadAssembly(pid, Path.GetFullPath("TRInjections.dll"), "TRInjections.TRInjections");
		}

		/// <summary>
		/// 如果你不知道这是干什么的
		/// 请不要调用
		/// </summary>
		private void ProcessAllInjections()
		{
			if (AssembliesLoaded)
				return;
			InjectHookHelper.Process(this, "QTRInjectionBase.dll");
			InjectHookHelper.Process(this, "TRInjections.dll");
		}

		/// <summary>
		/// 凡是没有标明Pointer的，获取的均为基址，而非指向该地址的指针
		/// 基础数据类型没有基址，获得的是地址，而非基址
		/// </summary>
		/// <param name="pid">游戏的进程ID</param>
		private GameContext(int pid)
		{
			LoadAllInjections(pid);
			HContext = Context.Create(pid);
			ProcessAllInjections();


			ArrayHeadLength = HContext.ArrayHeadLength;
			InitalizeOffsets();



			int v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "player");
			Player_Array_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			Player_Array_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "myPlayer");
			My_Player_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "npc");
			NPC_Array_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			NPC_Array_Address = v;

			//下面两个取的是值，而非地址
			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "maxTilesX");
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			MaxTilesX = v;
			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "maxTilesY");
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			MaxTilesY = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "Map");
			Main_Map_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			Main_Map_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "refreshMap");
			Main_RefreshMap_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreen");
			MapFullScreen_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mouseRight");
			MouseRight_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mouseRightRelease");
			MouseRightRelease_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "screenWidth");
			ScreenWidth_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "screenHeight");
			ScreenHeight_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreenPos");
			MapFullscreenPos_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			MapFullscreenPos_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mouseX");
			MouseX_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mouseY");
			MouseY_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreenScale");
			MapFullScreenScale_Address = v;


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "tile");
			Tile_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			Tile_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "netMode");
			NetMode_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetX");
			TileTargetX_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetY");
			TileTargetY_Address = v;

			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "debuff");
			Debuff_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			Debuff_Address = v;

			//这里使用了自适应的数组头部长度，如果无效将会被修改
			int bbbb = 0;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v + HContext.ArrayHeadLength - 4, ref bbbb, 4, 0);
			Debuff = new int[bbbb];
			for (int i = 0; i < bbbb; i++)
				NativeFunctions.ReadProcessMemory(HContext.Handle, Debuff_Address + HContext.ArrayHeadLength + i, ref Debuff[i], 1, 0);


			v = HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "ActiveWorldFileData");
			ActiveWorldFileData_Pointer = v;
			NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
			ActiveWorldFileData_Address = v;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pid">游戏的进程ID</param>
		/// <returns></returns>
		public static GameContext OpenGame(int pid)
		{
			return new GameContext(pid);
		}
		public void Close()
		{
			HContext?.Close();
		}

		public void Dispose()
		{
			Close();
		}


	}
}
