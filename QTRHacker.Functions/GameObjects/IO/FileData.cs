using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.IO
{
	[GameFieldOffsetTypeName("Terraria.IO.FileData")]
	public class FileData : GameObject
	{
		[GameFieldOffsetFieldName("Name")]
		public const int OFFSET_Name = 0xc;
		public string Name
		{
			get
			{
				ReadFromOffset(OFFSET_Name, out int a);
				int b = 0;
				NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 0x4, ref b, 4, 0);
				byte[] c = new byte[b * 2];
				NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 0x8, c, c.Length, 0);
				return Encoding.Unicode.GetString(c);
			}
		}
		public FileData(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
