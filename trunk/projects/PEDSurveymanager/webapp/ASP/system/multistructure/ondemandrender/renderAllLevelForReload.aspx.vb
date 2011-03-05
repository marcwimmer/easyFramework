'================================================================================
'Class:     renderAllLevelForReload

'--------------------------------------------------------------------------------'
'Module:    renderAllLevelForReload.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   optimizes the loading of a multistructure
'           usually each level is rendered via the renderLevel.aspx
'           if the multistructure has to load 10 items with several
'           sub-items, then each time a web-access to the renderlevel.aspx
'           has to be done;
'           this asp renders the html of all elements in a multistructure
'           and returns it.
'--------------------------------------------------------------------------------'
'Created:   02.05.2004 18:56:53 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools.Tools
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys
Imports easyFramework

Public Class renderAllLevelForReload
    Inherits efDataPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Protected sXmlFilename As String
    Protected sXmlDialogPraefix As String
    Protected sHtmlFormName As String
    Protected oXmlDefinition As New XmlDocument

    Protected oDataIdToLevelId As New efHashTable 'stores the key in the data-stream and the corresponding level-id


    Public Overrides Function sGetData(ByVal oClientInfo As Sys.ClientInfo, ByVal oRequest As Sys.Xml.XmlDocument) As String


        sHtmlFormName = oRequest.sGetValue("formname")
        sXmlFilename = oRequest.sGetValue("multixml")
        Dim sData As String = oRequest.sGetValue("data")
        sXmlDialogPraefix = oRequest.sGetValue("xmldialogpraefix")


        '-------------------load the xml-definition of the struct----------------------
        If Left(sXmlFilename, 1) <> "/" Then
            Throw New efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer.")
        End If


        sXmlFilename = Server.MapPath(sXmlFilename)
        oXmlDefinition.gLoad(sXmlFilename)



        '-----------------render and return html------------------------------
        Dim oMultiStructure As New MultistructureRenderer

        Dim oResultHtml As New FastString
        If Left(sData, 6) = "OK-||-" Then sData = Right(sData, Len(sData) - 6)
        Dim asSplittedDatalines As String() = Split(sData, "-||-")


        gBuildHtml(oResultHtml, asSplittedDatalines, 0, oMultiStructure, "")



        Return "OK-||-" & oResultHtml.ToString


    End Function


    '================================================================================
    'Sub:       gBuildHtml
    '--------------------------------------------------------------------------------'
    'Purpose:   makes HTML out of a line of the data; steps through the data-lines and
    '           calls itself iterativley
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   17.06.2004 16:02:57 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Sub gBuildHtml(ByRef oResult As FastString, ByRef asSplittedDatalines As String(), _
        ByVal lDataLine As Integer, ByVal oMultiStructure As MultistructureRenderer, _
        ByVal sParentLevelId As String)


        Dim sLevelName As String
        Dim sId As String
        Dim sParentId As String

        '-----------get values from data-line-------------------
        For i As Integer = lDataLine To asSplittedDatalines.Length - 2

            Dim asDataLine As String() = Split(asSplittedDatalines(i), ".|.")

            sLevelName = asDataLine(0)
            sId = asDataLine(2)

            '-----transform the parent-id to a level-id--------------
            sParentId = asDataLine(1)
            sParentId = msResolveIdToLevelId(sParentId)

            Dim sLevelId As String = msGetNextLevelId(sParentId)
            mAddNewLevelIdPair(sLevelId, sId)


            '------------------get the data, to be added to the dialog---------------
            Dim oXmlData As XmlDocument = moXmlGetDataForDialogInput(asSplittedDatalines(i))


            '-------------------build result html-----------------------

            oResult.Append(sLevelId)

            oResult.Append("||||||--------||||||")

            oResult.Append(oMultiStructure.gsRenderSpecificLevel(oClientInfo, _
                oXmlDefinition, _
                sHtmlFormName, _
                sXmlDialogPraefix & sLevelId, _
                sLevelId, _
                sLevelName, _
                Request, Application, Server, oXmlData))

            

            oResult.Append("||||||*****||||||")


        Next


    End Sub


    '================================================================================
    'Function:  msGetNextLevelId
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the next-level-id
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   17.06.2004 16:21:38 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected oLevelIds As New efArrayList 'for optimized storing of the level-ids
    Protected Function msGetNextLevelId(ByVal sParentLevelId As String) As String

        If sParentLevelId = "" Then

            Dim sLastLevelId As String
            Dim sLevelId As String
            For i As Integer = oLevelIds.Count - 1 To 0 Step -1

                sLevelId = gsCStr(oLevelIds(i))

                If Split(sLevelId, "_").Length = 3 Then
                    Exit For
                End If

            Next

            Dim sNextLevelId As String
            If sLevelId = "" Then
                sNextLevelId = "LV_1_"
            Else
                Dim lNumber As Integer = glCInt(Split(sLevelId, "_")(1))
                lNumber += 1

                sNextLevelId = "LV_" & lNumber & "_"

            End If

            oLevelIds.Add(sNextLevelId)

            Return sNextLevelId

        Else

            Dim sLastLevelId As String
            Dim sLevelId As String
            Dim lHierarchyParent As Integer = Split(sParentLevelId, "_").Length
            For i As Integer = oLevelIds.Count - 1 To 0 Step -1

                sLevelId = gsCStr(oLevelIds(i))

                If Microsoft.VisualBasic.Left(sLevelId, Len(sParentLevelId)) = sParentLevelId Then

                    If Split(sLevelId, "_").Length - 1 = lHierarchyParent Then
                        sLastLevelId = sLevelId
                        Exit For
                    End If

                End If

            Next

            Dim sNextLevelId As String
            If sLastLevelId = "" Then
                sNextLevelId = sParentLevelId & "1_"
            Else
                Dim lNumber As Integer = glCInt(Split(sLastLevelId, "_")(Split(sLastLevelId, "_").Length - 2))
                lNumber += 1

                sNextLevelId = sParentLevelId & lNumber & "_"

            End If

            oLevelIds.Add(sNextLevelId)

            Return sNextLevelId

        End If

    End Function



    '================================================================================
    'Function:  msResolveIdToLevelId
    '--------------------------------------------------------------------------------'
    'Purpose:   searches, wether for then given data-id a level-id exists
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   17.06.2004 16:24:10 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function msResolveIdToLevelId(ByVal sId As String) As String

        If sId <> "" Then

            If oDataIdToLevelId.ContainsKey(sId) Then
                Return gsCStr(oDataIdToLevelId(sId))
            End If
        End If

    End Function


    '================================================================================
    'Sub:       mAddNewLevelIdPair
    '--------------------------------------------------------------------------------'
    'Purpose:   adds a new pair to the hash-table
    '--------------------------------------------------------------------------------'
    'Params:    sLevelId - the id of the level
    '           sDataId - the data-value
    '--------------------------------------------------------------------------------'
    'Created:   17.06.2004 16:29:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Sub mAddNewLevelIdPair(ByVal sLevelId As String, ByVal sDataId As String)

        If Not oDataIdToLevelId.ContainsKey(sDataId) Then
            oDataIdToLevelId.Add(sDataId, sLevelId)
        End If

    End Sub


    '================================================================================
    'Function:  mlGetNextSortValueOfLevel
    '--------------------------------------------------------------------------------'
    'Purpose:   
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   18.06.2004 00:51:24 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function mlGetNextSortValueOfLevel(ByVal sLevelId As String) As Integer

        Dim sLastNumber As Integer = glCInt(Split(sLevelId, "_")(UBound(Split(sLevelId, "_")) - 1))
        Return sLastNumber

    End Function

    '================================================================================
    'Function:  moXmlGetDataForDialogInput
    '--------------------------------------------------------------------------------'
    'Purpose:   transforms an data-line of multistructure, to a dialog-input 
    '           xml-document
    '--------------------------------------------------------------------------------'
    'Params:    the line of data of the multistructure
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   17.06.2004 23:46:24 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function moXmlGetDataForDialogInput(ByVal sDataLine As String) As XmlDocument

        Dim asSplitted As String() = Split(sDataLine, ".|.")

        Dim oXmlResult As New XmlDocument("<DIALOGINPUT/>")

        For i As Integer = 3 To asSplitted.Length - 1 Step 2
            If asSplitted(i) <> "" Then
                oXmlResult.selectSingleNode("/DIALOGINPUT").AddNode(asSplitted(i), True).sText = asSplitted(i + 1)
            End If
        Next

        Return oXmlResult

    End Function

End Class
