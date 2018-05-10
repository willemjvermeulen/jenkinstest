Imports Intuitive
Public Class ContractRegionTax
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractRegionTax"

        With Me.Fields
            .Add(New Field("ContractRegionTaxID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("RegionTaxID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Inclusive", "String", 10, ValidationType.NotEmpty))

            Me.Fields("Inclusive").ControlIDOverride = "ddlRegionInclusive"
        End With

        Me.Clear()
    End Sub

End Class
