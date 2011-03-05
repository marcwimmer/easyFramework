/* (c) Promain Software-Betreuung GmbH 2004 */

function gReturnModalResult(sResult) {
	
	if (lModalDialogType == null) lModalDialogType = 1; //if dialog is called without being modal, 
														//some functions try to access the
														//modal-result; they are able to access this
														//value, if the dialog-type is 1 (default);	
	if (lModalDialogType == 1)  {
		opener.gInternalSetModalResult(sResult);
		window.close();
	}	
	else if (lModalDialogType == 2) {
		gInternalSetModalResult(sResult);
		
	}
}
function gReturnNoModalResult() {
	
	if (lModalDialogType == 1)  {
		window.close();
	}
	else if (lModalDialogType == 2) {
		oModalDialog.parentNode.removeChild(oModalDialog);
		oModalDialog = null;
	}
}

var msCallAfterModal;

function gInternalSetModalResult(oResult) { //called by the modal-dialog

	oModalResult = oResult;
	bModalDialogClosed = true;
	if (oModalDialog != null) {
	
		if (lModalDialogType == 1) {
			oModalDialog.close();
			oModalDialog = null;
		}
		else if (lModalDialogType == 2) {
			oModalDialog.parentNode.removeChild(oModalDialog);
			oModalDialog = null;
		}
	}
		
	//call after modal-function:
	bNowModalDialogClosed = false;
	try {
		if (msCallAfterModal != null)   {
			msCallAfterModal += "";	//needed, if javascript is called from textarea-transfered javacode; it is not executed otherwise
			
			//some replacements
			var sCallAferModalReplaced = msCallAfterModal;
			while (sCallAferModalReplaced.indexOf("efApostroph") > 0)  {
				sCallAferModalReplaced = sCallAferModalReplaced.replace("efApostroph", "'");
			}
			
			
			eval(sCallAferModalReplaced);
		}
			
	}
	catch (e) {
	}
	bNowModalDialogClosed = true;
	
}


function gsShowDivModalDialog(sURL, lWidth, lHeight, sCallAfterwards) {
//shows a modal-dialog within a div.
//
//the advantage: in the after-modal-function the xml-http-object
//can be used.
//so you should use this function, when the modal dialog is very integrated

	//hide current one:
	if (oModalDialog != null && lModalDialogType == 2) {
		oModalDialog.parentNode.removeChild(oModalDialog);
		oModalDialog = null;
	}


	var oDiv = document.createElement("div");
	oDiv.style.position = "absolute";
	oDiv.style.height = lHeight;
	oDiv.style.width = lWidth;
	oDiv.style.overflow = "hidden";
	oDiv.style.top = gOffsetTop() + (glWindowHeight() - lHeight) / 2;
	oDiv.style.left = (glWindowWidth() - lWidth) / 2;
	
	oDiv.id = "modalDiv";
	oDiv.style.zIndex = 1000;

	var sInnerHtml = gsCallServerMethod(sURL, "");
	
	//search for <!--DIVMODALDIALOG BEGIN-->
	var sBegin = "<!--DIVMODALDIALOG BEGIN-->";
	var sEnd = "<!--DIVMODALDIALOG END-->";
	if (sInnerHtml.indexOf(sBegin) < 0)	 {
		efMsgBox("Invalid html for div-dialog-box. Must have string \""+sBegin+"\"! Wrong is: " + sInnerHtml, "ERROR");
		return
	}
	if (sInnerHtml.indexOf(sEnd) < 0) {
		efMsgBox("Invalid html for div-dialog-box. Must have string \""+sEnd+"\"! Wrong is: " + sInnerHtml, "ERROR");
		return
	}
	
	sInnerHtml = sInnerHtml.substring(sInnerHtml.indexOf(sBegin));
	sInnerHtml = sInnerHtml.substring(0, sInnerHtml.indexOf(sEnd) - 1);
	
	
	
	oDiv.innerHTML = sInnerHtml;
	document.forms[0].parentNode.appendChild(oDiv);
	
	//get javascript from javascript-container and insert it:
	for (var i=0; i < document.forms.length; i++) {
		for (var y=0; y < document.forms[i].elements.length; y++) {
			var oCtl = document.forms[i].elements[y];
			
			if (oCtl.name == "javascriptcontainerModalDialog") {
				oCtl.style.visibility = "hidden";
				var oScript = document.createElement("script");
				oScript.text = oCtl.value;
				oDiv.appendChild(oScript);
			}
		
		}	
	}
	
	
	oModalDialog = oDiv;
	msCallAfterModal = sCallAfterwards;
	lModalDialogType = 2;

}

function gsShowWindow(sURL, bModal, callAfterwards, oReplaceLocationOfThatWindow) {
/*big problem is the story of modal and non-modal dialogs.
it is relativley simply here: if you are using a modal-
dialog, then you have to enter the function call afterwards
refer to the manual for explicit description.

You can pass a window in bReplaceLocationOfThatWindow. Its location is being
changed then.

*/

	

	try {
	if (sURL.search(/\?/i)==-1)
		sURL = sURL + '?ClientID=' + msClientID;
	else
		sURL = sURL + '&ClientID=' + msClientID;
	}
	catch (e)
	{
	
	}
	msCallAfterModal = callAfterwards;
	
	
	var sFeatures = "menubar=0, titlebar=0,scrollbars=1, status=1, toolbar=0, left=200, top=200, width=300, height=250";
	if (bModal == true) {
		sFeatures += ", resizable=0," }
	else {
		sFeatures += ", resizable=1,"
	}


	
	if (bModal==true) {
		gShowModalWindow(sURL);
	}
	else {	  
		if (oReplaceLocationOfThatWindow == null) 
			window.open(sURL, "_blank", sFeatures); 			
		else
			oReplaceLocationOfThatWindow.location = sURL;
	}
}
		
function gResizeTo(lWidth, lHeight) {
	//works for dialogs modal und not modal
	try {
		window.dialogWidth = lWidth + "px";
		window.dialogHeight = lHeight + "px";
	} catch (e) {}
	
	
	window.resizeTo(lWidth, lHeight);
  
    mlWindowHeight();
	mlWindowWidth();

}	

function gMoveTo(lX, lY) {
	window.moveTo(lX, lY);
}

function gWindowMaximize() {
	//works for dialogs modal und not modal
	gResizeTo(glScreenWidth(), 0.95 * glScreenHeight());
	gMoveTo(0, 0);
}

function gPosWindowDefault() {

	var lX = (glScreenWidth() - glWindowWidth()) / 2;
	var lY = (glScreenHeight() - glWindowHeight()) / 4;
	window.moveTo(lX, lY);
}

function gOffsetTop() {

	var lTop = document.body.scrollTop;
	if (lTop == null) lTop = 0;
	
	return lTop;
}


/*----------------getting window width
in IE getting the window-width by this methods, the tab-dialog jumps
around. it seems, that by getting these values, internet-explorer calculates
widths and realigns objects by doing this. so only one-time at resizing,
the widths shall be calculated
*/
var mlVarWindowWidth;
var mlVarWindowHeight;
mlWindowWidth();
mlWindowHeight();
gAdd_DocumentResize(gHandleSize, true);

function mlWindowWidth()
{
 if (window.innerWidth) mlVarWindowWidth = window.innerWidth;
 else if (document.body && document.body.offsetWidth) mlVarWindowWidth = document.body.offsetWidth;
 else mlVarWindowWidth = 0;
}

function mlWindowHeight()
{
 if (window.innerHeight) mlVarWindowHeight = window.innerHeight;
 else if (document.body && document.body.offsetHeight) mlVarWindowHeight = document.body.offsetHeight;
 else mlVarWindowHeight = 0;
}

function glWindowWidth() {
	if (mlVarWindowWidth == "NaN")
		mlWindowWidth();
	return mlVarWindowWidth;
}
function glWindowHeight() {
	if (mlVarWindowHeight == "NaN")
		mlWindowHeight();
	return mlVarWindowHeight;
}
function gHandleSize() {
	mlWindowWidth();
	mlWindowHeight();
}


//--crossbroswer show modal:
var lModalDialogType; //1=default-modaldialog;2=div-modaldialog
var oModalDialog; //if lmodalDialogType=1 then the window-object, lModalDialog=2--> the div-element
var bModalDialogClosed=true;
var bNowModalDialogClosed=true; //for the after-event after modal-dialog closed
var oModalResult;

function IgnoreEvents(e)
{
  return false
} 

function gShowModalWindow(sURL)
{
	//if there is already a dialog, then try to close it:
	try {
		if (oModalDialog != null && lModalDialogType == 1)  {
			oModalDialog.close();
			bModalDialogClosed = true;
		}
	}
	catch (ex) { }

	//if (bModalDialogClosed==true) {
  		oModalResult = null;
    	bModalDialogClosed=false;
    	//try {
    		//window.top.captureEvents (Event.CLICK|Event.FOCUS)
    	//} catch (e) {}
    	//window.top.onclick=IgnoreEvents; 
    	//window.top.onfocus=HandleFocus; 
    	oModalDialog = window.open(sURL, "_blank", "dependant=true")
    	oModalDialog.lModalDialogType = 1;
    	lModalDialogType = 1;
    	
		oModalDialog.focus()
	//}
}

function HandleFocus()
{
  if (oModalDialog != null && lModalDialogType == 1)
  {
    if (!oModalDialog.closed)
    {
		oModalDialog.focus()
    }
    else
    {      
    	if (oModalDialog.top != null) {
    		oModalDialog.top.releaseEvents (Event.CLICK|Event.FOCUS)
      		oModalDialog.top.onclick = ""
      	}
    }
  }
  return false
}


