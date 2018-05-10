Imports Intuitive

Public Class PreDefinedNote
    Inherits TableBase

    Public Sub New()

        Me.Table = "PreDefinedNote"

        With Me.Fields
            .Add(New Field("PreDefinedNoteID", "Integer"))
            .Add(New Field("Subject", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text"))
        End With

        Me.Clear()
    End Sub

End Class
