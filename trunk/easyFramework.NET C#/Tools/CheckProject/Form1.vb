Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Vom Windows Form Designer generierter Code "

    Public Sub New()
        MyBase.New()

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Initialisierungen nach dem Aufruf InitializeComponent() hinzufügen

    End Sub

    ' Die Form überschreibt den Löschvorgang der Basisklasse, um Komponenten zu bereinigen.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' Für Windows Form-Designer erforderlich
    Private components As System.ComponentModel.IContainer

    'HINWEIS: Die folgende Prozedur ist für den Windows Form-Designer erforderlich
    'Sie kann mit dem Windows Form-Designer modifiziert werden.
    'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabPage_CheckProject As System.Windows.Forms.TabPage
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents tabPage_UniformProjects As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCompileOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents cboConfiguration As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents txtAppPath As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.txtAppPath = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabPage_CheckProject = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.tabPage_UniformProjects = New System.Windows.Forms.TabPage
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.cboConfiguration = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.txtCompileOutputPath = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.tabPage_CheckProject.SuspendLayout()
        Me.tabPage_UniformProjects.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(608, 23)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "System-Components directory (e.g. c:\projects\easyFramework C#)"
        '
        'txtAppPath
        '
        Me.txtAppPath.Location = New System.Drawing.Point(16, 40)
        Me.txtAppPath.Name = "txtAppPath"
        Me.txtAppPath.Size = New System.Drawing.Size(472, 20)
        Me.txtAppPath.TabIndex = 3
        Me.txtAppPath.Text = "txtAppPath"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(488, 40)
        Me.Button2.Name = "Button2"
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "..."
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabPage_CheckProject)
        Me.TabControl1.Controls.Add(Me.tabPage_UniformProjects)
        Me.TabControl1.Location = New System.Drawing.Point(16, 88)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(824, 432)
        Me.TabControl1.TabIndex = 5
        '
        'tabPage_CheckProject
        '
        Me.tabPage_CheckProject.Controls.Add(Me.Button1)
        Me.tabPage_CheckProject.Controls.Add(Me.TextBox1)
        Me.tabPage_CheckProject.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_CheckProject.Name = "tabPage_CheckProject"
        Me.tabPage_CheckProject.Size = New System.Drawing.Size(816, 406)
        Me.tabPage_CheckProject.TabIndex = 0
        Me.tabPage_CheckProject.Text = "Check Project"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(312, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(216, 24)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Check Project (no data is changed)"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 48)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(800, 304)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = ""
        '
        'tabPage_UniformProjects
        '
        Me.tabPage_UniformProjects.Controls.Add(Me.TextBox2)
        Me.tabPage_UniformProjects.Controls.Add(Me.cboConfiguration)
        Me.tabPage_UniformProjects.Controls.Add(Me.Label3)
        Me.tabPage_UniformProjects.Controls.Add(Me.GroupBox1)
        Me.tabPage_UniformProjects.Controls.Add(Me.Label2)
        Me.tabPage_UniformProjects.Location = New System.Drawing.Point(4, 22)
        Me.tabPage_UniformProjects.Name = "tabPage_UniformProjects"
        Me.tabPage_UniformProjects.Size = New System.Drawing.Size(816, 406)
        Me.tabPage_UniformProjects.TabIndex = 1
        Me.tabPage_UniformProjects.Text = "Uniformation"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(8, 192)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(800, 208)
        Me.TextBox2.TabIndex = 5
        Me.TextBox2.Text = ""
        '
        'cboConfiguration
        '
        Me.cboConfiguration.Items.AddRange(New Object() {"Debug", "Release"})
        Me.cboConfiguration.Location = New System.Drawing.Point(184, 64)
        Me.cboConfiguration.Name = "cboConfiguration"
        Me.cboConfiguration.Size = New System.Drawing.Size(128, 21)
        Me.cboConfiguration.TabIndex = 4
        Me.cboConfiguration.Text = "DEBUG"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 23)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "choose configuration:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.txtCompileOutputPath)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 96)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(752, 88)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "set compiler-outputpath for configuration:"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(288, 56)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(104, 24)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "set output-path"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(672, 29)
        Me.Button3.Name = "Button3"
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "..."
        '
        'txtCompileOutputPath
        '
        Me.txtCompileOutputPath.Location = New System.Drawing.Point(96, 29)
        Me.txtCompileOutputPath.Name = "txtCompileOutputPath"
        Me.txtCompileOutputPath.Size = New System.Drawing.Size(576, 20)
        Me.txtCompileOutputPath.TabIndex = 3
        Me.txtCompileOutputPath.Text = "TextBox3"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(16, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 23)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "output-path:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(752, 32)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Caution: changes are made to all *.csproj-files in all subdirectories of the give" & _
        "n path! if you are using source-safe, then you should check out your files befor" & _
        "e proceeding"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(672, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 24)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Version 1.0b"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(848, 526)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.txtAppPath)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "CheckProject"
        Me.TabControl1.ResumeLayout(False)
        Me.tabPage_CheckProject.ResumeLayout(False)
        Me.tabPage_UniformProjects.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim WithEvents oCheck As New checkProjectFiles
    Dim WithEvents oChange As New changeProjectFiles

    Public Const ProjectNameForRegistry As String = "checkProject C#"
    Public Const regSetting As String = "settings"

   
    Private Sub oCheck_gWriteInfo(ByVal sValue As String) Handles oCheck.gWriteInfo

        Me.TextBox1.Text &= vbCrLf & vbCrLf & sValue

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        FolderBrowserDialog1.SelectedPath = txtAppPath.Text
        FolderBrowserDialog1.ShowDialog()

        txtAppPath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtAppPath.Text = GetSetting(ProjectNameForRegistry, regSetting, "path", Application.StartupPath)
        txtCompileOutputPath.Text = GetSetting(ProjectNameForRegistry, regSetting, "outputpath", "")
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSetting(ProjectNameForRegistry, regSetting, "path", txtAppPath.Text)
        SaveSetting(ProjectNameForRegistry, regSetting, "outputpath", txtCompileOutputPath.Text)
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.TextBox1.Text = "Started at: " & Now.ToString

        oCheck.gStart(txtAppPath.Text)

        Me.TextBox1.Text &= vbCrLf & "Done at " & Now.ToString
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        FolderBrowserDialog1.SelectedPath = txtCompileOutputPath.Text
        FolderBrowserDialog1.ShowDialog()

        txtCompileOutputPath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Try

            If MsgBox("Following things are changed in *.csproj-files and *.csproj.user-files: " & vbCrLf & _
                "- references with hint-paths are set to the output-paths" & vbCrLf & _
                "- Private=False is added to those reference-elements" & vbCrLf & _
                "- the reference-path is set to the output-path", MsgBoxStyle.Information Or MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then Exit Sub


            TextBox2.Text = ""
            oChange.gSetOutputPath(txtAppPath.Text, cboConfiguration.Text, txtCompileOutputPath.Text)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub oChange_gWriteInfo(ByVal sValue As String) Handles oChange.gWriteInfo
        Me.TextBox2.Text &= vbCrLf & vbCrLf & sValue

    End Sub
End Class
