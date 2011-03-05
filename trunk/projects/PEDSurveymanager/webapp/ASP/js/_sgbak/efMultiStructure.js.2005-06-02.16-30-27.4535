/* (c) Promain Software-Betreuung GmbH 2004 */

//--------------------------------local functions---------------------------
var aoMultiStructs = new Array();

var sLevelPraefix = new String("LV_");


var lMainCounter = 1; //counter for unique-numeration of div-elements

var sMultiStructureFormName;  //name of the form of the elements; set by the init-function

var sMultiStructureXml; //name of the multistructure-definition-xml; set by the init-function 

var sMultiStructureDataPage;
var sMultiStructureRenderAllLevelsPage;

var sMultiStructureParentDiv; 

var lMultiStructurePageSize;
var lMultiStructurePage;

var lButtonDivHeight = 40; //height of option-buttons of each xml-dialog

var lDetailsButtonDivWidth = 20; //width of the small ...-button, to expand or collapse an enitry
var lDetailsButtonDivHeight = 20; //width of the small ...-button, to expand or collapse an enitry

var lMarginToNextLevel = 20; //german: inwieweit das naechste Level eingerueckt wird

var sDetailsButtonSuffix = "_detailbuttons";
var sOptionButtonSuffix = "_optionbuttons";
var sXmlDialogHtmlSuffix = "_xmldialoghtml";
var sSubContentSuffix = "_subcontent";
var sCollapseBtnSuffix = "_collapsebtn";
var sMultiStructDlgPraefix = "dlg";

var oTopMostHiddenElement = null;

var sMultiXmlStructureFileHiddenElement = "multixmlfile"; //name of the hidden-field, which holds the name of the used-xml file
var sMultiXmlStructurePageSizeHiddenElement = "multixmlpagesize"; //name of the hidden-field, which holds the size of a page (=amount of top-elements)
var sMultiXmlStructurePageHiddenElement = "multixmlpage"; //name of the hidden-field, which holds the current page

var sMultiStructureToppestLevelName = "";

var sMultiStructureNewButtonCaptionTopElement = "";
var sMultiStructureInitialSMultiXml = "";
var sMultiStructureTopMostElementName = "";
var sMultiStructureTopMostElementValue = "";
var sMultiStructureParentDiv = "";

var sMultiStructureColumnSep = new String(".|.");
var sMultiStructureLineSep = new String("-||-");

var sMultiStructureElementDeleteFlag = "deleted";
var sMultiStructureOnAfterAskToSave = ""; //the function, which is called, after paging and any ask to save

var aoMultistructure_PrecompiledHtmlForReload = new Array();


//--------------------------MAIN INIT FUNCTION--------------------------------------
function msGetMultiInitHtml(sParentDiv, sLevelName, sFormName, sMultiXml, 
	sTopMostElementName, sTopMostElementValue, sDataPage, 
	sNewButtonCaptionTopElement, bCallReload, lPageSize, lPage, sOnAfterAskToSave,
	sRenderAllLevelsPage) {
	
	if (bCallReload == null) bCallReload = true;
	
	lMainCounter = 1;
	
	var oDiv = document.getElementById(sParentDiv);

	if (oDiv == null) {
		efMsgBox("Div \"" + sParentDiv + "\" nicht gefunden!", "ERROR");
		return;
	}
	
	if (sMultiXml.substring(0,1) != "/") {
		sMultiStructureInitialSMultiXml = sMultiXml;
		sMultiXml = gsMakeAbsoluteURLPath(sMultiXml);
	}
	
	//-------------------set local variables--------------------
	sMultiStructureFormName = sFormName;
	sMultiStructureXml = sMultiXml;
	sMultiStructureDataPage = sDataPage; 
	sMultiStructureRenderAllLevelsPage = sRenderAllLevelsPage;
	sMultiStructureToppestLevelName = sLevelName;
	sMultiStructureNewButtonCaptionTopElement = sNewButtonCaptionTopElement;
	sMultiStructureInitialSMultiXml = sMultiXml;
	sMultiStructureTopMostElementName =	sTopMostElementName;
	sMultiStructureTopMostElementValue = sTopMostElementValue;
	sMultiStructureParentDiv = sParentDiv;
	lMultiStructurePageSize = lPageSize;
	lMultiStructurePage = lPage;
	sMultiStructureOnAfterAskToSave = sOnAfterAskToSave;
	
	
	//------------create a hidden element, to store the used multi-xml-file-----------
	var oHidden = document.createElement("input");
	oHidden.setAttribute("type", "hidden");
	oHidden.setAttribute("id", sMultiXmlStructureFileHiddenElement);
	oHidden.setAttribute("name", sMultiXmlStructureFileHiddenElement);
	oHidden.setAttribute("value", sMultiXml);
	document.forms[sFormName].appendChild(oHidden);
	 
	//------------create a hidden element, to store the page-size------------
	oHidden = document.createElement("input");
	oHidden.setAttribute("type", "hidden");
	oHidden.setAttribute("id", sMultiXmlStructurePageSizeHiddenElement);
	oHidden.setAttribute("name", sMultiXmlStructurePageSizeHiddenElement);
	oHidden.setAttribute("value", lPageSize);
	document.forms[sFormName].appendChild(oHidden);
	
	//------------create a hidden element, to store the page------------
	oHidden = document.createElement("input");
	oHidden.setAttribute("type", "hidden");
	oHidden.setAttribute("id", sMultiXmlStructurePageHiddenElement);
	oHidden.setAttribute("name", sMultiXmlStructurePageHiddenElement);
	oHidden.setAttribute("value", lPage);
	document.forms[sFormName].appendChild(oHidden);
	
	 
	//------------create hidden-field for top-most element name and value----------------
	oTopMostHiddenElement = document.createElement("input")
	oTopMostHiddenElement.setAttribute("type", "hidden");
	oTopMostHiddenElement.setAttribute("id", sTopMostElementName);
	oTopMostHiddenElement.setAttribute("name", sTopMostElementName);
	oTopMostHiddenElement.setAttribute("value", sTopMostElementValue);
	document.forms[sFormName].appendChild(oTopMostHiddenElement);
	
	
	 
	//-------------create default-empty-line on demand--------------------------------
	mAddNewButton(null, sLevelName, sNewButtonCaptionTopElement, true);
	//-----------------render the paging, if paged------------------------------
	mAddPagedControl();
	//--------------load dialog data----------------------
	if (bCallReload)
		gReloadMultiStructure("", true);
	
	

}
//--------------------------MAIN INIT FUNCTION--------------------------------------



function msGetMultiReInit() {

	msGetMultiInitHtml(sMultiStructureParentDiv, sMultiStructureToppestLevelName, 
		sMultiStructureFormName, sMultiStructureInitialSMultiXml, sMultiStructureTopMostElementName, 
		sMultiStructureTopMostElementValue,	sMultiStructureDataPage, sMultiStructureNewButtonCaptionTopElement, false,
		lMultiStructurePageSize, lMultiStructurePage, sMultiStructureOnAfterAskToSave, sMultiStructureRenderAllLevelsPage);

}









//--------------------------TO ReLOAD DIALOG DATA--------------------------------------

function gClearMultiStructure(sId, bCalledFromInit) {

	document.getElementById(sMultiStructureParentDiv).innerHTML = "";
	aoMultiStructs = new Array();
	
	if (bCalledFromInit == false)
		msGetMultiReInit();
	else {
		mAddNewButton(null, sMultiStructureToppestLevelName, sMultiStructureNewButtonCaptionTopElement, true);
		mAddPagedControl();
	}

	lMainCounter = 1;
}

function gReloadMultiStructure(sId, bCalledFromInit) {
			 
	efStatus("Struktur wird vom Server geladen. Bitte warten...");

	if (bCalledFromInit == null) bCalledFromInit = false;
	
	
	//reload the multistructure from a data-page
	gClearMultiStructure(sId, bCalledFromInit);
	
	var sPage = "";
	try {
		sPage = document.forms[sMultiStructureFormName].elements["multistructurepage"].value;
	
	}
	catch (ex) {
	}
	
	var sData = new String(gsCallServerMethod(sMultiStructureDataPage, 
		"<multistructurexml>" + sMultiStructureXml + "</multistructurexml>" + 
		"<topelementname>" + oTopMostHiddenElement.getAttribute("id") + "</topelementname>" +
		"<topelementvalue>" + oTopMostHiddenElement.getAttribute("value") + "</topelementvalue>" + 
		"<page>" + sPage + "</page>"));
	
	if (sData.substring(0, 2 + sMultiStructureLineSep.length) != "OK" + sMultiStructureLineSep) {
	
		try {
			gClearMultiStructure(sId, bCalledFromInit);
		}
		catch (ex) { }
	
		alert(sData);
	
		return;
	}
	
	
	//from database comes any id; this javascript gives own ids;
	//the following array makes a match possible:
	var translation_DataId_ThisJavaScriptID = new Array();   
	
	
	var sLines = sData.split(sMultiStructureLineSep);
	var sCurrentParentId = null;
	var iLiner;
	
	
	//----------each time, when a new level is added, then the level
	//          is rendered on the server and the html is transferred each time
	//          so each time a server connection is established, which costs a lot of 
	//			resources;
	//			the clue now: gather the html for all levels in one webaccess
	//			and store the results in an array, which is passed to the addNewLevel
	//			-function
	var sParamsToAllLevels = "";
	sParamsToAllLevels = 
		"<multixml>" + sMultiStructureXml + "</multixml>" + 
		"<formname>" + sMultiStructureFormName + "</formname>" + 
		"<xmldialogpraefix>" + sMultiStructDlgPraefix + "</xmldialogpraefix>" + 
		"<page>" + sPage + "</page>" + 
		"<data><![CDATA[" + sData + "]]></data>";
	
	//-----------------store the precompiled html into the local html-cache------------------------
	var sAllLevelsHtml = new String(gsCallServerMethod(sMultiStructureRenderAllLevelsPage, sParamsToAllLevels));
	
	if (sAllLevelsHtml.substring(0, 6) != "OK-||-") {
		efMsgBox(sAllLevelsHtml, "ERROR");
	}
	else {
		sAllLevelsHtml = sAllLevelsHtml.substring(6);
	}
		
   	aoMultistructure_PrecompiledHtmlForReload = new Array();
	var allLevelsSplitted = sAllLevelsHtml.split("||||||*****||||||");
	var iPrecompiledLevels = 0;
	for (iPrecompiledLevels = 0; iPrecompiledLevels < allLevelsSplitted.length; iPrecompiledLevels++) {
	
		var sLevelId;
		var sLevelHtml;
		
		sLevelId = allLevelsSplitted[iPrecompiledLevels].split("||||||--------||||||")[0];
		sLevelHtml = allLevelsSplitted[iPrecompiledLevels].split("||||||--------||||||")[1];
		
		var oPrecompiledHtml = new oMultiStruct_PrecompiledHtml(sLevelId, sLevelHtml);
		
		aoMultistructure_PrecompiledHtmlForReload.push(oPrecompiledHtml);
		
	}			   
   			   
   			   
   			   
   			   
   			   
   			   
	for (iLiner = 1; iLiner < sLines.length - 1; iLiner++) {
		
		var sDataColumns = sLines[iLiner].split(sMultiStructureColumnSep);
		
		//-------------get parent--------------------------------
		sCurrentParentId = sDataColumns[1];
		if (sCurrentParentId == "") 
			sCurrentParentId = null;
		else {
		
		}
		
		
		//-----------------add to local translation--------------
		var oField = new sMultiStructureDBJavaSField();
		oField.sDatabase = sDataColumns[2];
		if (! (oField.sDatabase == null || oField.sDatabase == "")) {
			oField.sJavascript = msGetNextLevelId(mMultiStructFindField(translation_DataId_ThisJavaScriptID, sDataColumns[1]), false);
		}
		translation_DataId_ThisJavaScriptID.push(oField);
		
		
		var sParentId;
		if (sDataColumns[1] == null || sDataColumns[1] == "")
			sParentId = null;
		else sParentId = mMultiStructFindField(translation_DataId_ThisJavaScriptID, sDataColumns[1]);
		
		
		mAddNewLevel(sParentId, sDataColumns[0], false);
			
		//---------------fill dialog data------------------------
		
		/* Now done when precompiling the html; otherwise too long
		for (iCols = 3; iCols < sDataColumns.length; iCols+=2 ) {
		
			var sColName = sDataColumns[iCols];
			var sColValue = sDataColumns[iCols + 1];
			
			mSetValueInDialog( sMultiStructureFormName, 
				sColName, sColValue, 0, oField.sJavascript); 
			
		}
		*/
		
		
		efStatus("noch " + new String(sLines.length - iLiner) + " Zeilen...");
	}
	
	
	mReArrangeMultiLevels();
		
	efStatus("Struktur geladen!");
	
}

function mMultiStructFindField(aFields, sDatabaseValue) {

	for (i=0; i < aFields.length; i++) {
	
		if (aFields[i].sDatabase == sDatabaseValue) {
			return aFields[i].sJavascript;
		}
	
	}
	
	return null;
}



function sMultiStructureDBJavaSField() { //constructor

	this.sDatabase = "";
	this.sJavascript = "";
	
	return this;
}

//--------------------------TO ReLOAD DIALOG DATA--------------------------------------











function gMultiStructLevel(sParentId, sLevelName, sLevelId, lHeight, sWidth, sSortField,
	sIconMoveUp, sIconMoveDown) { //constructor

	//properties:
	this.sLevelName = sLevelName;
	this.sLevelId = sLevelId;
	this.sParentId = sParentId;
	if (this.sParentId == "") this.sParentId = null;
	this.lHeight = lHeight;
	this.sWidth = sWidth;
	this.bCollapsed = true;
	this.aoChildren = new Array();
	this.bDeleted = false;
	this.sSortField = sSortField;
	this.sIconMoveUp = sIconMoveUp;
	this.sIconMoveDown = sIconMoveDown;
	this.lSortNumber = 99999; //append behind
	
	
	//methods:
	this.gExpand = multistruct_Expand;
	this.gCollapse = multistruct_Collapse;
	this.glRelevantHeight = multistruct_glRelevantHeight;
	this.gArrange = multistruct_Arrange;
	this.glHeightOfPreviousSiblings	= multistruct_lHeightOfPreviousSiblings;
	this.gbHeightIsRelevant = multistruct_gbHeightIsRelevant;
	this.gaoGetPreviousSiblings = multistruct_gaoGetPreviousSiblings;
	this.toggleCollapse = multistruct_toggleCollapse;
	this.internalHide = multistruct_internalHide;
	this.internalShow = multistruct_internalShow;
	this.goGetNeighbourLevelBySortNumber = multistruct_goGetNeighbourLevelBySortNumber;
	
	
	
	//-----------add to children of parent--------
	if (sParentId != null &&  sParentId != "") {
		var oParentLevel = goFindMultiStructLevel(sParentId);
		if (oParentLevel != null) 
			oParentLevel.aoChildren.push(this);
	}
	
	
	aoMultiStructs.push(this);
	
	
	return this;
}

function goFindMultiStructLevel(sLevelId) {

	for (i=0; i < aoMultiStructs.length; i++) {
		if (aoMultiStructs[i].sLevelId == sLevelId) {
			return aoMultiStructs[i];
		}
	
	}

	alert("Multilevel \""+ sLevelId + "\" not found!");

}

function mReArrangeMultiLevels() {

	var iMultiLevel = 0;
	
	for (iMultiLevel=0; iMultiLevel < aoMultiStructs.length; iMultiLevel++) {
	
		aoMultiStructs[iMultiLevel].gArrange();
	
	
	}
	

}



function msGetNextLevelId(sParentLevel, bIncrease) {
	
	if (bIncrease == null) bIncrease = true;
	
	var sResult;
	
	
	//if it is a new root, then get the next id from lMainCounter and increase lMainCounter
	if (sParentLevel == null || sParentLevel == "") {
		
		sResult = sLevelPraefix + lMainCounter + "_";
		if (bIncrease) lMainCounter += 1;
		
		
	}
	else {
			 
		//otherwise search the parent and get the last children of this parent:
	
		var oLevel = goFindMultiStructLevel(sParentLevel);
	   
		if (oLevel==null) return;
		var oLastChild = oGetLastChildrenOfParent(sParentLevel);
		if (oLastChild == null) {
		
			//start with 1:
			sResult = sParentLevel + "1_";
		}
		else {
			
			//get next number:
			var sLastChildId = oLastChild.sLevelId;
			
			sLastChildId = sLastChildId.split("_")[sLastChildId.split("_").length-2];
			
			var lLastChildNumber = 1 + Number(sLastChildId);
			
			sResult = sParentLevel + String(lLastChildNumber) + "_";			
			
		}
		
				
	}
	
	return sResult;

}

function oGetLastChildrenOfParent(sParentId) {
    
	for (i=aoMultiStructs.length - 1; i >= 0; i--) {
		if (aoMultiStructs[i].sParentId == sParentId) {
		
			return aoMultiStructs[i];
		}
	
	}
	

}

function mAddNewButton(sParentId, sLevelName, sNewButtonCaption,
	bForLevelsOnSameHierarchy,
	bFetchOptionValuesFromAsp, 
	sOptionDialogText, sOptionDialogValues, sOptionDialogCaptions) {
	
	//if bForLevelsOnSameHierarchy is true, then the new-button creates levels on the same;
	//otherwise the new-button assumes, that it creates sub-ordinated levels
																  

	var oParentDiv;
	var sFunc;	
	var oNewDiv;
	var sParentAsString;
	
	if (sParentId == "" || sParentId == null ) {
		oParentDiv = document.getElementById(sMultiStructureParentDiv);	
	}
	else {									   
		oParentDiv = document.getElementById(sParentId+sSubContentSuffix);	
	}
	
	if (bFetchOptionValuesFromAsp == null) bFetchOptionValuesFromAsp = true;
	
	if (bFetchOptionValuesFromAsp) {
	
		var oOptions = moGetNewButtonOptionsFromAsp(sLevelName, bForLevelsOnSameHierarchy);
		sOptionDialogText = oOptions.sOptionText;
		sOptionDialogValues = oOptions.sOptionValues;
		sOptionDialogCaptions = oOptions.sOptionCaptions
	
	}	
	
	if (sParentId == null) sParentAsString = "";
	else sParentAsString = sParentId;
	
	sFunc = msGetFuncStringToAddNewLevel(sParentAsString, sLevelName, 
		sOptionDialogText, sOptionDialogValues, sOptionDialogCaptions);
	
	var sNewLevelId = msGetNextLevelId(sParentId, false);
	
	
	oNewDiv = document.createElement("div");
	oNewDiv.setAttribute("id", sLevelName + sOptionButtonSuffix);
	oNewDiv.style.position = "absolute";
	oNewDiv.style.left = lDetailsButtonDivWidth;
	oNewDiv.style.width = 200;
	oNewDiv.style.height = lButtonDivHeight;
	if (sParentAsString == "")
		oNewDiv.style.top= multistruct_lGetTopOffset() + "px";
	else
		oNewDiv.style.top= "0px";
	oNewDiv.style.overflow = "visible";
	
	oNewDiv.innerHTML = msMultiStructOptionsButton(sNewLevelId, sFunc, sNewButtonCaption, false);
	oParentDiv.appendChild(oNewDiv);
		

}

function mAddNewLevel(sParentId, sLevelName, bRealign) {
 //adds a new level on the same hierarchy stage by calling the aspToGetMultiHtml-aspx
 //and inserting the html 
 
	//alert("calling mAddNewLevel with sParentId="+sParentId+";sLeveName="+sLevelName+";bOnlyNewButton="+bOnlyNewButton+";sNewButtonCaption="+sNewButtonCaption);
	
	var URL = aspToGetMultiHtml();
	
	var sLevelId;
	
	
	//---------------apply level-id to namepraefix and dlg-id---------------
	var sNamepraefix;
	var sXmlDialogId; 
	var oParentDiv;
	var sFunc;	
	var oNewDiv;
	var sParentAsString;
			
	
	sLevelId = msGetNextLevelId(sParentId);
	sNamepraefix = sLevelId; 
	sXmlDialogId = sMultiStructDlgPraefix + sLevelId;
	

	
	//---------------render on server and get result -----------------------
	URL += "?namepraefix=" + sNamepraefix + "&xmldialogid="+sXmlDialogId + 
		"&formname="+sMultiStructureFormName+"&multixml="+sMultiStructureXml+"&startlevel="+sLevelName;
	
	//-----------first look in the precompiled-array, if the html isn't compiled already-----------
	var iPCC = 0;
	var sHtml = "";
	
	for (iPCC = 0; iPCC < aoMultistructure_PrecompiledHtmlForReload.length; iPCC++) {
		var oPrecompiled = aoMultistructure_PrecompiledHtmlForReload[iPCC];
		
		if (oPrecompiled.sLevelId == sLevelId)  {
			sHtml = oPrecompiled.sHtml;
			break;	
		}
			
	}
	if (sHtml == "") {
		sHtml = gsCallServerMethod(URL, "", "");
	}
	
	
	//-------------get values from html-string--------------------
	var sHtmlLevel = sHtml;
	var lLastIndex = 4;
	
	if (sHtmlLevel.substring(0,6) != "OK-||-") {
		efMsgBox(sHtmlLevel, "ERROR");
		return
	}
	else sHtmlLevel = sHtmlLevel.substring(6);
	
	var sNewLevelId = sHtmlLevel.split(";")[0]
	var sLevelName = sHtmlLevel.split(";")[1];
	var sLevelHeight = sHtmlLevel.split(";")[2];
	var sLevelWidth = sHtmlLevel.split(";")[3];
	var sNewButtonCaption = sHtmlLevel.split(";")[4];
	var sOptionDialogText = sHtmlLevel.split(";")[5];
	var sOptionDialogValues = sHtmlLevel.split(";")[6];
	var sOptionDialogCaptions = sHtmlLevel.split(";")[7];
	var sNextSubLevelName = sHtmlLevel.split(";")[8];
	var sNextSubLevelNewButtonCaption = sHtmlLevel.split(";")[9];
	var sNextSubLevelOptionDialogText = sHtmlLevel.split(";")[10];
	var sNextSubLevelOptionDialogValues = sHtmlLevel.split(";")[11];
	var sNextSubLevelOptionDialogCaptions = sHtmlLevel.split(";")[12];
	var sSortField = sHtmlLevel.split(";")[13];
	var sIconMoveUp = sHtmlLevel.split(";")[14];
	var sIconMoveDown = sHtmlLevel.split(";")[15];
	lLastIndex = 15; //don't forget to set!!
		
	var sLevelHtml = sHtmlLevel;
	for (i=0; i <= lLastIndex; i++) {
		sLevelHtml = sLevelHtml.substring(sLevelHtml.indexOf(";") + 1);
	}
	
	
	//create level-object
	var oNewLevel = new gMultiStructLevel(sParentId, sLevelName, sNewLevelId, sLevelHeight, 
		sLevelWidth, sSortField, sIconMoveUp, sIconMoveDown);	  

	//---------------get html-parent-div---------
	if (sParentId == "" || sParentId == null ) {
		oParentDiv = document.getElementById(sMultiStructureParentDiv);	
	}
	else {									   
		oParentDiv = document.getElementById(sParentId+sSubContentSuffix);	
	}

	//---------create a new div and insert---------------
	//the main div of this level, which contains the sub-divs: detailsbuttons, optionbuttons, xmlhtml and subelements:
	
	var oLevelDiv = document.createElement("div");
	oLevelDiv.setAttribute("id", sNewLevelId);
	oLevelDiv.style.position = "absolute";
	oLevelDiv.style.height = sLevelHeight;
	oLevelDiv.style.left = lDetailsButtonDivWidth;
	oLevelDiv.style.top = 0;
	oLevelDiv.style.width = sLevelWidth;
	oLevelDiv.style.overflow = "visible";
	oParentDiv.appendChild(oLevelDiv);
	
	
	//--------------set the sortfield-value; calculated by count of neighbours--------------
	var aoPrecedingLevels = oNewLevel.gaoGetPreviousSiblings();
	var iNeighbour = 0;
	var iNextSortNumber = -1;
	for (v = 0; iNeighbour < aoPrecedingLevels.length; iNeighbour++) {
		if (aoPrecedingLevels[iNeighbour].lSortNumber > iNextSortNumber) 
			iNextSortNumber = aoPrecedingLevels[iNeighbour].lSortNumber;
	}
	iNextSortNumber++;
	
	oNewLevel.lSortNumber = Number(iNextSortNumber);
	
	//-----------------append hidden field for sort-column----------------------
	var oInput = document.createElement("input");
	oInput.setAttribute("type", "hidden");
	oInput.setAttribute("name", multistruct_sGetSortHiddenFieldName(oNewLevel.sLevelId));
	oInput.setAttribute("id", multistruct_sGetSortHiddenFieldName(oNewLevel.sLevelId));
	oInput.setAttribute("value", iNextSortNumber);
	oLevelDiv.appendChild(oInput);
	
	
	
	//------------now render the sub-divs of a level div------------------------
																		
	
	//small ...-button to collapse:
	var sThreePointClick = "goFindMultiStructLevel('" + sNewLevelId + "').toggleCollapse();";
	oNewDiv = document.createElement("div");
	oNewDiv.setAttribute("id",  sNewLevelId + sDetailsButtonSuffix);
	oNewDiv.style.position = "absolute";
	oNewDiv.style.left = 0;
	oNewDiv.style.width = lDetailsButtonDivWidth;
	oNewDiv.style.position = "absolute";
	oNewDiv.style.overflow = "hidden";
	oNewDiv.innerHTML = "<button id=\"" + sNewLevelId + sCollapseBtnSuffix + "\"type=\"button\" class=\"cmdButtonSmall\" onclick=\""+sThreePointClick+"\">+</button>"
	oNewDiv.innerHTML += "<img src=\"" + oNewLevel.sIconMoveUp + "\" onclick=\"multistruct_moveup('" + oNewLevel.sLevelId + "');\" />";
	oNewDiv.innerHTML += "<img src=\"" + oNewLevel.sIconMoveDown + "\" onclick=\"multistruct_movedown('" + oNewLevel.sLevelId + "');\" />";
	oLevelDiv.appendChild(oNewDiv);
	
	//the xml-dialog html:
	oNewDiv = document.createElement("div");
	oNewDiv.setAttribute("id",  sNewLevelId + sXmlDialogHtmlSuffix);
	oNewDiv.style.position = "absolute";
	oNewDiv.style.left = lDetailsButtonDivWidth;
	oNewDiv.style.width = sLevelWidth;
	oNewDiv.style.height = sLevelHeight;
	oNewDiv.style.top= 0;
	oNewDiv.style.overflow = "hidden";
	oNewDiv.innerHTML = sLevelHtml;
	oLevelDiv.appendChild(oNewDiv);
	
	//get the javascript out of the inserted-html's textarea
	//and insert that code as javascript:
	var oJavascriptNode = document.createElement("script");
	var oTextAreaJavascript = document.getElementById(sXmlDialogId + "_javascripts");
	if (oTextAreaJavascript != null) {
		oJavascriptNode.text = oTextAreaJavascript.innerHTML;
		oTextAreaJavascript.parentNode.appendChild(oJavascriptNode);
	}
	
	//the option-buttons:
	if (sParentId == null) sParentAsString = "";
	else sParentAsString = sParentId;
	
	sFunc = msGetFuncStringToAddNewLevel(sParentAsString, sLevelName,   
		sOptionDialogText, sOptionDialogValues, sOptionDialogCaptions);

	oNewDiv = document.createElement("div");
	oNewDiv.setAttribute("id", sNewLevelId + sOptionButtonSuffix);
	oNewDiv.style.position = "absolute";
	oNewDiv.style.left = lDetailsButtonDivWidth;
	oNewDiv.style.width = sLevelWidth;
	oNewDiv.style.height = lButtonDivHeight;
	oNewDiv.style.top= sLevelHeight;
	oNewDiv.style.overflow = "hidden";
	oNewDiv.innerHTML = msMultiStructOptionsButton(sNewLevelId, sFunc, sNewButtonCaption, true);
	oLevelDiv.appendChild(oNewDiv);
	
 	//the sub-content area:
 	var lSubContentTop;
 	lSubContentTop = Number(sLevelHeight.replace("px","").replace("%","")) + Number(lButtonDivHeight);
	
	oNewDiv = document.createElement("div");
	oNewDiv.setAttribute("id", sNewLevelId + sSubContentSuffix);
	oNewDiv.style.position = "absolute";
	oNewDiv.style.left = lDetailsButtonDivWidth;
	oNewDiv.style.width = sLevelWidth;
	oNewDiv.style.height = lButtonDivHeight;
	oNewDiv.style.top= lSubContentTop;
	oNewDiv.style.overflow = "visible";
	oLevelDiv.appendChild(oNewDiv);
	
	
	//collpase per default:
	//goFindMultiStructLevel(sNewLevelId).gCollapse(); //do not use false, because IE displays small lines of xml-dialogs
	
	//add a new-button of sub-hierarchical element:
	mAddNewButton(sNewLevelId, sNextSubLevelName, sNextSubLevelNewButtonCaption, false, false, 
		sNextSubLevelOptionDialogText, sNextSubLevelOptionDialogValues, 
		sNextSubLevelOptionDialogCaptions);
	
	//----------------display the level ------------------------------------
	goFindMultiStructLevel(sLevelId).gCollapse(bRealign);
	
}

function msHandleNewOptionButtonClick(sParentId) {
	
	mAddNewLevel(sParentId, new String(oModalResult), true);
	
	
}

function msGetFuncStringToAddNewLevel(sParentId, sActualLevelName,
	sOptionText, sOptionValues, sOptionCaptions) {
	
	
	var sResult;
	
	if (sOptionValues == null || sOptionValues == "")  {
		sResult = "mAddNewLevel('" + sParentId + "','"+sActualLevelName+"', true);";
	}
	else {
	
		
		var sCallAfterwards = "msHandleNewOptionButtonClick(efApostroph" + sParentId + "efApostroph);";
		
	
		sResult = "gShowStringOptionDialog('" + sOptionText + "','" + sOptionValues + "'" + 
					",'" + sOptionCaptions + "','" + sCallAfterwards + "');";
	
	}
	
	
	return sResult

}

function multistruct_RemoveLevelInputBoxes(sLevelId, bIsRecursiveCall) {

	if (bIsRecursiveCall == null) bIsRecursiveCall = false;

	//collapse before:
	goFindMultiStructLevel(sLevelId).gCollapse();

	//hide the div
	gHideDiv(document.getElementById(sLevelId));
	
	//set the deleted flag
	var oLevel = goFindMultiStructLevel(sLevelId);
	oLevel.bDeleted = true;
	
	
	//insert a hidden element, which says, that level x_x_x has been deleted:
	var oDeletedFlag = document.createElement("input");
	oDeletedFlag.setAttribute("id", sLevelId + sMultiStructureElementDeleteFlag);
	oDeletedFlag.setAttribute("name", sLevelId + sMultiStructureElementDeleteFlag);
	oDeletedFlag.setAttribute("value", "1");
	oDeletedFlag.setAttribute("type", "hidden");
	document.getElementById(sLevelId).appendChild(oDeletedFlag);
	
	//set each children to deleted
	var i = 0;
	for (i=0; i < oLevel.aoChildren.length; i++) {
		multistruct_RemoveLevelInputBoxes(oLevel.aoChildren[i].sLevelId, true);
	
	}
	
	if (bIsRecursiveCall == false)
		mReArrangeMultiLevels();

}

function moGetNewButtonOptionsFromAsp(sStartLevel, bForLevelOnSameHierarchy) {


	var sLevelHierarchy;
	if (bForLevelOnSameHierarchy == true) 
		sLevelHierarchy = "this";
	else
		sLevelHierarchy = "sub";

	var sParams = "<levelhierarchy>" + sLevelHierarchy + "</levelhierarchy>" +
			"<multixml>"+sMultiStructureXml+"</multixml>" + 
			"<startlevel>"+sStartLevel+"</startlevel>";

	var sResult = gsCallServerMethod(aspToGetNewButtonOptions(), sParams);	

	if (sResult.substring(0,7) != "SUCCESS") {
		efMsgBox(sResult, "ERROR");
		return
	}
	
	sResult = sResult.substring(7);
	

	//first the option-text:
	this.sOptionText = sResult.split(";")[0];

	//then the option-values:
	this.sOptionValues = sResult.split(";")[1];

	//then the option-captions:
	this.sOptionCaptions = sResult.split(";")[2];

	return this;
}


function msMultiStructOptionsButton(sLevelId, sFunc, sNewButtonCaption, bDelete) {
//renders all option-buttons

	if (bDelete == null) bDelete = false;
	var sResult = "";
	
	if (sNewButtonCaption != null && sNewButtonCaption != "")  {

		sResult += "<button type=\"button\" onclick=\""+sFunc+"\">"+sNewButtonCaption+"</button>";

	}
	
	if (bDelete == true)  {
		
		sResult += "&nbsp;&nbsp;&nbsp;<button type=\"button\" onclick=\"multistruct_RemoveLevelInputBoxes('"+sLevelId+"')\">Entfernen</button>";


	}
		

	return sResult;
}

function multistruct_Expand(bRealign) {

	this.bCollapsed = false;
	this.internalShow(true);
	
	if (bRealign == false) {
	}
	else
	mReArrangeMultiLevels();
}

function multistruct_Collapse(bRealign) {

	this.bCollapsed = true;
	this.internalHide(true);
	if (bRealign == false) {
	}
	else
	mReArrangeMultiLevels();
}

function multistruct_internalHide(bWithChildren) {
	var oDiv = document.getElementById(this.sLevelId + sSubContentSuffix);
	gHideDiv(oDiv);
	
	if (bWithChildren) 
		var i=0;
		for (i=0; i < this.aoChildren.length; i++) {
			this.aoChildren[i].internalHide(bWithChildren);
		}
	
}

function multistruct_internalShow(bWithChildren) {
	var oDiv = document.getElementById(this.sLevelId + sSubContentSuffix);
	
	if (this.bCollapsed == false)
		gShowDiv(oDiv);

	if (bWithChildren) 
		var i=0;
		for (i=0; i < this.aoChildren.length; i++) {
			this.aoChildren[i].internalShow(bWithChildren);
		}
	
}

function multistruct_gbHeightIsRelevant() {
	//returns true/false in despite of the parents
	
	var bResult;
	
	if (this.bDeleted == true) {
		bResult = false;
		
	}
	else {
	
		if (this.sParent == null)
			bResult = true;
		else {
			var oParent = goFindMultiStructLevel(this.sParentId);
			
			if (oParent.bCollapsed == true) {
				bResult = false;
			}
			else {
				bResult = oParent.gbHeightIsRelevant();
			}
		}
	
	}
		
	return bResult;
}

function multistruct_glRelevantHeight() {


	var lResult = 0;
	if (this.gbHeightIsRelevant() == true) {
	
		lResult += Number(this.lHeight); //height of xml-dialog
		lResult += Number(lButtonDivHeight); //height of option-buttons:
		
		if (this.bCollapsed == false) {
			var i;
			lResult += Number(lButtonDivHeight); //height of option-buttons:
			for (i=0; i < this.aoChildren.length; i++) {
				lResult += Number(this.aoChildren[i].glRelevantHeight());
			}
		}	
	}
	
	
	return lResult;
	
}

function multistruct_gaoGetPreviousSiblings() {

	var aoResult = new Array();
	
	if (this.sParentId == null || this.sParentId == "") {
	
		for (i=0; i < aoMultiStructs.length; i ++) {
			if (aoMultiStructs[i].sParentId == null || aoMultiStructs[i].sParentId == "") {
				if (aoMultiStructs[i].lSortNumber < this.lSortNumber)
					aoResult.push(aoMultiStructs[i]);
			}
		}
	
	}
	else {
	
		var oParentLevel = goFindMultiStructLevel(this.sParentId);
		
		var i = 0;
		for (i = 0; i < oParentLevel.aoChildren.length; i++) {
		
				
			if (oParentLevel.aoChildren[i].lSortNumber < this.lSortNumber)
				aoResult.push(oParentLevel.aoChildren[i]);
		
		}
	}
	
	return aoResult;

}


function multistruct_lHeightOfPreviousSiblings() {

	var aoPrecedingLevels = this.gaoGetPreviousSiblings();

	var lHeight = 0;
	var i = 0;
		
	for (i=0; i < aoPrecedingLevels.length; i++) {
		if (aoPrecedingLevels[i] == this) break;
		lHeight += aoPrecedingLevels[i].glRelevantHeight();	
	}
	return lHeight;
	
}

function multistruct_Arrange() {


	var lHeightOfPreviousSiblings = this.glHeightOfPreviousSiblings();
	
	var oDiv = document.getElementById(this.sLevelId);
	
	//----if it is paged, then there is a basic offset-----------
	var lOffset = 0;
	if (this.sParentId == null || this.sParentId == "")
		lOffset += multistruct_lGetTopOffset();
	
	
	oDiv.style.top = lOffset + lButtonDivHeight + lHeightOfPreviousSiblings + "px";

	
}

function multistruct_toggleCollapse() {

	var oDiv = document.getElementById(this.sLevelId + sSubContentSuffix); 
	var oBtn = document.getElementById(this.sLevelId + sCollapseBtnSuffix);
	
	if (this.bCollapsed) {
		this.gExpand();
	}
	else {
		this.gCollapse();
	}
	

}

function multistruct_goGetNeighbourLevelBySortNumber(lSortNumber) {

	if (this.sParentId == null || this.sParentId == "") {
	
		for (i=0; i < aoMultiStructs.length; i ++) {
			if (aoMultiStructs[i].sParentId == null || aoMultiStructs[i].sParentId == "") {
				if (aoMultiStructs[i].lSortNumber == lSortNumber) {
					return aoMultiStructs[i];
				}
			}
		}
	
	}
	else {
	
		var oParentLevel = goFindMultiStructLevel(this.sParentId);
		
		var i = 0;
		for (i = 0; i < oParentLevel.aoChildren.length; i++) {
		
			if (oParentLevel.aoChildren[i].lSortNumber == lSortNumber) 
				return oParentLevel.aoChildren[i];		
		}
	}
	
	 
}

function multistruct_movedown(sLevelId) {

	var oLevel = goFindMultiStructLevel(sLevelId);
	var lOldSortNumber = oLevel.lSortNumber;
	
	//get the level with the new sort-number:	
	var oOtherLevel = oLevel.goGetNeighbourLevelBySortNumber(lOldSortNumber + 1);
	if (oOtherLevel != null) {
		oOtherLevel.lSortNumber --;
		oLevel.lSortNumber ++;

		//update hidden-fields:
		document.forms[sMultiStructureFormName].elements[multistruct_sGetSortHiddenFieldName(oOtherLevel.sLevelId)].value =oOtherLevel.lSortNumber;
		document.forms[sMultiStructureFormName].elements[multistruct_sGetSortHiddenFieldName(oLevel.sLevelId)].value =oLevel.lSortNumber;
	}
	
	mReArrangeMultiLevels();

}


function multistruct_moveup(sLevelId) {

	var oLevel = goFindMultiStructLevel(sLevelId);
	var lOldSortNumber = oLevel.lSortNumber;
	
	//get the level with the new sort-number:	
	var oOtherLevel = oLevel.goGetNeighbourLevelBySortNumber(lOldSortNumber - 1);
	if (oOtherLevel != null) {
		oOtherLevel.lSortNumber ++;
		oLevel.lSortNumber --;
		
		//update hidden-fields:
		document.forms[sMultiStructureFormName].elements[multistruct_sGetSortHiddenFieldName(oOtherLevel.sLevelId)].value =oOtherLevel.lSortNumber;
		document.forms[sMultiStructureFormName].elements[multistruct_sGetSortHiddenFieldName(oLevel.sLevelId)].value =oLevel.lSortNumber;
		
	}
	
	mReArrangeMultiLevels();
}

function multistruct_sGetSortHiddenFieldName(sLevelId) {
	//returns the name of the hidden-field, which contains the sort-value:
	return  sLevelId + "__sortvalue";
	
}	

//-----------------render the paging, if paged------------------------------
function mAddPagedControl() {

	if (lMultiStructurePageSize > 0) {
		
		var oDivPageInfo = document.createElement("div");
		oDivPageInfo.style.position = "absolute";
		oDivPageInfo.style.height = 40;
		oDivPageInfo.style.left = 50;
		oDivPageInfo.style.top = 0;
		oDivPageInfo.style.width = 400;
		oDivPageInfo.style.overflow = "visible";
		
		var oBtn = document.createElement("button");
		oBtn.setAttribute("type", "button");
		oBtn.setAttribute("onclick", "multistruct_prevPage();");
		oBtn.setAttribute("accesskey", "z");
		oBtn.innerHTML = "<u>Z</u>urück";
		oDivPageInfo.appendChild(oBtn);
		
		var oPagedTextBox = document.createElement("input");
		oPagedTextBox.setAttribute("type", "text");
		oPagedTextBox.setAttribute("size", 4);
		oPagedTextBox.setAttribute("value", lMultiStructurePage);
		oPagedTextBox.setAttribute("name", "multistructurepage");
		oPagedTextBox.setAttribute("onblur", "multistruct_pageBlur();"); 
		oDivPageInfo.appendChild(oPagedTextBox);
	
		oBtn = document.createElement("button");
		oBtn.setAttribute("type", "button");
		oBtn.setAttribute("onclick", "multistruct_nextPage();");
		oBtn.setAttribute("accesskey", "w");
		oBtn.innerHTML = "<u>W</u>eiter";
		oDivPageInfo.appendChild(oBtn);
		
		
		document.getElementById(sMultiStructureParentDiv).appendChild(oDivPageInfo);
		
	}

}

function multistruct_lGetTopOffset() {

	var lResult;
	
	if (lMultiStructurePageSize > 0) 
		lResult = 40;
	else lResult= 0;
	
	return lResult;

}

function multistruct_nextPage() {

    var oCtl = document.forms[sMultiStructureFormName].elements["multistructurepage"];
	var lPage = Number(oCtl.value);
	
	if (multistruct_askToSave() == false) 
		return;
		
	if (isNaN(lPage)) {
		//if number is invalid, then set page = 1:
		lPage = lMultiStructurePage;
		oCtl.value = lPage;
	}
	else {
		lPage++;
	}

	if (lMultiStructurePage != lPage) {
		lMultiStructurePage = lPage;
		oCtl.value = lPage;

		gReloadMultiStructure("", false);
	}

}

function multistruct_prevPage() {

    var oCtl = document.forms[sMultiStructureFormName].elements["multistructurepage"];
	var lPage = Number(oCtl.value);
	
	if (multistruct_askToSave() == false) 
		return;
		
	if (isNaN(lPage)) {
		//if number is invalid, then set page = 1:
		lPage = lMultiStructurePage;
		oCtl.value = lPage;
	}
	else {
		if (lPage > 1) 
			lPage--;
		else
			return;
	}

	if (lMultiStructurePage != lPage) {
		lMultiStructurePage = lPage;
		oCtl.value = lPage;

		gReloadMultiStructure("", false);
	}
}

function multistruct_askToSave() {

	if (multistruct_IsDirty() == true) {
	
		var sDlgResult = efMsgBox("Änderungen zuvor speichern?", "QUESTION", "YESNOCANCEL");
		
		if (sDlgResult == "YES") {
		
			if (sMultiStructureOnAfterAskToSave == null) {
				efMsgBox("Event sOnAfterAskToSave has to be defined of multistructure!", "ERROR");
				return 
			}
			var bResult = sMultiStructureOnAfterAskToSave();
			
			if (bResult == false)
				return false;
				
			return true;
		}
		else if (sDlgResult == "CANCEL")
			return false;
		else if (sDlgResult == "NO")
			return true;
	
	
	}

}

function multistruct_pageBlur() {

    var oCtl = document.forms[sMultiStructureFormName].elements["multistructurepage"];
	var lPage = Number(oCtl.value);

	if (multistruct_askToSave() == false) 
		return;
		
		
	if (isNaN(lPage)) {
		//if number is invalid, then set page = 1:
		lPage = lMultiStructurePage;
		oCtl.value = lPage;
	}
	
	if (lMultiStructurePage != lPage) {
	
		lMultiStructurePage = lPage;
		oCtl.value = lPage;

		gReloadMultiStructure("", false);
	}
}

function multistruct_IsDirty() {
//checks, wether any multistructure is dirty or not

	var i = 0;
	for (i=0; i < aoMultiStructs.length; i++) {
		var sXmlDialogId = sMultiStructDlgPraefix + aoMultiStructs[i].sLevelId;

		if (gbXmlDialogGetDirty(sXmlDialogId))
			return true;
	}
	
	return false;

}


function oMultiStruct_PrecompiledHtml(sLevelId, sHtml) { //constructor

	this.sLevelId = sLevelId;
	this.sHtml = sHtml;
	
	
	return this;

}

function aspToGetMultiHtml() {
	return gsGetWebPageRoot() + "/asp/system/multistructure/ondemandrender/renderlevel.aspx";
}

function aspToGetNewButtonOptions() {
	return gsGetWebPageRoot() + "/asp/system/multistructure/ondemandrender/newbuttonOptions.aspx";
}

