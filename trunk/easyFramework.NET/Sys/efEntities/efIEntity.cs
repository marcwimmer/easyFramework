using System;
using easyFramework.Sys;
using easyFramework.Sys.RecordsetObjects;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data.Table;
using easyFramework.Sys.SysEvents;

namespace easyFramework.Sys.Entities
{
	//================================================================================
	//Interface: IEntity
	//--------------------------------------------------------------------------------'
	//Module:    efIEntity.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   Interface for entities; entites are basic items, which
	//           can be created, edited, stored, looked for
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 01:07:15
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	
	public interface IEntity
	{
		
		//for comments read the defaultentity-object:
		
		bool bIsLoaded{
			get;
		}
		bool bIsNew{
			get;
		}
		RecordsetObjects.FieldList oFields{
			get;
		}
		string sSearchDialog{
			get;
		}
		string sEditDialog{
			get;
		}
		string sEditDialogXmlFile{
			get;
		}
		TableDef oTableDef{
			get;
		}
		string sKeyFieldName{
			get;
		}
		RecordsetObjects.Field oKeyField{
			get;
		}
		string sLastUpdateDateField{
			get;
		}
		string sLastUpdateUserField{
			get;
		}
		string sName{
			get;
		}
		string sTableName{
			get;
		}
		EntitySearchField[] asSearchFields{
			get;
		}
		string[] asDataTableColumns{
			get;
		}
		string[] asDataTableWidths{
			get;
		}
		string[] asDataTableCaptions{
			get;
		}
		string sTitle{
			get;
		}
		string sPrimarySearchField{
			get;
		}
		int lEditWindowWidth{
			get;
		}
		int lEditWindowHeight{
			get;
		}
		
		Recordset gRsGetRecordset();
		
		Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy, int lPage, int lPageSize, ref int lTotalRecordCount);
		Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy, int lPage, int lPageSize);
		Recordset gRsSearch(ClientInfo oClientInfo, string sClause, string sOrderBy);
		Recordset gRsSearchMainField(ClientInfo oClientInfo, string sSearchValue, string sOrderBy);
		string gsToString(ClientInfo oClientInfo);
		string gsEditDialogResolved(string sEntityID);
		
		void mInitFromDB (ClientInfo oClientInfo);
		void gNew (ClientInfo oClientInfo);
		void gNew (ClientInfo oClientInfo, string sEntityID);
		void gLoad (ClientInfo oClientInfo, string sEntityID);
		bool gSave(ClientInfo oClientInfo);
		bool gDelete(ClientInfo oClientInfo);
		
		TPopupLink[] gaoGetPopupLinks();
		
		
	}
	
	
	//================================================================================
//Class:     TPopupLink
	
	//--------------------------------------------------------------------------------'
//Module:    efIEntity.vb
	//--------------------------------------------------------------------------------'
//Purpose:   an entity-may have links to other objects, defined in
	//--------------------------------------------------------------------------------'
//Created:   29.04.2004 21:35:27
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	public class TPopupLink
	{
		public TPopupLink()
		{
			mbFollowsSpacer = false;
		}
		
		
		private int mlId;
		private string msCaption;
		private string msUrl;
		private bool mbFollowsSpacer;
		
		public string sUrl
		{
			get{
				return msUrl;
			}
			set
			{
				msUrl = value;
			}
		}
		
		public string sCaption
		{
			get{
				return msCaption;
			}
			set
			{
				msCaption = value;
			}
		}
		
		
		public int lId
		{
			get{
				return mlId;
			}
			set
			{
				mlId = value;
			}
		}
		
		public bool bFollowsSpacer
		{
			get{
				return mbFollowsSpacer;
			}
			set
			{
				mbFollowsSpacer = value;
			}
		}
	}
	
}
