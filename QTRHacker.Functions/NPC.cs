using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class NPC : GameObject
	{
		public const int MAXNUMBER = 201;
		public const int OFFSET_Active = 0x18;
		public const int OFFSET_X = 0x20;
		public const int OFFSET_Y = 0x24;

		public float X
		{
			get
			{
				ReadFromOffset(OFFSET_X, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_X, value);
		}

		public float Y
		{
			get
			{
				ReadFromOffset(OFFSET_Y, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Y, value);
		}

		public bool Active
		{
			get
			{
				ReadFromOffset(OFFSET_Active, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Active, value);
		}

		public NPC(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public static void NewNPC(GameContext Context, int x, int y, int type, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255)
		{

			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.NPC::NewNPC"),
				null,
				true,
				x, y, type, start, ai0, ai1, ai2, ai3, target);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}

		public void AddBuff(int type, int time, bool quiet = false)
		{

			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.NPC::AddBuff"),
				null,
				true,
				BaseAddress, type, time, quiet);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}
	}
}
