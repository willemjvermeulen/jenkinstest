Imports Intuitive
Imports Intuitive.Functions

Public Class SystemUserAccessRight

    Private iAccessKey As Integer = Nothing
    Private sAccessRight As String = ""
    Private iNextAvaliableKey As Integer

#Region "Properties"
    Public Property AccessKey() As Integer
        Get
            Return iAccessKey
        End Get
        Set(ByVal Value As Integer)
            iAccessKey = Value
        End Set
    End Property
    Public Property AccessRight() As String
        Get
            Return sAccessRight
        End Get
        Set(ByVal Value As String)
            sAccessRight = Value
        End Set
    End Property
    Public Property NextAvaliableKey() As Integer
        Get
            Return iNextAvaliableKey
        End Get
        Set(ByVal Value As Integer)
            iNextAvaliableKey = Value
        End Set
    End Property
#End Region

    Public Function Go(ByVal iAccessRightID As Integer) As Boolean
        Dim dr As DataRow = _
            SQL.GetDataRow("GetAccessKey {0}, {1}", _
                            iAccessRightID, UserSession.SystemUserID)
        If Not dr Is Nothing Then
            Me.AccessRight = dr("AccessRight").ToString
            Me.AccessKey = SafeInt(dr("AccessKey"))
            Me.NextAvaliableKey = SafeInt(dr("NextAvaliableKey"))
        End If
    End Function


    Public Sub Update(ByVal iAccessRightID As Integer, ByVal iAccessKey As Integer)
        'Procedure deletes then adds
        SQL.Execute("AddSystemUserAccessRight {0},{1},{2}", _
            UserSession.SystemUserID, iAccessRightID, iAccessKey)
    End Sub
End Class
