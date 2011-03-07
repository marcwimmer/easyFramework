/* (c) Promain Software-Betreuung GmbH 2004 */

/*===============================
Show Option dialog:
Displays a dialog, from which the user can select an option

user goOptionValue to create an option-value

example:

var aoValues = new Array();
aoValues.push(new goOptionValue("h", "Herr"));
aoValues.push(new goOptionValue("f", "Frau"));
gsShowOptionDialog("bitte wählen!", aoValues, "handleBox");

*/
function gShowOptionDialog(sText, asCaptionValuePairs, callAfterwards, lWidth, lHeight) {

	var sCaptions = new String;
	var sValues = new String;
	for (i = 0; i < asCaptionValuePairs.length; i++) {
	
		sCaptions += asCaptionValuePairs[i].sCaption + "-||-";		
		sValues += asCaptionValuePairs[i].sValue + "-||-";		
	
	}
	
	gShowStringOptionDialog(sText, sValues, sCaptions, callAfterwards, lWidth, lHeight);
	
	
}
function goOptionValue(sValue, sCaption) {

	this.sValue = sValue;
	this.sCaption = sCaption;

	return this
}

//the sValues and sCaptions are seperated by -||-
function gShowStringOptionDialog(sText, sValues, sCaptions, callAfterwards, lWidth, lHeight) {

	
	if (lWidth == null) lWidth = 450;
	if (lHeight == null) lHeight = 350;

	var sUrl = gsGetWebPageRoot() + "/ASP/system/optiondialog/optionDialog.aspx";
	
	sUrl += "?text=" + sText;
	sUrl += "&captions=" + sCaptions + "&values=" + sValues; 
	
	gsShowDivModalDialog(sUrl, lWidth, lHeight, callAfterwards);
	
}

