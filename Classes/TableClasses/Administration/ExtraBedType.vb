Imports Intuitive

Public Class ExtraBedType
    Inherits TableBase

    Public Sub New()

        Me.Table = "ExtraBedType"

        With Me.Fields
            .Add(New Field("ExtraBedTypeID", "Integer"))
            .Add(New Field("ExtraBedType", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyRoomType
        If Not SQL.FKCheck("PropertyRoomType", "ExtraBedTypeID", iTableID) Then
            Me.Warnings.Add("The Extra Bed Type cannot be deleted as it is associated with one or more properties")
        End If

        'check for records in ContractRoomType
        If Not SQL.FKCheck("ContractRoomType", "ExtraBedTypeID", iTableID) Then
            Me.Warnings.Add("The Extra Bed Type cannot be deleted as it is associated with one or more contracts")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class

