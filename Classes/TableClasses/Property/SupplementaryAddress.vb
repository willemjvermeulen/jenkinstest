Imports Intuitive
Imports Intuitive.Functions

Public Class SupplementaryAddress
    Inherits TableBase

#Region "New"

    Public Sub New()

        Me.Table = "SupplementaryAddress"

        With Me.Fields
            .Add(New Field("SupplementaryAddressID", "Integer"))

            .Add(New Field("AddressTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("PropertyID", "Integer", ValidationType.NotEmpty))

            .Add(New Field("Address1", "String", 40))
            .Add(New Field("Address2", "String", 40))
            .Add(New Field("TownCity", "String", 30))
            .Add(New Field("County", "String", 30))
            .Add(New Field("Postcode", "String", 15))
            .Add(New Field("Country", "String", 30))

            .Add(New Field("Telephone", "String", 20))
            .Add(New Field("Fax", "String", 20))
            .Add(New Field("Email", "String", 50, ValidationType.IsEmail))
            .Add(New Field("Website", "String", 50))

            .Add(New Field("ContactName", "String", 40))
            .Add(New Field("ContactPosition", "String", 40))
            .Add(New Field("ContactTelephone", "String", 20))
            .Add(New Field("ContactFax", "String", 20))
            .Add(New Field("ContactEmail", "String", 50, ValidationType.IsEmail))
            .Add(New Field("ContactMobile", "String", 20))

            .Field("Postcode").ControlIDOverride = "txtPostcodeZip"
        End With

        Me.Clear()
    End Sub

#End Region

#Region "After Update"

    Private Sub SupplementaryAddress_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate
        'sync settings
        PropertyTable.SetSyncRequired(SafeInt(Me("PropertyID")))
    End Sub

#End Region

End Class
