using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Terraria_Hacker
{
    class ItemIcon : PictureBox
    {
        public int Number;
        public bool Selected = false;
        private int lastID;
		private ToolTip Tip;
        public ItemIcon(int num)
        {
            Number = num;
			Tip = new ToolTip();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            /*int nowID = HackFunctions.getItemType(Number);
            if (lastID != nowID)
            {
                var img = MainForm.item_images.Images["Item_" + nowID];
                if (img != null)
                {
                    Graphics g = Graphics.FromImage(img);
                    Image newImg = (Image)img.Clone();
                    g.DrawImage(newImg, 0, 0);
                    this.Image = newImg;
                    g.Dispose();
					Tip.SetToolTip(this, MainForm.resource.Items.First(i => i.id == nowID).name);
				}
				else
				{
					Tip.SetToolTip(this, "");
				}
                this.lastID = nowID;
            }
            base.OnPaint(pe);
            pe.Graphics.DrawString(HackFunctions.getItemStack(Number).ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 10, 35);
            if (Selected)
            {
                pe.Graphics.DrawRectangle(new Pen(Color.BlueViolet, 3), 1, 1, pe.ClipRectangle.Width - 3, pe.ClipRectangle.Height - 3);
            }*/
        }
    }
    class AltItemIcon : PictureBox
    {
        public int ID = 0, Stack = 0;
        public byte Prefix = 0;
        
    }
    public partial class InvEditor : Form
    {
        private int ControlID = 0;
        private Hashtable hacks;
        private Panel HackPanel, AltPanel;
        private ItemIcon[] ItemSlots;
        private AltItemIcon[] AltSlots;
        private const int AltPanelWidth = 90, AltPanelHeight = 270, AltGap = 3, AltWidth = 30 - AltGap;
        private const int SlotsWidth = 50;
        private const int SlotsGap = 5;
        private Panel SlotsPanel;
        private CheckBox AutoReuse, Equippable;
        private ComboBox PrefixComboBox;
        private Timer timer;
        private int Selected = 0, LastSelectedID = 0;
        private AltItemIcon AltSelected;
#pragma warning disable CS1690
        public InvEditor()
        {
			/*BackColor = Color.LightGray;
            hacks = new Hashtable();
            HackPanel = new Panel();
            ItemSlots = new ItemIcon[HackFunctions.INV_MAX_COUNT];
            AltSlots = new AltItemIcon[AltPanelWidth * AltPanelHeight];

            SlotsPanel = new Panel();
            InitializeComponent();

            SlotsPanel.Size = new Size(ItemSlots.Length / 5 * (SlotsWidth + SlotsGap), this.ClientSize.Height);
            SlotsPanel.Location = new Point(5, 5);
            this.Controls.Add(SlotsPanel);

            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Copy");
            cms.Items.Add("Paste");
            cms.ItemClicked += (sender, e) =>
            {
                switch (e.ClickedItem.Text)
                {
                    case "Copy":
                        HackFunctions.CopyItem(Selected);
                        RefreshSelected();
                        break;
                    case "Paste":
                        if (HackFunctions.IsCopiedItem())
                            HackFunctions.PasteItem(Selected);
                        RefreshSelected();
                        break;
                }
            };
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                int row = (int)Math.Floor((double)(i / 10));
                int off = i % 10;
                ItemSlots[i] = new ItemIcon(i)
                {
                    Size = new Size(SlotsWidth, SlotsWidth),
                    Location = new Point(off * (SlotsWidth + SlotsGap), row * (SlotsWidth + SlotsGap)),


                    BackColor = Color.CadetBlue,
                    SizeMode = PictureBoxSizeMode.CenterImage
                };
                ItemSlots[i].Click += (sender, e) =>
                  {
                      MouseEventArgs mea = (MouseEventArgs)e;
                      ItemIcon ii = (ItemIcon)sender;

                      foreach (var s in ItemSlots)
                      {
                          s.Selected = false;
                      }
                        ((ItemIcon)sender).Selected = true;
                      SlotsPanel.Refresh();
                      Selected = ((ItemIcon)sender).Number;
                      InitData(Selected);
                      if (mea.Button == MouseButtons.Right)
                      {
                          cms.Show(ii, mea.Location.X, mea.Location.Y);
                      }
                  };
                this.SlotsPanel.Controls.Add(ItemSlots[i]);
            }

            ContextMenuStrip altCms = new ContextMenuStrip();
            altCms.Items.Add("Edit");

            altCms.ItemClicked += (sender, e) =>
            {
                switch (e.ClickedItem.Text)
                {
                    case "Edit":
                        {
                            Form f = new Form();
                            TextBox ItemID = new TextBox();
                            TextBox ItemCount = new TextBox();
                            ComboBox prefix = new ComboBox();
                            Button et = new Button();

                            f.Text = "Edit";
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.FormBorderStyle = FormBorderStyle.FixedSingle;
                            f.MaximizeBox = false;
                            f.MinimizeBox = false;
                            f.Size = new Size(250, 90);

                            Label tip1 = new Label()
                            {
                                Text = "ItemID",
                                Location = new Point(0, 5),
                                Size = new Size(80, 20)
                            };
                            f.Controls.Add(tip1);

                            ItemID.Location = new Point(85, 0);
                            ItemID.Size = new Size(95, 20);
                            ItemID.Text = AltSelected.ID.ToString();
                            ItemID.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                            {
                                if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8)
                                {
                                    e1.Handled = true;
                                }
                            };
                            f.Controls.Add(ItemID);


                            Label tip2 = new Label()
                            {
                                Text = "ItemStack",
                                Location = new Point(0, 25),
                                Size = new Size(80, 20)
                            };
                            f.Controls.Add(tip2);

                            ItemCount.Location = new Point(85, 20);
                            ItemCount.Size = new Size(95, 20);
                            ItemCount.Text = AltSelected.Stack.ToString();
                            ItemCount.KeyPress += delegate (object sender1, KeyPressEventArgs e1)
                            {
                                if (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8)
                                {
                                    e1.Handled = true;
                                }
                            };
                            f.Controls.Add(ItemCount);

                            prefix.Location = new Point(85, 40);
                            prefix.Size = new Size(95, 20);
                            prefix.DropDownStyle = ComboBoxStyle.DropDownList;
                            prefix.DropDownHeight = 150;
                            foreach (var o in MainForm.resource.Prefix)
                            {
                                string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                string v = t[0];
                                prefix.Items.Add(v);
                            }
                            prefix.SelectedIndex = GetIndexFromPrefix(AltSelected.Prefix);

                            f.Controls.Add(prefix);

                            Label tip3 = new Label()
                            {
                                Text = "ItemPrefix",
                                Location = new Point(0, 45),
                                Size = new Size(80, 20)
                            };
                            f.Controls.Add(tip3);


                            et.Text = Lang.confirm;
                            et.Size = new Size(65, 60);
                            et.Location = new Point(180, 0);
                            et.Click += delegate (object sender1, EventArgs e1)
                            {
                                AltSelected.ID = Convert.ToInt32(ItemID.Text);
                                AltSelected.Stack = Convert.ToInt32(ItemCount.Text);
                                AltSelected.Prefix = GetPrefixFromIndex(prefix.SelectedIndex);
                                f.Dispose();
                                var img = MainForm.item_images.Images["Item_" + AltSelected.ID];
                                if (img != null)
                                    AltSelected.Image = img;
                                SaveAltItems();
                            };
                            f.Controls.Add(et);
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.ShowDialog(this);
                        }
                        break;
                }
            };
            AltPanel = new Panel() { Location = new Point(560, 5), Size = new Size(AltPanelWidth, AltPanelHeight) };
            Controls.Add(AltPanel);
            if (!File.Exists("AlternativeItem"))
            {
                BinaryWriter bw = new BinaryWriter(File.Open("AlternativeItem", FileMode.OpenOrCreate));//ID Stack Prefix
                for (int i = 0; i < (AltPanelHeight / AltWidth) * (AltPanelWidth / AltWidth); i++)
                {
                    bw.Write(0);        //ID
                    bw.Write(0);        //Stack
                    bw.Write((byte)0);  //Prefix
                }
                bw.Close();
            }
            BinaryReader br = new BinaryReader(File.Open("AlternativeItem", FileMode.Open));//ID Stack Prefix
            for (int i = 0; i < AltPanelHeight / AltWidth; i++)
            {
                for (int j = 0; j < AltPanelWidth / AltWidth; j++)
                {
                    int n = i * AltPanelWidth / AltWidth + j;
                    AltSlots[n] = new AltItemIcon()
                    {
                        Size = new Size(AltWidth - AltGap, AltWidth - AltGap),
                        Location = new Point(j * (AltWidth + AltGap), i * (AltWidth + AltGap) + 2),
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        ID = br.ReadInt32(),
                        Stack = br.ReadInt32(),
                        Prefix = br.ReadByte(),
                    };
                    Image img;
                    if ((img = MainForm.item_images.Images["Item_" + AltSlots[n].ID]) != null)
                    {
                        AltSlots[n].Image = img;
                    }
                    AltSlots[n].BackColor = Color.CadetBlue;
                    AltSlots[n].Click += (sender, e) =>
                    {

                        MouseEventArgs mea = (MouseEventArgs)e;
                        AltItemIcon aii = (AltItemIcon)sender;
                        this.AltSelected = aii;
                        if (mea.Button == MouseButtons.Right)
                        {
                            altCms.Show(aii, mea.Location.X, mea.Location.Y);
                        }
                    };
                    AltSlots[n].DoubleClick += (sender, e) =>
                    {
                        for (int h = 0; h < HackFunctions.INV_MAX_COUNT; h++)
                        {
                            if (HackFunctions.getItemType(h) == 0)
                            {
                                HackFunctions.SetItemDefaults(h, AltSelected.ID, AltSelected.Prefix);
                                HackFunctions.setItemStack(h, AltSelected.Stack);
                                break;
                            }
                        }
                    };
                    AltPanel.Controls.Add(AltSlots[n]);
                }
            }
            br.Close();

            HackPanel.Location = new Point(ItemSlots.Length / 5 * (SlotsWidth + SlotsGap) + 105, 5);
            HackPanel.Size = new Size(350, this.ClientSize.Height);
            this.Controls.Add(HackPanel);




            AddTextBox(Lang.itemID, "ItemType", null);
            AddTextBox(Lang.damage, "ItemDamage", null);
            AddTextBox(Lang.number, "ItemStack", null);
            AddTextBox(Lang.knockBack, "ItemKnockBack", null, true);
            AddTextBox(Lang.crit, "ItemCrit", null);
            AddTextBox(Lang.buff, "ItemBuffType", null);
            AddTextBox(Lang.buffTime, "ItemBuffTime", null);
            AddTextBox(Lang.manaInc, "ItemHealMana", null);
            AddTextBox(Lang.lifeIncrease, "ItemHealLife", null);
            AddTextBox(Lang.useCD, "ItemUseTime", null);
            AddTextBox(Lang.waveCD, "ItemUseAnimation", null);
            AddTextBox(Lang.scale, "ItemScale", null, true);
            AddTextBox(Lang.defense, "ItemDefense", null);
            AddTextBox(Lang.projSpeed, "ItemShootSpeed", null, true);
            AddTextBox(Lang.projID, "ItemShoot", null);
            AddTextBox(Lang.dig, "ItemPick", null);
            AddTextBox(Lang.hag, "ItemAxe", null);
            AddTextBox(Lang.hammer, "ItemHammer", null);
            AddTextBox(Lang.digRange, "ItemTileBoost", null);
            AddTextBox(Lang.tileID, "ItemCreateTile", null);
            AddTextBox(Lang.placeStyle, "ItemPlaceStyle", null);
            AddTextBox(Lang.fishingPower, "ItemFishingPole", null);
            AddTextBox(Lang.baitPower, "ItemBait", null);

            PrefixComboBox = AddComboBox(Lang.prefix, MainForm.resource.Prefix);

            AutoReuse = new CheckBox()
            {
                Text = Lang.autoReuse,
                Size = new Size(130, 20),
                Location = new Point(0, 245)
            };
            HackPanel.Controls.Add(AutoReuse);

            Equippable = new CheckBox()
            {
                Text = Lang.equippable,
                Size = new Size(130, 20),
                Location = new Point(135, 245)
            };
            HackPanel.Controls.Add(Equippable);

            Button OK = new Button();
            OK.Click += (sender, e) =>
            {
                PixelData(Selected);
                InitData(Selected);
                RefreshSelected();
            };
            OK.Text = Lang.confirmHack;
            OK.Size = new Size(80, 30);
            OK.Location = new Point(260, 0);
            HackPanel.Controls.Add(OK);


            Button Refresh = new Button();
            Refresh.Click += (sender, e) =>
            {
                InitData(Selected);
                SlotsPanel.Refresh();
            };
            Refresh.Text = Lang.refresh;
            Refresh.Size = new Size(80, 30);
            Refresh.Location = new Point(260, 30);
            HackPanel.Controls.Add(Refresh);


            Button SaveInv = new Button();
            SaveInv.Click += (sender, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "inv files (*.inv)|*.inv"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    HackFunctions.SaveInv(sfd.FileName);
                    SlotsPanel.Refresh();
                }
            };
            SaveInv.Text = Lang.save;
            SaveInv.Size = new Size(80, 30);
            SaveInv.Location = new Point(260, 60);
            HackPanel.Controls.Add(SaveInv);

            Button LoadInv = new Button();
            LoadInv.Click += (sender, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "inv files (*.inv)|*.inv"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    HackFunctions.LoadInv(this, ofd.FileName);
                    SlotsPanel.Refresh();
                    InitData(Selected);
                }
            };
            LoadInv.Text = Lang.load;
            LoadInv.Size = new Size(80, 30);
            LoadInv.Location = new Point(260, 90);
            HackPanel.Controls.Add(LoadInv);

            Button InitItem = new Button();
            InitItem.Click += (sender, e) =>
            {
                HackFunctions.SetItemDefaults(Selected, Convert.ToInt32(((TextBox)hacks["ItemType"]).Text), GetPrefixFromIndex(PrefixComboBox.SelectedIndex));
                int stack = Convert.ToInt32(((TextBox)hacks["ItemStack"]).Text);
                HackFunctions.setItemStack(Selected, stack == 0 ? 1 : stack);
                RefreshSelected();
                InitData(Selected);
            };
            InitItem.Text = Lang.init;
            InitItem.Size = new Size(80, 30);
            InitItem.Location = new Point(260, 120);
            HackPanel.Controls.Add(InitItem);

            ItemSlots[0].Selected = true;
            InitData(0);
            timer = new Timer()
            {
                Interval = 500
            };
            timer.Tick += (sender, e) =>
            {
                if (this.Enabled)
                {
                    SlotsPanel.Refresh();
                    if (LastSelectedID != HackFunctions.getItemType(Selected))
                    {
                        InitData(Selected);
                        LastSelectedID = HackFunctions.getItemType(Selected);
                    }
                }
            };
            timer.Start();*/
        }
        private void RefreshSelected()
        {
            ItemSlots[Selected].Refresh();
        }
        private void InitData(int slot)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType("Terraria_Hacker.HackFunctions");
            foreach (DictionaryEntry de in hacks)
            {
                object[] args = new object[1];
                args[0] = slot;
                MethodInfo mi = type.GetMethod("get" + de.Key);
                if (mi == null)
                    return;
                ((TextBox)de.Value).Text = Convert.ToString(mi.Invoke(null, args));

            }/*
            {
                PrefixComboBox.SelectedIndex = GetIndexFromPrefix(HackFunctions.getItemPrefix(slot));
            }
            {
                AutoReuse.CheckState = HackFunctions.getItemAutoReuse(slot) ? CheckState.Checked : CheckState.Unchecked;
            }
            {
                Equippable.CheckState = HackFunctions.getItemAccessory(slot) ? CheckState.Checked : CheckState.Unchecked;
            }*/
        }
        private void PixelData(int slot)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType("Terraria_Hacker.HackFunctions");
            foreach (DictionaryEntry de in hacks)
            {
                object[] args = new object[2];
                MethodInfo mi = type.GetMethod("set" + de.Key);
                args[0] = slot;
                if (mi.GetParameters()[1].ParameterType == typeof(int))
                {
                    args[1] = Convert.ToInt32(((TextBox)de.Value).Text);
                }
                else if (mi.GetParameters()[1].ParameterType == typeof(float))
                {
                    args[1] = (float)Convert.ToDouble(((TextBox)de.Value).Text);
                }
                mi.Invoke(null, args);
            }/*
            {
                HackFunctions.setItemPrefix(slot, GetPrefixFromIndex(PrefixComboBox.SelectedIndex));
            }
            {
                HackFunctions.setItemAutoReuse(slot, (AutoReuse.CheckState == CheckState.Checked));
            }
            {
                HackFunctions.setItemAccessory(slot, (Equippable.CheckState == CheckState.Checked));
            }*/
        }
        public void SaveAltItems()
        {
            BinaryWriter bw = new BinaryWriter(File.Open("AlternativeItem", FileMode.OpenOrCreate));//ID Stack Prefix
            for (int i = 0; i < AltPanelHeight / AltWidth; i++)
            {
                for (int j = 0; j < AltPanelWidth / AltWidth; j++)
                {
                    int n = i * AltPanelWidth / AltWidth + j;
                    bw.Write(AltSlots[n].ID);
                    bw.Write(AltSlots[n].Stack);
                    bw.Write(AltSlots[n].Prefix);
                }
            }
            bw.Close();
        }
        public static byte GetPrefixFromIndex(int id)
        {
            return Convert.ToByte(MainForm.resource.Prefix[id].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }
        private int GetIndexFromPrefix(byte id)
        {
            int j = 0;
            foreach (var o in MainForm.resource.Prefix)
            {
                string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                byte i = Convert.ToByte(t[1]);
                if (i == id)
                    return j;
                j++;
            }
            return 0;
        }
        private ComboBox AddComboBox(string tipstr, string[] src)
        {
            int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
            Label tip = new Label();
            ComboBox box = new ComboBox();
            tip.Text = tipstr;
            tip.Location = new Point(130 * a, 20 * b);
            tip.Size = new Size(60, 20);
            tip.Font = new Font("Arial", 9);
            box.Size = new Size(60, 20);
            box.Location = new Point(60 + 130 * a, 20 * b);
            box.DropDownStyle = ComboBoxStyle.DropDownList;
            box.DropDownHeight = 150;
            foreach (var o in src)
            {
                string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                string v = t[0];
                box.Items.Add(v);
            }
            HackPanel.Controls.Add(tip);
            HackPanel.Controls.Add(box);
            ControlID++;
            return box;
        }
        private TextBox AddTextBox(string tipstr, string hack, EventHandler handler, bool f = false)
        {
            int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
            Label tip = new Label();
            TextBox val = new TextBox();
            tip.Text = tipstr;
            tip.Location = new Point(130 * a, 20 * b);
            tip.Size = new Size(60, 20);
            tip.Font = new Font("Arial", 9);
            val.Size = new Size(60, 20);
            val.Location = new Point(60 + 130 * a, 20 * b);
            val.Multiline = true;
            val.MaxLength = 7;
            val.Font = new Font("Arial", 8);
            val.KeyDown += delegate (object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                }
            };
            val.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
                if (e.KeyChar == '.' && f)
                    e.Handled = false;
            };
            if (val != null)
                val.TextChanged += handler;
            HackPanel.Controls.Add(tip);
            HackPanel.Controls.Add(val);
            if (hack != "")
                hacks.Add(hack, val);
            ControlID++;
            return val;
        }
    }
}
