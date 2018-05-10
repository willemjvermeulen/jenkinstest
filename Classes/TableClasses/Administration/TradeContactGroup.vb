Imports Intuitive

Public Class TradeContactGroup
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeContactGroup"

        With Me.Fields
            .Add(New Field("TradeContactGroupID", "Integer"))
            .Add(New Field("TradeContactGroup", "String", 25, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Reservations", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in TradeContact
        If Not SQL.FKCheck("TradeContact", "TradeContactGroupID", iTableID) Then
            Me.Warnings.Add("The Trade Contact Group cannot be deleted as it is associated with one or more Trade Contacts")
        End If

        Return Me.Warnings.Count = 0
    End Function

End Class

