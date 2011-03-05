using System;
using System.Xml;
using System.Collections;

namespace easyAnt
{
	/// <summary>
	/// Zusammenfassung für Builder.
	/// </summary>
	public class Builder
	{

		private XmlDocument moXmlBuild;

		private string sCompiler_Exe;
		private string sCompiler_Args;
		private string sLogfile;

		private ArrayList asProjects = new ArrayList();


		//================================================================================
		//Function:		constructor
		//--------------------------------------------------------------------------------
		//Purpose:		on constructing the builder it invokes the compilation process at once
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	08.09.2004 21:58:02 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public Builder(XmlDocument oXmlBuild)
		{
			//
			// TODO: Fügen Sie hier die Konstruktorlogik hinzu
			//
			moXmlBuild = oXmlBuild;


			//---------get options-----------
			sCompiler_Exe = sGetNode("/build/options/compiler_exe");
			sCompiler_Args = sGetNode("/build/options/compiler_arguments");
			sLogfile = "log.txt";



			//---------get projects-----------
			XmlNodeList projects = moXmlBuild.SelectNodes("/build/compile/project");
			for (int i = 0; i < projects.Count; i++) 
			{
				asProjects.Add(projects[i].Attributes["path"].InnerText);
			}

			//----------------------compile--------------------
			compileProjects();
			

			
		}



		//================================================================================
		//Function:		oGetNode
		//--------------------------------------------------------------------------------
		//Purpose:		gets an xml-node and gives nice error message
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	08.09.2004 21:57:43 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		private XmlNode oGetNode(string sXPath) 
		{
			XmlNode oResult = moXmlBuild.SelectSingleNode(sXPath);

			if (oResult == null) throw new Exception("The build.xml has errors. The node " +
									 "\"" + sXPath + "\" is missing.");

			return oResult;
		}
		private string sGetNode(string sXPath) 
		{
			XmlNode oResult = oGetNode(sXPath);

			return oResult.InnerText;
		}


		//================================================================================
		//Function:		<compileProjects>
		//--------------------------------------------------------------------------------
		//Purpose:		compiles all projects
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	08.09.2004 22:05:02 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		private void compileProjects() 
		{
			for (int i = 0; i < asProjects.Count; i++) 
			{
				string sArguments = this.sCompiler_Args;
				sArguments = sArguments.Replace("%1", (string) asProjects[i]);
				sArguments = sArguments.Replace("%2", (string) sLogfile);

				//----delete log-file, if exists-----
				if (System.IO.File.Exists(sLogfile)) System.IO.File.Delete(sLogfile);

				//-------execute compiler-------
				Console.WriteLine("=============================================");
				Console.WriteLine("compiling {0}", (string) asProjects[i]);
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo.UseShellExecute = true;
				p.StartInfo.FileName = sCompiler_Exe;
				p.StartInfo.Arguments = sArguments;
				p.Start();

				DateTime start = DateTime.Now;
				p.WaitForExit();
				DateTime end = DateTime.Now;

				Console.WriteLine("compiled in {0} seconds", (end - start) );
				

				//---show log-file------
				showLogfile();
			}

		}


		//================================================================================
		//Function:		showLogFile
		//--------------------------------------------------------------------------------
		//Purpose:		displays the logged output
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	08.09.2004 22:11:22 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		private void showLogfile() 
		{
			System.IO.StreamReader r = System.IO.File.OpenText(sLogfile);
			Console.Write(r.ReadToEnd());
			r.Close();
		}
	}
}
