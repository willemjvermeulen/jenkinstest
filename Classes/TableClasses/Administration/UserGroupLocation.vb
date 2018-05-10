Imports Intuitive

Public Class UserGroupLocation

    'addlocation
    Public Shared Function AddLocation(ByVal iUserGroupID As Integer, _
        ByVal sType As String, ByVal iID As Integer) As Integer

        Dim sField As String = ""
        Select Case sType
            Case "Geography Set"
                sField = "GeographySetID"
            Case "Level 1"
                sField = "GeographyLevel1ID"
            Case "Level 2"
                sField = "GeographyLevel2ID"
            Case "Level 3"
                sField = "GeographyLevel3ID"
            Case "Level 4"
                sField = "PropertyID"
        End Select

        Return SQL.ExecuteSingleValue("AddUserGroupLocation {0},{1},{2}", _
            iUserGroupID, SQL.GetSqlValue(sField, "String"), iID)

    End Function

    'delete location
    Public Shared Sub DeleteLocation(ByVal iUserGroupLocationID As Integer)

        SQL.Execute("Delete from UserGroupLocation Where UserGroupLocationID={0}", _
            iUserGroupLocationID)
    End Sub


End Class
