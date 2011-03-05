using System;
using System.Reflection;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using Microsoft.Office.Interop;

namespace easyFramework.AddOns.OfficeTalk
{
	//================================================================================
	//Class:     ExcelTalker

	//--------------------------------------------------------------------------------'
	//Module:    ExcelTalker.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   abstract interaction with Excel
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 11:41:32
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class ExcelTalker : IDisposable
	{



		//================================================================================
		//Private Fields:
		//================================================================================
		private string msPhysicalTemporyFolder;
		private Microsoft.Office.Interop.Excel.Application moExcel;
		private Microsoft.Office.Interop.Excel.Workbooks moWorkbooks;
		private Microsoft.Office.Interop.Excel.Workbook moWorkbook;
		private Microsoft.Office.Interop.Excel.Worksheet moWorksheet;
		private Microsoft.Office.Interop.Excel.Sheets moWorksheets;



		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sPhysicalTemporyFolder
		//--------------------------------------------------------------------------------'
		//Purpose:   in this folder excel creates its temporary
		//                                   files
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:50:26
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sPhysicalTemporyFolder
		{
			get
			{
				return msPhysicalTemporyFolder;
			}
			set
			{
				msPhysicalTemporyFolder = value;
			}
		}


		//================================================================================
		//Property:  oExcel
		//--------------------------------------------------------------------------------'
		//Purpose:   allow access, if user wants to make some very very special settings
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:50:50
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		/*
		 * not supported by CLS C#
		 * 
		 * public Microsoft.Office.Interop.Excel.Application oExcel
		{
			get
			{
				return oExcel;
			}
		}
		*/

		//================================================================================
		//Public Methods:
		//================================================================================



		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:   constructor
		//--------------------------------------------------------------------------------'
		//Params:    sPhysicalTemporyFolder - in this folder excel creates its temporary
		//                                   files
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:46:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public ExcelTalker(string paramsPhysicalTemporyFolder) 
		{
			msPhysicalTemporyFolder = paramsPhysicalTemporyFolder;
			mInitExcel();
		}
		public ExcelTalker() 
		{
			mInitExcel();
		}


		//================================================================================
		//Function:  goGetFileContent
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the excel-file content
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:46:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public byte[] goGetFileContent()
		{

			//----------------------check file-path--------------------------------
			if (Functions.IsEmptyString(msPhysicalTemporyFolder))
			{
				throw (new efException("Please " + " give the temporary-filepath!"));
			}

			//----------------------store in temporary physical file---------------
			string sTemporaryFileName = Functions.gsGetTemporaryFile(msPhysicalTemporyFolder, "xls", 10);

			moWorkbook.SaveAs(sTemporaryFileName, Missing.Value, 
				Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
				Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Missing.Value, 
				Missing.Value, Missing.Value, Missing.Value, Missing.Value);

			//----------------------get file-content---------------

			System.IO.FileStream oStream = System.IO.File.OpenRead(sTemporaryFileName);
			System.IO.BinaryReader oReader = new System.IO.BinaryReader(oStream);
			byte[] abResult;
			abResult = new byte[oStream.Length];

			oReader.Read(abResult, 0, System.Convert.ToInt32(oStream.Length));

			oStream.Close();

			return abResult;




		}



		//================================================================================
		//Sub:       Dispose
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 13:30:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void Dispose ()
		{
			//---------------release excel-app-----------------
			if (moExcel != null)
			{
	
				moExcel.Quit();
	
				if (moWorksheet == null == false)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(moWorksheet);
				}
				if (moWorksheets == null == false)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(moWorksheets);
				}
				if (moWorkbook == null == false)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(moWorkbook);
				}
				if (moWorkbook == null == false)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(moWorkbooks);
				}
				System.Runtime.InteropServices.Marshal.ReleaseComObject(moExcel);
	
				moWorksheet = null;
				moWorkbook = null;
				moWorkbooks = null;
				moExcel = null;
	
				//was in a use group, to use two times -  "Michael Kofler" said it, too
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
				GC.WaitForPendingFinalizers();
	
			}

		}

		//================================================================================
		//Sub:       gFillExcelWithRecordset
		//--------------------------------------------------------------------------------'
		//Purpose:   fills the first worksheet in excelsheet with a recordset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 12:09:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gFillExcelWithRecordset (Recordset rs)
		{

			//---------------------headings------------------------
			Microsoft.Office.Interop.Excel.Range oCells;
			for (int i = 1; i <= rs.oFieldDefList.Count; i++)
			{
	
				oCells = moWorksheet.Cells;
				Microsoft.Office.Interop.Excel.Range oC = 
						(Microsoft.Office.Interop.Excel.Range)oCells[1, i];
				oC.Value2 = rs.oFieldDefList[i - 1].sName;
	
				Microsoft.Office.Interop.Excel.Font oFont;
				oFont = oC.Font;
				oFont.Bold = true;
	
				System.Runtime.InteropServices.Marshal.ReleaseComObject(oFont);
				System.Runtime.InteropServices.Marshal.ReleaseComObject(oC);
				System.Runtime.InteropServices.Marshal.ReleaseComObject(oCells);
				oFont = null;
				oC = null;
				oCells = null;
	
	
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();

			//----------------------content-------------------------
			rs.MoveFirst();
			int lLine = 2;

			oCells = moWorksheet.Cells;

			while (! rs.EOF)
			{
	
				for (int i = 1; i <= rs.oFieldDefList.Count; i++)
				{
		
					Microsoft.Office.Interop.Excel.Range oC = (Microsoft.Office.Interop.Excel.Range) oCells[lLine, i];
					oC.Value2 = rs.oFields[i - 1].sValue;
		
					System.Runtime.InteropServices.Marshal.ReleaseComObject(oC);
					oC = null;
		
				}
				lLine += 1;
				rs.MoveNext();
			};

			System.Runtime.InteropServices.Marshal.ReleaseComObject(oCells);
			oCells = null;


			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();


		}


		//================================================================================
		//Sub:       gFillExcelWithSQLServerTable
		//--------------------------------------------------------------------------------'
		//Purpose:   fills the first worksheet in excelsheet with a table or view
		//           from a sql-server;
		//           it is very fast, because it uses the build-in excel functions
		//           Excel 2003 is required.
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 12:09:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gFillExcelWithSQLServerTable (ClientInfo oClientInfo, string sSql)
		{

			string sConnstr = OfficeTalkerUtilities.gsGetNativeConnString(oClientInfo);
			gFillExcelWithSQLServerTable(sConnstr, sSql);

		}
		public void gFillExcelWithSQLServerTable (string sConnStr, string sSql)
		{

			Microsoft.Office.Interop.Excel.Range oDestinationRange;
			oDestinationRange = (Microsoft.Office.Interop.Excel.Range) moWorksheet.Cells[1, 1];

			//sConnStr = "PROVIDER=SQLOLEDB.1;data source=.;initial catalog=efAdoptionlist;pwd=logitech;uid=sa"
			//sSql = "SELECT * FROM results_pub_45"

			//-------test connection---------
			try		 
			{
				System.Data.OleDb.OleDbConnection oConn = new System.Data.OleDb.OleDbConnection(sConnStr);
				oConn.Open();
				oConn.Close();
			}
			catch (Exception ex) {


            throw new efException("Invalid connection-string. " +
					"Try to use OfficeTalkerUtilities.gsGetNativeConnString. Error-Message: " +
					ex.Message);
			}


			//-------test command---------
			try	
			{
				System.Data.OleDb.OleDbConnection oConn = new System.Data.OleDb.OleDbConnection(sConnStr);
				System.Data.OleDb.OleDbCommand oCmd = new System.Data.OleDb.OleDbCommand(sSql, oConn);
				oConn.Open();
				oCmd.ExecuteNonQuery();
				oConn.Close();
			}
			catch (Exception ex) 
			{

            
				throw new efException("Invalid sql-command. Error-Message: " +
					ex.Message);
			}

	        //------append for excel--------
			sConnStr = "OLEDB;" + sConnStr;

			Microsoft.Office.Interop.Excel.QueryTables oQueryTables;
			Microsoft.Office.Interop.Excel.QueryTable oQueryTable;

			oQueryTables = moWorksheet.QueryTables;
			oQueryTable = oQueryTables.Add(sConnStr, oDestinationRange, null);

			Microsoft.Office.Interop.Excel.QueryTable with_1 = oQueryTable;

			with_1.CommandType = Microsoft.Office.Interop.Excel.XlCmdType.xlCmdSql;
			with_1.CommandText = sSql;
			with_1.Name = "localhost excelTalker";
			with_1.FieldNames = true;
			with_1.RowNumbers = false;
			with_1.FillAdjacentFormulas = false;
			with_1.PreserveFormatting = true;
			with_1.RefreshOnFileOpen = false;
			with_1.BackgroundQuery = true;
			with_1.RefreshStyle = Microsoft.Office.Interop.Excel.XlCellInsertionMode.xlInsertDeleteCells;
			with_1.SavePassword = false;
			with_1.SaveData = true;
			with_1.AdjustColumnWidth = true;
			with_1.RefreshPeriod = 0;
			with_1.PreserveColumnInfo = true;
			with_1.Refresh(false);


			System.Runtime.InteropServices.Marshal.ReleaseComObject(oQueryTables);
			oQueryTables = null;

			System.Runtime.InteropServices.Marshal.ReleaseComObject(oQueryTable);
			oQueryTable = null;


			System.Runtime.InteropServices.Marshal.ReleaseComObject(oDestinationRange);
			oDestinationRange = null;



			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();

		}

		//================================================================================
		//Private Methods:
		//================================================================================
		public void mInitExcel ()
		{

			moExcel = new Microsoft.Office.Interop.Excel.Application();
			moWorkbooks = moExcel.Workbooks;
			moExcel.DisplayAlerts = false;
			moWorkbook = moWorkbooks.Add(Missing.Value);
			moWorksheets = moWorkbook.Sheets;
			moWorksheet = (Microsoft.Office.Interop.Excel.Worksheet) moWorksheets[1];
		}



	}

}
