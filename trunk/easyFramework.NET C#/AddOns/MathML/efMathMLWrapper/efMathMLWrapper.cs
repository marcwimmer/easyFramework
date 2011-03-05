using System;
using System.Drawing.Imaging;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Addons.efMathMLWrapper
{
	/// <summary>
	/// interface for the math-ml rendering engine
	/// </summary>
	public class efMathMLRenderer
	{
		public efMathMLRenderer()
		{

		}

		/// <summary>
		/// creates an image-file of the formula
		/// </summary>
		/// <param name="sXmlFilePath"></param>
		/// <param name="sOutputPath">hier kann entweder ein Dateipfad oder das XML übergeben werden</param>
		/// <param name="sOutputFileNameWithoutExtension"></param>
		/// <param name="imageformat"></param>
		public static void gRenderXml(string sXmlFilePath, string sOutputPath, string sOutputFileNameWithoutExtension, ImageFormat imageformat) 
		{
			MathML.MathMLDocument doc = new MathML.MathMLDocument();
			if (sXmlFilePath.Substring(0, 1).ToLower().Equals("<"))
				doc.LoadXml(sXmlFilePath);
			else
				doc.Load(sXmlFilePath);
			MathML.Rendering.MathMLControl mc = new MathML.Rendering.MathMLControl();
			mc.MathElement = (MathML.MathMLMathElement)doc.DocumentElement;

			string sExt = ".";
			if (imageformat == ImageFormat.Bmp)
				sExt += "bmp";
			else if (imageformat == ImageFormat.Gif)
				sExt += "gif";
			else if (imageformat == ImageFormat.Jpeg)
				sExt += "jpg";
			else if (imageformat == ImageFormat.Tiff)
				sExt += "tif";
			else if (imageformat == ImageFormat.Png)
				sExt += "png";
			else	
				throw new efException("unhandled imageformat: " + imageformat.ToString());

			string sOutputFileName = sOutputPath + "\\" + sOutputFileNameWithoutExtension + sExt;
			sOutputFileName	= sOutputFileName.Replace("\\\\", "\\");

			mc.Save(sOutputFileName, imageformat);

		}

	}
}
