Imports Intuitive
Imports Intuitive.Functions
Imports System.Text

'Access Keys collection
Public Class AccessKeys
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal AccessKey As AccessKey)
        List.Add(AccessKey)
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As AccessKey
        Get
            Return CType(List(Index), AccessKey)
        End Get
    End Property

    Public Sub DrawAccessKeys(ByVal oPage As Page)
        'Get base URL
        Dim sBaseUrl As String = System.Configuration.ConfigurationManager.AppSettings.Get("BaseURL")

        'Build controls
        Dim oSB As New StringBuilder

        For i As Integer = 0 To Me.Count - 1
            oSB.AppendFormat("<input type=""button"" class=""hotkey"" tabindex=""-1"" " & _
                    "accesskey=""{0}"" onclick=""javascript:document.location='{1}';""/>\n", _
                    Me(i).Key, oPage.ResolveUrl("~/" + Me(i).URL))
        Next

        oSB.AppendFormat("<input type=""button"" class=""hotkey"" tabindex=""-1"" " & _
        "accesskey=""1"" onclick=""javascript:document.location='{0}';""/>\n", _
        oPage.ResolveUrl("~/Secure/Homepage.aspx"))

        Dim oControls As New LiteralControl
        oControls.Text = oSB.ToString.Replace("\n", ControlChars.NewLine)

        Dim oForm As HtmlForm = CType(oPage.FindControl("frm"), HtmlForm)
        oForm.Controls.AddAt(oPage.Controls(1).Controls.Count, oControls)
    End Sub
End Class

'Access Key
Public Class AccessKey
    Public Key As Integer
    Public URL As String
End Class
