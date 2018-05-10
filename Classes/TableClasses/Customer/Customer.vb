Imports Intuitive
Imports Intuitive.Functions
Public Class Customer
    Inherits TableBase

    Public Sub New()

        Me.Table = "Customer"

        With Me.Fields
            .Add(New Field("CustomerID", "Integer"))
            .Add(New Field("Salutation", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Title", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Forename", "String", 50))
            .Add(New Field("Surname", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Company", "String", 50))
            .Add(New Field("Address1", "String", 50))
            .Add(New Field("Address2", "String", 50))
            .Add(New Field("TownCity", "String", 50))
            .Add(New Field("County", "String", 50))
            .Add(New Field("BookingCountryID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Postcode", "String", 50))
            .Add(New Field("Telephone", "String", 50))
            .Add(New Field("Mobile", "String", 50))
            .Add(New Field("Fax", "String", 50))
            .Add(New Field("Email", "String", 50, ValidationType.IsEmail))
            .Add(New Field("DateOfBirth", "Date", ValidationType.IsDate))
            .Add(New Field("WeddingDate", "Date", ValidationType.IsDate))
            .Add(New Field("DoNotContact", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from CustomerBooking
        SQL.Execute("Delete from CustomerBooking Where CustomerID=" & iTableID.ToString)

        'delete child records from CustomerInterest
        SQL.Execute("Delete from CustomerInterest Where CustomerID=" & iTableID.ToString)

        'delete child records from CustomerNote
        SQL.Execute("Delete from CustomerNote Where CustomerID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

    Public Shared Function CustomerIDFromPostcodeAndName(ByVal sSurname As String, ByVal sPostcode As String) As Integer
        Return SafeInt(SQL.GetValue("Select CustomerID From Customer Where Surname={0} and Postcode={1}", _
     SQL.GetSqlValue(sSurname, SQL.SqlValueType.String), SQL.GetSqlValue(sPostcode, SQL.SqlValueType.String)))
    End Function

End Class
