using System;
using System.Drawing;

namespace WindowsApplication4
{
#if !OLDCODE
	static class DrawStuff
#else
	public class DrawStuff
#endif
	{
		/// <summary>
		/// Draws the funky shadowed text, also in DescripBox
		/// </summary>
		/// <param name="g">Graphics object to draw to</param>
		/// <param name="text">the text to be drawn/shadowed</param>
		/// <param name="fnt">the font face to be used</param>
		/// <param name="face">the color of the main text</param>
		/// <param name="x">the left most point</param>
		/// <param name="y">the top most point</param>
		public static void DrawTextShadow(Graphics g, string text, Font fnt, Brush face, float x, float y)
		{
			g.DrawString(text, fnt, Brushes.Black, x + 1, y + 1);
			g.DrawString(text, fnt, Brushes.Black, x + 1, y);
			g.DrawString(text, fnt, Brushes.Black, x, y + 1);
			g.DrawString(text, fnt, face, x, y);
		}

		public static void DrawTextShadowLeft(Graphics g, string text, Font fnt, Brush face, float y, float right, float height)
		{
			SizeF siz = g.MeasureString(text, fnt);
			if (siz.Height > height)
				DrawTextShadow(g, text, fnt, face, (int)(right - siz.Width), y);
			else
				DrawTextShadow(g, text, fnt, face, (int)(right - siz.Width), y + ((height) / 2 - (siz.Height / 2)));
		}

		public static void DrawHatchedBackground(Graphics g, int width, int height, int interval, Pen lg, Pen ng, Pen dg)
		{
			for (int i = 1; i < width / interval; i++)
				g.DrawLine(lg, (i * interval) - 1, 6, (i * interval) - 1, height - 6);
			for (int i = 1; i < height / interval; i++)
				g.DrawLine(lg, 6, (i * interval) - 1, width - 6, (i * interval) - 1);

			for (int i = 1; i < width / interval; i++)
				g.DrawLine(dg, (i * interval) + 1, 6, (i * interval) + 1, height - 6);
			for (int i = 1; i < height / interval; i++)
				g.DrawLine(dg, 6, (i * interval) + 1, width - 6, (i * interval) + 1);

			for (int i = 1; i < width / interval; i++)
				g.DrawLine(ng, i * interval, 6, i * interval, height - 6);
			for (int i = 1; i < height / interval; i++)
				g.DrawLine(ng, 6, i * interval, width - 6, i * interval);
		}

		public static void DrawArcBorder(Graphics g, int width, int height, int l_width, Pen p)
		{
			g.DrawArc(p, l_width, l_width, 30, 30, 180, 90);
			g.DrawArc(p, l_width, height - 30 - l_width, 30, 30, 90, 90);
			g.DrawArc(p, width - 30 - l_width, height - 30 - l_width, 30, 30, 0, 90);
			g.DrawArc(p, width - 30 - l_width, l_width, 30, 30, 270, 90);

			g.DrawLine(p, l_width, 14, l_width, height - 14);
			g.DrawLine(p, width - l_width, 14, width - l_width, height - 14);

			g.DrawLine(p, 14, l_width, width - 14, l_width);
			g.DrawLine(p, 14, height - l_width, width - 14, height - l_width);
		}

		public static Region DoRegion(int width, int height)
		{
			Region r = new Region(Rectangle.FromLTRB(0, 0, width, height));

			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			gp.StartFigure();
			gp.AddLine(0, 0, 15, 0);
			gp.AddLine(0, 0, 0, 15);
			gp.AddArc(0, 0, 31, 31, 180, 90);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(0, height - 30, 31, 31, 90, 90);
			gp.AddLine(0, height, 0, height - 15);
			gp.AddLine(0, height, 15, height);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(width - 30, height - 30, 30, 30, 0, 90);
			gp.AddLine(width, height, width, height);
			gp.CloseFigure();

			gp.StartFigure();
			gp.AddArc(width - 30, 0, 30, 30, 270, 90);
			gp.AddLine(width, 0, width, 0);
			gp.CloseFigure();

			r.Exclude(gp);

			return r;
		}
	}
}
