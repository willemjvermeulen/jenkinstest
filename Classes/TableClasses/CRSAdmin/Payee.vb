Imports Intuitive

Public Class Payee
    Inherits TableBase

#Region "New"

    Public Sub New()

        Me.Table = "Payee"

        With Me.Fields
            .Add(New Field("PayeeID", "Integer"))
            .Add(New Field("PayeeName", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("BankName", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("BankAddress1", "String", 40))
            .Add(New Field("BankAddress2", "String", 40))
            .Add(New Field("BankTownCity", "String", 30))
            .Add(New Field("BankCountry", "String", 50))
            .Add(New Field("BankPostcode", "String", 15))
            .Add(New Field("BankTelephone", "String", 20))
            .Add(New Field("BankAccountName", "String", 70))
            .Add(New Field("BankAccountNumber", "String", 50))
            .Add(New Field("BankSortCode", "String", 20))
            .Add(New Field("BankSwiftCode", "String", 50))
            .Add(New Field("BankIBAN", "String", 50))
            .Add(New Field("ChargesPaidBy", "String", 20))
            .Add(New Field("CurrencyID", "Integer", 4, ValidationType.NotEmpty))
            .Add(New Field("CLABE", "Boolean"))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Property
        If Not SQL.FKCheck("Property", "PayeeID", iTableID) Then
            Me.Warnings.Add("The Payee cannot be deleted as it is associated with one or more Properties")
        End If

        'check for records in BookingPayment
        If Not SQL.FKCheck("BookingPayment", "PayeeID", iTableID) Then
            Me.Warnings.Add("The Payee cannot be deleted as it is associated with one or more Payments")
        End If

        'check for records in BookingPaymentSchedule
        If Not SQL.FKCheck("BookingPaymentSchedule", "PayeeID", iTableID) Then
            Me.Warnings.Add("The Payee cannot be deleted as it is associated with one or more Scheduled Payments")
        End If

        If Me.Warnings.Count = 0 Then
            'delete child records from PayeeCreditCardType
            SQL.Execute("Delete from PayeeCreditCardType Where PayeeID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "GetDefaultCurrencyID"

    Public Shared Function GetDefaultCurrencyID(ByVal iPayeeID As Integer) As Integer

        Dim iCurrencyID As Integer

        iCurrencyID = SQL.ExecuteSingleValue("select CurrencyID from Payee where PayeeID={0}", _
            iPayeeID)

        Return iCurrencyID

    End Function
#End Region
End Class
