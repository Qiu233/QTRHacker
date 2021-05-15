using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanguagesManager
{
	public class CDataGridView : DataGridView
	{
		public static readonly Color ContentBackColor = Color.FromArgb(150, 150, 150);
		public CDataGridView()
		{
			BackgroundColor = Color.FromArgb(120, 120, 120);
			BorderStyle = BorderStyle.Fixed3D;
			TopLeftHeaderCell.Value = "Index";
			RowHeadersWidth = 220;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			AllowUserToResizeColumns = false;
			AllowUserToResizeRows = false;
			AllowUserToAddRows = false;
			EnableHeadersVisualStyles = false;
			SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			EditMode = DataGridViewEditMode.EditOnEnter;
			MultiSelect = false;
			ColumnHeadersHeight = 30;

			AdvancedRowHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
			AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
			AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;

			ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(120, 130, 130);
			ColumnHeadersDefaultCellStyle.Font = new Font(SystemFonts.DefaultFont.FontFamily, 12f);

			DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			DefaultCellStyle.BackColor = ContentBackColor;
			DefaultCellStyle.SelectionBackColor = Color.FromArgb(130, 130, 130);

			RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			RowHeadersDefaultCellStyle.BackColor = ContentBackColor;
			RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(160, 160, 130);

			GridColor = Color.FromArgb(80, 80, 80);

			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		public int AddColumn(string key)
		{
			int id = Columns.Add(key, key);
			var cl = Columns[id];
			cl.Width = 150;
			cl.SortMode = DataGridViewColumnSortMode.NotSortable;
			return id;
		}
		public int AddRow(string key)
		{
			int id = Rows.Add();
			var r = Rows[id];
			r.HeaderCell.Value = key;
			return id;
		}
		public bool ExistRow(string key)
		{
			foreach (DataGridViewRow dgr in Rows)
			{
				if (dgr.HeaderCell.Value as string == key)
					return true;
			}
			return false;
		}
	}
}
