Imports Intuitive

Public Class TradeAttribute
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeAttribute"

        With Me.Fields
            .Add(New Field("TradeAttributeID", "Integer"))
            .Add(New Field("TradeAttribute", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in TradeAttributeBridge
        If Not SQL.FKCheck("TradeAttributeBridge", "TradeAttributeID", iTableID) Then
            Me.Warnings.Add("Trade Attribute cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
