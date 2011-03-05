/* Copyright Promain Software-Betreuung GmbH, 2004 */
function gSetClientValue(sName, sValue) {

	var sUrl = gsGetWebPageRoot() + "/ASP/system/clientinfoData/storeClientData.aspx";
	var sParams = "<name><![CDATA[" + sName + "]]></name><value><![CDATA[" + sValue + "]]></value>";

	
	var sResult = gsCallServerMethod(sUrl, sParams);
	
	if (sResult != "OK") {
		efMsgBox(sResult, "ERROR");
	}

}

function gsGetClientValue(sName) {

	var sUrl = gsGetWebPageRoot() + "/ASP/system/clientinfoData/getClientData.aspx";
	var sParams = "<name><![CDATA[" + sName + "]]></name>";

	
	var sResult = gsCallServerMethod(sUrl, sParams);
	
	if (sResult.substring(0, 2) != "OK") {
		efMsgBox(sResult, "ERROR");
		return "";
	}
	
	sResult = sResult.substring(2);

	return sResult;
}