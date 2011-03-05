Public Class checkProjectFiles

    Public Event gWriteInfo(ByVal sValue As String)

    Public Sub gStart(ByVal sStartPath As String)


        mCheckVBProjectFiles(sStartPath)




    End Sub



    '================================================================================
    'Sub:       mCheckVBProjectFiles
    '--------------------------------------------------------------------------------'
    'Purpose:   iterates all project-files and checks, wether dlls are included with
    '           private <> "false"
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   04.06.2004 00:48:27 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Sub mCheckVBProjectFiles(ByVal sApplicationStartupPath As String)

        Dim asSubDirs As String() = System.IO.Directory.GetDirectories(sApplicationStartupPath)
        Dim asFiles As String() = System.IO.Directory.GetFiles(sApplicationStartupPath)


        For i As Integer = 0 To asFiles.Length - 1
            If LCase(Right(asFiles(i), 7)) = ".csproj" Then

                Dim oXmlDocument As New Xml.XmlDocument
                oXmlDocument.Load(asFiles(i))

                Dim oReferencesNode As Xml.XmlNode = oXmlDocument.SelectSingleNode("//References")

                For y As Integer = 0 To oReferencesNode.ChildNodes.Count - 1

                    Dim oRefNode As Xml.XmlNode = oReferencesNode.ChildNodes(y)

                    If oRefNode.Attributes("Private") Is Nothing Then

                        Dim sHintPath As String = ""
                        If Not oRefNode.Attributes("HintPath") Is Nothing Then
                            sHintPath = oRefNode.Attributes("HintPath").Value
                        End If


                        Dim sName As String = ""
                        If Not oRefNode.Attributes("Name") Is Nothing Then
                            sName = oRefNode.Attributes("Name").Value
                        End If

                        '----if it is not a system-assembly, then it should not be private------
                        If InStr(LCase(sHintPath), LCase("\Microsoft.NET\")) = 0 And _
                                sName <> "System" _
                            And LCase(Left(sName, Len("system."))) <> "system." _
                            And LCase(Left(sName, Len("msxml2."))) <> "msxml2." _
                            And LCase(Left(sName, Len("microsoft."))) <> "microsoft." _
                            And LCase(Left(sName, Len("mscorlib"))) <> "mscorlib" Then

                            RaiseEvent gWriteInfo("Warning: reference to " & _
                                """" & sName & """" & _
                                " should be private=""false"" in file " & _
                                asFiles(i))
                        End If

                    End If

                Next



            End If
        Next


        For i As Integer = 0 To asSubDirs.Length - 1
            mCheckVBProjectFiles(asSubDirs(i))
        Next




    End Sub


End Class

