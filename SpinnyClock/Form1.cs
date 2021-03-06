//#define OLDCODE

using System;
#if !OLDCODE
using System.Collections.Generic;
#else
using System.Collections;
#endif
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace WindowsApplication4
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

#if !OLDCODE
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
#else
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
#endif
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void timTick_Tick(object sender, EventArgs e)
		{
			//DateTime now = DateTime.Now;
			//textBox1.Text = now.ToString("HH:mm:ss fffffff");
			//textBox2.Text = now.AddHours(-9.0).ToString("HH:mm:ss fffffff");
			this.Refresh();
		}

#if !OLDCODE
		private List<TimeItem> lst;
		private List<Button> butRem;
#else
		private ArrayList lst;
		private ArrayList butRem;
#endif

		private void Form1_Load(object sender, EventArgs e)
		{
#if !OLDCODE
			lst = new List<TimeItem>();
			butRem = new List<Button>();
#else
			lst = new ArrayList();
			butRem = new ArrayList();
#endif
			FileInfo f = new FileInfo("data.xml");
			if (!f.Exists)
			{
				StreamWriter write = new StreamWriter("data.xml");
				write.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				write.WriteLine("<root>");
				write.WriteLine("	<time name=\"Anoyomouse\" offset=\"+2\" dst=\"False\" />");
				write.WriteLine("</root>");
				write.Flush();
				write.Close();
			}

			XPathDocument doc = new XPathDocument("data.xml");

			XPathNavigator nav = doc.CreateNavigator();
			nav.MoveToNext();

			ParseTree(nav, ref lst);

			/*
			lst.Add(new TimeItem("My Time", +2, false));
			lst.Add(new TimeItem("x_panther", -8, true)); // -9 // CHECK
			lst.Add(new TimeItem("GeeKMan", -7, true)); // -8 // DST ...
			lst.Add(new TimeItem("Rick2Tails", -7, false)); // -9 // NO DST
			lst.Add(new TimeItem("FurtiveFox", -8, true)); // -9 // CHECK
			lst.Add(new TimeItem("Dodger013", -6, true)); // -7 // DST
			lst.Add(new TimeItem("Zan", -6, true)); // -7 // CHECK
				*/
			// DST BEGAN ON THE 3rd APRIL at 2:00 AM local SUNDAY
			// DST ENDS ON THE 30th OCTOBER at 2:00 AM DST local SUNDAY
		}

#if !OLDCODE
		private void ParseTree(XPathNavigator nav, ref List<TimeItem> lst)
#else
		private void ParseTree(XPathNavigator nav, ref ArrayList lst)
#endif
		{
			if (nav.NodeType == XPathNodeType.Root)
			{
				nav.MoveToFirstChild();
				ParseTree(nav, ref lst);
			}
			else if (nav.NodeType == XPathNodeType.Text)
			{
			}
			else if (nav.NodeType == XPathNodeType.Comment)
			{
			}
			else if (nav.NodeType == XPathNodeType.Element)
			{
				if (nav.Name == "time")
				{
					string name = "Me", dst = "False", offset = "+2";

					if (nav.HasAttributes)
					{
						nav.MoveToFirstAttribute();
						do
						{
							if (nav.Name == "name")
								name = nav.Value;
							else if (nav.Name == "offset")
								offset = nav.Value;
							else if (nav.Name == "dst")
								dst = nav.Value;
						}
						while (nav.MoveToNextAttribute());
						nav.MoveToParent();

						lst.Add(new TimeItem(name, float.Parse(offset), bool.Parse(dst)));
					}
				}

				if (nav.HasChildren)
				{
					if (!nav.MoveToFirstChild())
						Console.WriteLine("ERROR, cannot move to the child node");

					do
					{
						ParseTree(nav, ref lst);
					}
					while (nav.MoveToNext());
					nav.MoveToParent();
				}
			}
			else
			{
				Console.WriteLine("Node is a \"{0}\" Node", nav.NodeType);
			}
		}

		Bitmap bmp;
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.ClientSize.Height != (30 + lst.Count * 20 + 100 + 20))
			{
				int offset = this.Height - this.ClientSize.Height;

				this.Height = (30 + lst.Count * 20 + 100 + 20) + offset;

				cmdExit.Top = this.Height - 10 - cmdExit.Height;

				cmdAdd.Top = cmdExit.Top - 10 - cmdAdd.Height;

				cmdExit.Left = this.Width - cmdExit.Width - 10;
				cmdAdd.Left = cmdExit.Left;
			}

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

			DateTime now = DateTime.Now;
			int i = 0;
			foreach (TimeItem itm in lst)
			{
				DateTime newtime = now.ToUniversalTime().AddHours(itm.Offset);
				if (itm.DST)
				{
					bool bDST = DSTTime.Get.Offset(now);
					if (bDST)
						newtime = newtime.AddHours(1);
				}
				DrawStuff.DrawTextShadowLeft(g, itm.Name, fnt, Brushes.Goldenrod, 20 + i * 20, 100, 0);
				DrawStuff.DrawTextShadow(g, newtime.ToString("HH:mm:ss fffffff"),
					fnt, Brushes.Goldenrod, 110, 20 + i * 20);

				if ((i + 1) == Button_num)
					DrawStuff.DrawTextShadow(g, "Remove", fnt, Brushes.Gold, 110 + 120, 20 + i * 20);
				else
					DrawStuff.DrawTextShadow(g, "Remove", fnt, Brushes.LightGreen, 110 + 120, 20 + i * 20);

				i++;
			}

			DrawSpinnyClock(g, now, 20, 30 + lst.Count * 20);

			e.Graphics.DrawImage(bmp, 0, 0);
		}

		private void DrawSpinnyClock(Graphics g, DateTime now, int left, int top)
		{
			float x, y;
			double theta;

			for (int c = 0; c < 360; c += (360 / 12))
			{
				theta = ((c + (360 - 90)) % 360) * (Math.PI / 180);

				x = (float)Math.Cos(theta) * (100 / 2);
				y = (float)Math.Sin(theta) * (100 / 2);
				g.DrawLine(Pens.Black, left + (100 / 2) - x, top + (100 / 2) - y, left + (100 / 2) + x, top + (100 / 2) + y);
			}

			theta = ((((now.Hour % 12) / 12.0f) * 360) + (((now.Minute % 60) / 60.0f) * (360 / 12)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 4);
			y = (float)Math.Sin(theta) * (100 / 4);
			g.DrawLine(Pens.Goldenrod, left + (100 / 2), top + (100 / 2), left + (100 / 2) + x, top + (100 / 2) + y);

			theta = ((((now.Minute % 60) / 60.0f) * 360) + (((now.Second % 60) / 60.0f) * (360 / 60)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 3);
			y = (float)Math.Sin(theta) * (100 / 3);
			g.DrawLine(Pens.Goldenrod, left + (100 / 2), top + (100 / 2), left + (100 / 2) + x, top + (100 / 2) + y);

			theta = (((((now.Second % 60) / 60.0f)) * 360) + ((now.Millisecond / 1000.0) * (360 / 60)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 2);
			y = (float)Math.Sin(theta) * (100 / 2);
			g.DrawLine(Pens.Red, left + (100 / 2), top + (100 / 2), left + (100 / 2) + x, top + (100 / 2) + y);

			theta = (((((now.Millisecond % 1000) / 1000.0f)) * 360) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 2);
			y = (float)Math.Sin(theta) * (100 / 2);
			g.DrawLine(Pens.Red, left + (100 / 2), top + (100 / 2), left + (100 / 2) + x, top + (100 / 2) + y);

			g.DrawEllipse(Pens.LightGreen, left, top, 100, 100);

			g.DrawEllipse(Pens.LightGreen, left + (100 / 2) - (6 / 2), top + (100 / 2) - (6 / 2), 6, 6);
		}

		bool bDragging;
		Point pntStart;
		int Button_num;
		private void StartDrag(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pntStart = new Point();
			if (e.Button == MouseButtons.Left)
			{
				Button_num = 0;
				if (e.X > 110 + 120 && e.X < 110 + 120 + 40 && e.Y > 20 && e.Y < (20 + lst.Count * 20))
				{
					Button_num = ((e.Y - 20) / 20) + 1;
				}
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

		private void EndDrag(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Button_num != 0)
			{
				DialogResult res = MessageBox.Show("Are you sure you want to remove " + lst[Button_num - 1].Name + "?", "Spinny Clock", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (res == DialogResult.Yes)
				{
					lst.RemoveAt(Button_num - 1);
					SaveXML();
				}

				Button_num = 0;
			}
			bDragging = false;
		}

		private void SaveXML()
		{
			StreamWriter write = new StreamWriter("data.xml");
			write.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			write.WriteLine("<root>");
			foreach(TimeItem itm in lst)
				write.WriteLine("	<time name=\""+itm.Name+"\" offset=\"" + itm.Offset.ToString("0.0") +"\" dst=\"" + itm.DST.ToString() + "\" />");
			write.WriteLine("</root>");
			write.Flush();
			write.Close();
		}

		private void cmdExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		AddBox addb;
		private void cmdAdd_Click(object sender, EventArgs e)
		{
			if (addb == null)
				addb = new AddBox();

			cmdAdd.Enabled = false;
			cmdExit.Enabled = false;

			addb.Reset();
			DialogResult res = addb.ShowDialog(this);

			cmdAdd.Enabled = true;
			cmdExit.Enabled = true;

			if (res == DialogResult.OK)
			{
				lst.Add(new TimeItem(addb.NickName, addb.Offset, addb.IsDST));
				SaveXML();
			}
		}
	}

	internal class DSTTime
	{
		DateTime dstStart;
		DateTime dstEnd;

		protected DSTTime()
		{
			DateTime date = new DateTime(DateTime.Now.Year, 4, 1);
			while (date.DayOfWeek != DayOfWeek.Sunday)
				date = date.AddDays(1);

			dstStart = date;

			date = new DateTime(DateTime.Now.Year, 10, 30);
			while (date.DayOfWeek != DayOfWeek.Sunday)
				date = date.AddDays(-1);

			dstEnd = date;
		}

		private static DSTTime _inst;
		public static DSTTime Get
		{
			get
			{
				if (_inst == null)
					_inst = new DSTTime();
				return _inst;
			}
		}

		public bool Offset(DateTime in_dte)
		{
			if (in_dte.DayOfYear > dstStart.DayOfYear && in_dte.DayOfYear < dstEnd.DayOfYear)
			{
					return true;
			}
			else if (in_dte.DayOfYear == dstStart.DayOfYear)
			{
				if (in_dte.Hour >= 2)
					return true;
				return false;
			}
			else if (in_dte.DayOfYear == dstEnd.DayOfYear)
			{
				if (in_dte.Hour <= 3)
					return true;
				return false;
			}
			return false;
		}
	}

	internal class TimeItem
	{
		private string sName;

		public string Name
		{
			get
			{
				return sName;
			}
			set
			{
				sName = value;
			}
		}

		private float dOffset;

		public float Offset
		{
			get
			{
				return dOffset;
			}
			set
			{
				dOffset = value;
			}
		}

		private bool bDST;

		public bool DST
		{
			get
			{
				return bDST;
			}
			set
			{
				bDST = value;
			}
		}

		public TimeItem(string name, float offset, bool dst)
		{
			sName = name;
			dOffset = offset;
			bDST = dst;
		}
	}
}