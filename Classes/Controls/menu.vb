Imports System.Text
Imports Intuitive
Imports Intuitive.WebControls
Imports Intuitive.Functions

Public Class Menu
    Inherits Intuitive.WebControls.Menu


    Public Overrides Sub Setup()
        Dim dtMenu As DataTable
        Dim sCacheName As String = "menuItem" & UserSession.UserGroupID

        'try and get the menu from the cache 
        dtMenu = CType(HttpContext.Current.Cache(sCacheName), DataTable)
        If dtMenu Is Nothing Then

            dtMenu = SQL.GetDatatable("exec GetHomePageMenu " & UserSession.UserGroupID)

            AddToCache(sCacheName, dtMenu, "UserGroupAccessRight.txt")
        End If

        Me.PopulateMenu(dtMenu)
    End Sub
End Class