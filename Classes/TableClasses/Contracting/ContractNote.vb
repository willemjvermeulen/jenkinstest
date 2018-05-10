Imports Intuitive
Public Class ContractNote
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractNote"

        With Me.Fields
            .Add(New Field("ContractNoteID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractSectionId", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Notes", "Text", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
