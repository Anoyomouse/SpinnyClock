using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication4
{
	public partial class AddBox : Form
	{
		public AddBox()
		{
			InitializeComponent();

#if !OLDCODE
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
#else
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
#endif
		}

		private void AddBox_Load(object sender, EventArgs e)
		{
		}

		public void Reset()
		{
			// RESET Textboxes here to default values
			txtName.Text = "Anoyomouse";
			txtOffset.Text = "+2";
			chkDST.Checked = false;
		}

		public string NickName
		{
			get
			{
				return txtName.Text;
			}
		}

		public float Offset
		{
			get
			{
				return float.Parse(txtOffset.Text);
			}
		}

		public bool IsDST
		{
			get
			{
				return chkDST.Checked;
			}
		}

		Bitmap bmp;
		protected override void OnPaint(PaintEventArgs e)
		{
			if (bmp == null || (bmp.Width != this.ClientSize.Width || bmp.Height != this.ClientSize.Height))
			{
				bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
				this.Region = DrawStuff.DoRegion(this.ClientSize.Width, this.ClientSize.Height);
			}

			Graphics g = Graphics.FromImage(bmp);
			Color bg = Color.FromArgb(255, 7, 47, 0);
			this.BackColor = bg;
			g.FillRectangle(new SolidBrush(bg), 0, 0, this.ClientSize.Width, this.ClientSize.Height);

			Pen lg, ng, dg;
			lg = new Pen(Color.FromArgb(255, 7, 70, 0));
			ng = new Pen(Color.FromArgb(255, 7, 50, 0));
			dg = new Pen(Color.FromArgb(255, 7, 30, 0));
			{
				Pen tmp;
				tmp = lg;
				lg = dg;
				dg = tmp;
			}
			DrawStuff.DrawHatchedBackground(g, this.ClientSize.Width, this.ClientSize.Height, 10, lg, ng, dg);

			Pen p = new Pen(Color.FromArgb(255, 14, 65, 0), 6);

			DrawStuff.DrawArcBorder(g, this.ClientSize.Width, this.ClientSize.Height, 3, p);

			p = new Pen(Color.Goldenrod, 2);

			DrawStuff.DrawArcBorder(g, this.ClientSize.Width, this.ClientSize.Height, 0, p);

			Font fnt = new System.Drawing.Font("MS Sans Serif", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

			DrawStuff.DrawTextShadow(g, "GMT", fnt, Brushes.Goldenrod, txtOffset.Left + txtOffset.Width + 10, txtOffset.Top + 3);
			DrawStuff.DrawTextShadowLeft(g, "Name", fnt, Brushes.Goldenrod, txtName.Top, txtName.Left - 5, txtName.Height);
			DrawStuff.DrawTextShadowLeft(g, "Offset", fnt, Brushes.Goldenrod, txtOffset.Top, txtOffset.Left - 5, txtOffset.Height);
			DrawStuff.DrawTextShadowLeft(g, "DST", fnt, Brushes.Goldenrod, chkDST.Top, chkDST.Left - 5, chkDST.Height);

			e.Graphics.DrawImage(bmp, 0, 0);
		}

		bool bDragging;
		Point pntStart;
		private void StartDrag(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pntStart = new Point();
			if (e.Button == MouseButtons.Left)
			{
				bDragging = true;
				pntStart.X = e.X;
				pntStart.Y = e.Y;
			}
		}

		private void DoDrag(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (bDragging && e.Button == MouseButtons.Left)
			{
				int dx = -pntStart.X + e.X;
				int dy = -pntStart.Y + e.Y;

				this.Left += dx;
				this.Top += dy;
			}
		}

		private void EndDrag(object sender, MouseEventArgs e)
		{
			bDragging = false;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			try
			{
				float.Parse(txtOffset.Text);
			}
			catch (FormatException)
			{
				return;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}