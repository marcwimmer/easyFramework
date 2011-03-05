/* (c) Promain Software-Betreuung GmbH 2004 */

//-----call online help-----------
function ONLHELP_showHelp() { 
	
	var sUrl = "";
	var sEntity = "";
	
	
	//try to get an entity:
	try {
		sEntity = ONLINEHELP_entity();
		if (sEntity == null) sEntity = "";
	}
	catch (ex) {
		sEntity = "";
	}
	
	//try to get an url:
	try {
		sUrl = ONLINEHELP_url();
		if (sUrl == null) sUrl = "";
	}
	catch (ex) {
		sUrl = "";
	}
	
	
	//show help:
	gsShowWindow(gsGetWebPageRoot() + "/ASP/system/help/help.aspx?url=" + sUrl + "&entity=" + sEntity);
	
	return false
}

function ONLINEHELP_url() {
	//returns the url for calling the online-help
	var sUrl = location.pathname;
	
	//remove the wegpage-root:
	sUrl = sUrl.substring(gsGetWebPageRoot().length);
	
	
	return sUrl;
}

function ONLHELP_NNShowHelp(ev) { 
	
	if (bIsIE() == false) {
		if (ev.keyCode == 112) 	{
			ONLHELP_showHelp();
			return false; //don't open browser-help
		}
	}
	return true;
	
}

//init online-help, so that it appears, when the f1-key is pressed (in IE and in Netscape)

//Netscape:
gAdd_DocumentKeyPress(ONLHELP_NNShowHelp);

//IE:
document.onhelp = ONLHELP_showHelp;

