/* (c) Promain Software-Betreuung GmbH 2004 */
	
//---------standard-----------------

function efStatus(sStatusString) {
	//display status
	
	window.status = sStatusString;

}

function getSearchParam(sName) {
			
	
	//extracts per javascript an url-parameter
	var t = location.search;					 
	if (t == "") return "";
	var sSearchFor = sName.toLowerCase() + "=";
	if (t.toLowerCase().indexOf(sSearchFor) == -1) return "";
	t = t.substring(t.toLowerCase().indexOf(sSearchFor), t.length)
	t = t.substring(sSearchFor.length, t.length);
	if (t.indexOf("&") >= 0)
		t = t.substring(0, t.indexOf("&"));
		
	return t;
	 
}	 

function glScreenWidth()
{
	return screen.width;
}

function glScreenHeight()
{
	return screen.height;
}

function efPrompt(sPrompt, sDefaultValue) {

	return prompt(sPrompt, sDefaultValue);
}

function efMsgBox(sText, sType, sButtons, sTitle) {
//displays a messagebox; possible values for sType are "INFO", "ERROR", "WARNING", "EXCLAMATION
//sButtons: "YESNOCANCEL", "OKCANCEL", null, ""
//good info at: http://www.webreference.com/dhtml/column22/js-vbMsgBox.html

	if (sButtons==null || sButtons == "") {
		if (bIsIE()) {
			sButtons = "OK"; //default OK
			efIEMsgBox(sText, sType, sButtons, sTitle);
		}
		else alert(sText);
	}
	else {

		if (bIsIE()) {
			if (sButtons == null) sButtons = 0;
			if (sTitle == null) sTitle = "";
			
			
			var sResult = efIEMsgBox(sText, sType, sButtons, sTitle);
			return sResult;
			
		}

		if (sButtons =="YESNOCANCEL") {
			
			var bResult = confirm(sText);
			
			if (bResult == true) 
				return "YES";
			
			if (bResult == false) 
				return "NO"
			
		}
			
		if (sButtons =="YESNO") {
			
			var bResult = confirm(sText);
			
			if (bResult == true) 
				return "YES";
			
			if (bResult == false) 
				return "NO"
			
		}
			
		if (sButtons =="OKCANCEL") {
			
			var bResult = confirm(sText);
			
			if (bResult == true) 
				return "OK";
			
			if (bResult == false) 
				return "CANCEL"
			
		}
			
	}

}

function bConfirm(sText, sType) {
	//sType: "YESNO", "OKABORT"
	
	return (confirm(sText));
	

}


function bIsNetscape() {
	if (navigator.appName == "Netscape")
		return true;
	else
		return false;

}

function bIsIE() {
	if (navigator.appName.indexOf("Internet") >= 0)
		return true;	  
	else
		return false;

}

function bSupportsActiveX() {

	try {
	
		var o = new ActiveXObject("Microsoft.XMLHTTP");
	}
	catch (ex) {
	
		return false;
	}			
	
	
	
	
	return true;

}

function bIsWindows() {

	if (navigator.platform == "Win32") 
		return true;
	else
		return false;
}


function gTranslateIntoEntities(toTranslate) {

	var sResult = new String(toTranslate);
	sResult = sResult.replace("ä","&auml;");
	sResult = sResult.replace("ö","&ouml;");
	sResult = sResult.replace("ü","&uuml;");
	sResult = sResult.replace("Ä","&Auml;");
	sResult = sResult.replace("Ö","&Ouml;");
	sResult = sResult.replace("Ü","&Uuml;");

	
	return sResult;

}


function gHideDiv(oDiv) {

 oDiv.style.visibility = "hidden";
 
}

function gShowDiv(oDiv) {
				
 oDiv.style.visibility = "visible";
 
}


//--------------register global events----------------


	//-------register functions------------------------
	function gAdd_DocumentMouseDown(theFunc, bAddBefore) {
		if (bAddBefore == null) bAddBefore=false;
		if (theFunc != null)	{
			if (bAddBefore)
				mAddInFrontOfArray(aoRegisteredOnDocumentMouseDown, theFunc)
			else
				aoRegisteredOnDocumentMouseDown.push(theFunc);
		}
		
	}
	function gRemove_DocumentMouseDown(theFunc) {
		for (i=0; i < aoRegisteredOnDocumentMouseDown.length; i++) {
			if (aoRegisteredOnDocumentMouseDown[i] == theFunc) {
				aoRegisteredOnDocumentMouseDown[i] = null;
			}
		}
	}
	function gAdd_DocumentMouseMove(theFunc, bAddBefore) {
		if (bAddBefore == null) bAddBefore=false;
		if (theFunc != null)	{
			if (bAddBefore)
				mAddInFrontOfArray(aoRegisteredOnDocumentMouseMove, theFunc)
			else
				aoRegisteredOnDocumentMouseMove.push(theFunc);
		}
	}
	function gRemove_DocumentMouseMove(theFunc) {
		for (i=0; i < aoRegisteredOnDocumentMouseMove.length; i++) {
			if (aoRegisteredOnDocumentMouseMove[i] == theFunc) {
				aoRegisteredOnDocumentMouseMove[i] = null;
			}
		}
	}

	function gAdd_DocumentKeyPress(theFunc, bAddBefore) {
		if (bAddBefore == null) bAddBefore=false;
		if (theFunc != null)	{
			if (bAddBefore)
				mAddInFrontOfArray(aoRegisteredOnDocumentKeyPress, theFunc)
			else
				aoRegisteredOnDocumentKeyPress.push(theFunc);
		}
	}
	function gRemove_DocumentKeyPress(theFunc) {
		for (i=0; i < aoRegisteredOnDocumentKeyPress.length; i++) {
			if (aoRegisteredOnDocumentKeyPress[i] == theFunc) {
				aoRegisteredOnDocumentKeyPress[i] = null;
			}
		}
	}
	function gAdd_DocumentResize(theFunc, bAddBefore) {
		if (bAddBefore == null) bAddBefore=false;
		if (theFunc != null)	{
			if (bAddBefore)
				mAddInFrontOfArray(aoRegisteredOnDocumentResize, theFunc)
			else
				aoRegisteredOnDocumentResize.push(theFunc);
		}
	}
	function gRemove_DocumentResize(theFunc) {
		for (i=0; i < aoRegisteredOnDocumentResize.length; i++) {
			if (aoRegisteredOnDocumentResize[i] == theFunc) {
				aoRegisteredOnDocumentResize[i] = null;
			}
		}
	}
	//----------function to add an element in front in an array------------
	function mAddInFrontOfArray(oArray, oElement) {
		oArray.push(oElement);
		var oTemp = oArray[0];
		oArray[0] = oArray[oArray.length-1];
		oArray[oArray.length-1] = oTemp;
	}
	//----------arrays of registered event-handlers-------------------
	var aoRegisteredOnDocumentMouseDown = new Array();
	var aoRegisteredOnDocumentKeyPress = new Array();
	var aoRegisteredOnDocumentMouseMove = new Array();
	var aoRegisteredOnDocumentResize = new Array();


	//--------the event-handlers-----------------------------
	function onDocumentMouseDown(e) {
		if (e == null) e = window.event; //for ie
		for (i=0; i < aoRegisteredOnDocumentMouseDown.length;i++) {
			if (aoRegisteredOnDocumentMouseDown[i] != null) {
				if (aoRegisteredOnDocumentMouseDown[i](e) == false) return false;
			}
		}
		return true;
	}
	function onDocumentMouseMove(e) {
		if (e == null) e = window.event; //for ie
		for (i=0; i < aoRegisteredOnDocumentMouseMove.length;i++) {
			if (aoRegisteredOnDocumentMouseMove[i] != null) {
				if (aoRegisteredOnDocumentMouseMove[i](e) == false) return false;
			}
		}
		return true;
	}
	function onDocumentKeyPress(e) {
		if (e == null) e = window.event; //for ie
		for (i=0; i < aoRegisteredOnDocumentKeyPress.length;i++) {
			if (aoRegisteredOnDocumentKeyPress[i] != null) {
				if (aoRegisteredOnDocumentKeyPress[i](e) == false) return false;
			}
		}
		return true;
	}
	function onDocumentResize(e) {
		if (e == null) e = window.event; //for ie
		for (i=0; i < aoRegisteredOnDocumentResize.length;i++) {
			if (aoRegisteredOnDocumentResize[i] != null) {
				if (aoRegisteredOnDocumentResize[i](e) == false) return false;
			}
		}
		return true;
	}

	//---------store current events---------------
	if (document.onmousedown != null)
			gAdd_DocumentMouseDown(document.onmousedown);
	if (document.onkeypress != null) 
			gAdd_DocumentKeyPress(document.onkeypress);
	if (document.onmousemove != null) 
			gAdd_DocumentMouseMove(document.onmousemove);
	if (window.onresize != null) 
		gAdd_DocumentResize(document.onresize);
	
	//--------------set default-handlers----------------------------
	document.onmousemove = onDocumentMouseMove;
	document.onmousedown = onDocumentMouseDown;
	document.onkeypress = onDocumentKeyPress;
	
	window.onresize = onDocumentResize; //IE knows, NetScape not
	
	
//------------------------------------------------------------
//CAPTURE MOUSE MOVE

var mouseX, mouseY;
gAdd_DocumentMouseMove(getXY);

function getXY(e) {
    mouseX = (window.Event) ? e.pageX : event.clientX;
    mouseY = (window.Event) ? e.pageY : event.clientY;
}

//-----------own interval management------------------------------
var aSetIntervals_Funcs = new Array;
var aSetIntervals_States = new Array;
var aSetIntervals_IDs = new Array;
var aSetIntervals_BrowserIDs = new Array;

function glAddInterval(sId, sFunc, lMilliSeconds) {
	
	var idxFound = -1;
	for (i=0; i < aSetIntervals_IDs.length; i++) {
		if (aSetIntervals_IDs[i] == sId && aSetIntervals_States[i]==true) {
			idxFound = i;
			break;
		}
	}
	if (idxFound != -1) {
		window.clearInterval(aSetIntervals_BrowserIDs[idxFound]);
		aSetIntervals_States[idxFound] = false;
	}
	
	aSetIntervals_Funcs.push(sFunc);
	aSetIntervals_States.push(true);
	
	var sBrowserId = window.setInterval(sFunc, lMilliSeconds);
	aSetIntervals_BrowserIDs.push(sBrowserId);
	aSetIntervals_IDs.push(sId);
	
	
}

function gRemoveInterval(sId) {

	var idxFound = -1;
	for (i=0; i < aSetIntervals_IDs.length; i++) {
		if (aSetIntervals_IDs[i] == sId && aSetIntervals_States[i]==true) {
			idxFound = i;
			break;
		}
	}
	if (idxFound != -1) {
		window.clearInterval(aSetIntervals_BrowserIDs[idxFound]);
		aSetIntervals_States[idxFound] = false;
	}

}


function glEventX(ev) {

	var oResult;
	
	try	{
		oResult = ev.pageX;
	}
	catch (e) {}
	
	if (oResult == null) {
		try	{
			oResult = ev.x;
		}
		catch (e) {}
	}
	
	return oResult;
	
}

function glEventY(ev) {
	var oResult;
	
	try	{
		oResult = ev.pageY;
	}
	catch (e) {}
	
	if (oResult == null) {
		try	{
			oResult = ev.y;
		}
		catch (e) {}
	}
	
	return oResult;

}

function gbEventInDiv(ev, oDiv) {

	lLeft = Number(oDiv.style.left.replace("px",""));
	lTop = Number(oDiv.style.top.replace("px",""));
	lWidth = Number(oDiv.style.width.replace("px",""));
	lHeight = Number(oDiv.style.height.replace("px",""));
	
	var lX = glEventX(ev); 
	var lY = glEventY(ev);
	
	if (lX >= lLeft && lX <= lLeft + lWidth && lY >= lTop && lY <= lTop + lHeight) {
		return true;
	}
	else
		return false;
}



//----------------------------------------------------------------------------
//URL - functions 



function gsMakeAbsoluteURLPath(sPath) {
	//"../../a.aspx --> "/easyFramework/asp/a.aspx"

	if (sPath.substring(0,1) == "/") 
		return sPath;

	var sLocationPath = location.pathname;
	sLocationPath = sLocationPath.substring(0, sLocationPath.lastIndexOf("/"));
	while (sPath.substring(0, 2) == "..") {
		sLocationPath = sLocationPath.substring(0, sLocationPath.lastIndexOf("/"));
		sPath = sPath.substring(sPath.indexOf("/") + 1);
	}
		
	if (sLocationPath.substring(0, 2) != "..") {

		sPath = sLocationPath + "/" + sPath;
	}
	
	
	return sPath;
	

}


var sWebPageRoot = "not initialized";
function gsGetWebPageRoot() {

	if (sWebPageRoot == "not initialized") 
		alert("sWebPageRoot isn't initialized yet - include the defaultheader.aspx.inc and don't use gsGetWebPageRoot for public consts.");

	return sWebPageRoot; //from defaultheader
	
}


function gFocusFirstElement(sHtmlFormName) {

	var i;
			
	var frm =  document.forms[sHtmlFormName];
	
	for (i = 0; i < frm.elements.length; i++) {
		if (frm.elements[i].type != "hidden" && frm.elements[i].type != "button")   {
		
			try {
				
				frm.elements[i].focus(); 
				return;
			} catch(ec) {
			
			}
		}
	}
}

function gsResolveEntities(s) {


	while (s.indexOf("&gt;") >= 0)	
		s = s.replace("&gt;", ">");
	while (s.indexOf("&lt;") >= 0)
		s = s.replace("&lt;", "<");
	while (s.indexOf("&quot;") >= 0)
		s = s.replace("&quot;", "\"");
	
	return s;
}




