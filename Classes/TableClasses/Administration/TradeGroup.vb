Imports Intuitive

Public Class TradeGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeGroup"

        With Me.Fields
            .Add(New Field("TradeGroupID", "Integer"))
            .Add(New Field("TradeParentGroupID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeGroup", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Trade
        If Not SQL.FKCheck("Trade", "TradeGroupID", iTableID) Then
            Me.Warnings.Add("The Trade Group cannot be deleted as it is associated with one or more Trade records")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class

