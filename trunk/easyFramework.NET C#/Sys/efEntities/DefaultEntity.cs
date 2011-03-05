using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data.Table;
using easyFramework.Sys.SysEvents;

namespace easyFramework.Sys.Entities
{
	//================================================================================
	//Class:     DefaultEntity

	//--------------------------------------------------------------------------------'
	//Module:    DefaultEntity.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   Default-Entity-Handler for unspecial entities; can be used to
	//           very quickly create basic entities like product-groups etc.
	//--------------------------------------------------------------------------------'
	//Created:   05.04.2004 15:52:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class DefaultEntity : IEntity
	{

		//================================================================================
		//protected Fields:
		//================================================================================
		protected bool mbAllowConcurrentUpdate;
		protected bool mbIsNew;
		protected string msEntityName;
		protected string msSearchDialog;
		protected string msEditDialog;
		protected string msEditDialogRuleFile;
		protected string msEditDialogXmlFile;
		protected string msTitle;
		protected string msIcon;
		protected EntitySearchField[] masSearchFields;
		protected string msToStringExpression;
		protected Recordset mrsRecordset;
		protected string[] masDataTableColumns;
		protected string[] masDataTableWidths;
		protected string[] masDataTableCaptions;
		protected TableDef moTableDef;
		protected string msPrimarySearchField;
		protected efArrayList maoPopupLinks;
		protected int mlEditWindowHeight;
		protected int mlEditWindowWidth;



		//================================================================================
		//Public Properties:
		//================================================================================

		public string sPrimarySearchField
		{
			get
			{
				return msPrimarySearchField;
			}
		}
		public string sLastUpdateDateField
		{
			get
			{
				if (moTableDef == null == false)
				{
					if (moTableDef.oField_Updated == null == false)
					{
						return moTableDef.oField_Updated.sName;
					}
				}

				return null;
			}
		}

		public string sLastUpdateUserField
		{
			get
			{
				if (moTableDef == null == false)
				{
					if (moTableDef.oField_UpdatedBy == null == false)
					{
						return moTableDef.oField_UpdatedBy.sName;
					}
				}
		
				return null;
			}
		}
		public string sSearchDialog
		{
			get
			{
				return msSearchDialog;
			}
		}
		public string sTitle
		{
			get
			{
				return msTitle;
			}
		}
		public EntitySearchField[] asSearchFields
		{
			get
			{
				return masSearchFields;
			}
		}

		//================================================================================
		//Property:  sName
		//--------------------------------------------------------------------------------'
		//Purpose:   the name of the entity; must match with database
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 15:54:49
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sName
		{
			get
			{
				return msEntityName;
			}
		}


		//================================================================================
		//Property:  sKeyFieldName
		//--------------------------------------------------------------------------------'
		//Purpose:   the name of the primary-column
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:05:05
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sKeyFieldName
		{
			get
			{
				if (moTableDef == null == false)
				{
					if (moTableDef.aoPrimaryKeyFields == null == false)
					{
						if (moTableDef.aoPrimaryKeyFields.Length > 1)
						{
							throw (new efException("For entities, only one key-field is supported!"));
						}
						return moTableDef.aoPrimaryKeyFields[0].sName;
				
					}
				}

				return null;
		
			}
		}



		//================================================================================
		//Property:  lEditWindowHeight
		//--------------------------------------------------------------------------------'
		//Purpose:   the height of the default-edit window
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 00:55:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int lEditWindowHeight
		{
			get
			{
				return mlEditWindowHeight;
			}
		}


		//================================================================================
		//Property:  lEditWindowWidth
		//--------------------------------------------------------------------------------'
		//Purpose:   the width of the default-edit window
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 00:55:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int lEditWindowWidth
		{
			get
			{
				return mlEditWindowWidth;
			}
		}

		//================================================================================
		//Property:  sTableName
		//--------------------------------------------------------------------------------'
		//Purpose:   the name of the data-table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:05:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sTableName
		{
			get
			{
				if (moTableDef == null == false)
				{
					return moTableDef.sTableName;
				}
				return null;
			}


	
		}


		//================================================================================
		//Property:  sEditDialog
		//--------------------------------------------------------------------------------'
		//Purpose:   the href to the asp-dialog
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:05:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sEditDialog
		{
			get
			{
				return msEditDialog;
			}
		}




		//================================================================================
		//Property:  bIsLoaded
		//--------------------------------------------------------------------------------'
		//Purpose:   returns, if a record was loaded or not
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:54:05
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bIsLoaded
		{
			get
			{
				if (mrsRecordset == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}




		//================================================================================
		//Property:  oFields
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the fields of the entity-recordset to edit
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:54:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public RecordsetObjects.FieldList oFields
		{
			get
			{
				if (! bIsLoaded)
				{
					throw (new efException("Entity must be loaded or a new dataset must be added, " + "to access the fields-collection."));
				}
		
				return mrsRecordset.oFields;
			}
		}


		//================================================================================
		//Property:  oKeyField
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the key-field from the recordset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:34:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public RecordsetObjects.Field oKeyField
		{
			get
			{
				if (bIsLoaded == false)
				{
					throw (new efException("not loaded yet"));
				}
				return oFields[this.sKeyFieldName];
			}
		}

		//================================================================================
		//Property:  bIsNew
		//--------------------------------------------------------------------------------'
		//Purpose:   true, if the entity was new created
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:37:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bIsNew
		{
			get
			{
				return mbIsNew;
			}
		}

		//================================================================================
		//Property:  sDataTableCaptions
		//--------------------------------------------------------------------------------'
		//Purpose:   the captions of the data-table columns
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:39:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string[] asDataTableCaptions
		{
			get
			{
				return masDataTableCaptions;
			}
		}


		//================================================================================
		//Property:  sDataTableColumns
		//--------------------------------------------------------------------------------'
		//Purpose:   the name of the columns in the data-table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:39:44
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string[] asDataTableColumns
		{
			get
			{
				return masDataTableColumns;
			}
		}


		//================================================================================
		//Property:  sDataTableWidths
		//--------------------------------------------------------------------------------'
		//Purpose:   the widths of the columns of the data-table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:39:46
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string[] asDataTableWidths
		{
			get
			{
				return masDataTableWidths;
			}
		}


		//================================================================================
		//Property:  oTableDef
		//--------------------------------------------------------------------------------'
		//Purpose:   the table-definition for the entity
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   09.04.2004 00:02:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public TableDef oTableDef
		{
			get
			{
				return moTableDef;
			}
		}


		//================================================================================
		//Property:  sEditDialogXmlFile
		//--------------------------------------------------------------------------------'
		//Purpose:   the xml-dialogdefinition
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   09.04.2004 00:03:05
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sEditDialogXmlFile
		{
			get
			{
				return msEditDialogXmlFile;
			}
		}








		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:    sEntityName - the name of the entity
		//           oTableDef - a loaded, initialized table-definition of the entity
		//           sDosPathToBinaries - used for calling the sysevents
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 15:54:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public DefaultEntity(ClientInfo oClientInfo, string sEntityName, TableDef oTableDef) 
		{
			maoPopupLinks = new efArrayList();
	
			msEntityName = sEntityName;
	
			moTableDef = oTableDef;
	
			if (moTableDef.aoPrimaryKeyFields.Length != 1)
			{
				throw (new efException("For the entity there must be exactly one key-field of type int."));
			}
			else if (moTableDef.aoPrimaryKeyFields[0].enType != RecordsetObjects.Field.efEnumFieldType.efInteger)
			{
				throw (new efException("For the entity there must be exactly one key-field of type int."));
			}
	
	
			mInitFromDB(oClientInfo);
	
		}



		//================================================================================
		//Sub:       gFlushData
		//--------------------------------------------------------------------------------'
		//Purpose:   flushes an entity; the initialized values from the tabledef are kept
		//           the recordset is washed away
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   03.06.2004 01:15:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gFlushData ()
		{
			mrsRecordset = null;
		}


		//================================================================================
		//Sub:       gDelete
		//--------------------------------------------------------------------------------'
		//Purpose:   deletes the entity
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 21:32:46
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gDelete(ClientInfo oClientInfo)
		{
	
			if (! bIsLoaded)
			{
				throw (new efException("Entity must be loaded, before it can be deleted."));
			}
	
			string sQry = moTableDef.gsGetSqlDelete(oClientInfo, mrsRecordset);
	
	
			//-------------call before events----------------------
			bool bCancel = false;
			object[] oReturnedObjects = null;
			SysEventEngine oSysEventEngine = new SysEventEngine();
			oSysEventEngine.gRaiseBeforeEvents(oClientInfo, "EntityDelete", msEntityName, oKeyField.sValue, this, ref bCancel, ref oReturnedObjects);
	
			if (bCancel)
			{
				return false;
			}
	
			//-------------execute--------------------------------
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, sQry);
	
			//-------------call after events---------------------
			bool bRollback = false;
			oSysEventEngine.gRaiseAfterEvents(oClientInfo, "EntityDelete", msEntityName, oKeyField.sValue, this, ref bRollback, ref oReturnedObjects);
	
			if (bRollback)
			{
				return false;
			}
	
			return true;
	
		}

		//================================================================================
		//Function:  goLoad
		//--------------------------------------------------------------------------------'
		//Purpose:   loading the entity
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo and entity-id
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 15:50:50
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gLoad (ClientInfo oClientInfo, string sEntityID)
		{
			//If bIsLoaded Then
			//    Throw New efException("this entity is already loaded")
			//End If
	
	
			mLoad(oClientInfo, sEntityID);
	
	
		}



		//================================================================================
		//Sub:       gNew
		//--------------------------------------------------------------------------------'
		//Purpose:   creates a new entity of the entity-type;
		//           by calling gsSave it is stored in the database
		//--------------------------------------------------------------------------------'
		//Params:    oclientinfo
		//           optional you can give the key-value here; otherwise it is taken from
		//           tsRecordIDs
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:38:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gNew (ClientInfo oClientInfo) 
		{
			gNew(oClientInfo, "");
		}
		public void gNew (ClientInfo oClientInfo, string sEntityID)
		{
			//If bIsLoaded Then
			//    Throw New efException("this entity is already loaded")
			//End If
	
	
			//use field collection, to also get the lookup-fields:
	
			mrsRecordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, sTableName, moTableDef.gsGetSqlSelectFields(oClientInfo, true), "1=2", "", "", "");
			mrsRecordset.AddNew();
	
			if (!Functions.IsEmptyString(sEntityID))
			{
				mrsRecordset[sKeyFieldName].sValue = sEntityID;
			}
	
			mbIsNew = true;
		}
		//================================================================================
		//Sub:       gSave
		//--------------------------------------------------------------------------------'
		//Purpose:   storing the entity in the database
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 15:50:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gSave(ClientInfo oClientInfo)
		{
	
			if (bIsLoaded == false)
			{
				throw (new efException("this entity is not loaded and cannot be saved"));
			}
	
			long lInitTicks = Functions.Now().Ticks;

			//-------------check insert or update----------------------
			bool bIsInsert;
			if (Functions.IsEmptyString(oKeyField.sValue))
			{
				bIsInsert = true;
			}
			else
			{
				bIsInsert = false;
			}
	
			//-------------call before events----------------------
			bool bCancel = false;
			string sEventName;
	
			if (bIsInsert)
			{
				sEventName = "EntityInsert";
			}
			else
			{
				sEventName = "EntityUpdate";
			}
	
			object[] oReturnedObjects = null;
			SysEventEngine oSysEventEngine = new SysEventEngine();
			oSysEventEngine.gRaiseBeforeEvents(oClientInfo, sEventName, msEntityName, oKeyField.sValue, this, ref bCancel, ref oReturnedObjects);
	
			if (bCancel)
			{
				return false;
			}
	
	
	
			//-------------real storing----------------------
	
			//--------start transaction--------------
			bool bManagerTransactionHere;
			if (oClientInfo.bHasTransaction)
			{
				bManagerTransactionHere = false;
			}
			else
			{
				bManagerTransactionHere = true;
				oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
			}
	
			bool bResult;
			try
			{
		
		
				if (! moTableDef.gUpdateInDatabase(oClientInfo, this.mrsRecordset, true, mbAllowConcurrentUpdate))
				{
					return false;
				}
				else
				{
					bResult = true;
				}
		
		
		
				//-------------call after events---------------------
				bool bRollback = false;
				oSysEventEngine.gRaiseAfterEvents(oClientInfo, sEventName, msEntityName, oKeyField.sValue, this, ref bRollback, ref oReturnedObjects);
		
				if (bRollback)
				{
					bResult = false;
				}
		
				if (bManagerTransactionHere)
				{
					if (bRollback)
					{
				
						//-------do roll-back of transaction-----
						oClientInfo.RollbackTrans();
					}
					else
					{
						//---------commit transaction---------
						oClientInfo.CommitTrans();
					}
				}
		
		
		
		
				if (oClientInfo.bHasErrors)
				{
					bResult = false;
				}
		
		
		
		
			}
			catch (System.Exception ex)
			{
				//-------do roll-back of transaction-----
				if (bManagerTransactionHere)
				{
					oClientInfo.RollbackTrans();
				}
				
				throw ex;
			}
	
	
	
			//--------------return storing result----------------
			return bResult;
	
		}


		//================================================================================
		//Function:  gsToString
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a string-representation of the object
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   the string
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:59:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsToString(ClientInfo oClientInfo)
		{
			if (! bIsLoaded)
			{
				throw (new efException("must be loaded before!"));
			}
	
			if (msToStringExpression == null)
			{
				return "no fields to string given";
			}
			else
			{
				string sResult;
		
				string sQry = "SELECT ($1) AS ToStringValue FROM $2 WHERE $3";
		
				sQry = Functions.Replace(sQry, "$1", msToStringExpression);
				sQry = Functions.Replace(sQry, "$2", this.sTableName);
				sQry = Functions.Replace(sQry, "$3", this.sKeyFieldName + "='" + this.oKeyField.sValue + "'");
		
				Recordset rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
		
				if (rs.EOF)
				{
					sResult = "";
				}
				else
				{
					sResult = rs.oFields[0].sValue;
				}
				sResult = Functions.Trim(sResult);
		
				return sResult;
			}
	
	
		}



		//================================================================================
		//Function:  gaoGetLinkings
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the popup-links of the entity
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Returns:   array of TPopupLinks
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 21:37:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public TPopupLink[] gaoGetPopupLinks()
		{
			return ((TPopupLink[])(maoPopupLinks.ToArray(typeof(TPopupLink))));
		}


		//================================================================================
		//Function:  gsSearch
		//--------------------------------------------------------------------------------'
		//Purpose:   searches in the database with the given clause
		//--------------------------------------------------------------------------------'
		//Params:    the where-clause
		//           order-by clause
		//           lPage: if paged recordsets shall be returned, then give the page-number
		//                  here
		//           lPageSize: the size of a page
		//--------------------------------------------------------------------------------'
		//Returns:   recordset of found entities
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:57:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Recordset gRsSearch(ClientInfo oClientInfo, string sClause)
		{
			int lRecordcount = 0;
			return gRsSearch(oClientInfo, sClause, "", -1, -1, ref lRecordcount);
		}
		public Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy)
		{
			int lRecordcount = 0;
			return gRsSearch(oClientInfo, sClause, sOrderBy, -1, -1, ref lRecordcount);
		}
		public Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy, 
			int lPage, int lPageSize)
		{
			int lRecordcount = 0;
			return gRsSearch(oClientInfo, sClause, sOrderBy, lPage, lPageSize, ref lRecordcount);
		}
		public Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy, 
			int lPage, int lPageSize, ref int lTotalRecordCount)
		{
	
			Recordset oResult;
	
			string sReturnedFields = moTableDef.gsGetSqlSelectFields(oClientInfo, true);
	
	
			oResult = mrsGetRecordsByEntitySelectEvent(oClientInfo, sClause, sReturnedFields, sOrderBy, lPage, lPageSize, ref lTotalRecordCount);
	
			return oResult;
	
		}





		//================================================================================
		//Function:  gRsSearchMainField
		//--------------------------------------------------------------------------------'
		//Purpose:   searches in the database within the given main-searchfield
		//--------------------------------------------------------------------------------'
		//Params:    the value to search
		//--------------------------------------------------------------------------------'
		//Returns:   recordset of found entities
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:57:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Recordset gRsSearchMainField(ClientInfo oClientInfo, string sSearchValue, string sOrderBy)
		{
	
			string sClause;
	
			sSearchValue = Functions.Replace(sSearchValue, "*", "%");
			sClause = this.sPrimarySearchField + " LIKE '" + easyFramework.Sys.Data.DataTools.SQLString(sSearchValue) + "%'";
	
	
	
			Recordset oResult;
	
			oResult = mrsGetRecordsByEntitySelectEvent(oClientInfo, sClause, "*", sOrderBy);
	
			return oResult;
	
	
		}

		//================================================================================
		//Sub:       mInitFromDB
		//--------------------------------------------------------------------------------'
		//Purpose:   loads the values from the database into this object; usually called
		//           by the constructor
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 16:41:34
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public virtual void mInitFromDB (ClientInfo oClientInfo)
		{
			if (Functions.IsEmptyString(msEntityName))
			{
				throw (new efException("entityname not set yet"));
			}
	
			Recordset rs;
			rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntities", "*", "ety_name='" + easyFramework.Sys.Data.DataTools.SQLString(msEntityName) + "'", "", "", "");
	
			if (rs.EOF)
			{
				throw (new efException("Entity \"" + msEntityName + "\" wasn't found!"));
			}
			else
			{
				this.msEditDialog = rs["ety_editdialog"].sValue;
				this.msIcon = rs["ety_icon"].sValue;
				mParseSearchFields(rs["ety_searchfields"].sValue);
				this.msToStringExpression = rs["ety_tostringFields"].sValue;
				this.masDataTableWidths =Functions.String2Array(rs["ety_colwidths"].sValue, ";");
				this.masDataTableColumns =Functions.String2Array(rs["ety_cols"].sValue, ";");
				this.masDataTableCaptions =Functions.String2Array(rs["ety_colcaptions"].sValue, ";");
				this.msEditDialogRuleFile = rs["ety_tableDefXml"].sValue;
				this.msEditDialogXmlFile = rs["ety_editdialogxml"].sValue;
				this.mbAllowConcurrentUpdate = rs["ety_concurrentUpdates"].bValue;
				this.msSearchDialog = rs["ety_searchdialog"].sValue;
				this.msTitle = rs["ety_title"].sValue;
				this.msPrimarySearchField = rs["ety_primarySearchField"].sValue;
				this.mlEditWindowHeight = rs["ety_windowheight"].lValue;
				this.mlEditWindowWidth = rs["ety_windowwidth"].lValue;
		
				//set default-search-dialog if there is no one given
				if (Functions.IsEmptyString(this.msSearchDialog))
				{
					this.msSearchDialog = "/ASP/system/entitysearch/entitysearch.aspx?Entity=" + this.sName + "&";
				}
		
		
				//check length of arrays:
				if (this.masDataTableCaptions.Length != this.masDataTableColumns.Length | this.masDataTableCaptions.Length != this.masDataTableWidths.Length | this.masDataTableColumns.Length != this.masDataTableWidths.Length)
				{
			
					throw (new efException("the count of columns in ety_colwidths, ety_cols and ety_colcaptions have to match - they don't"));
			
				}
		
		
				//--------------------- load popup links --------------------
				Recordset rsPopupLinks = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityPopups", "*", "epp_ety_name='" + easyFramework.Sys.Data.DataTools.SQLString(msEntityName) + "'", "", "", "epp_index");
		
				while (! rsPopupLinks.EOF)
				{
			
					TPopupLink oPopupLink = new TPopupLink();
					oPopupLink.lId = rsPopupLinks["epp_id"].lValue;
					oPopupLink.sCaption = rsPopupLinks["epp_caption"].sValue;
					oPopupLink.sUrl = rsPopupLinks["epp_url"].sValue;
			
					maoPopupLinks.Add(oPopupLink);
			
					rsPopupLinks.MoveNext();
				};
		
		
		
				//----------------------------------------------------------
		
			}
		}

		//================================================================================
		//Function:  gsEditDialogResolved
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the url to the edit-dialog; all parameters are replaced
		//           by the key-values
		//--------------------------------------------------------------------------------'
		//Params:    optional sEntityID - the key-field-value of the entity
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 16:00:04
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsEditDialogResolved(string sEntityID)
		{
	
			string sResult = this.sEditDialog;
			if (Functions.IsEmptyString(sEntityID))
			{
				sEntityID = this.oKeyField.sValue;
			}
	
			sResult = Functions.Replace(sResult, "$1", sEntityID);
	
			return sResult;
		}

		//================================================================================
		//Function:  grsGetRecordset
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the recordset of the current-entity; could be used for
		//           displaying the content in an xml-dialog or anything else
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   07.04.2004 22:23:55
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Recordset gRsGetRecordset()
		{
			return mrsRecordset;
		}


		//================================================================================
		//Protected Methods:
		//================================================================================




		//================================================================================
		//Sub:       mLoad
		//--------------------------------------------------------------------------------'
		//Purpose:   loads entity from database; no further check like in gLoad is done;
		//           gLoad calls mLoad; also mSave calls mLoad
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 19:21:42
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mLoad (ClientInfo oClientInfo, string sEntityID)
		{
	
			mrsRecordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, moTableDef.sTableName, "*", this.sKeyFieldName + "='" + easyFramework.Sys.Data.DataTools.SQLString(sEntityID) + "'", "", "", "");
			if (mrsRecordset.EOF)
			{
				throw (new efException("No entity of type \"" + sName + "\" with id \"" + sEntityID + "\" found!"));
			}
			mbIsNew = false;
	
		}


		//================================================================================
		//Sub:       mParseSearchFields
		//--------------------------------------------------------------------------------'
		//Purpose:   parses the string of the search-fields and creates the search-fields
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   24.08.2004 11:45:31
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mParseSearchFields (string sSearchFieldsString)
		{
	
			string[] asSearchColumns =Functions.String2Array(sSearchFieldsString, ";", true, false);
	
			masSearchFields = new EntitySearchField[asSearchColumns.Length];
	
			for (int i = 0; i <= asSearchColumns.Length - 1; i++)
			{
		
				//------create search-field object--------
				EntitySearchField oSearchField;
				oSearchField = new EntitySearchField();
		
				oSearchField.sValue = asSearchColumns[i];
				masSearchFields[i] = oSearchField;

			}
	
	
		}


		//================================================================================
		//Function:  mrsGetRecordsByEntitySelectEvent
		//--------------------------------------------------------------------------------'
		//Purpose:   gets recordsets while respecting security-informations
		//           the event "EntitySelect" is raised here, which gets the information
		//           to which objects the user has access
		//--------------------------------------------------------------------------------'
		//Params:
		//           lPage: if paged recordsets shall be returned, then give the page-number
		//                  here
		//           lPageSize: the size of a page
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 01:38:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected Recordset mrsGetRecordsByEntitySelectEvent(ClientInfo oClientInfo, string sWhere, string sFields, string sOrderBy)
		{

			int lRecordcount = 0;

			return mrsGetRecordsByEntitySelectEvent(oClientInfo, sWhere, sFields, sOrderBy,
				-1, -1, ref lRecordcount);
	
		}
		protected Recordset mrsGetRecordsByEntitySelectEvent(ClientInfo oClientInfo, string sWhere, string sFields, string sOrderBy, 
			int lPage, int lPageSize, ref int lTotalRecordcount)
		{
	
	
			Recordset oRsResult;
			EntitySysEventEngine oSysEventEngine = new EntitySysEventEngine();
	
			//-------------call before events----------------------
			bool bCancel = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseBeforeEvents(oClientInfo, "EntitySelect", msEntityName, null, this, ref bCancel, ref aoReturnedObjects);
	
			if (bCancel)
			{
				return null;
			}
	
			//-----------create a temporary table and insert the allowed entity-ids ----------------
			//-----------if the entity is not already in a transaction, then create a transaction   --
			if (oClientInfo.bHasTransaction == false)
			{
				oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
				oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION] = "1";
			}
	
			bool bExistsRestrictions = false;
			bool bAnyAllowedRestrictions = false;
			bool bAnyForbiddenRestrictions = false;
	
			if (
				Functions.IsEmptyString(
					DataConversion.gsCStr(
						oClientInfo.oVolatileField[
							EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE
						]
					)
				)
			)
			{
		
				//-------create now the temporary table of the ids, because it was needed created before-----
		
				string sTempTableName = "#" + Functions.gsGetRandomString(10);
				oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE] = sTempTableName;
				oSysEventEngine.gCreateTemporaryEntitySelectTable(oClientInfo, sTempTableName);
		
				if (aoReturnedObjects == null == false)
				{
					if (aoReturnedObjects.Length > 0)
					{
						bExistsRestrictions = true;
					}
			
			
					for (int i = 0; i <= aoReturnedObjects.Length - 1; i++)
					{
				
						if ((aoReturnedObjects[i]) is string |(aoReturnedObjects[i]) is string)
						{
					
							string[] asString;
					
							//------get a string-array------
							if ((aoReturnedObjects[i]) is string)
							{
								asString = new string[1];
								asString[0] = System.Convert.ToString(aoReturnedObjects[i]);
							}
							else
							{
								asString = ((string[])(aoReturnedObjects[i]));
							}
					
					
							//-------insert the values: either Allowed (+) or Forbidden(-)-----------
							for (int iString = 0; iString <= asString.Length - 1; iString++)
							{
						
						
								string sSql = "'$1'";
						
								if (Functions.Len(sSql) > 1)
								{
									sSql = Functions.Replace(sSql, "$1", DataTools.SQLString(Functions.Right(asString[iString], Functions.Len(asString[iString]) - 1))); //remove the plus/minus
								}
						
								if (Functions.Left(asString[iString], 1) == "+")
								{
									DataMethodsClientInfo.gInsertTable(oClientInfo, sTempTableName, "allowedIds", sSql);
									oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS] = "1";
								}
								else if (Functions.Left(asString[iString], 1) == "-")
								{
									DataMethodsClientInfo.gInsertTable(oClientInfo, sTempTableName, "forbiddenIds", sSql);
									oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS] = "1";
								}
								else
								{
									throw (new efException("When giving String-Ids for EntitySelect, then the first " + "character must be a minus(-) for forbidden or plus(+) for allowed records."));
								}
						
						
							}
					
					
						}
						else if ((aoReturnedObjects[i]) is Recordset)
						{
					
							Recordset rs = ((Recordset)(aoReturnedObjects[i]));
							rs.MoveFirst();
					
							bool bExistsAllowField = false;
							bool bExistsForbiddenField = false;
					
							if (rs.oFieldDefList.gbIsField("Allowed"))
							{
								bExistsAllowField = true;
							}
							if (rs.oFieldDefList.gbIsField("Forbidden"))
							{
								bExistsForbiddenField = true;
							}
					
							if (bExistsAllowField == false & bExistsForbiddenField == false)
							{
								throw (new efException("Event \"EntitySelect\" expects the sql-fields \"Allowed\" and/or " + "\"Forbidden\"!"));
							}
					
							//----------better performance: get the field-ids---------
							int lFieldIdAllowed = 0;
							int lFieldIdForbidden = 0;
					
							if (bExistsAllowField)
							{
								lFieldIdAllowed = rs.oFieldDefList[lFieldIdAllowed].lIndex;
								oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS] = "1";
							}
					
							if (bExistsForbiddenField)
							{
								lFieldIdForbidden = rs.oFieldDefList[lFieldIdAllowed].lIndex;
								oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS] = "1";
							}
					
							//-------------get the concrete sql-values-----------
							while (! rs.EOF)
							{
						
								string sSql = "'$1'";
						
						
								if (bExistsAllowField)
								{
									sSql = Functions.Replace(sSql, "$1", DataTools.SQLString(rs.oFields[lFieldIdAllowed].sValue));
									DataMethodsClientInfo.gInsertTable(oClientInfo, sTempTableName, "allowedIds", sSql);
							
							
								}
						
								if (bExistsForbiddenField)
								{
									sSql = Functions.Replace(sSql, "$1", DataTools.SQLString(rs.oFields[lFieldIdForbidden].sValue));
									DataMethodsClientInfo.gInsertTable(oClientInfo, sTempTableName, "forbiddenIds", sSql);
							
							
								}
						
								rs.MoveNext();
							};
					
						}
						else
						{
							throw (new efException("Unhandled return-object: " + aoReturnedObjects[i].GetType().ToString()));
						}
				
				
				
					}
				}
		
			}
			else
			{
		
				//-------if the temporary table was created before, usually in the EntitySysEventEngine for
				//       entitySelect, then there are Restrictions-----------
		
				bExistsRestrictions = true;
			}
	
			//------------set values of bAnyAllowedRestrictions and bAnyForbiddenRestrictions------------
			if (DataConversion.gsCStr( oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS]) == "1")
			{
				bAnyAllowedRestrictions = true;
			}
			else
			{
				bAnyAllowedRestrictions = false;
			}
			if (DataConversion.gsCStr(oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS]) == "1")
			{
				bAnyForbiddenRestrictions = true;
			}
			else
			{
				bAnyForbiddenRestrictions = false;
			}
	
	
			//-------------adapt the where-clause, so it uses the temporary table--------------------------
			string sNewWhereClause = sWhere;
			if (Functions.IsEmptyString(sNewWhereClause))
			{
				sNewWhereClause = "1=1";
			}
	
			//--------if user is supervisor, then there are no restrictions----------
			if (oClientInfo.rsLoggedInUser["usr_supervisor"].bValue)
			{
				bExistsRestrictions = false;
			}
	
			if (bExistsRestrictions)
			{
		
				if (bAnyAllowedRestrictions)
				{
					sNewWhereClause += " AND " + sKeyFieldName + " IN (SELECT allowedIds FROM " + oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE] + "  WHERE NOT allowedIds IS NULL)";
			
				}
		
				if (bAnyForbiddenRestrictions)
				{
					sNewWhereClause += " AND NOT " + sKeyFieldName + " IN (SELECT forbiddenIds FROM " + oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE] + "  WHERE NOT forbiddenIds IS NULL)";
			
				}
		
			}
	
			//-------------execute--------------------------------
			if (lPage > -1)
			{
				//bugfix:
				//wenn orderby nicht angegeben wird, kann es sein, dass der sql-server nicht die richtige Reihenfolge zurückgibt. So aufgetreten bei
				//adoptionliste. es soll dann standardmäßig nach der primärspalte sortiert werden:
				if (sOrderBy.Length == 0) 
				{
					sOrderBy = this.oTableDef.aoPrimaryKeyFields[0].sName;
				}
				oRsResult = DataMethodsClientInfo.gRsGetTablePaged(oClientInfo, sTableName, lPage, lPageSize, sKeyFieldName, sFields, sNewWhereClause, "", "", sOrderBy, ref lTotalRecordcount);
			}
			else
			{
				oRsResult = DataMethodsClientInfo.gRsGetTable(oClientInfo, sTableName, sFields, sNewWhereClause, "", "", sOrderBy);
		
				//---get total-recordcount-----
				if (lTotalRecordcount != -1)
				{
					lTotalRecordcount = DataMethodsClientInfo.glDBCount(oClientInfo, sTableName, "*", sNewWhereClause, "");
				}
		
			}
	
			//------close the local transaction, if it was initiated locally------
			if (DataConversion.gsCStr(oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION])== "1")
			{
				oClientInfo.CommitTrans();
			}
	
			//-------clear the volatile-fields----------
			oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE] = "";
			oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION] = "";
			oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS] = "";
			oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS] = "";
	
	
			//-------------call after events---------------------
			bool bRollback = false;
			oSysEventEngine.gRaiseAfterEvents(oClientInfo, "EntitySelect", msEntityName, null, this, ref bRollback, ref aoReturnedObjects);
	
			if (bRollback)
			{
				return null;
			}
	
			return oRsResult;
	
	
	
		}




	}

}
