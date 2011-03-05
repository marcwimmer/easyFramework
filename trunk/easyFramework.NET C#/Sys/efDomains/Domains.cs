using System;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data
{
	//================================================================================
	//Class:     Domains

	//--------------------------------------------------------------------------------'
	//Module:    Domains.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   domain-concept
	//--------------------------------------------------------------------------------'
	//Created:   24.05.2004 18:38:07
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

    public class Domains
	{
		
		
		//================================================================================
		//Sub:       gInsertDomain
		//--------------------------------------------------------------------------------'
		//Purpose:   stores a domain in database
		//--------------------------------------------------------------------------------'
		//Params:    oClientInfo - Clientinfo
		//           lDom_id - the domain of the value
		//           sKey - the key of the domain-value
		//           sCaption - the caption of the domain-value
		//           lDvl_id - if given, then the domain is updated, if the domain-internal
		//                     value is different
		//
		//--------------------------------------------------------------------------------'
		//Created:   24.05.2004 18:39:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static void gInsertDomainValue (ClientInfo oClientInfo, int lDom_id, string sInternalValue, string sCaption, int lDvl_id)
		{
			
			//---if internal-value was changed, then update in table---
			if (lDvl_id != -1)
			{
				DataMethodsClientInfo.gUpdateTable(oClientInfo, "tsDomainValues", "dvl_internalvalue='" + DataTools.SQLString(sInternalValue) + "'", "dvl_id=" + lDvl_id + " AND dvl_dom_id=" + lDom_id);
			}
			
			
			if (! DataMethodsClientInfo.gbExists(oClientInfo, "tsDomainValues", "dvl_dom_id=" + lDom_id + " AND " + "dvl_internalvalue='" + DataTools.SQLString(sInternalValue) + "'", "*"))
			{
				
				DataMethodsClientInfo.gInsertTable(oClientInfo, "tsDomainValues", "dvl_dom_id, dvl_internalvalue, dvl_caption, dvl_inserted, dvl_insertedBy", lDom_id + ",'" + DataTools.SQLString(sInternalValue) + "','" + DataTools.SQLString(sCaption) + "'," + "getdate(),'" + DataTools.SQLString(oClientInfo.gsGetUsername()) + "'");
			}
			else
			{
				
				DataMethodsClientInfo.gUpdateTable(oClientInfo, "tsDomainValues", "dvl_caption='" + DataTools.SQLString(sCaption) + "'," + "dvl_updated=getdate()," + "dvl_updatedBy='" + DataTools.SQLString(oClientInfo.gsGetUsername()) + "'", "dvl_dom_id=" + lDom_id + " AND dvl_internalvalue='" + DataTools.SQLString(sInternalValue) + "'");
				
			}
			
			
			
		}
		
		
		//================================================================================
		//Sub:       gRemoveDomainValue
		//--------------------------------------------------------------------------------'
		//Purpose:   deletes a domain-value
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   24.05.2004 19:43:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static void gRemoveDomainValue (ClientInfo oClientInfo, int lDom_id, string sInternalValue)
		{
			
			DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsDomainValues", "dvl_dom_id=" + lDom_id + " AND dvl_internalvalue='" + DataTools.SQLString(sInternalValue) + "'");
			
			
		}
		public static void gRemoveDomainValue (ClientInfo oClientInfo, int lDvl_id)
		{
			
			DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsDomainValues", "dvl_id=" + lDvl_id);
			
		}
		
		
		
		//================================================================================
		//Function:  grsGetDomainsForComboBox
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.05.2004 00:06:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static Recordset grsGetDomainsForComboBox(ClientInfo oClientInfo, string sDomainName)
		{
			
			
			string sQry;
			Recordset orsResult;
			
			//-----check the domain-name------
			sQry = "dom_name ='$1'";
			sQry = Functions.Replace(sQry, "$1", DataTools.SQLString(sDomainName));
			if (DataMethodsClientInfo.glDBCount(oClientInfo, "tsDomains", "*", sQry, "") == 0)
			{
				throw (new efException("Domain \"" + sDomainName + "\" doesn't exist."));
			}
			
			
			sQry = "SELECT dvl_internalvalue, dvl_caption, dvl_id FROM tsDomainValues WHERE dvl_dom_id=$1 ORDER BY $2";
			
			sQry = Functions.Replace(sQry, "$1", DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tsDomains", "dom_id", "dom_name='" + DataTools.SQLString(sDomainName) + "'", ""));
			sQry = Functions.Replace(sQry, "$2", "dvl_caption ASC");
			
			orsResult = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
			
			return orsResult;
			
		}
		
		
	}
	
}
