Imports Intuitive

Public Class CustomerInterest
    Inherits TableBase

    Public Sub New()

        Me.Table = "CustomerInterest"

        With Me.Fields
            .Add(New Field("CustomerInterestID", "Integer"))
            .Add(New Field("CustomerID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("InterestID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub
    Public Shared Sub ClearRights(ByVal iCustomerID As Integer)

        SQL.Execute("Delete from CustomerInterest Where CustomerID=" & iCustomerID)

    End Sub

    Public Shared Sub AddInterest(ByVal iCustomerID As Integer, _
        ByVal iInterestID As Integer)
        Dim sSql As String

        sSql = String.Format("insert into CustomerInterest " & _
            "(CustomerID, InterestID) values ({0},{1})", _
            iCustomerID, iInterestID)
        SQL.Execute(sSql)
    End Sub
End Class
