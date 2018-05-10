Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Partial Class CallCentreMenu
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents divBookingMenu As System.Web.UI.HtmlControls.HtmlGenericControl

    Protected WithEvents PackageConfirmation As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public CurrentPage As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim oLink As HtmlGenericControl = CType(Me.FindControl(Me.CurrentPage), HtmlGenericControl)

        If Not oLink Is Nothing Then
            oLink.Attributes.Add("class", oLink.Attributes("class") & " selected")
        End If

        If Me.CurrentPage.StartsWith("Property") Then
            Me.mnuProperty.Attributes("class") = "menusection selected"
        ElseIf Me.CurrentPage.StartsWith("Package") Then
            Me.mnuPackage.Attributes("class") = "menusection selected"
        ElseIf Me.CurrentPage <> "" Then
            Me.mnuBooking.Attributes("class") = "menusection selected"
        End If
    End Sub

    Public Function GetBaseUrl() As String
        Return Request.RawUrl.Substring(0, Request.RawUrl.LastIndexOf("/"))
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim sb As New StringBuilder
        Dim sw As New StringWriter(sb)
        Dim htmlWriter As New HtmlTextWriter(sw)
        MyBase.Render(htmlWriter)

        Dim sHTML As String = sb.ToString

        writer.Write(sHTML.Replace("#BaseUrl#", ResolveUrl("~")))
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        Me.mnuPackage.Visible = CType(Session.Item("ShowPackage"), Boolean)
    End Sub
End Class
