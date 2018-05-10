Imports Intuitive
Public Class ContractSupplementTax
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractSupplementTax"

        With Me.Fields
            .Add(New Field("ContractSupplementTaxID", "Integer"))
            .Add(New Field("ContractSupplementID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractTaxID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
