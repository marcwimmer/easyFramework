using System;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Tasks.Distribution;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Project.Default.ASP.tasks.distribution
{
	/// <summary>
	/// receives the package-file and extracts the files 
	/// </summary>
	public class receiveProcess : System.Web.UI.Page //we don't have a client-id here, so use the default-page; otherwise
		//client-id missing error
	{
		
		public string gsProcess() 
		{
			
			
			
			try 
			{
				//----------get the zip-file from the posted stream--------------
				
				//get xml-document
				
				string sRequest = "";
			
				//often there is something given in the inputstream,especially then,
				//when the javascript gServerProcess method is called with arguments:
			
				if (Request.InputStream.CanRead)
				{
				
					System.IO.Stream str;
				
					int strLen;
					int strRead;
				
					System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding(false);
				
				
					// Create a Stream object.
					str = Request.InputStream;
					// Find number of bytes in stream.
					strLen = System.Convert.ToInt32(str.Length);
					// Create a byte array.
					byte[] strArr = new byte[strLen + 1];
					// Read stream into byte array.
					strRead = str.Read(strArr, 0, strLen);
				
					// Convert byte array to a text string:
					FastString oSBuilder = new FastString();
					oSBuilder.Append(enc.GetString(strArr, 0, strLen));

					sRequest = oSBuilder.ToString();
				
				}
				else throw new efException("Input stream expected");

				XmlDocument oXmlRequest = new XmlDocument(sRequest);
				
				

				//------------unpack-----------------------
				string sEnvelope = oXmlRequest.selectSingleNode("/package/content").sText;
				string sZipFileName = oXmlRequest.selectSingleNode("/package/filename").sText;

				byte[] abZipFileContent = Envelope.gabDeEnvelope(ref sEnvelope);


				Christmas.gOpenPackage(sZipFileName, abZipFileContent, 
					Convert.ToString(Application["distribution_received_package_folder"]), 
					Request.PhysicalApplicationPath);

			}
			catch (Exception e) 
			{
				return "Error: \"" + e.Message + "\"";
			}

			return "OK";
		}


	}
}
