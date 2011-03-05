var aoTabDialogs = new Array;

var selectedZIndex = 1000; //z-index of the tab, which is selected 
var sUnselectedZIndex = 5; //z-index of the tab, which is unselected 

function gInitTabDialog(sTabDialogId) {

	var oDialog = new oTabDialog(sTabDialogId);
}

function gAppendTab(sTabDialogId, sTabId, sUrl, sXmlDialogDefinitionFile, sXmlDialogDataPage,
	sXmlFormName) {
										 
	var oTabDialog = goFindTabDialog(sTabDialogId)
	
	
	 
	var oTab = new oTabConstructor(sTabId, sUrl, sXmlDialogDefinitionFile, sXmlDialogDataPage, 
		sXmlFormName, oTabDialog);
	
	oTabDialog.mAppendTab(oTab);
	
	
		
}

function gHandleTabItemClick(sTabDialogId, sTabId) {
	//called, wenn the item is clicked:
	
	var oDialog = goFindTabDialog(sTabDialogId);
	if (oDialog == null) {
		oDialog = new oTabDialog(sTabDialogId);
	}
	
	oDialog.mOnTabClick(sTabId);
	
}


function goFindTabDialog(sId, bNoErrorAllert) {
	
	for (i=0; i < aoTabDialogs.length; i++) {
		if (aoTabDialogs[i].id == sId) 
			return aoTabDialogs[i];
	}
	
	if (bNoErrorAllert != false) {
		alert("tab-dialog \"" + sId + "\" nicht gefunden!");
	}
	
	return null;
}



//private methods:
function oTabDialog(sId) {
	//constructor:
   
 //properties:
	this.id = sId;
	this.tabs = new Array;
	this.activeTab = null;

	
	//public methods:
	this.refresh = mTabDialog_Refresh;
	
	//private methods:	
	this.mAppendTab = mAppendTab;
	this.mSelectTabItem = mSelectTabItem;
	this.mUnselectTabItem = mUnselectTabItem;
	this.mOnTabClick = mOnTabClick;
	this.mbExistsTab = mbExistsTab;
	this.goGetTab = moGetTab;
	
	//inits
	aoTabDialogs.push(this);
	
	return this;
}



function mAppendTab(oTabId) {
	this.tabs.push(oTabId);
}

function moGetTab(sTabId) {

	for (i=0; i < this.tabs.length; i++) {
	
		if (this.tabs[i].id == sTabId) 
			return this.tabs[i];
	}
	
	return null;

}

function mOnTabClick(sTabId) {

	if (!this.mbExistsTab(sTabId)) {
		alert("Tab " + sTabId + " existiert nicht!");
	}

	//if (this.activeTab != null) 
		//if (this.activeTab.id == sTabId) return;

	//unselect all other tabs:
	for (i=0; i < this.tabs.length; i++) {
		this.mUnselectTabItem(this.tabs[i].id);
	}
	
	this.activeTab = this.goGetTab(sTabId);
	
	this.mSelectTabItem(this.activeTab.id);

}

function mbExistsTab(sTabId) {

	for (i=0; i < this.tabs.length; i++) {
		if (this.tabs[i].id == sTabId)
			return true;
	}
	return false;
}

function mSelectTabItem(sTabId) {

	var sDiv = sTabId + "_tab";
	var oDiv = document.getElementById(sDiv);
	oDiv.attributes["class"].value = "clsTabSel";

	//set attribute of text:
	var oCaptionText = document.getElementById(sTabId + "_captiontext")
	oCaptionText.attributes["class"].value = "clsTabTextSel";
	
	//show the content:
	var sDivContent = sTabId + "_content";
	gShowDiv(document.getElementById(sDivContent));
	
	//display the html of the tab:
	var oTab = this.goGetTab(sTabId);
	oTab.doOnSelect();
		
}

function mUnselectTabItem(sTabId) {
	var sDiv = sTabId + "_tab";
	var oDiv = document.getElementById(sDiv);
	oDiv.attributes["class"].value = "clsTab";
	
	//set attribute of text:
	var oCaptionText = document.getElementById(sTabId + "_captiontext")
	oCaptionText.attributes["class"].value = "clsTabText";
	
	//hide the content:
	var sDivContent = sTabId + "_content";
	gHideDiv(document.getElementById(sDivContent));
	
	//change z-index:
	document.getElementById(sDivContent).style.zIndex = sUnselectedZIndex;
	
}


function mTabDialog_Refresh(sParams, sTabUrlParametervalue) {
	//sTabUrlParametervalue: if there is a $1-Parameter in the tab-url, then this
	//is replaced by the given value
	
	var i = 0 ;
	for (i=0; i < this.tabs.length; i++) {
		
		this.tabs[i].refresh(sParams, sTabUrlParametervalue);
	}

	//fetch-data for xml-dialogs:
	gXmlDialogFetchData(this.tabs[0].sXmlDialogId(), sParams);
		
}














//--------------TAB ELEMENT-------------------------------------------
function oTabConstructor(sId, sUrl, sXmlDialogDefinitionFile, sXmlDialogDataPage, sXmlFormName, oParent) {
	//constructor for a tab


	//fields:
	this.sUrl = sUrl
	this.sAdaptedUrl = ""
	this.id = sId;
	this.sXmlDialogDefinitionFile = sXmlDialogDefinitionFile;
	this.sXmlFormName = sXmlFormName;
	this.sXmlDialogDataPage = sXmlDialogDataPage;
	this.oParent = oParent; 
	this.sParams = "";
	this.sTabUrlParametervalue = "";
	this.bLoaded = false;
	this.bEverRefreshed = false;
	

	//methods:
	this.refresh = tabdialog_TabRefresh;
	this.refreshWithLastData = tabdialogtab_RefreshUsingLastData;
	this.fetchHtml = tabdialogtab_fetchHtmlContent;
	this.bIsActiveTab = tabdialogtab_IsActive;
	this.bHasXmlDialog = tabdialogtab_HasXmlDialog;
	this.sXmlDialogId = tabdialogtab_XmlDialogId;
	this.doOnSelect = tabdialogtab_doOnSelect;
	

	return this;
}


function tabdialogtab_XmlDialogId() {

	return this.id + "_xmldialog";
}

function tabdialogtab_IsActive() {

	if (this.oParent.activeTab == this) 
		return true;
	return false;

}

function tabdialogtab_HasXmlDialog() {
	//returns true, if tab has an xml-dialog; otherwise, if url, false
	if (this.sXmlDialogDefinitionFile != "" && this.sXmlDialogDefinitionFile != null) 
		return true;
	else return false;
}

function tabdialogtab_doOnSelect() {
	
	if (this.bEverRefreshed == true) {
	
		if (this.bLoaded == false) {

			this.fetchHtml();

			
		}
		else {
		
		
		}
	}
	
	document.getElementById(this.id + "_content").style.zIndex = selectedZIndex;
	
}

function tabdialogtab_RefreshUsingLastData() {

	
	this.refresh(this.sParams, this.sTabUrlParametervalue);
	
}

function tabdialog_TabRefresh(sParams, sTabUrlParametervalue) {
	//is called, when the tab-dialog has to refresh its contents.
	//This is usually done, when another parent-item is selected, and the information
	//in the tab-dialog has to be refreshed.
	
	
	this.sParams = sParams;
	this.sTabUrlParametervalue = sTabUrlParametervalue;
	var oDiv = document.getElementById(this.id + "_content");
	this.sAdaptedUrl = this.sUrl.replace("$1", sTabUrlParametervalue);

	
	if (this.bIsActiveTab() == true) {
	
		if (this.bHasXmlDialog() == true) {
		
			if (this.bLoaded == false)
				this.fetchHtml();
		
			
		}
		else {
			
			this.fetchHtml();
			
	
		}
	}
	else {
	
		if (this.bHasXmlDialog() == true) {
			
				if (this.bEverRefreshed == false) {
					this.fetchHtml();
				}
			
		}
		else {
			oDiv.innerHTML = "";
			this.bLoaded = false;
		}
		
	}
	
	this.bEverRefreshed = true;
	
}


function tabdialogtab_fetchHtmlContent() {
	
	
	var oDiv = document.getElementById(this.id + "_content");
	oDiv.style.background = "#AAC4E6"; //hard-kodiert; kann mal parametrisiert werden; dient dazu, um die hässlichen linien im ie wegzukriegen, falls verschiedene tabseiten verwendet werden
		
	if (this.bHasXmlDialog() == false) {
		 //clear current content:
		oDiv.innerHTML = "";
		
		oDiv.innerHTML = gsCallServerMethod(this.sAdaptedUrl, this.sParams);
		
		//now todo: the content of the javascripts has to be inserted as a javascript-object:
		var oJscript = document.createElement("script");
		var oJavascriptContainer = document.getElementById("javacontainer");
		
		if (oJavascriptContainer != null)  {
			oJscript.text = gsResolveEntities(oJavascriptContainer.value);
			oJavascriptContainer.appendChild(oJscript);
			
			//remove the javascript-container, so that the next tab, which could also have a javascript-container
			//will work properly:
			oJavascriptContainer.parentNode.removeChild(oJavascriptContainer);
			
		
		}
		else {
			alert("oJavascriptContainer not found!");
		}		
	
	}
	else {
				
	    //check, wether the dialog was already loaded or not:
	    if (this.bLoaded == null || this.bLoaded == false) {
			
			
			gXmlDialog2Div(oDiv, this.sXmlDialogDefinitionFile, this.sXmlDialogDataPage, this.sXmlFormName, this.sXmlDialogId(), this.sParams);
		}
		
	}

  //set loaded flag:
	this.bLoaded = true;
}
