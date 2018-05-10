Imports Intuitive

Public Class TradeSetDef
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeSetDef"

        With Me.Fields
            .Add(New Field("TradeSetDefID", "Integer"))
            .Add(New Field("TradeSetID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
