Imports Intuitive

Public Class PayeeCreditCardType
    Inherits TableBase

    Public Sub New()

        Me.Table = "PayeeCreditCardType"

        With Me.Fields
            .Add(New Field("PayeeCreditCardTypeID", "Integer"))
            .Add(New Field("PayeeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("CreditCardTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("SurchargePercentage", "Numeric"))
        End With

        Me.Clear()
    End Sub

End Class
