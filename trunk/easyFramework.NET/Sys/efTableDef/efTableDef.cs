using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
	//Class:     TableDef
	//--------------------------------------------------------------------------------'
	//Module:    TableDef.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   this class provides methods, to check a recordset against an
	//           XML-table-def file;
	//           it creates also SQL-Statements to insert a given recordset
	//           into the table
	//--------------------------------------------------------------------------------'
	//Created:   08.04.2004 10:10:33
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class TableDef
	{

		//================================================================================
		//Private Fields:
		//================================================================================
		private string msTableName;
		private TableDefField[] maoField;
		private TableDefLookupField[] maoLookupFields;
		private TableDefField[] maoField_PrimaryKey;
		private TableDefField moField_Inserted;
		private TableDefField moField_Updated;
		private TableDefField moField_InsertedBy;
		private TableDefField moField_UpdatedBy;

		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  aoPrimaryKeyFields
		//--------------------------------------------------------------------------------'
		//Purpose:   the primary-key columns of the table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 08:14:09
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public TableDefField[] aoPrimaryKeyFields
		{
			get
			{
				return maoField_PrimaryKey;
			}
		}

		public TableDefField oField_Inserted
		{
			get
			{
				return moField_Inserted;
			}
		}
		public TableDefField oField_InsertedBy
		{
			get
			{
				return moField_InsertedBy;
			}
		}
		public TableDefField oField_Updated
		{
			get
			{
				return moField_Updated;
			}
		}
		public TableDefField oField_UpdatedBy
		{
			get
			{
				return moField_UpdatedBy;
			}
		}
		public string sTableName
		{
			get
			{
				return msTableName;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================
		public TableDef(XmlDocument oXmlTableDef) 
		{
	
			mInit(oXmlTableDef);
		}

		public TableDef(string sXmlTableDef) 
		{
			XmlDocument oXml = new XmlDocument(sXmlTableDef);
			mInit(oXml);
	
		}


		//================================================================================
		//Function:  gsGetFieldDescription
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a user-readable description of the field
		//--------------------------------------------------------------------------------'
		//Params:    the field-name of db
		//--------------------------------------------------------------------------------'
		//Returns:   the description of the field
		//--------------------------------------------------------------------------------'
		//Created:   26.04.2004 02:29:13
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetFieldDescription(string sFieldName)
		{
	
			TableDefField oField = goGetField(sFieldName);
			if (oField == null == false)
			{
				return oField.sDesc;
			}
	
			throw (new efException("The field \"" + sFieldName + "\" doesn't exist."));
	
		}


		//================================================================================
		//Function:  goGetField
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the table-field by its name
		//--------------------------------------------------------------------------------'
		//Params:    the name of the field
		//--------------------------------------------------------------------------------'
		//Returns:   tabledeffield
		//--------------------------------------------------------------------------------'
		//Created:   26.04.2004 02:31:49
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Sys.Data.Table.TableDefField goGetField(string sName)
		{
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
				if (maoField[i].sName == Functions.LCase(sName))
				{
					return maoField[i];
				}
			}
	
			//----now search the lookup-fields------
			for (int i = 0; i <= maoLookupFields.Length - 1; i++)
			{
				if (maoLookupFields[i].sName == Functions.LCase(sName))
				{
					return maoLookupFields[i];
				}
			}

			return null;
	
		}

		//================================================================================
		//Function:  aoCheckRecordset
		//--------------------------------------------------------------------------------'
		//Purpose:   checks the recordset; returns an array of errors, if there are errors,
		//           otherwise it returns nothing
		//--------------------------------------------------------------------------------'
		//Params:    the recordset
		//--------------------------------------------------------------------------------'
		//Returns:   fills the error-collection in oClientInfo, if there were any errors
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 10:45:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbCheckRecordset(ClientInfo oClientInfo, Recordset rs)
		{
	
			RecordsetObjects.Bookmark oBookmark = rs.oBookmark;
			rs.MoveFirst();
	
			while (! rs.EOF)
			{
		
				//check each field:
				for (int lField = 0; lField <= maoField.Length - 1; lField++)
				{
			
					RecordsetObjects.Field oRsField = null;
					TableDefField oTableDefField = maoField[lField];
			
					if (rs.oFields.gbIsField(maoField[lField].sName))
					{
						oRsField = rs.oFields[maoField[lField].sName];

						oTableDefField.gbCheckValue(oClientInfo, oRsField);
					}
			
			
				}
		
		
				rs.MoveNext();
			};
	
	
			rs.oBookmark = oBookmark;
	
			if (oClientInfo.bHasErrors == true)
			{
				return false;
			}
			else
			{
				return true;
			}
	
		}


		//================================================================================
		//Sub:       UpdateDatabase
		//--------------------------------------------------------------------------------'
		//Purpose:   makes a update/insert in the database, depending, if the key-field
		//           values are given or not
		//--------------------------------------------------------------------------------'
		//Params:    bPerformRuleCheck - if gbCheckRecordset is called before or not
		//           bAllowConcurrentUpdate - if the last-update fields are given and this
		//                             value is false, then before the update the
		//                             last-update fields are compared; if they
		//                             are the same, then the update can be done, otherwise
		//                             an efTableUpdateConflictException is thrown
		//--------------------------------------------------------------------------------'
		//Returns:   false and the errors in oclientinfo or it returns true
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 00:38:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gUpdateInDatabase(ClientInfo oClientInfo, Recordset rs, bool bPerformRuleCheck, bool bAllowConcurrentUpdate)
		{
	
			bool bResult;
			if (bPerformRuleCheck == true)
			{
		
				bResult = gbCheckRecordset(oClientInfo, rs);
				if (bResult == false)
				{
					return false;
				}
			}
	
			RecordsetObjects.Bookmark oBookmark = rs.oBookmark;
	
			//------------update the field-types; if it comes from a html-dialog, the field-type is "undefined"------------
	
			gUpdateFieldTypes(rs);
	
	
			rs.MoveFirst();
			while (! rs.EOF)
			{
		
		
				//---------------determine update-status -----------------------------
				//check, wether all key-fields are not null and have values; otherwise
				//it is not update, but insert:
				bool bUpdate = true; //if false, then insert
				bool bAllKeyFieldsHaveValues = true;
		
				TableDefField[] aoPrimaryFields = this.aoPrimaryKeyFields;
				if (aoPrimaryFields == null)
				{
					throw (new TableDefParseException("Update not possible - at least on key-field " + "is needed!"));
				}
				else
				{
			
					for (int i = 0; i <= aoPrimaryFields.Length - 1; i++)
					{
						if (Functions.IsEmptyString(rs[aoPrimaryFields[i].sName].sValue))
						{
							bUpdate = false;
							bAllKeyFieldsHaveValues = false;
							break;
						}
					}
				}
		
				//------------------- check fields --------------------------------
				if (oField_Inserted == null)
				{
					throw (new efException("Field inserted wasn't found. Please check tabledef!"));
				}
				if (oField_InsertedBy == null)
				{
					throw (new efException("Field insertedBy wasn't found. Please check tabledef!"));
				}
				if (oField_Updated == null)
				{
					throw (new efException("Field updated wasn't found. Please check tabledef!"));
				}
				if (oField_UpdatedBy == null)
				{
					throw (new efException("Field updatedBy wasn't found. Please check tabledef!"));
				}
		
		
				//------------------- get rsInDb ----------------------------------
				//if it is an update, then check, if the dataset exists in the
				//database; otherwise it is a new dataset and the user
				//has entered the primary-key values by his own:
				Recordset rsInDb = null;
				string sClause = this.gsGetSQLClauseForPrimaryFields(rs, false);
				string sQrySelect = "SELECT " + aoPrimaryFields[0].sName + "," + oField_Inserted.sName + "," + oField_InsertedBy.sName + "," + oField_Updated.sName + "," + oField_UpdatedBy.sName + "  FROM " + this.msTableName + " WHERE " + sClause;
				if (bUpdate)
				{
					rsInDb = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQrySelect);
					if (rsInDb.lRecordcount == 0)
					{
						bUpdate = false; //do insert
					}
				}
		
		
				//-----------try to get automatic record-id if insert and enter value in rs: --------------
				bool bUpdateRecordIDsAfterExecute = false;
				int lNextRecordID;
				if (bUpdate)
				{
			
				}
				else
				{
			
					//a new record-id has to be retrieved, if not already set.
					//if there is only one primary-keyfield and the key-field is an int,
					//then use the tsRecordIDs table to get the new value
					if (bAllKeyFieldsHaveValues == false)
					{
				
				
						if (maoField_PrimaryKey.Length > 1)
						{
							throw (new TableUpdateException("If there are more than 1 key-fields, " + "the key cannot be automatically taken from tsRecordIDs. You " + "have to provide the key-values by yourself."));
						}
				
						//type must be int for primary-key:
						if (maoField_PrimaryKey[0].enType != RecordsetObjects.Field.efEnumFieldType.efInteger)
						{
							throw (new TableUpdateException("Key-field is not of type int. " + "The key cannot be automatically taken from tsRecordIDs. You " + "have to provide the key-value by yourself."));
						}
				
						lNextRecordID = Data.InternalClientInfo.glGetNextRecordID(oClientInfo, this.msTableName);
						bUpdateRecordIDsAfterExecute = true;
						rs[maoField_PrimaryKey[0].sName].lValue = lNextRecordID;
				
					}
				}
		
		
				//-----------get sql-query-------------------------
				string sQry;
				if (bUpdate)
				{
					sQry = this.gsGetSqlUpdate(oClientInfo, rs);
				}
				else
				{
					sQry = this.gsGetSqlInsert(oClientInfo, rs);
				}
		
		
		
		
				//-----------check concurrent update-------------------------
				if (bAllowConcurrentUpdate == false)
				{
					//the last-update field is needed (as well as the last user-field)
			
					if (this.moField_Updated == null | this.moField_UpdatedBy == null)
					{
						throw (new efException("For concurrent-updates the updated and the updated" + "-by columns are needed in the table-definition."));
					}
			
					if ( rsInDb!= null)
					{
						if (rsInDb.oFields[moField_Updated.sName].sValue != rs[moField_Updated.sName].sValue)
						{
					
							//concurrent update:
							throw (new TableUpdateConflictException(oClientInfo, this.msTableName, rsInDb[moField_UpdatedBy.sName].sValue, 
								rsInDb[moField_Updated.sName].dtValue));
					
						}
					}
			
				}
		
		
				//--------------- execute sql-statement ---------------------
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, sQry);
		
				if (bUpdateRecordIDsAfterExecute)
				{
					InternalClientInfo.gUpdateTsRecordIds(oClientInfo, msTableName, maoField_PrimaryKey[0].sName);
				}
		
		
				rs.MoveNext();
			} ;
	
	
			rs.oBookmark = oBookmark;
	
			return true;
	
		}


		//================================================================================
		//Sub:       gUpdateFieldTypes
		//--------------------------------------------------------------------------------'
		//Purpose:   if the recordset is generated by a html-dialog, then the
		//           field-types are undefined. so the field-types can be updated
		//           by the table-def
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 20:15:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gUpdateFieldTypes (Recordset rs)
		{
	
	
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
		
				if (rs.oFields.gbIsField(maoField[i].sName))
				{
			
					if (rs[maoField[i].sName].enType == RecordsetObjects.Field.efEnumFieldType.efUndefined)
					{
				
						rs[maoField[i].sName].enType = maoField[i].enType;
				
				
					}
			
				}
		
			}
		}

		//================================================================================
		//Sub:       gDeleteInDatabase
		//--------------------------------------------------------------------------------'
		//Purpose:   deletes the given-recordset from the database
		//--------------------------------------------------------------------------------'
		//Params:    bOnlyCurrentRecord - if false, then all entries in the recordset
		//                                are deleted
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 08:17:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gDeleteInDatabase (ClientInfo oClientInfo, Recordset rs, bool bOnlyCurrentRecord)
		{
	
			if (bOnlyCurrentRecord == false)
			{
				rs.MoveFirst();
			}
	
			while (! rs.EOF)
			{
		
				string sClause;
				sClause = this.gsGetSQLClauseForPrimaryFields(rs, true);
		
				string sQry = "DELETE FROM " + this.msTableName + " WHERE " + sClause;
		
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, sQry);
		
		
				if (bOnlyCurrentRecord)
				{
					return;
				}
		
				rs.MoveNext();
			} ;
	
	
		}
		//================================================================================
		//Function:  gsGetSqlSelectFields
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the sql-statement for selecting all fields, including the
		//           lookup fields
		//--------------------------------------------------------------------------------'
		//Params:    oClientInfo
		//           bIncludeLookupFields -  False: no lookup fields are include
		//                                   True: lookup-fields are included
		//--------------------------------------------------------------------------------'
		//Returns:   sql of fields (including lookup-fields)
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:19:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetSqlSelectFields(ClientInfo oClientInfo, bool bIncludeLookupFields)
		{
	
			string sSelectFields = "";
	
			//-------include default-fields---------
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
				sSelectFields += maoField[i].sName + ",";
			}
			sSelectFields = Functions.Left(sSelectFields, Functions.Len(sSelectFields) - 1);
	
	
			//-------include lookup-fields-------------
			if (bIncludeLookupFields)
			{
		
				sSelectFields = sSelectFields + ",";
		
				for (int i = 0; i <= maoLookupFields.Length - 1; i++)
				{
					sSelectFields += maoLookupFields[i].sGetAsSQL( true) + ",";
				}
				sSelectFields = Functions.Left(sSelectFields, Functions.Len(sSelectFields) - 1);
			}
	
	
			return sSelectFields;
	
	
		}
		//================================================================================
		//Function:  gsGetSqlInsert
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the sql-statement for inserting a record of a recordset
		//--------------------------------------------------------------------------------'
		//Params:    the recordset; only the current item is used for build the sql
		//--------------------------------------------------------------------------------'
		//Returns:   sql of insert-statement
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:19:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetSqlInsert(ClientInfo oClientInfo, Recordset rs)
		{
	
	
			string sInsertFields = "";
			string sInsertValues = "";
	
			if (maoField == null)
			{
				return "";
			}
	
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
				sInsertFields += maoField[i].sName + ",";
			}
			sInsertFields = Functions.Left(sInsertFields, Functions.Len(sInsertFields) - 1);
	
	
	
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
		
				bool bHandled = false;
		
				if (moField_Inserted == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_Inserted.sName))
					{
						sInsertValues += DataConversion.gsSqlDate(Functions.Now(), DataConversion.efEnumSqlDateFormat.dfTimeStamp) + ",";
						bHandled = true;
					}
				}
				if (moField_InsertedBy == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_InsertedBy.sName))
					{
						sInsertValues += "'" + easyFramework.Sys.Data.DataTools.SQLString(oClientInfo.gsGetUsername()) + "',";
						bHandled = true;
					}
				}
				if (moField_Updated == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_Updated.sName))
					{
						sInsertValues += DataConversion.gsSqlDate( Functions.Now(), DataConversion.efEnumSqlDateFormat.dfTimeStamp) + ",";
						bHandled = true;
					}
				}
				if (moField_UpdatedBy == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_UpdatedBy.sName))
					{
						sInsertValues += "'" + easyFramework.Sys.Data.DataTools.SQLString(oClientInfo.gsGetUsername()) + "',";
						bHandled = true;
					}
				}
		
		
				if (! bHandled)
				{
					sInsertValues += moGetField(rs, maoField[i]).sGetForSQL() + ",";
				}
		
		
		
		
			}
			sInsertValues = Functions.Left(sInsertValues, Functions.Len(sInsertValues) - 1);
	
	
			string sQry;
			sQry = "INSERT INTO $1($2) VALUES($3)";
	
			sQry = Functions.Replace(sQry, "$1", this.msTableName);
			sQry = Functions.Replace(sQry, "$2", sInsertFields);
			sQry = Functions.Replace(sQry, "$3", sInsertValues);
	
			return sQry;
		}

		//================================================================================
		//Function:  gsGetSqlUpdate
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the sql-statement for updating a record of a recordset
		//--------------------------------------------------------------------------------'
		//Params:    the recordset; only the current item is used for build the sql
		//--------------------------------------------------------------------------------'
		//Returns:   sql of update-statement
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:19:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetSqlUpdate(ClientInfo oClientInfo, Recordset rs)
		{
	
			string sSetClause = "";
	
			for (int i = 0; i <= maoField.Length - 1; i++)
			{
		
		
				bool bHandled = false;
		
				if (moField_Updated == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_Updated.sName))
					{
						sSetClause += maoField[i].sName + "=" + DataConversion.gsSqlDate(Functions.Now(), DataConversion.efEnumSqlDateFormat.dfTimeStamp) + ",";
						bHandled = true;
					}
				}
				if (moField_UpdatedBy == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_UpdatedBy.sName))
					{
						sSetClause += maoField[i].sName + "='" + easyFramework.Sys.Data.DataTools.SQLString(oClientInfo.gsGetUsername()) + "',";
						bHandled = true;
					}
				}
		
				if (moField_Inserted == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_Inserted.sName))
					{
						//do nothing - don't touch inserted
						bHandled = true;
					}
				}
				if (moField_InsertedBy == null == false)
				{
					if (Functions.LCase(maoField[i].sName) == Functions.LCase(moField_InsertedBy.sName))
					{
						//do nothing - don't touch insertedby
						bHandled = true;
					}
				}
		
		
		
		
		
				if (! bHandled)
				{
					sSetClause += maoField[i].sName + "=" + moGetField(rs, maoField[i]).sGetForSQL() + ",";
				}
		
			}
			sSetClause = Functions.Left(sSetClause, Functions.Len(sSetClause) - 1);
	
	
			string sQry;
			sQry = "UPDATE $1 SET $2 $3";
	
			sQry = Functions.Replace(sQry, "$1", this.msTableName);
			sQry = Functions.Replace(sQry, "$2", sSetClause);
	
	
	
			sSetClause = gsGetSQLClauseForPrimaryFields(rs, true);
	
			if (!Functions.IsEmptyString(sSetClause))
			{
				sQry = Functions.Replace(sQry, "$3", " WHERE " + sSetClause);
			}
			else
			{
				sQry = Functions.Replace(sQry, "$3", "");
			}
	
			return sQry;
	
		}

		//================================================================================
		//Function:  gsGetSqlDelete
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the sql-statement for deleting a record of a recordset
		//--------------------------------------------------------------------------------'
		//Params:    the recordset; only the current item is used for build the sql
		//--------------------------------------------------------------------------------'
		//Returns:   sql of update-statement
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:19:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetSqlDelete(ClientInfo oClientInfo, Recordset rs)
		{
	
	
			string sQry;
			sQry = "DELETE FROM $1 WHERE $2";
	
			sQry = Functions.Replace(sQry, "$1", this.msTableName);
	
			string sClause = gsGetSQLClauseForPrimaryFields(rs, true);
	
			if (!Functions.IsEmptyString(sClause))
			{
				sQry = Functions.Replace(sQry, "$2", sClause);
			}
			else
			{
				throw (new efException("Cannot delete from whole table, via table-definition file!"));
			}
	
			return sQry;
	
		}

		//================================================================================
		//Function:  gsGetSQLClauseForPrimaryFields
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the sql-clause of "where" for getting the dataset
		//           in the recordset; all key-fields are known, so it is easy to
		//           get the where-clause
		//--------------------------------------------------------------------------------'
		//Params:    the recordset
		//           bExceptionIfNullValues - if true, then an error is raised,
		//                                    if values are not set
		//--------------------------------------------------------------------------------'
		//Returns:   WHERE-clause (sql)
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 07:55:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetSQLClauseForPrimaryFields(Recordset rs, bool bExceptionIfNullValues)
		{
	
			FastString sResult = new FastString();
	
			if (aoPrimaryKeyFields != null)
			{
				for (int i = 0; i <= aoPrimaryKeyFields.Length - 1; i++)
				{
			
					if (moGetField(rs, aoPrimaryKeyFields[i]).bIsNull() | Functions.IsEmptyString(moGetField(rs, aoPrimaryKeyFields[i]).sValue))
					{
				
						if (bExceptionIfNullValues)
						{
							throw (new efException("Primary key-field not set: " + aoPrimaryKeyFields[i].sName));
					
						}
					}
			
			
					sResult.Append(aoPrimaryKeyFields[i].sName + "=" + moGetField(rs, aoPrimaryKeyFields[i]).sGetForSQL());
			
					if (i < aoPrimaryKeyFields.Length - 1)
					{
						sResult.Append(",");
					}
			
				}
			}
	
			return sResult.ToString();
	
		}


		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       mInit
		//--------------------------------------------------------------------------------'
		//Purpose:   called by constructor
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.04.2004 14:30:36
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mInit (XmlDocument oXmlTableDef)
		{
	
			//parse the xml and load structur:
			XmlNode oNode;
			oNode = oXmlTableDef.selectSingleNode("/table");
			if (oNode == null)
			{
				throw (new efException("document-element missing: table"));
			}
	
			if (Functions.IsEmptyString(oNode.oAttributeList["name"].sText))
			{
				throw (new efException("attribute of table-node missing: name"));
			}
			else
			{
				msTableName = oNode.oAttributeList["name"].sText;
			}
	
			//-----------get the table fields-------------------
			XmlNodeList nlField = oXmlTableDef.selectNodes("/table/field");
	
			efArrayList oFields = new efArrayList();
			efArrayList oPrimaryFields = new efArrayList();
	
			for (int i = 0; i <= nlField.lCount - 1; i++)
			{
		
				TableDefField oField = new TableDefField();
				oField.gInitFromXml(nlField[i]);
		
				oFields.Add(oField);
		
				if (oField.bPrimaryKey)
				{
					oPrimaryFields.Add(oField);
				}
		
				if (Functions.LCase(oNode.oAttributeList["inserted"].sText) == Functions.LCase(oField.sName))
				{
					moField_Inserted = oField;
				}
				else if (Functions.LCase(oNode.oAttributeList["insertedby"].sText) == Functions.LCase(oField.sName))
				{
					moField_InsertedBy = oField;
				}
				else if (Functions.LCase(oNode.oAttributeList["updated"].sText) == Functions.LCase(oField.sName))
				{
					moField_Updated = oField;
				}
				else if (Functions.LCase(oNode.oAttributeList["updatedby"].sText) == Functions.LCase(oField.sName))
				{
					moField_UpdatedBy = oField;
				}
		
			}
	
	
			//---------copy the temporary arraylist to the class member-variables----------
			maoField = new TableDefField[oFields.Count-1 + 1];
			oFields.CopyTo(maoField);
	
			maoField_PrimaryKey = new TableDefField[oPrimaryFields.Count-1 + 1];
			oPrimaryFields.CopyTo(maoField_PrimaryKey);
	
	
	
			//----------check for special fields, like "inserted", "insertedBy", "updated"....
			if (!Functions.IsEmptyString(oNode.oAttributeList["inserted"].sText) & moField_Inserted == null)
			{
				throw (new TableDefParseException("Invalid inserted-column \"" + oNode.oAttributeList["inserted"].sText + "\""));
			}
			if (!Functions.IsEmptyString(oNode.oAttributeList["insertedby"].sText) & moField_Inserted == null)
			{
				throw (new TableDefParseException("Invalid insertedby-column \"" + oNode.oAttributeList["insertedby"].sText + "\""));
			}
			if (!Functions.IsEmptyString(oNode.oAttributeList["updated"].sText) & moField_Inserted == null)
			{
				throw (new TableDefParseException("Invalid updated-column \"" + oNode.oAttributeList["updated"].sText + "\""));
			}
			if (!Functions.IsEmptyString(oNode.oAttributeList["updatedby"].sText) & moField_Inserted == null)
			{
				throw (new TableDefParseException("Invalid updatedby-column \"" + oNode.oAttributeList["updatedby"].sText + "\""));
			}
	
	
	
	
	
	
			//----------------get the lookup-fields-------------------------
			XmlNodeList nlLookupFields = oXmlTableDef.selectNodes("/table/lookup");
	
			efArrayList oLookupFields = new efArrayList();
	
			for (int i = 0; i <= nlLookupFields.lCount - 1; i++)
			{
		
				TableDefLookupField oLookupField = new TableDefLookupField();
				oLookupField.gInitFromXml(nlLookupFields[i]);
		
				oLookupFields.Add(oLookupField);
		
		
			}
	
			//---------copy the temporary arraylist to the class member-variables----------
			maoLookupFields = new TableDefLookupField[oLookupFields.Count-1 + 1];
			oLookupFields.CopyTo(maoLookupFields);
	
		}


		//================================================================================
		//Function:  moGetField
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the field of a recordset by the given definition field
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:30:39
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected RecordsetObjects.Field moGetField(Recordset rs, TableDefField oField)
		{
	
			bool bFound = false;
			for (int i = 0; i <= rs.oFields.Count - 1; i++)
			{
				if (Functions.LCase(rs.oFields[i].sName) == Functions.LCase(oField.sName))
				{
					bFound = true;
					return rs.oFields[i];
			
				}
			}
	
			if (! bFound)
			{
				throw (new TableDefParseException("Invalid column \"" + oField.sName + "\"!"));
			}
	
			return null;

	
		}
		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================

	}



}
