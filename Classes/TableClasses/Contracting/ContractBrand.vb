Imports Intuitive

Public Class ContractBrand
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractBrand"

        With Me.Fields
            .Add(New Field("ContractBrandID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("BrandID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub


    Public Shared Sub UpdateContractBrands(ByVal iContractID As Integer, ByVal aBrandIDs As ArrayList)

        'first delete existing ones
        ContractBrand.DeleteContractBrands(iContractID)

        'now add the new ones
        Dim iBrandID As Integer
        For Each iBrandID In aBrandIDs
            SQL.Execute("Insert into ContractBrand (ContractID, BrandID) Values ({0}, {1})", _
                iContractID, iBrandID)
        Next

    End Sub

    Public Shared Sub DeleteContractBrands(ByVal iContractID As Integer)

        SQL.Execute("Delete from ContractBrand Where ContractID=" & iContractID)
    End Sub

End Class
