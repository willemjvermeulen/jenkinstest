Imports Intuitive
Imports System.Text

Public Class UserGroupPropertyGroup

    Public Shared Sub SetPropertyGroup(ByVal iUserGroupID As Integer, _
        ByVal aPropertyGroupIDs As ArrayList)

        Dim sb As New StringBuilder
        Dim iPropertyGroupID As Integer
        For Each iPropertyGroupID In aPropertyGroupIDs
            sb.Append(iPropertyGroupID).Append(",")
        Next

        Dim sCSVPropertyGroupIDs As String = Intuitive.Functions.Chop(sb.ToString)

        SQL.Execute("exec AddUserGroupPropertyGroups {0},{1}", _
            iUserGroupID, SQL.GetSqlValue(sCSVPropertyGroupIDs, "String"))

    End Sub
End Class
