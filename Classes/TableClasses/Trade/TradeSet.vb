Imports Intuitive

Public Class TradeSet
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeSet"

        With Me.Fields
            .Add(New Field("TradeSetID", "Integer"))
            .Add(New Field("TradeSet", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("TradeSetCode", "String", 5, ValidationType.NotEmptyNotDupe))
            .Add(New Field("BookingSourceID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from TradeSetDef
        SQL.Execute("Delete from TradeSetDef Where TradeSetID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
End Class
