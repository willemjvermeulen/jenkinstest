Imports Intuitive

Public Class CreditCardType
    Inherits TableBase

    Public Sub New()

        Me.Table = "CreditCardType"

        With Me.Fields
            .Add(New Field("CreditCardTypeID", "Integer"))
            .Add(New Field("CreditCardType", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PayeeCreditCardType
        If Not SQL.FKCheck("PayeeCreditCardType", "CreditCardTypeID", iTableID) Then
            Me.Warnings.Add("CreditCardType cannot be deleted as it is in use")
        End If

        'delete child records from PayeeCreditCardType
        SQL.Execute("Delete from PayeeCreditCardType Where CreditCardTypeID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
End Class
