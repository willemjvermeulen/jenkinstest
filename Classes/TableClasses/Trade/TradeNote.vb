Imports Intuitive

Public Class TradeNote
    Inherits TableBase

    Public Sub New()

        Me.Table = "TradeNote"

        With Me.Fields
            .Add(New Field("TradeNoteID", "Integer"))
            .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeContactID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Subject", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text"))
            .Add(New Field("ActivityID", "Integer"))
            .Add(New Field("DateAdded", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("SystemUserID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class

