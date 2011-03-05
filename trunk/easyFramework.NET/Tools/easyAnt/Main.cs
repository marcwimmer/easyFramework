using System;
using System.Xml;

namespace easyAnt
{
	/// <summary>
	/// Zusammenfassung für Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Fügen Sie hier Code hinzu, um die Anwendung zu starten
			//

			string sBuildXml = Environment.CurrentDirectory + "\\build.xml";
			Console.WriteLine(sBuildXml);

			XmlDocument oXml = new XmlDocument();

			DateTime begin = DateTime.Now;
			try
			{
				oXml.Load(sBuildXml);
				Builder builder = new Builder(oXml);
				Console.WriteLine("Done.");
			}
			catch (Exception ex) 
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("There were errors.");
			}
			
			Console.WriteLine("***********************************");
			Console.WriteLine("Time used: {0}", DateTime.Now - begin);
			
			System.Threading.Thread.Sleep(2000);
		}
	}
}
