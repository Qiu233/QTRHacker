/*
 * Created by SharpDevelop.
 * User: Qiu233
 * Date: 2016/7/28
 * Time: 12:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Drawing;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of Resources.
	/// </summary>
	public class Resources
	{
		public struct Item
		{
			public int id;
			public string name;
		}
		public struct Buff
		{
			public int id;
			public string name;
		}
		public ResourceManager res;
		public string[] Prefix;
		public string[] Pets;
		public string[] Mounts;
		public Item[] Items;
		public Buff[] Buffs;
		public byte[] ItemImage;

		public Resources()
		{
			res = new ResourceManager("Terraria_Hacker.Res", this.GetType().Assembly);
			ItemImage = (byte[])res.GetObject("ItemImage");
			Prefix = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("PreFix")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
#if ENG
			Pets = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("Pet_Eng")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Mounts=System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("Mount_Eng")).Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
			string[] tmp_Items=System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("ItemID_Eng")).Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
#else
			Pets = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("Pet")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Mounts = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("Mount")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			string[] tmp_Items = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("ItemID")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
#endif
			string[] tmp_Buffs = System.Text.Encoding.UTF8.GetString((byte[])res.GetObject("BuffID")).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Items = new Item[tmp_Items.Length];
			Buffs = new Buff[tmp_Buffs.Length];
			for (int i = 0; i < tmp_Items.Length; i++)
			{
				string[] r = tmp_Items[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				Items[i].id = Convert.ToInt32(r[1]);
				Items[i].name = r[0];
			}
			for (int i = 0; i < tmp_Buffs.Length; i++)
			{
				string[] r = tmp_Buffs[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				Buffs[i].id = Convert.ToInt32(r[1]);
				Buffs[i].name = r[0];
			}

		}
		public BinaryReader GetItemInfoStream()
		{
			byte[] data = (byte[])res.GetObject("ItemInfo");
			return new BinaryReader(new MemoryStream(data));
		}
		//used to read file list
		/*public Image GetItemImageFromIndex(int index)
		{
			
			string path="./Item_Images/Item_"+index+".png";
			if (!File.Exists(path)) return null;
			return Image.FromFile(path);
		}*/
	}
}
