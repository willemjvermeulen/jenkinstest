Imports Intuitive

Public Class Interest
    Inherits TableBase

    Public Sub New()

        Me.Table = "Interest"

        With Me.Fields
            .Add(New Field("InterestID", "Integer"))
            .Add(New Field("Interest", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in CustomerInterest
        If Not SQL.FKCheck("CustomerInterest", "InterestID", iTableID) Then
            Me.Warnings.Add("Interest cannot be deleted as it is in use")
        End If

        'delete child records from CustomerInterest
        SQL.Execute("Delete from CustomerInterest Where InterestID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
End Class
