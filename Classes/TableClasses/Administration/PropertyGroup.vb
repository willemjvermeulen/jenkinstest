Imports Intuitive
Public Class PropertyGroup
    Inherits TableBase

    Public Sub New()
        Me.Table = "PropertyGroup"
        With Me.Fields
            .Add(New Field("PropertyGroupID", "Integer"))
            .Add(New Field("PropertyGroupCode", "String", 10, ValidationType.NotEmptyNotDupe))
            .Add(New Field("PropertyGroup", "String", 25, ValidationType.NotEmptyNotDupe))
        End With
        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        If Not SQL.FKCheck("Property", "PropertyGroupID", iTableID) Then
            Me.Warnings.Add("The Property Group cannot be deleted as it contains one or more Properties")
        End If

        If Me.Warnings.Count = 0 Then
            'delete child records from UserGroupPropertyGroup
            SQL.Execute("Delete from UserGroupPropertyGroup Where PropertyGroupID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0

    End Function

    Private Sub PropertyGroup_AfterAdd(ByVal iTableID As Integer) Handles Me.AfterAdd
        SQL.Execute("insert into UserGroupPropertyGroup select UserGroupID, {0} from UserGroup", iTableID)
    End Sub
End Class
