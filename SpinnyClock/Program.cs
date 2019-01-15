using System;
using System.Windows.Forms;

namespace WindowsApplication4
{
#if !OLDCODE
	static class Program
#else
	public class Program
#endif
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new Form1());
		}
	}
}