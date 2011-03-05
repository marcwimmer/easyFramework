'================================================================================
'Class:     optionDialog
'--------------------------------------------------------------------------------'
'Module:    optionDialog.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   let's the user select, an option-value
'
'           this page is usually invoked by the javascript-function
'           gsShowOptionDialog() from efStandard.js
'--------------------------------------------------------------------------------'
'Created:   29.05.2004 22:11:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys


Public Class optionDialog
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents btnOk As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents btnAbort As easyFramework.Frontend.ASP.WebComponents.efButton

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object



    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Protected aoOptions As tOptionValues()
    Protected sText As String

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        Me.sTitle = "Auswahl"

        Dim sCaptions As String
        Dim sValues As String


        sCaptions = oXmlRequest.sGetValue("captions")
        sValues = oXmlRequest.sGetValue("values")

        '------seperated by "-||-"--------
        Const sSep As String = "-||-"
        If InStr(sCaptions, sSep) > 0 And InStr(sValues, sSep) > 0 Then

            Dim asCaptions As String() = Split(sCaptions, sSep)
            Dim asValues As String() = Split(sValues, sSep)

            For i As Integer = 0 To asCaptions.Length - 1
                If asValues(i) <> "" Then


                    If aoOptions Is Nothing Then
                        ReDim aoOptions(0)
                    Else
                        ReDim Preserve aoOptions(UBound(aoOptions) + 1)
                    End If

                    Dim oOptValue As tOptionValues = New tOptionValues
                    oOptValue.sCaption = asCaptions(i)
                    oOptValue.sValue = asValues(i)
                    aoOptions(UBound(aoOptions)) = oOptValue
                End If

            Next


        End If

        If aoOptions Is Nothing Then
            Throw New efException("Please give some options!")
        End If


        '---------------get text----------
        sText = oXmlRequest.sGetValue("text")


        '--------css & javascript-----------
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddScriptLink("../../js/efTabDialog.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddScriptLink("../../js/efIESpecials.js", True, "VBScript")
        gAddScriptLink("../../js/efOptionDialog.js", True)

    End Sub

    Protected Class tOptionValues
        Public sValue As String
        Public sCaption As String
    End Class

End Class


