using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.AddOns.efICSharpZLibWrapper
{
	/// <summary>
	/// Allows compressing and decompressing in the zip-format.
	/// </summary>
	public class efZLib
	{

		//================================================================================
		//private fields:
		//================================================================================
		
		//================================================================================
		//public consts:
		//================================================================================

		//================================================================================
		//public properties:
		//================================================================================

		//================================================================================
		//public events:
		//================================================================================

		//================================================================================
		//public methods:
		//================================================================================
		public efZLib()
		{
			
			

		}

		/// <summary>
		/// creates a zip-file
		/// </summary>
		/// <param name="sZipFileName">name of the zip-file (complete name)</param>
		/// <param name="oXmlFileList">xml like the following: 
		///		&lt;filelist&gt;
		///			&lt;file&gt;d:\temp\file1.txt&lt;/file&gt;
		///			&lt;file&gt;d:\temp\subfolder1\subfolder2\file2.txt&lt;/file&gt;
		///			&lt;directory&gt;c:\temp&lt;/directory&gt;
		///		&lt;/filelist&gt;		
		/// </param>
		/// <param name="lCompressionLevel">0 = store, 9 = best compression</param>
		/// <param name="sRelativePath">stores the files relative to this directory</param>
		/// <param name="bOverwrite">true=delete the zip-archive before, if exists</param>
		public static  void gCreateZipFile(string sZipFileName, XmlDocument oXmlFileList, 
			int lCompressionLevel, string sRelativePath, bool bOverwrite)
		{

			//-------------delete existing file---------------------
			if (bOverwrite == true) 
			{
				if (File.Exists(sZipFileName)) 
					File.Delete(sZipFileName);
			}

			//----------------resolve directory-tags-------------------------
			mResolveDirectoryTags(oXmlFileList);
		

			Crc32 crc = new Crc32();
			System.IO.FileStream oZipFileStream = new FileStream(sZipFileName, FileMode.CreateNew, FileAccess.Write,FileShare.None);
			ZipOutputStream s = new ZipOutputStream(oZipFileStream);
		
			s.SetLevel(lCompressionLevel); // 0 - store only to 9 - means best compression
		
			XmlNodeList nlFiles = oXmlFileList.selectNodes("/filelist/file");

			for (int i =0; i < nlFiles.lCount; i++) 
			{
				string sFilename = nlFiles[i].sText;
				FileStream fs = File.OpenRead(sFilename);
			
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);

				//---------set the zip-file relative name---------
				if (sRelativePath != "") 
					sFilename = sFilename.ToLower().Replace(sRelativePath.ToLower(), "");

				if (sFilename.Substring(0, 1) == "\\")
					sFilename = sFilename.Substring(1, sFilename.Length - 1);

				ZipEntry entry = new ZipEntry(sFilename);
			
				entry.DateTime = DateTime.Now;
			
				// set Size and the crc, because the information
				// about the size and crc should be stored in the header
				// if it is not set it is automatically written in the footer.
				// (in this case size == crc == -1 in the header)
				// Some ZIP programs have problems with zip files that don't store
				// the size and crc in the header.
				entry.Size = fs.Length;
				fs.Close();
			
				crc.Reset();
				crc.Update(buffer);
			
				entry.Crc  = crc.Value;
			
				s.PutNextEntry(entry);
			
				s.Write(buffer, 0, buffer.Length);
			
			}
		
			s.Finish();
			s.Close();

			oZipFileStream.Close();

		}

		/// <summary>
		/// returns the content of a zip-file as xml
		/// </summary>
		/// <param name="sZipFileName">name of the zip-archive</param>
		/// <returns>
		///		<filelist>
		///			<file size="x" >temp\file1</file>
		///			<file>temp\file2.txt</file>
		///		</filelist>
		/// </returns>
		public static  XmlDocument goXmlGetFileList(string sZipFileName) 
		{
			XmlDocument oXmlResult = new XmlDocument("<filelist/>");

			ZipFile zFile = new ZipFile(sZipFileName);
			foreach (ZipEntry e in zFile) 
			{
				XmlNode n = oXmlResult.selectSingleNode("/filelist").AddNode("file",true);
				n.sText=e.Name;
				n["size"].sText = Convert.ToString(e.Size);
			}

			return oXmlResult;
		}

		/// <summary>
		/// extracts the files of the zip-archive
		/// </summary>
		/// <param name="sZipFileName">the name of the zip-archive</param>
		/// <param name="sDestRootFolder">the destination folder</param>
		/// <param name="bOverWrite">true = overwrite existing files</param>
		/// <param name="bOverWriteReadOnly">true= even overwrite file, if it read-only</param>
		public static  void gExtractFromZipFile(string sZipFileName, string sDestRootFolder, bool bOverWrite,
			bool bOverWriteReadOnly) 
		{
			System.IO.FileStream oZipFileStream = new FileStream(sZipFileName, FileMode.Open, 
				FileAccess.Read,FileShare.ReadWrite);
			ZipInputStream s = new ZipInputStream(oZipFileStream);
		
			//-----------adapt dest-folder-------
			if (sDestRootFolder.Substring(sDestRootFolder.Length - 1, 1) != "\\")
				sDestRootFolder += "\\";

			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null) 
			{
			
				string directoryName = Path.GetDirectoryName(sDestRootFolder + theEntry.Name);
				string fileName      = Path.GetFileName(sDestRootFolder + theEntry.Name);
			
				// create directory
				if (!Directory.Exists(directoryName))
					Directory.CreateDirectory(directoryName);
			
				if (fileName != String.Empty) 
				{
					string sExtractedFilePath = sDestRootFolder + theEntry.Name;

					//check for existing and read-only
					if (File.Exists(sExtractedFilePath)) 
					{
						if (!bOverWrite)
							throw new efException("File \"" + theEntry.Name + "\" already exists " + 
								"and may not be overwritten. You may set the bOverwrite-Parameter to true.");
						else 
						{
							//check read-only
							if ((File.GetAttributes(sExtractedFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
								try 
								{
									System.IO.File.SetAttributes(sExtractedFilePath,  System.IO.FileAttributes.Archive);
								}
								catch (Exception ex) 
								{
									throw new efException("Error setting file-attribute to normal for \"" + 
										sExtractedFilePath + "\". Message: " + 
										ex.Message);
								}
						}
						
						//delete file
						File.Delete(sExtractedFilePath);
					}

					//extract file
					FileStream streamWriter = File.Create(sExtractedFilePath);
				
					int size = 2048;
					byte[] data = new byte[2048];
					while (true) 
					{
						size = s.Read(data, 0, data.Length);
						if (size > 0) 
						{
							streamWriter.Write(data, 0, size);
						} 
						else 
						{
							break;
						}
					}
				
					streamWriter.Close();
				}
			}
			s.Close();
		}

		//================================================================================
		//internal properties:
		//================================================================================

		//================================================================================
		//internal methods:
		//================================================================================
		
		/// <summary>
		/// makes several file-tags out of a directory-tag
		/// </summary>
		/// <param name="oXmlFileList"></param>
		private static void mResolveDirectoryTags(XmlDocument oXmlFileList) 
		{
			XmlNodeList nlDirectories = oXmlFileList.selectNodes("//directory");

			for (int i = 0; i < nlDirectories.lCount; i++) 
			{
				if (!Directory.Exists(nlDirectories[i].sText))
					throw new efException("Directory not found: \"" + 
						nlDirectories[i].sText + "\".");

				//-------------get files and directories as array--------------
				String[] asFiles = Directory.GetFiles(nlDirectories[i].sText);
				String[] asSubdirectories = Directory.GetDirectories(nlDirectories[i].sText);

				//--------------make a more usable arraylist-object------------
				efArrayList oFiles = new efArrayList();
				efArrayList oSubDirs = new efArrayList();

				for (int y = 0; y < asFiles.Length; y++) 
					oFiles.Add(Convert.ToString(asFiles[y]));
				for (int y = 0; y < asSubdirectories.Length; y++) 
				{
					oSubDirs.Add(Convert.ToString(asSubdirectories[y]));

					//------------also append subdirs--------------------
					mAppendSubDirectories(asSubdirectories[y], ref oSubDirs);
				}

				//----------append files from the current directory--------------
				for (int y = 0; y < oFiles.Count; y++) 
				{
					XmlNode node = nlDirectories[i].oParent.AddNode("file", true);
					node.sText = Convert.ToString(oFiles[y]);
				}

				//----------append files from all subdirectories--------------
				for (int z = 0; z < oSubDirs.Count; z++) 
				{
					asFiles = Directory.GetFiles(Convert.ToString(oSubDirs[z]));

					for (int y = 0; y < asFiles.Length; y++) 
					{
						XmlNode node = nlDirectories[i].oParent.AddNode("file", true);
						node.sText = asFiles[y];
					}
				}

				//-------remove resolved directory-node---------
				nlDirectories[i].gRemoveFromParent();
			}
		}

		/// <summary>
		/// gets all subdirectories and appends it to the string-array
		/// </summary>
		/// <param name="sParentDirectory"></param>
		private static void mAppendSubDirectories(string sParentDirectory, ref efArrayList oDirectoryList) 
		{
			string[] asSubDirectories = Directory.GetDirectories(sParentDirectory);

			for (int i = 0; i < asSubDirectories.Length; i++) 
			{
				oDirectoryList.Add (asSubDirectories[i]);

				mAppendSubDirectories(asSubDirectories[i], ref oDirectoryList);
			}
		}

		//================================================================================
		//private consts:
		//================================================================================

		//================================================================================
		//private fields:
		//================================================================================

		//================================================================================
		//private methods:
		//================================================================================


		
	

	}
}
