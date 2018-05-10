Imports Intuitive
Imports Intuitive.Functions

Public Class AccessRightGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "AccessRightGroup"

        With Me.Fields
            .Add(New Field("AccessRightGroupID", "Integer"))
            .Add(New Field("AccessRightGroup", "String", 20, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in AccessRight
        If Not SQL.FKCheck("AccessRight", "AccessRightGroupID", iTableID) Then
            Me.Warnings.Add("Access Right Group cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Private Sub AccessRightGroup_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        'set the sequence
        SQL.Execute("exec SetNewAccessRightSequence 'AccessRightGroup',{0}", iTableID)
    End Sub
End Class

