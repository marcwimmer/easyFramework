'================================================================================
'Class:     storeDialogData
'--------------------------------------------------------------------------------'
'Module:    storeDialogData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   used to retrieve the data for loading into the dialog   
'--------------------------------------------------------------------------------'
'Created:   05.05.2004 11:56:25 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys


Public Class storeDialogData
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

    Public Const efColumnSep As String = ".|."
    Public Const efLineEnd As String = "-||-"


    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        '------------get parameters --------------------------
        Dim sMultiStructFilename As String
        Dim sTopElementName As String
        Dim sTopElementValue As String

        If oRequest.selectSingleNode("//multistructurexml") Is Nothing Then
            Throw New efException("Request-parameter ""multistructurexml"" needed!")
        Else
            sMultiStructFilename = oRequest.selectSingleNode("//multistructurexml").sText
        End If

        If oRequest.selectSingleNode("//topelementname") Is Nothing Then
            Throw New efException("Request-parameter ""multistructurexml"" needed!")
        Else
            sTopElementName = oRequest.selectSingleNode("//topelementname").sText
        End If

        If oRequest.selectSingleNode("//topelementvalue") Is Nothing Then
            Throw New efException("Request-parameter ""topelementvalue"" needed!")
        Else
            sTopElementValue = oRequest.selectSingleNode("//topelementvalue").sText
        End If

        '------------load the multistruct-xml-----------------
        Dim oXml As New XmlDocument
        oXml.gLoad(Tools.sWebToAbsoluteFilename(Request, sMultiStructFilename, False))


        '-----------check multistructure for needed fields---------
        If oXml.selectSingleNode("/multistructure/topmostelement") Is Nothing Then
            Throw New efException("The ""topmostelement""-tag must be given under tag ""multistructure""!")
        End If
        If oXml.selectSingleNode("/multistructure/topmostelement/entity") Is Nothing Then
            Throw New efException("The ""topmostelement/entity""-tag must be given under tag ""multistructure""!")
        End If
        Dim sTopMostElementEntity As String = oXml.selectSingleNode("/multistructure/topmostelement/entity").sText


        '-------------begin with the top-most entity---------------------------
        Dim oResult As New FastString

        Dim oEntity As DefaultEntity = EntityLoader.goLoadEntity(oClientInfo, sTopMostElementEntity)

        oEntity.gLoad(oClientInfo, sTopElementValue)

        'mWriteDataLine(oResult, "", "", sTopMostElementEntity & sTopElementValue, moGetFieldsFromEntity(oEntity))


        Dim oLevelNode As XmlNode = oXml.selectSingleNode("/multistructure/level")
        If oLevelNode Is Nothing Then
            Throw New efException("Multistructure-xml needs level-nodes!")
        End If


        '---------------check, if it is a paged dialog----------------------------
        Dim lPageSize As Integer = glCInt(oXml.selectSingleNode("/multistructure").oAttribute("pagesize").sText)
        Dim lPage As Integer = glCInt(oRequest.sGetValue("page", "0"))
        If lPageSize = 0 Then
            lPage = 0
        End If


        mAppendSubEntities(oXml, oResult, oEntity, Nothing, True, lPage, lPageSize)



        Return "OK" & efLineEnd & oResult.ToString

    End Function





    '================================================================================
    'Private Methods:
    '================================================================================


    '================================================================================
    'Sub:       mAppendSubEntities
    '--------------------------------------------------------------------------------'
    'Purpose:   appends all children-entities of the entity; called recursivley
    '--------------------------------------------------------------------------------'
    'Params:    oresult: the result-faststring
    '           oTopEntity: the loaded top-entity
    '           oLevelNode: 
    '           lPage, lPageSize: if paged xml-multistructre, then put the values here;
    '                             if 0 then they are ignored
    '--------------------------------------------------------------------------------'
    'Created:   05.05.2004 15:51:12 
    '--------------------------------------------------------------------------------'
    'Changed:   30.05.2004 - condition-tag
    '--------------------------------------------------------------------------------'
    Private Sub mAppendSubEntities( _
        ByVal oMultiStructXml As XmlDocument, _
        ByVal oResult As FastString, _
        ByVal oTopEntity As IEntity, _
        ByVal oParentLevelNode As XmlNode, _
        ByVal bIsFirstLevel As Boolean, _
        ByVal lPage As Integer, _
        ByVal lPageSize As Integer)


        Dim sThisLevel As String
        Dim sTopLevel As String
        Dim sEntity As String

        '-----------------get all sub-level-nodes of the parent-level---------
        Dim nlSubLevelNodes As XmlNodeList
        If oParentLevelNode Is Nothing Then
            nlSubLevelNodes = oMultiStructXml.selectNodes("/multistructure/level")
        Else
            nlSubLevelNodes = oParentLevelNode.selectNodes("level")
        End If


        '-------iterate each level-node--------
        For i As Integer = 0 To nlSubLevelNodes.lCount - 1
            Dim oLevelNode As XmlNode = nlSubLevelNodes(i)

            If oLevelNode.selectSingleNode("relation") Is Nothing Then
                Throw New efException("Node ""relation"" required in level-node: " & oLevelNode.sXml)
            End If
            If oLevelNode.selectSingleNode("relation/thislevel") Is Nothing Then
                Throw New efException("Node ""relation/thislevel"" required in level-node: " & oLevelNode.sXml)
            Else
                sThisLevel = oLevelNode.selectSingleNode("relation/thislevel").sText
            End If
            If oLevelNode.selectSingleNode("relation/toplevel") Is Nothing Then
                Throw New efException("Node ""relation/toplevel"" required in level-node: " & oLevelNode.sXml)
            Else
                sTopLevel = oLevelNode.selectSingleNode("relation/toplevel").sText
            End If
            If oLevelNode.selectSingleNode("entity") Is Nothing Then
                Throw New efException("Entity-element expected for level-node")
            Else
                sEntity = oLevelNode.selectSingleNode("entity").sText
            End If

            Dim oEntity As DefaultEntity = EntityLoader.goLoadEntity(oClientInfo, sEntity)


            '------------------get sort-column-----------------
            Dim sSortField As String
            If oLevelNode.selectSingleNode("sortfield") Is Nothing = False Then
                sSortField = oLevelNode.selectSingleNode("sortfield").sText
            Else
                sSortField = ""
            End If

            '------------------select sub-entities of main entity ----------------------------
            Dim sClause As String
            sClause = sThisLevel & "=" & oTopEntity.oFields(sTopLevel).sGetForSQL

            Dim rsSubEntities As Recordset = oEntity.gRsSearch(oClientInfo, sClause, sSortField)

            '---------if paged, then jump to the offset--------------------
            If lPage > 0 Then
                rsSubEntities.Move((lPage - 1) * lPageSize)
            End If

            Dim lCounter As Integer
            lCounter = 0

            Do While Not rsSubEntities.EOF
                
                '-----------load entity----------------
                oEntity.gLoad(oClientInfo, rsSubEntities(oEntity.sKeyFieldName).sValue)

                '----------if there is a condition-tag, check if condition is true--------
                Dim bConditionMatching As Boolean = True
                If oLevelNode.selectSingleNode("condition") Is Nothing = False Then
                    bConditionMatching = mbIsCondition(oLevelNode.selectSingleNode("condition"), _
                        oEntity)
                End If

                If bConditionMatching = True Then


                    '-------append children--------

                    Dim sParentId As String
                    If bIsFirstLevel Then
                        sParentId = ""
                    Else
                        sParentId = oTopEntity.sName & "_" & oTopEntity.oKeyField.sValue
                    End If

                    mWriteDataLine(oResult, oLevelNode.oAttribute("name").sText, _
                        sParentId, _
                        oEntity.sName & "_" & oEntity.oKeyField.sValue, moGetFieldsFromEntity(oEntity))


                    mAppendSubEntities(oMultiStructXml, oResult, oEntity, oLevelNode, False, 0, 0)

                End If

                rsSubEntities.MoveNext()

                '-----------if paged and page-limit is reach, then stop-------------
                lCounter += 1
                If lCounter >= lPageSize And lPageSize > 0 Then
                    Exit Do
                End If

            Loop

        Next

    End Sub



    '================================================================================
    'Sub:       mWriteDataLine
    '--------------------------------------------------------------------------------'
    'Purpose:   write a concrete line to the result-string
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   05.05.2004 15:29:48 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Private Sub mWriteDataLine(ByVal oCurrentString As FastString, _
        ByVal sLevelName As String, _
        ByVal sParentID As String, _
        ByVal sId As String, _
        ByVal sColumns As efArrayList)


        oCurrentString.Append(sLevelName & efColumnSep)

        oCurrentString.Append(sParentID & efColumnSep)

        oCurrentString.Append(sId & efColumnSep)

        For i As Integer = 0 To sColumns.Count - 1
            Dim oField As TField = CType(sColumns(i), TField)
            oCurrentString.Append(Replace(oField.sName, efColumnSep, " ") & efColumnSep)
            oCurrentString.Append(Replace(oField.sValue, efColumnSep, " ") & efColumnSep)
        Next

        oCurrentString.Append(efLineEnd)
    End Sub



    '================================================================================
    'Function:  oGetFieldsFromEntity
    '--------------------------------------------------------------------------------'
    'Purpose:   retrieves the columns from an entity and puts it into an arraylist
    '--------------------------------------------------------------------------------'
    'Params:    the entity
    '--------------------------------------------------------------------------------'
    'Returns:   arraylist
    '--------------------------------------------------------------------------------'
    'Created:   05.05.2004 15:25:58 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Private Function moGetFieldsFromEntity(ByVal oEntity As DefaultEntity) As efArrayList

        Dim oResult As New efArrayList

        For i As Integer = 0 To oEntity.oFields.Count - 1

            Dim oField As TField
            oField = New TField
            oField.sName = oEntity.oFields(i).sName

            If oEntity.oFields(i).enType = RecordsetObjects.Field.efEnumFieldType.efBool Then
                If oEntity.oFields(i).bValue = True Then
                    oField.sValue = "1"
                Else
                    oField.sValue = "0"
                End If
            Else
                oField.sValue = oEntity.oFields(i).sValue
            End If

            oResult.Add(oField)


        Next

        Return oResult
    End Function



    '================================================================================
    'Function:  bIsCondition
    '--------------------------------------------------------------------------------'
    'Purpose:   checks, wether the condition tag matches or not
    '--------------------------------------------------------------------------------'
    'Params:    oConditionTag - the condition tag <condition...
    '           oEntity - the entity of the condition           
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   30.05.2004 18:09:44 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Private Function mbIsCondition(ByVal oConditionTag As XmlNode, _
        ByVal oEntity As IEntity) As Boolean


        '------------------check for entity-field---------------------
        Dim sFieldName As String
        If oConditionTag.selectSingleNode("entityfield") Is Nothing Then
            Throw New efException("Tag ""entityfield"" is missing under condition-tag!")
        Else
            sFieldName = oConditionTag.selectSingleNode("entityfield").sText
        End If

        '----------------get values-------------
        Dim nlValues As XmlNodeList = oConditionTag.selectNodes("value")
        If nlValues.lCount = 0 Then
            Throw New efException("At least one value-tag should be supplied under the condition-tag!")
        End If

        '----------check field-name--------------------------
        If Not oEntity.oFields.gbIsField(sFieldName) Then
            Throw New efException("Invalid field-name """ & sFieldName & """ of entity """ & oEntity.sName & """.")
        End If

        '------------------------- iterate values --------------------
        Dim sFieldValue As String = oEntity.oFields(sFieldName).sValue
        For i As Integer = 0 To nlValues.lCount - 1

            If LCase(nlValues(i).sText) = LCase(sFieldValue) Then
                Return True

            End If

        Next

        Return False

    End Function


    Private Class TField
        Public sName As String
        Public sValue As String
    End Class



End Class
