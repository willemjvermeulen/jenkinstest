Imports Intuitive

Public Class ContractTaxValue
    Inherits TableBase

#Region "New"

    Public Sub New()

        Me.Table = "ContractTaxValue"

        With Me.Fields
            .Add(New Field("ContractTaxValueID", "Integer"))
            .Add(New Field("ContractTaxID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Value", "Numeric"))
        End With

        Me.Clear()
    End Sub

#End Region

End Class
