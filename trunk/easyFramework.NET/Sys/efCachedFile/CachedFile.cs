using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using System.IO;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     CachedFileDocument

	//--------------------------------------------------------------------------------'
	//Module:    CachedFileDocument.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   cached file-document. the source is a file; it automatically reloads
	//           when the file changes
	//--------------------------------------------------------------------------------'
	//Created:   02.06.2004 23:18:32
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'



	public enum efEnumFileTypes
	{
		efAuto,
		efTextASCII,
		efTextUTF7,
		etTextUTF8,
		efXml,
		efBinary
	}

	public class CachedFile
	{
		public CachedFile()
		{
			mEfTreatFileContent = efEnumFileTypes.efAuto;
		}



		//================================================================================
		//Private Fields:
		//================================================================================
		private string msCompleteSourceFilePath;
		private string msFileContent;
		private byte[] mabFileContent = null;
		private XmlDocument moXmlFileContent;
		private efEnumFileTypes mEfTreatFileContent;
		private DateTime mdtModDate;

		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sCompleteSourceFilePath
		//--------------------------------------------------------------------------------'
		//Purpose:   the full path to the file, e.g. "C:\autoexec.bat"
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:45:05
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sCompleteSourceFilePath
		{
			get
			{
				return msCompleteSourceFilePath;
			}
			set
			{
				if (! mbFileExists(value))
				{
					throw (new CachedFileException("File \"" + value + "\" doesn't exist!"));
				}
		
				msCompleteSourceFilePath = value;
		
			}
		}


		//================================================================================
		//Property:  efTextTreatFileContent
		//--------------------------------------------------------------------------------'
		//Purpose:   set here, how the file should be read;
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:45:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efEnumFileTypes efTreatFileContent
		{
			get
			{
				return mEfTreatFileContent;
			}
			set
			{
				mEfTreatFileContent = value;
			}
		}


		//================================================================================
		//Property:  oXmlDocument
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the xml-document
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:56:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public XmlDocument oXmlDocument
		{
			get
			{
				//------get the latest file-content------
				mReloadIfChanged();
		
				//-------convert to the needed output-format------------
				if (mEfTreatFileContent == efEnumFileTypes.efXml)
				{
					return moXmlFileContent;
				}
				else
				{
					throw (new CachedFileException("Must be file-type \"xml-document\" to return " + "the xml-document."));
				}
		
			}
		}


		//================================================================================
		//Property:  sFileContent
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the file-content as string
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:56:33
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sFileContent
		{
			get
			{
				//------get the latest file-content------
				mReloadIfChanged();
		
				//-------convert to the needed output-format------------
				switch (mEfTreatFileContent)
				{
					case efEnumFileTypes.efXml:
				
						return moXmlFileContent.sXml;
					case efEnumFileTypes.efTextASCII:
						return msFileContent;
				
					case efEnumFileTypes.efTextUTF7:
						return msFileContent;
				
					case efEnumFileTypes.etTextUTF8:
				
						return msFileContent;
					default:
				
						throw (new CachedFileException("Must be a text-file. Otherwise text-content cannot be returned."));
				
				}
		
			}
		}

		//================================================================================
		//Property:  abFileContent
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the file-content as byte-array
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:56:33
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public byte[] abFileContent
		{
			get
			{
				//------get the latest file-content------
				mReloadIfChanged();
		
				//-------convert to the needed output-format------------
				switch (mEfTreatFileContent)
				{
					case efEnumFileTypes.efBinary:     						
						return mabFileContent;

					default:
				
						throw (new CachedFileException("Must be a binary-file. Otherwise binary-content cannot be returned."));
				
				}
			}
		}


		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Function:  gbFileChanged
		//--------------------------------------------------------------------------------'
		//Purpose:   determines, wether the file has been changed, since the last-time;
		//           use the modification-date of the file
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:52:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbFileChanged()
		{
	
			if (File.GetLastWriteTime(msCompleteSourceFilePath) != mdtModDate)
			{
				return true;
			}
			else
			{
				return false;
			}
	
		}

		/// <summary>
		/// forces a load of the file
		/// </summary>
		public void gForceFetchFileContent() 
		{
			mLoadFile(sCompleteSourceFilePath);
		}


		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================
		protected static bool mbFileExists(string sFile)
		{
	
			if (File.Exists(sFile) == false)
			{
				return false;
			}
			else
			{
				return true;
			}
	
		}


		//================================================================================
		//Sub:       mLoadFile
		//--------------------------------------------------------------------------------'
		//Purpose:   loads the file into the object
		//--------------------------------------------------------------------------------'
		//Params:    sFileName - the complete file-path
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:30:17
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mLoadFile (string sFileName)
		{
	
			FileStream fs = File.Open(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	
			//-------determine the file-type, if it is "auto"---------
			if (mEfTreatFileContent == efEnumFileTypes.efAuto)
			{
				mEfTreatFileContent = mEfEnumFileTypeGet(fs, sFileName);
			}
	
	
			//--------set file-content depending on the file-type----------
			switch (mEfTreatFileContent)
			{
		
				case efEnumFileTypes.efBinary:
		
					fs.Seek(0, SeekOrigin.Begin);
					fs.Read(mabFileContent, 0, easyFramework.Sys.ToolLib.DataConversion.glCInt(fs.Length, 0));
					break;
		
				case efEnumFileTypes.efTextASCII:
		
					fs.Seek(0, SeekOrigin.Begin);
					fs.Read(mabFileContent, 0, easyFramework.Sys.ToolLib.DataConversion.glCInt(fs.Length, 0));
					msFileContent = System.Text.ASCIIEncoding.ASCII.GetString(mabFileContent);
					break;
		
				case efEnumFileTypes.efTextUTF7:
		
					fs.Seek(0, SeekOrigin.Begin);
					fs.Read(mabFileContent, 0, easyFramework.Sys.ToolLib.DataConversion.glCInt(fs.Length, 0));
					msFileContent = System.Text.UTF7Encoding.UTF7.GetString(mabFileContent);
					break;
		
				case efEnumFileTypes.efXml:
		
					moXmlFileContent = new XmlDocument();
					moXmlFileContent.gLoad(sFileName);
					break;
		
				case efEnumFileTypes.etTextUTF8:
		
					fs.Seek(0, SeekOrigin.Begin);
					fs.Read(mabFileContent, 0, DataConversion.glCInt(fs.Length, 0));
					msFileContent = System.Text.UTF8Encoding.UTF8.GetString(mabFileContent);
					break;
		
			}

			//----get last mod-date------
			mdtModDate = File.GetLastWriteTime(sCompleteSourceFilePath);

		}


		//================================================================================
		//Function:  mEfEnumFileTypeGet
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the type of the file
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.06.2004 23:47:38
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static efEnumFileTypes mEfEnumFileTypeGet(FileStream fs, string sFilePath)
		{
			
			byte[] read = null;
			bool bIsXmlDocument = false;
			bool bHas0Byte = false;
			XmlDocument oXml = null;

			//----------test for xml-document------------
			try
			{
				oXml = new XmlDocument();
				oXml.gLoad(sFilePath);
			}
			catch 
			{
				bIsXmlDocument = false;
				
			}
			finally 
			{
				if (oXml.sXml == "") 
					bIsXmlDocument = false;
				else bIsXmlDocument = true;
			}

			if (bIsXmlDocument == false)
			{
	
				//---------------check for 0-byte-----------------
	
				fs.Seek(0, SeekOrigin.Begin);
	
	
				//---------only read the first 1024 bytes----------
				int lRead = fs.Read(read, DataConversion.glCInt((fs.Position), 0), 1024);
	
	
				for (int i = 0; i <= lRead - 1; i++)
				{
		
					if (read[i] == 0)
					{
						bHas0Byte = true;
						break;
					}
				}
	
			}

			if (bIsXmlDocument)
			{
				return efEnumFileTypes.efXml;
			}
			else if (bHas0Byte)
			{
				return efEnumFileTypes.efBinary;
			}
			else
			{
				return efEnumFileTypes.efTextASCII;
			}

		}


		//================================================================================
		//Sub:       mReloadIfChanged
		//--------------------------------------------------------------------------------'
		//Purpose:   reloads the file, if the file was changed
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   03.06.2004 00:00:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mReloadIfChanged ()
		{
			if (gbFileChanged())
			{
				mLoadFile(sCompleteSourceFilePath);
			}

		}

		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================

		//================================================================================
		//Private Methods:
		//================================================================================


	}

}
