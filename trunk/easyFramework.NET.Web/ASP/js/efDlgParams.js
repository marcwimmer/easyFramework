/* (c) Promain Software-Betreuung GmbH 2004 */


/*----------------------------------------------------
global functions for xml-dialogs
------------------------------------------------------*/
function gFocusFirstXmlDialogElement(sXmlDialogId) {
	var oXmlDialog = goFindXmlDialog(sXmlDialogId);
   
   try {
	document.forms[oXmlDialog.sHtmlFormName].elements[oXmlDialog.sFirstInputElementName].focus();
   }
	catch (ex) {
	
		
	
	}
   
}

function gXmlDialog2Div(oDestinationDiv, sXmlDialogDefinitionFile, sXmlDialogDataPage, sXmlFormName, sXmlDialogId, sXmlDataParams) {//liefert die HTML-Darstellung eines XML-Dialogs zurück; kann z.B. für Tabreiter verwendet werden, und allgemein für das dynamische Nachladen zur Javascript-Laufzeit

	oDestinationDiv.innerHTML = ""; //reset
	
	var sUrlToRenderXmlDialog = gsGetWebPageRoot() + "/ASP/system/xmldlgrenderer/renderXmlDlg.aspx"
				
	var sUrlParams = "?xmldeffile=" + gsGetWebPageRoot() + sXmlDialogDefinitionFile + 
		"&xmldatapage=" + sXmlDialogDataPage + 
		"&xmlformname=" + sXmlFormName +
		"&xmldialogid=" + sXmlDialogId;

	oDestinationDiv.innerHTML = gsCallServerMethod(sUrlToRenderXmlDialog + sUrlParams, sXmlDataParams);
			
	//now todo: the content of the javascripts has to be inserted as a javascript-object:
	var oJscript = document.createElement("script");
	var oJavascriptContainer = document.getElementById(sXmlDialogId + "_javascripts");
	
	
	if (oJavascriptContainer != null)  {
		oJscript.text = oJavascriptContainer.value;
		oJavascriptContainer.parentNode.appendChild(oJscript);
		
	}
	else {
		alert("oJavascriptContainer not found!");
	}	


}


function gXmlDialogFetchData(sXmlDialogId, sAddParams) {

    var oXmlDialog = goFindXmlDialog(sXmlDialogId);
	if (oXmlDialog == null) return;
	
	gApplyDataPageToDialog(oXmlDialog.sDataPage, oXmlDialog.sHtmlFormName, sAddParams) 

	gXmlDialogSetDirty(sXmlDialogId, false);
	
}

function gXmlDialogFlush(sXmlDialogId) {

    var oXmlDialog = goFindXmlDialog(sXmlDialogId);

	
	gFlushDialog(oXmlDialog.sHtmlFormName);
	
	gXmlDialogSetDirty(sXmlDialogId, false);
	
}

function gXmlDialogSetDirty(sXmlDialogId, bDirty) {
	//set the dirty-state:
	
	var oXmlDialog = goFindXmlDialog(sXmlDialogId);

	var oHidElement = document.forms[oXmlDialog.sHtmlFormName].elements["__dirty" + sXmlDialogId];
	if (oHidElement == null) return;
	
	if (bDirty == true) 
		oHidElement.value = "1";
	else
		oHidElement.value = "0";
	
	
}

function gbXmlDialogGetDirty(sXmlDialogId) {
	//returns the dirty-state:

    var oXmlDialog = goFindXmlDialog(sXmlDialogId);

	var oHidElement = document.forms[oXmlDialog.sHtmlFormName].elements["__dirty" + sXmlDialogId];
	if (oHidElement == null) return false;
	
	if (oHidElement.value == "0") 
		return false;
	else
		return true;
	
}

function gbAnyRegisteredXmlDialogDirty() {
 //returns true, if any of the registered xml-dialogs is dirty
 
	var i=0;
	for (i=0; i < aoAllXmlDialogs.length; i++) {
		if (gbXmlDialogGetDirty(aoAllXmlDialogs[i].sXmlDialogId) == true) 
			return true;
	
	}
	
	return false;

}

function gClearAllXmlDialogDirtyFlags() {
 //returns true, if any of the registered xml-dialogs is dirty
 
	var i=0;
	for (i=0; i < aoAllXmlDialogs.length; i++) {
		
		gXmlDialogSetDirty(aoAllXmlDialogs[i].sXmlDialogId, false);
			
	
	}
	
	return false;

}




function RegisterXmlDialog(sXmlDialogId, sHtmlFormName, sFirstInputElementName, sDataPage) {
						
	var oXmlDialog = new oXmlDialogConstructor(sXmlDialogId, sHtmlFormName, sFirstInputElementName, sDataPage);
	aoAllXmlDialogs.push(oXmlDialog);
	
}

var bAddedoXmlDialogHelperEnterKeyPress = false; //damit ereignishandler nicht doppelt hinzugefügt wird
function gRegisterXmlDialogDefaultButton(sXmlDialogId, sButtonId) {
	
	var oDefButInfo = new oXmlDialogDefaultButtonInfo(sXmlDialogId, sButtonId); //information default-button eines XMLDialogs speichern
	aoXmlDialogDefaultButtons.push(oDefButInfo);

	if (!bAddedoXmlDialogHelperEnterKeyPress) { //nur einmal hinzufügen
		gAdd_DocumentKeyPress(oXmlDialogHelperEnterKeyPress, true);
		bAddedoXmlDialogHelperEnterKeyPress = true;
		
	}
	
}



/*----------------------------------------------------
private functions for xml-dialogs
------------------------------------------------------*/
function oXmlDialogDefaultButtonInfo(sXmlDialogId, sButtonId) {
	this.sXmlDialogId = sXmlDialogId;
	this.sButtonId = sButtonId;
	return this;
}

var aoAllXmlDialogs = new Array();
var aoXmlDialogDefaultButtons = new Array(); //enthält den default-button pro xmldialogid

function oXmlDialogHelperEnterKeyPress(e) {

	if (e.keyCode != null)
		if (e.keyCode == 13) {
			var i = 0;
			for (i = 0; i < aoXmlDialogDefaultButtons.length; i++) {
				var sButtonId = aoXmlDialogDefaultButtons[i].sButtonId;
				var oBtn = document.getElementById(sButtonId);
				//alert("executing default-button code...");	
				if (oBtn.onclick != null) {
					oBtn.onclick();
					//alert(oBtn.onclick);
				}
				
			}
		}
}

function oXmlDialogConstructor(sXmlDialogId, sHtmlFormName, sFirstInputElementName, sDataPage) {

	this.sXmlDialogId = sXmlDialogId;
	this.sHtmlFormName = sHtmlFormName;
	this.sFirstInputElementName = sFirstInputElementName;
	this.sDataPage = sDataPage;
	
	return this;
}

function goFindXmlDialog(sXmlDialogId) {

	var i = 0;
	
	for (i=0; i - aoAllXmlDialogs.length; i++) {
	
	
		if (aoAllXmlDialogs[i].sXmlDialogId == sXmlDialogId) 
			return aoAllXmlDialogs[i];
	
	}

	efMsgBox("Xml-Dialog \"" + sXmlDialogId + "\" wasn't found!", "ERROR");
}







/*----------------------------------------------------
------------------------------------------------------*/


function gsXMLDlgOutput(frm, sAdd, bAddEmptyValues)
{ //returns the content of a dialog as xml
	 
	 
	var sParams;
	var l,lCount;
	var ctl;
	var sType,sName,sValue;
	var sID;
	var bMultiRow = false;
	var lDialogPageSize;
	var lCurrentRow = -1;
	var sValue;
	var bAddValue;
	var bOpenNewRow = false;
	var bEverCreatedRowTag = false;	
	var bNeedsToCloseRow = false;		
	
			
	sParams = '';
	if (frm == null)
		return(sParams);
		
	sParams = sParams + '<DIALOGOUTPUT>\n';
	
	sParams = sParams + sAdd;
	
	lCount = frm.elements.length;
	for (l=0 ; l<lCount ; l++ )
	{
		ctl = frm.elements[l];
		sType = ctl.type;
		sName = ctl.name;
		
		
		if (sName == '__dialogtype') {
			if (ctl.value == 'multirow') 
				bMultiRow = true;
		} 
		else if (sName == '__dialogpagesize') {
			lDialogPageSize = ctl.value;
		} 
		else if (sName!='')
		{
		
			bAddValue = false;
			
			sValue = "";
			
			if (sType=='text' || sType=='select-one' || sType=='textarea' || sType=='password' ||
			(sType=='hidden' && sName.substr(0,10)!='txtPrivate'))
			{
				sValue = ctl.value;
				if (bAddEmptyValues || sValue != '')
				{
					bAddValue = true;
				}
			}
 			if (sType == 'checkbox')
			{
				sValue = ctl.checked;
				if (bAddEmptyValues || sValue)
				{
					bAddValue = true;
					if (sValue)
						sValue = "1";
					else
						sValue = "0";
				}
			}

			if (sType == 'radio')
			{
				sID = ctl.id;
				sValue = ctl.checked;
				if (bAddEmptyValues || sValue)
				{
					if (sValue)	   {
						bAddValue = true;
						sParams = sParams + '<' + sName + '>' + sID + '</' + sName + '>\n';
					}
				}
			}
			
			bOpenNewRow = false;
			
			if (bMultiRow)	{
				if (sName.substring(0,3) == "ROW") {
				
					var lExtractedRowNumber = 
						sName.substring(3, sName.indexOf("_"));
					
					if (lExtractedRowNumber != lCurrentRow) {
						bOpenNewRow = true;
						lCurrentRow = lExtractedRowNumber;
					}
					
					sName = sName.substring(sName.indexOf("_") + 1, sName.length);
					
				}
			}
			
			
			
			if (bOpenNewRow) {
				if (bEverCreatedRowTag == true) {
					sParams += "</ROW>\n\n"
					bNeedsToCloseRow = false;
				}
				sParams += "<ROW NUMBER=\"" + lCurrentRow + "\">\n";
				bNeedsToCloseRow = true;
				bEverCreatedRowTag = true;
			}
			
			//make cdata:
			sValue = "<![CDATA[" + sValue + "]]>";
			if (bAddValue) 
				sParams = sParams + "<" + sName + ">" + sValue + "</" + sName + ">\n"
		}
	}
	
	if (bNeedsToCloseRow) 
		sParams += "</ROW>\n";

	sParams = sParams + '</DIALOGOUTPUT>\n';
	
	return(sParams);
}

function gApplyDataPageToDialog(sDataPage, sFormName, sAddParams) {
	//fills the given dialog with the values from the data-page:
		
	if (sDataPage == "" || sDataPage == null) {
		efMsgBox("Data cannot be fetched, because data-page is empty!");
		return 
	}
	
	var sDialogData = gsCallServerMethod(sDataPage, sAddParams);
	
	//first flush the dialog:
	gFlushDialog(sFormName);
	var sColumnSep = "|";
	var sLineEnd = "-||-";
	var lCurrentBlockRow; //the row-number of the current dialog-row

	
	
	if (sDialogData.substring(0, 6) != "OK-||-") {
	
		efMsgBox(sDialogData, "ERROR");
		return;
	}
		
	while (sDialogData.indexOf(sLineEnd) >= 0) {
		
		var sLine = new String(sDialogData.substr(0, 
			sDialogData.indexOf(sLineEnd)));
			
		if (sLine.substring(0, 5) == "__ROW")	{
			lCurrentBlockRow = sLine.substring(6, sLine.length);
			lCurrentBlockRow = lCurrentBlockRow.substring(0, lCurrentBlockRow.indexOf(sColumnSep));
			lCurrentBlockRow = Number(lCurrentBlockRow);
		} 
		else if (sLine.substring(0, 7) == "__CONST") {
			var sName = sLine.split(sColumnSep)[1];
			var sValue = sLine.split(sColumnSep)[2];
			
			mSetValueInDialog(sFormName, sName, sValue, "", "");
		
		}
		else {
			if (Number(lCurrentBlockRow) == "NaN") {
				alert("invalid data, missing row-number: " + sLine);
			}
			var sName = sLine.substr(0, sLine.indexOf(sColumnSep));
			sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.indexOf(sColumnSep, 
				sLine.indexOf(sColumnSep) + 1) + 1);
			
			var sValue = sLine.substr(0, sLine.indexOf(sColumnSep));
			sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);
								   
			mSetValueInDialog(sFormName, sName, sValue, Number(lCurrentBlockRow));
		}
					 
		 sDialogData = sDialogData.substring(sDialogData.indexOf(sLineEnd) + 
			sLineEnd.length, sDialogData.length);
	        	
	}

	
	return "";
}

function gFlushDialog(sFormName) {

	var oForm = document.forms[sFormName];
	oForm.reset();
	//in netscape the hidden-fields aren't resetted, do manually
	
	for (i=0; i < oForm.elements.length; i++) {
		if (oForm.elements[i].type == "hidden" && oForm.elements[i].name.substring(0, 2) != "__")
			oForm.elements[i].value = "";
	}
}

function gAfterModalEntitySearchDlg(sFormName, sElementName, sEntityName) {
/*		handler for setting the value, that was selected in the search-dialog
*/
	
	if (oModalResult != "") {
		document.forms[sFormName].elements[sElementName].value = oModalResult;
		eval("gUpdateEntityValue_" + sElementName + "();");
	}
}


function gUpdateEntityValue(sFormName, sElementName, sEntityName, sWebPageRoot, sEntityID) {
/*		usually called after an item was selected in a search-dialog or 
		when the dialog-values are loaded into the dialog.
		the tostring-value of the entity is retrieved by the existing key-value(sentity-id)
*/

	var oInputValueElement = document.forms[sFormName].elements[sElementName + "_EntityToStringValue"];
	var oInputKeyElement = document.forms[sFormName].elements[sElementName];
	

	var sHref = sWebPageRoot + "/ASP/system/entityDialogLoad/entityDialogLoad.aspx"
	    
	var sResult = gsCallServerMethod(sHref,"<entity>" +
                sEntityName + "</entity><keyvalue><![CDATA["+ 
					oInputKeyElement.value +"]]></keyvalue>");
	
	if (sResult.substring(0, 8) != "SUCCESS;") {
		if (sResult != "" && sResult != null) {
			efMsgBox(sResult);
			return;
		}
	}
	else sResult = sResult.substring(sResult.indexOf(";")+1);

	
	if (sResult == null || sResult == '') {
        oInputKeyElement.value=""; 
        oInputValueElement.value="";
		
	}                                                                      
    else {
			oInputKeyElement.value=sResult.split(";")[0];     
            oInputValueElement.value=sResult.split(";")[1];			
    }                                                                       

}

function gsSearchForKeyValue(sFormName, sElementName, sEntityName, sWebPageRoot) {
/*		usually called at OnBlur of an entity-edit element;
		the entity is search by the given search-clause (value of the input)
		if it is found, then the hidden-keyfield is set to the entitie's key-value
		and the input-box is set to the toString of the entity
		
*/
	var oInputValueElement = document.forms[sFormName].elements[sElementName + "_EntityToStringValue"];
	var oInputKeyElement = document.forms[sFormName].elements[sElementName];

	var sHref = sWebPageRoot + "/ASP/system/entityDialogLoad/entityDialogLoad.aspx"
	
	var sResult = gsCallServerMethod(sHref,"<entity>" +
                sEntityName + "</entity><value><![CDATA["+ 
					oInputValueElement.value +
				"]]></value><keyvalue></keyvalue>");
				
				
	if (sResult.substring(0, 8) != "SUCCESS;") {
		efMsgBox(sResult);
		return;
	}
	else sResult = sResult.substring(sResult.indexOf(";")+1);
				
				
	if (sResult == null || sResult == '') {
        oInputKeyElement.value=""; 
        oInputValueElement.value="";
	}                                                                      
    else {
			oInputKeyElement.value=sResult.split(';')[0];
            oInputValueElement.value=sResult.split(';')[1];
    }                                                                       
    

}

function mSetValueInDialog(sFormName, sElementName, sElementValue, lDialogRowNumber,	
	sCustomPraefix) {
	  
//sCustomPraefix: a customizable praefix, which is used to find the control-element

	var oForm = document.forms[sFormName];
	if (oForm == null) { 	
		alert("form \""+sFormName+"\" wasn't found!");
		return false;
	}
	
	//alert("setting " + sElementName + " to " + sElementValue);		
	for (i=0; i < oForm.elements.length; i++) {	
	
		var oCtl = oForm.elements[i];
	   
		var s = new String(oCtl.name);
		
		//remove the row-praefix:
		var sRowPraefix = new String("ROW" + lDialogRowNumber + "_");
		if (s.substring(0, sRowPraefix.length) == sRowPraefix) {
			s = s.substring(sRowPraefix.length, s.length);
		} 
		
		//try to remove the custom-praefix:
		if (sCustomPraefix != null && sCustomPraefix != "") {
			if (s.substring(0, sCustomPraefix.length) == sCustomPraefix)
				s = s.substring(sCustomPraefix.length);
		}
		
		  
		//remove the dialog-praefixes:
		if (s.substring(0, 3) == "txt") 
			s = s.substring(3, s.length);
		if (s.substring(0, 3) == "lbl") 
			s = s.substring(3, s.length);
		if (s.substring(0, 3) == "chk") 
			s = s.substring(3, s.length);
		if (s.substring(0, 3) == "cmd") 
			s = s.substring(3, s.length);
		if (s.substring(0, 3) == "cbo") 
			s = s.substring(3, s.length);
		
		//ignore the ignored-fields:
		if (s.substring(0, 2) == "__") 
			continue;
			
		//also set old-values:
		if (s.length >= 4) {
			if (s.substring(s.length - 4) == "_old")  {
				s = s.substring(0, s.length - 4);
					
			}
				
		}
		
		
		 //if name doesn't match, try the next one:
		if (s != sElementName) 
			continue;
		
			
		//sElementValue = gTranslateIntoEntities(sElementValue);
		switch(oCtl.type)
		{
			case 'text':
			case 'password':
			case 'textarea':
			case 'hidden':
				oCtl.value = sElementValue;
				
				//could be entity, so also update the to-string field:
				try {
				if (eval("gUpdateEntityValue_txt" + sElementName)) {
					eval("gUpdateEntityValue_txt" + sElementName + "();");
				}
				} catch(e) {};
				
				
				break;
			case 'select-one':
				gSetCboValue(oCtl, sElementValue);
				break;
			case 'checkbox':
				oCtl.checked = (sElementValue != "" && sElementValue != null && sElementValue!="0");
				break;
		}
		
		
		
		
	}

}


function gSetCboValue(oCtl, sValue)
{
    var i;
	     
	if (sValue=='')
		for (i=0; i < oCtl.options.length; i++)
		{
		    if (oCtl.options[i].text == '' && oCtl.options[i].value == '')
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
	else 
		for (i=0; i < oCtl.options.length; i++)
		{
		    if (oCtl.options[i].text == sValue || oCtl.options[i].value == sValue)
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
		
	if (sValue=='0')
		for (i=0; i < oCtl.options.length; i++)
		{
		    if (oCtl.options[i].text == '' && oCtl.options[i].value == '')
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
}

function gSetCboValueArray(oCtl, sValue, lIndex)
{
    var i;
    var sIntValue;
    var asValue;

	if (sValue=='')
		for (i=0; i < oCtl.options.length; i++)
		{
		    if (oCtl.options[i].text == '' && oCtl.options[i].value == '')
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
	else 
		for (i=0; i < oCtl.options.length; i++)
		{
			sIntValue = oCtl.options[i].value;
			asValue = sIntValue.split("$;$");
		    if (oCtl.options[i].text == sValue || asValue[lIndex] == sValue)
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
		
	if (sValue=='0')
		for (i=0; i < oCtl.options.length; i++)
		{
		    if (oCtl.options[i].text == '' && oCtl.options[i].value == '')
			{
				oCtl.options[i].selected=true;
				return;
			}
		}
}

function gsGetCboText(oCtl)
{
    var i;
     
	for (i=0; i < oCtl.options.length; i++)
	{
		if (oCtl.options[i].selected==true)
			return(oCtl.options[i].text);
	}
}

function gsGetCboValue(oCtl)
{
    var i;
     
	for (i=0; i < oCtl.options.length; i++)
	{
		if (oCtl.options[i].selected==true)
			return(oCtl.options[i].value);
	}
}

function gCreateNewCboOption(oCbo,sValue,sDesc,lPos,bSetToCurr)
{
	//add new option to cbo
	if (lPos =='undefined' || lPos == '') lPos=0;

	var oOption = document.createElement("OPTION");
	oCbo.options.add(oOption,lPos);
	oOption.value = sValue;
	oOption.innerText = sDesc;
	if (bSetToCurr)
	{
		oCbo.value=sValue;
	}
}

											   	
function formatDate(it)	{
		//alert ("it: " +  typeof it);
		//alert ("it.value: " + typeof it.value);
		if ((typeof it == "undefined")||(typeof it.value == "undefined"))
		{
			alert("ERROR: Parameter for formatDate is no object");
			return false;
		}
		
		var strIntDate = it.value;
		var chrCode = 0;
		for (var i=0; i < strIntDate.length; i++)
		{
			//alert(strIntDate.charCodeAt(i));
			chrCode = strIntDate.charCodeAt(i);
			if(!((chrCode == 46)||((chrCode >= 48)&&(chrCode <=57))))
			{
				alert("Das Zeichen: '"  + strIntDate.charAt(i) + "' ist in einem Datum unzulässig!");
				it.select();
				return false;
			}
		}
		
		var arrSplittedDate = new Array;

		if (!((strIntDate=="") || (strIntDate==" " )))
		{
			if (strIntDate.indexOf('.') > 0)
			{	// Datum des Formats: d*.m*.y*
				arrSplittedDate = strIntDate.split('.');
				if ((arrSplittedDate.length != 3) || ((arrSplittedDate[2].length != 2) && (arrSplittedDate[2].length != 4)))
					{
						alert("Bitte geben Sie das Datum im Format TT.MM.JJJJ oder TTMMJJJJ ein!");
						it.select();
						return false
					}
				if (arrSplittedDate[0].length==1) { arrSplittedDate[0] = "0" + arrSplittedDate[0];};
				if (arrSplittedDate[1].length==1) { arrSplittedDate[1] = "0" + arrSplittedDate[1];};
				if (arrSplittedDate[2].length==2)
				{
					if (arrSplittedDate[2] < 50)
					{
						arrSplittedDate[2] = "20" + arrSplittedDate[2];
					}
					else
					{
						arrSplittedDate[2] = "19" + arrSplittedDate[2];
					}
				} // if arrsplittedDate[2].length==2
			} // if strIntDate.indexOf('.') > 0
			else
			{	// Datum des Formats: ddmmyy*
				if ((strIntDate.length != 8) && (strIntDate.length != 6))
				{
					alert ("Wenn Sie das Datum ohne Trennzeichen angeben,\n müssen Sie es voll ausschreiben:\n \"TTMMJJJJ\" oder \"TTMMJJ\"");
					it.select();
					return false;
				}
				else
				{
					arrSplittedDate[0] = strIntDate.substr(0, 2);
					arrSplittedDate[1] = strIntDate.substr(2, 2);
					arrSplittedDate[2] = strIntDate.substr(4, 4);
					if (arrSplittedDate[2].length==2)
					{
						if (arrSplittedDate[2] < 50)
						{
							arrSplittedDate[2] = "20" + arrSplittedDate[2];
						}
						else
						{
							arrSplittedDate[2] = "19" + arrSplittedDate[2];
						}
					}
				}
			}
			if ((arrSplittedDate[0] > 31) || (arrSplittedDate[1] > 12))
			{
				alert ("Ungültiger Datumswert!");
				it.select();
				return false;
			}
			it.value = arrSplittedDate[0] + "." + arrSplittedDate[1] + "." + arrSplittedDate[2];

		}
		else
		{
			it.value = "";

		}
		return true;
}

function checkNumber(sNumber) {

	if (sNumber == null || sNumber == "") return "0";

	if (Number(sNumber) + 0 != sNumber)
		return "0";
	else
		return sNumber;



}


function gsRadioValue(sRadioName, oForm) {

	var el = oForm.elements;
	
	for (i = 0; i < el.length; i++) {
	
		if (el[i].name == sRadioName && el[i].type == "radio") {
			
			if (el[i].checked) {
		
				return el[i].value;
			}
		}
	
	}

	return null;

}

function gSetRadioValue(sRadioName, sValue, oForm) {

	var el = oForm.elements;
	
	for (i = 0; i < el.length; i++) {
	
		if (el[i].name == sRadioName && el[i].type == "radio" && el[i].value == sValue) {
			
			el[i].checked = true;
			return 
		}
	
	}


}