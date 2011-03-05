Function efIEMsgBox(sText, sType, sButtons, sTitle)
'Msgbox for internet-explorer

	Dim lType
	lType = 0
	If sType = "INFO" Then lType = 64
	If sType = "CRITICAL" Then lType = 16
	If sType = "QUERY" OR sType = "QUESTION" Then lType = 32
	If sType = "EXCLAMATION" Or sType = "WARNING" Then	lType = 48


	If sButtons = "YESNOCANCEL" Then lType = lType + 3
	If sButtons = "YESNO" Then lType = lType + 4
	If sButtons = "" OR sButtons = "OKCANCEL" Then lType = lType + 1

	Dim sResult
	sResult = CInt(MsgBox(sText, lType, sTitle))

	If sResult = 1 Then efIEMsgBox = "OK"
	If sResult = 2 Then efIEMsgBox = "CANCEL"
	If sResult = 6 Then efIEMsgBox = "YES"
	If sResult = 7 Then efIEMsgBox = "NO"
	
	
End Function