/* (c) Promain Software-Betreuung GmbH 2004 */
var aoDataTables = new Array();
	

function fillDataTableRows(sDivID, sDataPage, lPage, lRowsPerPage, sOnItemClick, 
	sAddParams, sOnItemDblClick,sOnAfterInit) {
												 
	//default-settings:
	if (lRowsPerPage == null) lRowsPerPage = 20;
	if (lPage == null) lPage = 1;
	var oDiv = document.getElementById(sDivID);
	var oTable = new goDataTable(sDivID, lRowsPerPage, sOnItemClick, sDataPage, sAddParams,
		 sOnItemDblClick, sOnAfterInit);
	
	if (sOnAfterInit != "")
		eval(sOnAfterInit);

}


function gDataTableUnselectItem() {
	//makes the item, which is select, unselecte;
	//if nothing is selected, then nothing happens:
	if (this.mtr_datatable_focused != null) {
	
		this.mtr_datatable_focused.style.background = this.mtr_datatable_focused_background;
		this.mtr_datatable_focused.style.color = this.mtr_datatable_focused_color; 
	}
	this.sCurrentSelectedItemId = null;
}

function gDataTableSelectItem(tr, oTable, sData) {

	oTable.mtr_datatable_focused_color = tr.style.color;
	oTable.mtr_datatable_focused_background = tr.style.background; 
	tr.style.background = oTable.sSelectBGColor;
	tr.style.color = oTable.sSelectFontColor;
	oTable.mtr_datatable_focused = tr;

	this.sCurrentSelectedItemId = sData;
}

function goDataTable(sDivID, lRowsPerPage, sOnItemClick, sDataPage, sAddParams, 
	sOnItemDblClick) { //constructor
	this.id = sDivID;
	this.divTable = document.getElementById(sDivID + "_table");
	this.divInfoText = document.getElementById(sDivID + "_infotext");
	this.divButtons = document.getElementById(sDivID + "_buttons");
	this.lDataTableRowsPerPage = lRowsPerPage;
	this.lPage = 1;
	this.mlPageCount = mlDataTablePageCount;
	this.lRecordcount = -1;
	this.sDataCurrenPage = "";
	this.sDivId = sDivID;
	this.mtr_datatable_focused = null;
	this.mtr_datatable_focused_color = null;
	this.mtr_datatable_focused_background = null;
	this.sOnItemClick = sOnItemClick;
	this.sOnItemDblClick = sOnItemDblClick;
	this.sDataPage = sDataPage;
	this.sAddParams = sAddParams;
	this.gUnselectItem = gDataTableUnselectItem;
	this.gSelectItem = gDataTableSelectItem;
	this.sSelectBGColor = "#000066";
	this.sSelectFontColor = "#FFFFFF";
	this.sFontColor = "#000000";
	this.sBGColor1 = "#FFFFFF";
	this.sBGColor2 = "#DCDCFF";
	
	
	//properties:
	this.sLineEnd = new String("-||-")
	this.sColumnSep = new String("|")
	this.sCurrentSelectedItemId = null;

	//methods:
	this.mGetData = mGetData;
	this.mRender = mDataTableRender;
	this.mlPageCount = mlDataTablePageCount;
	
	//public methods:
	this.moveFirst = DataTableMoveFirst;
	this.movePrev = DataTableMovePrev;
	this.moveNext = DataTableMoveNext;
	this.moveLast = DataTableMoveLast;
	this.reload = DataTableReload;
	
	this.moveFirst();
	
	aoDataTables.push(this);
	
	
	
	return this;

}

function goFindDataTable(sTableId) {

	for (i=0; i < aoDataTables.length; i++) {
		if (aoDataTables[i].id == sTableId) {
			return aoDataTables[i];
		}
	
	}

	alert("Datatable \""+ sTableId + "\" not found!");

}

function mlDataTablePageCount() {

	var iPageCount = 1 + Math.floor((this.lRecordcount / this.lDataTableRowsPerPage));
	
	return iPageCount;

	
}

function gDataTableOnButtonClick(sTableDivID, sButton) {

	var oDataTable = goFindDataTable(sTableDivID)
	if (oDataTable == null) {
		return;
	}
   

	//get the current-page:
	if (sButton == "first") 
		oDataTable.moveFirst();
	else if (sButton == "prev") 
		oDataTable.movePrev();
	else if (sButton == "next") 
		oDataTable.moveNext();
	else if (sButton == "last")
		oDataTable.moveLast();
	
	

}

function gDataTableOnClick(tr, sTableId, sData) {
	//the handle for the click in the data-grid (select-item-display and call the real-click event)
	var oTable = goFindDataTable(sTableId);
	
	
	var bResult = true;

	if (oTable.sOnItemClick != "") {
		var sFunc = oTable.sOnItemClick;
		sFunc = sFunc.replace("();", "");
		eval("bResult=" + sFunc + "(" + sData + ");");
	}
	
	if (bResult != false) {
		oTable.gUnselectItem();
		oTable.gSelectItem(tr, oTable, sData);
	}
	
}

function gDataTableOnDblClick(tr, sTableId, sData) {
	//the handle for the click in the data-grid (select-item-display and call the real-click event)
	var oTable = goFindDataTable(sTableId);
	oTable.gUnselectItem();
	
	oTable.gSelectItem(tr, oTable, sData);
	
	if (oTable.sOnItemDblClick != "") {
		eval(oTable.sOnItemDblClick + "(" + sData + ");");
	}
	
}



function mColumnLineToHtml(sColumnNameLine, sColumnWidthLine, sSep, sCurrentHTML) {
   
	sCurrentHTML += "<tr height=\"12\" class=\"dataRowHead\" >";

	while (sColumnNameLine.length > 0) {
				
		var sColName = sColumnNameLine.substr(0, sColumnNameLine.indexOf(sSep));
		sColumnNameLine = sColumnNameLine.substring(sColumnNameLine.indexOf(sSep) + 1, 
			sColumnNameLine.length);
		
			
		var sColWidth = sColumnWidthLine.substr(0, sColumnWidthLine.indexOf(sSep));
		sColumnWidthLine = sColumnWidthLine.substring(sColumnWidthLine.indexOf(sSep) + 1, 
			sColumnWidthLine.length);
						 
		sCurrentHTML += "<td nowrap=\"true\" class=\"dataHeader\"  width=\"" + sColWidth + "\">"+
			sColName + "</td>";


		
	}
	
	sCurrentHTML += "</tr>";

	return sCurrentHTML;
}

function mColumnDataLineToHtml(sColumnDataLine, sSep, sCurrentHTML, sTableID, lRowNumber, oTable) {

	var lColCounter = 0;
	
		
	
  	while (sColumnDataLine.length > 0) {
		var sValue = sColumnDataLine.substr(0, sColumnDataLine.indexOf(sSep));
		sColumnDataLine = sColumnDataLine.substring(sColumnDataLine.indexOf(sSep) + 1, 
		sColumnDataLine.length);
	
		
		var sStyle = "style=\"background:";
		if ((lRowNumber % 2) == 0) 
			sStyle += oTable.sBGColor1 + ";";
		else
			sStyle += oTable.sBGColor2 + ";";
		
		sStyle += "cursor:pointer; cursor:hand; ";
		
		sStyle += "\"";
		
		if (lColCounter == 0) {
			sCurrentHTML += "<tr "+sStyle+" class=\"dataRow\" id=\""+sTableID+"_"+lRowNumber+"\" " + 
				"onclick=\"gDataTableOnClick(this, '"+sTableID+"', '"+sValue+"'); return false;\"" + 
				"ondblclick=\"gDataTableOnDblClick(this, '"+sTableID+"', '"+sValue+"'); return false;\"" + 
				">";
		}
		else
			sCurrentHTML += "<td valign=\"top\" class=\"dataField\">"+
				sValue + "</td>";


		lColCounter += 1;
	}
	
	sCurrentHTML += "</tr>";


	return sCurrentHTML;
}

function mWriteInfo(oTable, sSep, lPage, lRowsPerPage) {
	//outupts: datensaetze von 1 bis 20...

	//align bottom:
	var lFrom = (oTable.lPage-1) * oTable.lDataTableRowsPerPage + 1;
	var lTo = (oTable.lPage-1) * oTable.lDataTableRowsPerPage + oTable.lDataTableRowsPerPage;
	if (lTo > oTable.lRecordcount) lTo = oTable.lRecordcount;
	if (lFrom > oTable.lRecordcount) lFrom = oTable.lRecordcount;
	
	if (oTable.mlPageCount() > 1) 
		oTable.divInfoText.innerHTML = "Datensatz " + lFrom + " - " + lTo + " von " + oTable.lRecordcount;
	else {
		//alert(oTable.lRecordcount);
		if (oTable.lRecordcount != 1)
			oTable.divInfoText.innerHTML = oTable.lRecordcount + " Datensätze";
		else
			oTable.divInfoText.innerHTML = "1 Datensatz";
	}
	
}

function mGetData() {

	//call data-page:			
	this.sDataCurrentPage = gsCallServerMethod(gsMakeAbsoluteURLPath(this.sDataPage), "<page>" + this.lPage + "</page>" + 
		"<rowsperpage>" + this.lDataTableRowsPerPage + "</rowsperpage>" +
		this.sAddParams);

	
	var sLocalData = this.sDataCurrentPage;
	var lLineCounter = 0;
	
	//jump over the ok-||-	
	var startPosLine2 = sLocalData.indexOf(this.sLineEnd) + this.sLineEnd.length;
	var sLine = new String(sLocalData);
	
	//remove OK:
	sLine = sLine.substring(sLocalData.indexOf(this.sLineEnd) + this.sLineEnd.length);
	sLine = sLine.substring(0, sLine.indexOf(this.sColumnSep) );
	
	this.lRecordcount = Number(sLine);
	  
}

function mDataTableRender() {
	
	//render head and get infos from line-info-lines:
	var sLocalData = this.sDataCurrentPage;

	if (sLocalData.substring(0,2) != "OK") {
		sInnerHTML = sLocalData;
	}
	else {
	
		sLocalData = sLocalData.substring(sLocalData.indexOf(this.sLineEnd)+this.sLineEnd.length, sLocalData.length);
		
		//build html:
 		var sInnerHTML = "<table border=\"0\" class=\"dataTable\" width=\"100%\" >";
		
		
		//just do char 13 instead of 13 + 10:
		var lLineCounter = 0;
		var sLine_ColCaptions;
		
		
		while (sLocalData.indexOf(this.sLineEnd) >= 0 && lLineCounter <= 2) {
			var sLine = new String(sLocalData.substr(0, sLocalData.indexOf(this.sLineEnd)));
			sLocalData = sLocalData.substring(sLocalData.indexOf(this.sLineEnd) + 
				this.sLineEnd.length, sLocalData.length);		 
			
			if (lLineCounter == 1) {
				sLine_ColCaptions = sLine;
			}
			else if(lLineCounter == 2)  
				sInnerHTML = mColumnLineToHtml(sLine_ColCaptions, sLine, this.sColumnSep, sInnerHTML);
			
			lLineCounter += 1;	
			
		}
			
		
		//now render data-lines:
		lLineCounter = 0;
		while (sLocalData.indexOf(this.sLineEnd) >= 0 && lLineCounter < this.lDataTableRowsPerPage) {
			var sLine = new String(sLocalData.substr(0, sLocalData.indexOf(this.sLineEnd)));
			sLocalData = sLocalData.substring(sLocalData.indexOf(this.sLineEnd) + 
				this.sLineEnd.length, sLocalData.length);
			 
			sInnerHTML = mColumnDataLineToHtml(sLine, this.sColumnSep, sInnerHTML, this.id, lLineCounter, this);
				
			lLineCounter += 1;	
			
		}
		
		sInnerHTML += "</table>"
	}
		
	mWriteInfo(this);
	
	this.divTable.innerHTML = sInnerHTML;
	
}

function DataTableMoveFirst() {
	this.lPage = 1;

	sDataCurrenPage = this.mGetData();
	this.mRender();
	
}

function DataTableMovePrev() {
	
    this.lPage -= 1;
    if (this.lPage < 1) this.lPage = 1;
	
	sDataCurrenPage = this.mGetData();
	this.mRender();
	
	
}

function DataTableMoveNext() {
	
	this.lPage += 1;
	
	if (this.lPage > this.mlPageCount()) 
		this.lPage = this.mlPageCount();
   
	sDataCurrenPage = this.mGetData();
	this.mRender();
}

function DataTableMoveLast() {

	this.lPage = this.mlPageCount();
	
	sDataCurrenPage = this.mGetData();
	this.mRender();
	
}

function DataTableReload(sXmlAddParams) {

	if (sXmlAddParams != null) {
		this.sAddParams = sXmlAddParams;
	}
	this.lPage = 1;
	
	this.mGetData();
	
	this.mRender();
	
}