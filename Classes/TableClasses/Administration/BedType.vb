Imports Intuitive

Public Class BedType
    Inherits TableBase

    Public Sub New()

        Me.Table = "BedType"

        With Me.Fields
            .Add(New Field("BedTypeID", "Integer"))
            .Add(New Field("BedType", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyRoomTypeBedType
        If Not SQL.FKCheck("PropertyRoomTypeBedType", "BedTypeID", iTableID) Then
            Me.Warnings.Add("The Bed Type cannot be deleted as it is associated with one or properties")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class

