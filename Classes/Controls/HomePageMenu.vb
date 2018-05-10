Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.CookieFunctions
Public Class HomePageMenu
    Inherits Intuitive.WebControls.XSL



    Private Sub HomePageMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.XSLTemplate = "XSL/HomePageMenu.xsl"
        Me.SourceSql = "exec XMLHomePageMenu " & UserSession.SystemUserID

        'if we have the access right group id in a cookie then restore it
        Dim iLastAccessRightGroupID As Integer = SafeInt(Cookies.GetValue("LastAccessRightGroupID"))
        If iLastAccessRightGroupID > 0 Then
            Me.XSLParameters.AddParam("SelectedID", iLastAccessRightGroupID.ToString)
        End If

        'if we have a referrer, try and get the selected group
        'If Not Me.Page.Request.UrlReferrer Is Nothing Then

        '    Dim sReferringPage As String = Me.Page.Request.UrlReferrer.AbsolutePath
        '    Dim iSecureIndex As Integer = sReferringPage.ToLower.IndexOf("/secure")

        '    If iSecureIndex > 0 Then
        '        sReferringPage = sReferringPage.Substring(iSecureIndex + 1)

        '        Dim sGroup As String = SQL.GetValue( _
        '            "select AccessRightGroup from AccessRight " & _
        '            " inner join AccessRightGroup " & _
        '            " on AccessRight.AccessRightGroupID=AccessRightGroup.AccessRightGroupID " & _
        '            "where AccessRight.AccessRightURL={0}", SQL.GetSqlValue(sReferringPage))

        '        If sGroup <> "" Then
        '            Me.XSLParameters.AddParam("SelectedGroup", sGroup)
        '        End If
        '    End If



        'End If
    End Sub





End Class
