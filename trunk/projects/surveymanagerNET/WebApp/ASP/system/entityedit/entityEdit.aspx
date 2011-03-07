<%@ import namespace="easyFramework.Frontend.ASP.ASPTools"%>
<%@ import namespace="easyFramework.Sys.Data"%>
<%@ import namespace="easyFramework.Sys.ToolLib"%>
<%@ import namespace="easyFramework.Sys"%>
<%@ Page Language="cs" AutoEventWireup="false" Src="entityEdit.aspx.cs" Inherits="easyFramework.Project.Default.entityEdit"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		var lastEntityId;
		var sEntityOnLoad = "<%=this.sEntityIdOnLoad%>";
		
		function ONLINEHELP_entity() {
		
			return "<%=oEntity.sName%>";
		}
		
		function mOnLoad() {
			gResizeTo(<%=lStartDialogWidth%>,<%=lStartDialogHeight%>);
			gPosWindowDefault();	
			
			if (sEntityOnLoad == "") 
				OnNewClick();
			
		}
		
		function mOnOptionsClick() {
			gShowPopup_EfPopupMenueEntity1();

		}
		
		function msGetEntityIdForOptions() {
			return lastEntityId;		
		}
		
		function mOnDataTableInit() {
		
			//------------load entity-----------------------
			if (sEntityOnLoad != "") {
				mSetFilter("<%=sKeyFieldName%>;" + sEntityOnLoad); 
				loadEntityIntoDialog(sEntityOnLoad);
			}
		
		}
				
		function mOnAbort() {
			window.close();
		}
		
		function gOnDataTableItemClick(sDataID) {
			sEntityOnLoad	= "";
			
			return loadEntityIntoDialog(sDataID);	
			 
		}
		 
		 
		function mClearFilter() {
			document.getElementById("img_filter").src="<%=sImg_clear%>";

			goFindDataTable("EfDataTable1").reload("<entity><%=sEntityName%></entity><searchdlg></searchdlg>");

		}
		

		function OnNewClick() {
			
			var oTable = goFindDataTable("EfDataTable1");
			oTable.gUnselectItem();
			
			refreshTabs();
			
			gFocusFirstElement("frmMain");
			
			
		}
		function OnSaveClick() {
		
			
			var sDialogData = gsXMLDlgOutput(document.forms["frmMain"], "<entity><%=this.sEntityName%></entity>", true);
			var sResult = new String(gsCallServerMethod("EntityProcess.aspx", sDialogData))
			if (sResult.substring(0, 8) == "SUCCESS_") {
			
				//daten gespeichert, no problems
				
				//clear dirty-flag
				gClearAllXmlDialogDirtyFlags();
				
				//refresh data-table:
				goFindDataTable("EfDataTable1").reload();
				
				//refresh dialog:
				var sEntityID = sResult.substring(8, sResult.length);
				loadEntityIntoDialog(sEntityID);
			}
			else {
				efMsgBox(sResult, "ERROR");
			}
			
			
			
		}
		function OnDeleteClick() {
			if (bConfirm("Datensatz wirklich löschen?") == false) return;
			
			var sDialogData = gsXMLDlgOutput(document.forms["frmMain"], "<entity><%=this.sEntityName%></entity><deleteEntity value=\"1\"/>", true);
			var sResult = new String(gsCallServerMethod("EntityProcess.aspx", sDialogData))
			if (sResult.substring(0, 7) == "SUCCESS") {
			
				//daten gespeichert, no problems
				
				//refresh data-table:
				goFindDataTable("EfDataTable1").reload();
				
				//refresh dialog:
				refreshTabs();
			}
			else {
				efMsgBox(sResult, "ERROR");
			}

			
		}
		function OnSearchClick() {
			if (bNowModalDialogClosed == true) {
				gsShowWindow("../entitySearchSelectField/entitySearchSelectField.aspx" +
					"?entity=<%=sEntityName%>", true, "OnSearchClick();");
				
				return;
			}
			
			mSetFilter(oModalResult);

		}
		
		function mSetFilter(sSearchPhrase) {
			goFindDataTable("EfDataTable1").reload("<entity><%=sEntityName%></entity><searchdlg><![CDATA[" + 
			sSearchPhrase + "]]></searchdlg>");
			
			document.getElementById("img_filter").src="<%=sImg_filter_search%>";
			document.getElementById("img_filter").alt=new String(sSearchPhrase);
		
		}
		
		function gbAskToSave() {
			var sResult = efMsgBox("Änderungen speichern?", "QUERY", "YESNOCANCEL", "Änderungen speichern?");
			
			if (sResult  == "CANCEL") {
				return false;
			}
			if (sResult == "YES") {
				OnSaveClick();
				return true;
			}
			
			if (sResult == "NO") {
				return true;
			}
		}
		
		function refreshTabs() {
		
			if (gbAnyRegisteredXmlDialogDirty() == true) {
				if (!gbAskToSave()) return;
			}

			var sParams = "<entity><%=this.sEntityName%></entity>";
			lastEntityId = "";
		
			goFindTabDialog("EfTabDialog1").refresh(sParams, "");
					
		}
		
		function loadEntityIntoDialog(sEntityID) {
			
			if (gbAnyRegisteredXmlDialogDirty() == true) {
				if (!gbAskToSave()) return false;
			}

			var sParams = "<entity><%=this.sEntityName%></entity>" + 
					"<entityId>"+sEntityID+"</entityId>";
			lastEntityId = sEntityID;
		
			goFindTabDialog("EfTabDialog1").refresh(sParams, sEntityID);
			
			return true;
		}
							
		</script>
		<!--#include file="../defaultheader.aspx"-->
	</HEAD>
	<BODY onload="mOnLoad()">
		<%
			//--------------load default-entity tab----------------------
			const string DefaultTabId = "defaultTab";
			EfTabDialog1.sInitialSelectedTab = DefaultTabId;
			efTab oNewTab = new efTab();
			oNewTab.sCaption = "Daten";
			oNewTab.ID = DefaultTabId;
			oNewTab.sXmlDialogDefinitionFile = oEntity.sEditDialogXmlFile;
			oNewTab.sXmlDialogDataPage = "entityDialogData.aspx";
			oNewTab.sXmlDialogFormName = "frmMain";
			EfTabDialog1.gAddTab(oNewTab);

					
			//--------------load entity-tabs----------------------
			Recordset rsEntityTabs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityTabs", 
				"*", "tab_entity='" + DataTools.SQLString(sEntityName) + "'", "", "", "tab_index");
			while (!rsEntityTabs.EOF)
			{

				if (oEntityTabSec.gbHasAccessFromCache(
						oClientInfo, rsEntityTabs["tab_id"].lValue))
				{
				
					oNewTab = new efTab();
					oNewTab.sCaption = rsEntityTabs["tab_tabtext"].sValue;
					oNewTab.ID = rsEntityTabs["tab_tabid"].sValue;
					
					oNewTab.sUrl = Tools.sWebToAbsoluteFilename(Request, rsEntityTabs["tab_url"].sValue, true);
					oNewTab.sXmlDialogDefinitionFile = rsEntityTabs["tab_xmldialogdefinitionfile"].sValue;
					oNewTab.sXmlDialogFormName = "frmMain";
					if (Functions.IsEmptyString(rsEntityTabs["tab_xmldialogdatapage"].sValue)) {
						//if there is no special datapage named, then take the default datapage of the entity:
						oNewTab.sXmlDialogDataPage = "entityDialogData.aspx";
					}
					else {
						//if the datapage shall not be the default data-page, then set the given data-page:
						oNewTab.sXmlDialogDataPage = rsEntityTabs["tab_xmldialogdatapage"].sValue;
					}
					
					
					EfTabDialog1.gAddTab(oNewTab);

				}
				
				rsEntityTabs.MoveNext();
			}
		%>
	
	
		<form name="frmMain" onsubmit="mOnOk(); return false;">
			<div id="filterImage" align="right" style="Z-INDEX:100;LEFT:0px;WIDTH:5%;POSITION:absolute;TOP:0px;HEIGHT:50px"><a href="#" onclick="mClearFilter(); return false;"><img border="0" id="img_filter" src="<%=sImg_clear%>"></a></div>
			<P><ef:efdatatable id="EfDataTable1" runat="server" lRowsPerPage="10" sOnItemClick="gOnDataTableItemClick"
					sDataPage="entityDataTable.aspx" Height="60%" Overflow="efScroll" Width="80%" Left="5%" Top="0" sOnAfterInit="mOnDataTableInit();"></ef:efdatatable>
			<P></P>
			<div style="Z-INDEX: 101; LEFT: 85%; WIDTH: 15%; POSITION: absolute; TOP: 0px; HEIGHT: 60%"><ef:efbutton id="EfButton1" runat="server" sText="$Neu" sOnClick="OnNewClick(); return false;"></ef:efbutton><ef:efbutton id="EfButton2" runat="server" sText="$Speichern" sOnClick="OnSaveClick();"></ef:efbutton><ef:efbutton id="EfButton3" runat="server" sText="$Löschen" sOnClick="OnDeleteClick();"></ef:efbutton><ef:efbutton id="EfButton4" runat="server" sText="S$uchen" sOnClick="OnSearchClick();"></ef:efbutton>
				<ef:efbutton id="Efbutton5" runat="server" sOnClick="mOnOptionsClick();" sText="$Optionen"></ef:efbutton></div>
			<ef:eftabdialog id="EfTabDialog1" runat="server" Height="40%" Width="100%" Left="0px" Top="60%"
					sInitialSelectedTab="EfTab1"></ef:eftabdialog>
			<ef:efPopupMenueEntity id="EfPopupMenueEntity1" runat="server" sJavaScriptGetId="msGetEntityIdForOptions();"></ef:efPopupMenueEntity>
		</form>
	</BODY>
</HTML>
