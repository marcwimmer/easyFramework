/* (c) Promain Software-Betreuung GmbH 2004 */

var PXML_aoPagedXmlDialogs = new Array();

//XmlPagedDialogRecordcount

function PXML_registerPagedDialog(sId, sAddParams) {

	var newDialog = new PXML_constructorPagedXmlDialog(sId);
	PXML_aoPagedXmlDialogs.push(newDialog);
	
	
	newDialog.sAddParams = sAddParams;	
	
	
	return
}

function PXML_AskToSave() {

	var bDialogDirty = gbXmlDialogGetDirty(this.sXmlDialogId)
	
	if (bDialogDirty == false) 
		return;
		
		
	var sResult = efMsgBox("Möchten Sie die Änderungen vorher speichern?", "QUESTION", "YESNOCANCEL", "Änderungen speichern");
	
	if (sResult == "YES") {
		if (this.sOnSaveChanges == "")	{
			efMsgBox("Keine Speicher-Funktion hinterlegt. Bitte speichern Sie zuerst die Daten, " + 
				"bevor Sie fortfahren!", "INFO");
			return false;
		}
		
		this.sOnSaveChanges();
		
	}
	else if (sResult == "CANCEL") {
		return false;
	}
	
	return true;
	
}

function PXML_gPagedXmlDialogClick(sPagedXmlDialogId, sButton) {

	
	var oDialog = PXML_goGetDialog(sPagedXmlDialogId);
	
	
	//get the current-page:
	if (sButton == "first") 
		oDialog.moveFirst();
	else if (sButton == "prev") 
		oDialog.movePrev();
	else if (sButton == "next") 
		oDialog.moveNext();
	else if (sButton == "last")
		oDialog.moveLast();

	
}


function PXML_goGetDialog(sId) {


	var i = 0;
	for (i=0; i < PXML_aoPagedXmlDialogs.length; i++) {
	
		if (PXML_aoPagedXmlDialogs[i].sId == sId) {
			return PXML_aoPagedXmlDialogs[i];
		}
	
	}

	return null;

}


function PXML_constructorPagedXmlDialog(sId) {


	this.sId = sId;
	this.lPage = 1;
	this.lPagecount = 0;
	this.moveFirst = PXML_moveFirst;
	this.movePrev = PXML_movePrev;
	this.moveNext = PXML_moveNext;
	this.moveLast = PXML_moveLast;
	this.bAskToSave = PXML_AskToSave;
	this.sXmlDialogId = sId + "_xmldialog";
	this.sAddParams = "";
	this.sOnSaveChanges = ""; //raised, on ask to save changes; set by the assembly webcomponents
	
	this.fetchData = PXML_fetchData;

	return this;
}

function PXML_moveFirst() {

	if (this.bAskToSave() == false) return;
	
	var frm = document.forms[goFindXmlDialog(this.sXmlDialogId).sHtmlFormName];
	this.lPage = 1;
	frm.elements["__Page"].value = this.lPage;
	this.fetchData();

}
function PXML_movePrev() {
	
	if (this.bAskToSave() == false) return;
	
	var frm = document.forms[goFindXmlDialog(this.sXmlDialogId).sHtmlFormName];
	this.lPage -= 1;
	if (this.lPage < 1) this.lPage = 1;
	frm.elements["__Page"].value = this.lPage;
	this.fetchData();


}
function PXML_moveNext() {

	if (this.bAskToSave() == false) return;
	
	var frm = document.forms[goFindXmlDialog(this.sXmlDialogId).sHtmlFormName];
	this.lPage += 1;
	if (this.lPage > this.lPageCount) this.lPage = this.lPageCount;
	frm.elements["__Page"].value = this.lPage;
	this.fetchData();
	
}
function PXML_moveLast() {

	if (this.bAskToSave() == false) return;
	
	var frm = document.forms[goFindXmlDialog(this.sXmlDialogId).sHtmlFormName];
	this.lPage = this.lPageCount;
	frm.elements["__Page"].value = this.lPage;
	this.fetchData();


}

function PXML_fetchData() {

	var sXmlDialogId = this.sXmlDialogId;
	var sAddParams = this.sAddParams;
	
	//get the current-page:
	var frm = document.forms[goFindXmlDialog(sXmlDialogId).sHtmlFormName];
	sAddParams += "<__Page>" + frm.elements["__Page"].value + "</__Page>";
	sAddParams += "<__dialogpagesize>" + frm.elements["__dialogpagesize"].value + "</__dialogpagesize>";
	gXmlDialogFetchData(sXmlDialogId, sAddParams);
	
	
	//get pagecount
	var lRecordcount = frm.elements["XmlPagedDialogRecordcount"].value;
	var lDialogPageSize = frm.elements["__dialogpagesize"].value;
		
	this.lPageCount = 1 + Math.round(lRecordcount / lDialogPageSize);
	
	
}