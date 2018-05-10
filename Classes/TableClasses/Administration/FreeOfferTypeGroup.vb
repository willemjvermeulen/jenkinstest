Imports Intuitive
Imports Intuitive.Functions

Public Class FreeOfferTypeGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "FreeOfferTypeGroup"

        With Me.Fields
            .Add(New Field("FreeOfferTypeGroupID", "Integer"))
            .Add(New Field("FreeOfferTypeGroup", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in FreeOfferType
        If Not SQL.FKCheck("FreeOfferType", "FreeOfferTypeGroupID", iTableID) Then
            Me.Warnings.Add("FreeOfferTypeGroup cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
