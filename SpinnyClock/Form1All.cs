//#define GENERICS

using System;
#if GENERICS
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
	public class Form1 : Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timTick = new System.Windows.Forms.Timer(this.components);
			this.cmdAdd = new System.Windows.Forms.Button();
			this.cmdExit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timTick
			// 
			this.timTick.Enabled = true;
			this.timTick.Interval = 50;
			this.timTick.Tick += new System.EventHandler(this.timTick_Tick);
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(204, 246);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(76, 25);
			this.cmdAdd.TabIndex = 4;
			this.cmdAdd.Text = "Add";
			this.cmdAdd.FlatStyle = FlatStyle.System;
			// 
			// cmdExit
			// 
			this.cmdExit.Location = new System.Drawing.Point(204, 277);
			this.cmdExit.Name = "cmdExit";
			this.cmdExit.Size = new System.Drawing.Size(76, 25);
			this.cmdExit.TabIndex = 5;
			this.cmdExit.Text = "Exit";
			this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
			this.cmdExit.FlatStyle = FlatStyle.System;
			// 
			// Form1
			// 
			this.ClientSize = new System.Drawing.Size(292, 314);
			this.Controls.Add(this.cmdExit);
			this.Controls.Add(this.cmdAdd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartDrag);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EndDrag);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DoDrag);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timTick;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.Button cmdExit;

		public Form1()
		{
			InitializeComponent();

			Application.EnableVisualStyles();
			Application.DoEvents();
			
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
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

#if GENERICS
		List<TimeItem> lst;
#else
		ArrayList lst;
#endif
		private void Form1_Load(object sender, EventArgs e)
		{
#if GENERICS
			lst = new List<TimeItem>();
#else
			lst = new ArrayList();
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

#if GENERICS
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
			if (bmp == null || (bmp.Width != this.ClientSize.Width || bmp.Height != this.ClientSize.Height))
			{
				bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
				DoRegion();
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

			int i = 0;

			DateTime now = DateTime.Now;
			foreach (TimeItem itm in lst)
			{
				DateTime newtime = now.ToUniversalTime().AddHours(itm.Offset);
				if (itm.DST)
				{
					bool bDST = DSTTime.Get.Offset(now);
					if (bDST)
						newtime = newtime.AddHours(1);
				}
				DrawStuff.DrawTextShadowLeft(g, itm.Name, fnt, Brushes.Goldenrod, 0, 20 + i * 20, 100, 0);
				DrawStuff.DrawTextShadow(g, newtime.ToString("HH:mm:ss fffffff"),
					fnt, Brushes.Goldenrod, 110, 20 + i * 20);

				i++;
			}

			float x, y;
			double theta;

			for (int c = 0; c < 360; c += (360 / 12))
			{
				theta = ((c + (360 - 90)) % 360) * (Math.PI / 180);

				x = (float)Math.Cos(theta) * (100 / 2);
				y = (float)Math.Sin(theta) * (100 / 2);
				g.DrawLine(Pens.Black, 20 + (100 / 2) - x, 180 + (100 / 2) - y, 20 + (100 / 2) + x, 180 + (100 / 2) + y);
			}

			theta = ((((now.Hour % 12) / 12.0f) * 360) + (((now.Minute % 60) / 60.0f) * (360 / 12)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 4);
			y = (float)Math.Sin(theta) * (100 / 4);
			g.DrawLine(Pens.Goldenrod, 20 + (100 / 2), 180 + (100 / 2), 20 + (100 / 2) + x, 180 + (100 / 2) + y);

			theta = ((((now.Minute % 60) / 60.0f) * 360) + (((now.Second % 60) / 60.0f) * (360 / 60)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 3);
			y = (float)Math.Sin(theta) * (100 / 3);
			g.DrawLine(Pens.Goldenrod, 20 + (100 / 2), 180 + (100 / 2), 20 + (100 / 2) + x, 180 + (100 / 2) + y);

			theta = (((((now.Second % 60) / 60.0f)) * 360) + ((now.Millisecond / 1000.0) * (360 / 60)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 2);
			y = (float)Math.Sin(theta) * (100 / 2);
			g.DrawLine(Pens.Red, 20 + (100 / 2), 180 + (100 / 2), 20 + (100 / 2) + x, 180 + (100 / 2) + y);

			theta = (((((now.Millisecond % 1000) / 1000.0f)) * 360) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 2);
			y = (float)Math.Sin(theta) * (100 / 2);
			g.DrawLine(Pens.Red, 20 + (100 / 2), 180 + (100 / 2), 20 + (100 / 2) + x, 180 + (100 / 2) + y);
/*
			now = now.AddHours(-9);

			theta = ((((now.Hour % 12) / 12.0f) * 360) + (((now.Minute % 60) / 60.0f) * (360 / 12)) + (360 - 90)) * (Math.PI / 180);
			x = (float)Math.Cos(theta) * (100 / 4);
			y = (float)Math.Sin(theta) * (100 / 4);
			g.DrawLine(Pens.Brown, 20 + (100 / 2), 180 + (100 / 2), 20 + (100 / 2) + x, 180 + (100 / 2) + y);
*/
			g.DrawEllipse(Pens.LightGreen, 20, 180, 100, 100);

			g.DrawEllipse(Pens.LightGreen, 20 + (100 / 2) - (6 / 2), 180 + (100 / 2) - (6 / 2), 6, 6);

			e.Graphics.DrawImage(bmp, 0, 0);
		}

		private void DoRegion()
		{
			Region r = new Region(Rectangle.FromLTRB(0, 0, this.ClientSize.Width, this.ClientSize.Height));
			//r.Exclude(Rectangle.FromLTRB(10,10,50,50));

			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			//gp.AddRectangle(Rectangle.FromLTRB(15,15,this.ClientSize.Width - 15,this.ClientSize.Height - 15));
			gp.StartFigure();
			gp.AddLine(0, 0, 15, 0);
			gp.AddLine(0, 0, 0, 15);
			gp.AddArc(0, 0, 31, 31, 180, 90);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(0, this.ClientSize.Height - 30, 31, 31, 90, 90);
			gp.AddLine(0, this.ClientSize.Height, 0, this.ClientSize.Height - 15);
			gp.AddLine(0, this.ClientSize.Height, 15, this.ClientSize.Height);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(this.ClientSize.Width - 30, this.ClientSize.Height - 30, 30, 30, 0, 90);
			gp.AddLine(this.ClientSize.Width, this.ClientSize.Height, this.ClientSize.Width, this.ClientSize.Height);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(this.ClientSize.Width - 30, 0, 30, 30, 270, 90);
			gp.AddLine(this.ClientSize.Width, 0, this.ClientSize.Width, 0);
			gp.CloseFigure();

			r.Exclude(gp);

			this.Region = r;
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

		private void EndDrag(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bDragging = false;
		}

		private void cmdExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
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
