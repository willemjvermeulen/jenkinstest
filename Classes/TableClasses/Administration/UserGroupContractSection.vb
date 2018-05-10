Imports Intuitive
Imports System.Text


Public Class UserGroupContractSection

    Public Shared Sub SetContractSections(ByVal iUserGroupID As Integer, _
        ByVal aContractSectionIDs As ArrayList)

        Dim sb As New StringBuilder
        Dim iContractSectionID As Integer
        For Each iContractSectionID In aContractSectionIDs
            sb.Append(iContractSectionID).Append(",")
        Next

        Dim sCSV As String = Intuitive.Functions.Chop(sb.ToString)

        SQL.Execute("exec AddUserGroupContractSections {0},{1}", _
            iUserGroupID, SQL.GetSqlValue(sCSV, "String"))

    End Sub
End Class
