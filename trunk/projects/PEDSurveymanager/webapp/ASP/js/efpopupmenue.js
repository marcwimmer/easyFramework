/* (c) Promain Software-Betreuung GmbH 2004 */

//popup-menue object

//--------------global settings--------------------

var clsContextMenuItem = "clsContextMenuItem";
var clsContextMenu = "clsContextMenu";
var safeQuote = "82857177JFHUENME";

//--------------global functions--------------------
gAdd_DocumentMouseDown(gHidePopups, false);
  
function gShowPopupMenue(sId, lMouseX, lMouseY) {
	var oPopup = goFindPopupMenue(sId);
	
	if (oPopup == null) return;
	
	if (oPopup.contentList.length == 0) {
		efMsgBox("Keine Einträge vorhanden.", "INFO");
		return;
	}
	
	
	if (lMouseX == null) lMouseX = mouseX;
	if (lMouseY == null) lMouseY = mouseY;

	//get windowwidth here, because in IE the window jumps otherwise	
	var lWindowWidth = glWindowWidth();
	var lWindowHeight = glWindowHeight();
	
	
	var useMouseX = lMouseX - oPopup.lWidth / 2;
	var useMouseY = lMouseY + 5;
	if (useMouseX < 0) useMouseX = 0;
	if (useMouseY < 0) useMouseY = 0;
	if (useMouseX == "NaN") useMouseX = 0;
	if (useMouseY == "NaN") useMouseY = 0;
	oPopup.div.style.position = "absolute";
	oPopup.div.style.width = oPopup.lWidth;
	oPopup.div.style.height= oPopup.glDivHeight();
	
	oPopup.div.style.left=0; 
	oPopup.div.style.top=0;
	oPopup.div.style.left = useMouseX;
	oPopup.div.style.top = useMouseY;


	//if the div is out of bounds then align it
	var lLeft, lTop, lWidth, lHeight;
	lLeft = Number(oPopup.div.style.left.replace("px",""));
	lTop = Number(oPopup.div.style.top.replace("px",""));
	lWidth = Number(oPopup.div.style.width.replace("px",""));
	lHeight = oPopup.glDivHeight();
	
	
	if (lLeft + lWidth > lWindowWidth - 25) 
		lLeft = lWindowWidth - lWidth - 25;	 
	if (lTop + lHeight > lWindowHeight - 50) 
		lTop = lWindowHeight - lHeight - 15;
	if (lTop < 0) 
		lTop = 0;
	if (lLeft < 0) 
		lLeft = 0;

	oPopup.div.style.left = lLeft;
	oPopup.div.style.top = lTop;
	oPopup.div.style.width = lWidth;
	oPopup.div.style.height = lHeight;

	
	gHideAllInputElements();
	oPopup.Show();
   	
} 
    
    
    
function gCreatePopupMenue(sPopupMenueID, lWidth) {

	var oPopup;
	oPopup = goFindPopupMenue(sPopupMenueID);
	if (oPopup == null) {
		//create the container div:
		var oDiv = document.createElement("div");
		oDiv.style.position = "absolute";
		oDiv.style.top = 0;
		oDiv.style.left = 0;
		oDiv.style.height = 20;
		oDiv.style.width = 20;
		oDiv.id = sPopupMenueID;
		oDiv.style.zIndex = 1000;
		oDiv.className=clsContextMenu;
		gHideDiv(oDiv);
		document.body.appendChild(oDiv);
		
		//constructor
		oPopup = new efPopupMenue(sPopupMenueID, oDiv, lWidth);
		aoPopupMenues.push(oPopup);
		
		
	}
	else oPopup.Reset();
	
}

function gHidePopups(e) {

	var theEvent;
	try {
		theEvent = event;
	}	catch (ex) {}
	if (theEvent == null) {
		theEvent = e;
	}
	if (theEvent == null) return;
	
	for (i=0; i < aoPopupMenues.length; i++) {
	
		if (gbEventInDiv(theEvent, aoPopupMenues[i].div)) {
		
		}
		else {
			aoPopupMenues[i].Hide();
		}
	}
	
	gRestoreInputElements();
	
}

function goFindPopupMenue(sId) {

	for (i = 0; i < aoPopupMenues.length; i++) {
		if (aoPopupMenues[i].id == sId) 
			return aoPopupMenues[i];
	}
	
	return null;
}


//----------------global vars-----------------------
var aoPopupMenues = new Array();



//----------------popup-object functions------------
function efPopupMenue(sId, oParentDiv, lWidth) { //constructor

	
	this.id = sId;
	this.AddEntry = mPopup_AddEntry;
	this.AddSpacer = mPopup_AddSpacer;
	this.Reset = mPopup_Reset;
	this.Show = mPopup_Show;
	this.Hide = mPopup_Hide;
	this.glDivHeight = mPopup_lDivHeight; 
	this.div = oParentDiv;
	this.handleClick = mPopup_handleClick;
	this.bVisible = false;
	
	
	
	
	this.lWidth = lWidth;
	
	this.contentList = new Array();

	return this;
}

function mPopup_Reset() {
	this.contentList = new Array();	
}

function mPopup_AddEntry(sUrl, sJavascript, bModal, sModalAfterFunc, sCaption, lItemHeight) {
	
	

	var oEntry = new Entry();
	oEntry.sUrl = sUrl;
	oEntry.sJavascript = sJavascript;
	oEntry.bModal = bModal;
	oEntry.sModalAfterFunc = sModalAfterFunc;
	oEntry.bIsSpacer = false;
	oEntry.sCaption = sCaption;
	oEntry.lItemHeight = lItemHeight;
	this.contentList.push(oEntry);


}

function mPopup_AddSpacer(sOnClick, sCaption) {

	var oEntry = new Entry();
	oEntry.bIsSpacer = true;
	this.contentList.push(oEntry);
}

function mPopup_handleClick(lIndex) {
	
	this.Hide();
	
	
	//call the javascript before, if there is one:
	if (this.contentList[lIndex].sJavascript != "") {
		this.contentList[lIndex].sJavascript+="";
		eval(this.contentList[lIndex].sJavascript);
	}
	
	
		
	//open the url, if given:
	if (this.contentList[lIndex].sUrl != "") {
	
		var sModalAfterFunc = this.contentList[lIndex].sModalAfterFunc;
		while (sModalAfterFunc.indexOf(safeQuote) >= 0) {
			sModalAfterFunc = sModalAfterFunc.replace(safeQuote,"\"");
		}
		
		gsShowWindow(this.contentList[lIndex].sUrl, 
			this.contentList[lIndex].bModal, 
			sModalAfterFunc);
	}
	
	this.Hide();
	
}

function mPopup_Show() {

	var sInnerHtml = "";
	sInnerHtml = "";
		 
	for (i=0; i < this.contentList.length; i++) {
		var oElement = this.contentList[i];
		
		if (oElement.bIsSpacer == true) {
			sInnerHtml += "<hr noshade=\"true\" size=\"1\">";
		}
		else {
		
						
				
			var sOnClick;
			sOnClick = "goFindPopupMenue('"+this.id+"').handleClick("+i+");";
			sInnerHtml += "<a href=\"#\" class=\""+clsContextMenuItem+"\"";
			sInnerHtml += " onclick=\""+sOnClick+";\">";
			sInnerHtml += oElement.sCaption;
			sInnerHtml += "</a><br>";
		}
		
	}
	
	this.div.innerHTML = sInnerHtml;
	this.bVisible = true;
	
	gShowDiv(this.div);
	
	
}

function mPopup_Hide() {

	if (this.bVisible == false) return;
	this.bVisible = false;
	
	gHideDiv(this.div);
	
	
}		  



function mPopup_lDivHeight() {

	var lResult = 0;
	for (i=0; i < this.contentList.length; i++) {
		lResult += Number(this.contentList[i].lItemHeight);
	}	 
	return lResult;
}

function Entry() { //constructor

	this.sUrl="";
	this.sCaption ="";
	this.bIsSpacer = false;
	this.lItemHeight = 18;

	return this;

}

var aoInputVisibleStates = new Array();

function goVisibleState() {

	this.sElementName = "";
	this.bVisible = true;
	this.sForm = "";

	return this;
}

function gHideAllInputElements() {
			
	var aoForms = document.forms;
	
	var iForm;
	
	while (aoInputVisibleStates.length > 0)
		aoInputVisibleStates.pop();
		 
	for (iForm = 0; iForm < aoForms.length; iForm++) {
	
		
		var iElements = 0;
		var oForm = aoForms[iForm];
		for (iElements = 0; iElements < oForm.elements.length; iElements++) {
		
			var oCtl = oForm.elements[iElements];
			
			if (oCtl.type == "text" || oCtl.type == "textarea" || oCtl.type == "password" 
				|| oCtl.type == "select-one" || oCtl.type == "checkbox") {
				
			
				if (oCtl.name != "" && oCtl.name != null) {
				
					var oVState = new goVisibleState();
					
					oVState.bVisible = oCtl.style.display;
					oVState.sElementName = oCtl.name;
					oVState.sForm = oForm.name;
					
					aoInputVisibleStates.push(oVState);
					
					oCtl.style.display="none";
				
				}	
				
			}
		
		}
	}
	
	
}

function gRestoreInputElements() {

	var i = 0;
	for (i = 0; i < aoInputVisibleStates.length; i++) {
	
		var oForm;
		
		oForm = document.forms[aoInputVisibleStates[i].sForm];
		
		var oElement = oForm.elements[aoInputVisibleStates[i].sElementName];
		
		oElement.style.display = aoInputVisibleStates[i].bVisible;
		
		
	}

}




