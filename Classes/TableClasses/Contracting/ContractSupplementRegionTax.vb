Imports Intuitive
Public Class ContractSupplementRegionTax
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractSupplementRegionTax"

        With Me.Fields
            .Add(New Field("ContractSupplementRegionTaxID", "Integer"))
            .Add(New Field("ContractSupplementID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("RegionTaxID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
