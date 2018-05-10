Imports Intuitive

Public Class AddressType
    Inherits TableBase

    Public Sub New()

        Me.Table = "AddressType"

        With Me.Fields
            .Add(New Field("AddressTypeID", "Integer"))
            .Add(New Field("AddressType", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in SupplementaryAddress
        If Not SQL.FKCheck("SupplementaryAddress", "AddressTypeID", iTableID) Then
            Me.Warnings.Add("The Address Type cannot be deleted as it is associated with one or more Property Addresses")
        End If

        Return Me.Warnings.Count = 0

    End Function
End Class
