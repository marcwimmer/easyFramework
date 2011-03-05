'================================================================================
'Class:     OnApplicationStart

'--------------------------------------------------------------------------------'
'Module:    OnApplicationStart.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   handles the OnApplicationStart-event in global.asax
'--------------------------------------------------------------------------------'
'Created:   29.06.2004 08:10:00 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'
Imports easyFramework.sys
Imports easyFramework.sys.SysEvents.Interfaces
Imports easyFramework.sys.Recordset
Imports easyFramework.sys.Data.DataMethods
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data
Imports easyFramework.sys.Xml

Imports pearson.Websites.WebsiteBL

Public Class OnApplicationStart
    Implements ISysEvents

    '================================================================================
    'Sub:       gHandleAfter
    '--------------------------------------------------------------------------------'
    'Purpose:   sends after submission a coupon, if activated at survey-level
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   29.06.2004 08:40:19 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Sub gHandleAfter(ByVal oClientInfo As sys.ClientInfo, ByVal oParam As Object, _
        ByRef oReturn As Object, ByRef bRollback As Boolean) _
        Implements sys.SysEvents.Interfaces.ISysEvents.gHandleAfter

        Dim oApplication As System.Web.HttpApplicationState = _
            CType(oParam, System.Web.HttpApplicationState)

        '---------------init csapp-ini-------------
        Dim oCSAppINI As CSAppIni

        oCSAppINI = New CSAppIni(oClientInfo.oHttpServer.oHttpServer.MapPath( _
                gsCStr(oApplication("sWebPageRoot")) & "/csapp.ini"))
        oApplication("CSAPPINI") = oCSAppINI



    End Sub

    Public Sub gHandleBefore(ByVal oClientInfo As sys.ClientInfo, ByVal oParam As Object, ByRef oReturn As Object, ByRef bCancel As Boolean) Implements sys.SysEvents.Interfaces.ISysEvents.gHandleBefore

    End Sub




End Class
