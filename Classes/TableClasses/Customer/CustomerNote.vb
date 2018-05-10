Imports Intuitive

Public Class CustomerNote
    Inherits TableBase

    Public Sub New()

        Me.Table = "CustomerNote"

        With Me.Fields
            .Add(New Field("CustomerNoteID", "Integer"))
            .Add(New Field("CustomerID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Subject", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text"))
            .Add(New Field("ActivityID", "Integer"))
            .Add(New Field("DateAdded", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("SystemUserID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
