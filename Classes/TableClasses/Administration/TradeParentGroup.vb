Imports Intuitive

Public Class TradeParentGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeParentGroup"

        With Me.Fields
            .Add(New Field("TradeParentGroupID", "Integer"))
            .Add(New Field("TradeParentGroup", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("TradeParentGroupCode", "String", 4, ValidationType.NotEmptyNotDupe))
            .Add(New Field("BookingSourceID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Commission", "Double"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Trade
        If Not SQL.FKCheck("Trade", "TradeParentGroupID", iTableID) Then
            Me.Warnings.Add("The Trade Parent Group cannot be deleted as it is in use")
        End If

        'check for records in TradeGroup
        If Not SQL.FKCheck("TradeGroup", "TradeParentGroupID", iTableID) Then
            Me.Warnings.Add("The Trade Parent Group cannot be deleted as it has one or more Trade Groups associated with it")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class

