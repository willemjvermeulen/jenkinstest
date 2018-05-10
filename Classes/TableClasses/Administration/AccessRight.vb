Imports Intuitive
Imports Intuitive.Functions

Public Class AccessRight
    Inherits TableBase

    Public Sub New()

        Me.Table = "AccessRight"

        With Me.Fields
            .Add(New Field("AccessRightID", "Integer"))
            .Add(New Field("AccessRightGroupID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("AccessRight", "String", 30, ValidationType.NotEmptyNotDupe, "AccessRightGroupID"))
            .Add(New Field("AccessRightURL", "String", 100, ValidationType.NotEmpty))
            Me.SetLastDisplayName("Access Right URL")
            .Add(New Field("CommonTask", "Boolean"))
            .Add(New Field("StartsGroup", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'delete child records from SystemUserAccessRight
            .Add("Delete from SystemUserAccessRight Where AccessRightID=" & iTableID.ToString)

            'delete child records from UserGroupAccessRight
            .Add("Delete from UserGroupAccessRight Where AccessRightID=" & iTableID.ToString)

            .Execute()

        End With

        Return Me.Warnings.Count = 0
    End Function

    Private Sub AccessRight_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        'set the sequence
        SQL.Execute("exec SetNewAccessRightSequence 'AccessRight',{0}", iTableID)
    End Sub
End Class

