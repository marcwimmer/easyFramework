'================================================================================
'Class:     CouponHandler

'--------------------------------------------------------------------------------'
'Module:    CouponHandler.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   manages Coupons, which can be sent per Mail after fulfilling a survey
'--------------------------------------------------------------------------------'
'Created:   28.06.2004 19:29:35 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports pearson.Gutscheinlogik

Public Class CouponHandler


    '================================================================================
    'Sub:       gSendNewCoupon
    '--------------------------------------------------------------------------------'
    'Purpose:   creates a new gutschein and sends it to the user
    '--------------------------------------------------------------------------------'
    'Params:    oClientinfo - needed parameters
    '           sCouponSource - from the coupon comes
    '           dKontostand - kontostand des gutscheins 
    '           sProduktReihe - oder produktreihe
    '           bPrintable - ausdruckbares eBook oder nicht
    '           sEmail - the email of the recipient
    '           sSpeakTo - anrede wie herr, frau, lieber
    '           sFirstname - 
    '           sLastname - 
    '--------------------------------------------------------------------------------'
    'Created:   30.06.2004 08:14:20 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Shared Sub gSendNewCoupon(ByVal oClientInfo As ClientInfo, _
        ByVal sCouponSource As String, _
        ByVal dKontostand As Decimal, _
        ByVal sProduktReihe As String, _
        ByVal bPrintable As Boolean, _
        ByVal sCustomText As String, _
        ByVal sGutscheinDownloadURLName As String, _
        byval sSender as String, _
        ByVal sEmail As String, _
        ByVal sSpeakTo As String, _
        ByVal sFirstname As String, _
        ByVal sLastname As String)


        '
        'Dim oGutschein As New Gutschein
        'Dim sGutscheinNr As String = _
        ' oGutschein.gsNewGutschein(oClientInfo.oHttpApp.oHttpApplication, sCouponSource, dKontostand, sProduktReihe, bPrintable, sEmail)

        '  oGutschein.gSendGutschein(oClientInfo.oHttpApp.oHttpApplication, sEmail, sGutscheinNr, _
        '     sCustomText, sGutscheinDownloadURLName, sSender, _
        '    sSpeakTo, sFirstname, sLastname)

    End Sub


End Class
