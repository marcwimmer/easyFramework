Public Class changeProjectFiles

    Public Event gWriteInfo(ByVal sValue As String)


    '================================================================================
    'Sub:       gSetOutputPath
    '--------------------------------------------------------------------------------'
    'Purpose:   sets the output-path for the given configuration
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   04.06.2004 00:48:27 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Sub gSetOutputPath(ByVal sAppPath As String, _
        ByVal sConfigName As String, ByVal sOutputPath As String)

        Dim asSubDirs As String() = System.IO.Directory.GetDirectories(sAppPath)
        Dim asFiles As String() = System.IO.Directory.GetFiles(sAppPath)


        For i As Integer = 0 To asFiles.Length - 1
            If LCase(Right(asFiles(i), 7)) = ".csproj" Then

                Dim oXmlDocument As New Xml.XmlDocument
                oXmlDocument.Load(asFiles(i))

                Dim oConfigNode As Xml.XmlNode = oXmlDocument.SelectSingleNode("//Config[@Name='" & _
                    sConfigName & "']")

                If oConfigNode Is Nothing Then
                    RaiseEvent gWriteInfo("Configuration """ & sConfigName & """ not found in " & asFiles(i))
                Else

                    oConfigNode.Attributes.GetNamedItem("OutputPath").Value = sOutputPath
                    RaiseEvent gWriteInfo("Configuration """ & sConfigName & """ updated in " & asFiles(i))

                End If



                '--------update the paths of the references; the shall point to the common-bin directory,
                '        where the compiler outputs the code---------------
                Dim oReferencesNodes As Xml.XmlNodeList = oXmlDocument.SelectNodes("//Reference")
                For y As Integer = 0 To oReferencesNodes.Count - 1
                    If oReferencesNodes(y).Attributes("HintPath") Is Nothing = False Then

                        Dim sValueBefore As String = oReferencesNodes(y).Attributes("HintPath").InnerText
                        Dim sNewValue As String
                        '----------DLL-Namen anfügen-------------
                        sNewValue = sOutputPath
                        If Right(sNewValue, 1) <> "\" Then
                            sNewValue &= "\"
                        End If
                        sNewValue &= oReferencesNodes(y).Attributes("Name").InnerText & ".dll"

                        If sNewValue <> sValueBefore Then
                            oReferencesNodes(y).Attributes("HintPath").InnerText = sNewValue
                            RaiseEvent gWriteInfo("Changed Reference-HintPath from """ & sValueBefore & """ to " & sOutputPath & """")
                        End If


                        '----------set private = false at this references----------------

                        If oReferencesNodes(y).Attributes("Private") Is Nothing Then
                            Dim oAttribute As System.Xml.XmlNode = oXmlDocument.CreateAttribute("Private")
                            oReferencesNodes(y).Attributes.SetNamedItem(oAttribute)
                        End If
                        oReferencesNodes(y).Attributes("Private").InnerText = "False"

                    End If
                Next

                



                oXmlDocument.Save(asFiles(i))
                oXmlDocument = Nothing

            ElseIf LCase(Right(asFiles(i), 12)) = ".csproj.user" Then

                Dim oXmlDocument As New Xml.XmlDocument
                oXmlDocument.Load(asFiles(i))

                oXmlDocument.SelectSingleNode("/VisualStudioProject/CSHARP/Build/Settings").Attributes("ReferencePath").InnerText = _
                    sOutputPath

                oXmlDocument.Save(asFiles(i))

            End If
        Next


        For i As Integer = 0 To asSubDirs.Length - 1
            gSetOutputPath(asSubDirs(i), sConfigName, sOutputPath)
        Next

    End Sub



End Class
