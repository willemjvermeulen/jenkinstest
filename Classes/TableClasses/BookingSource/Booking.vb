Imports Intuitive

Public Class Booking
    Inherits TableBase

    Public Sub New()

        Me.Table = "Booking"

        With Me.Fields
            .Add(New Field("BookingID", "Integer"))
            .Add(New Field("BookingReference", "String", 15, ValidationType.NotEmpty))
            .Add(New Field("BookingType", "String", 10, ValidationType.NotEmpty))
            .Add(New Field("Status", "String", 15, ValidationType.NotEmpty))
            .Add(New Field("BookingSourceID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeReference", "String", 15, ValidationType.NotEmpty))
            .Add(New Field("BookingDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("SystemUserID", "Integer", ValidationType.NotEmpty))
            '.Add(New Field("TotalAmount", ))
            '.Add(New Field("DiscountAmount", ))
            '.Add(New Field("CommissionAmount", ))
            .Add(New Field("LeadGuestTitle", "String", 10, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestFirstName", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestLastName", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestAddress1", "String", 40, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestAddress2", "String", 40, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestTownCity", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestCounty", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestPostcode", "String", 15, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestBookingCountryID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("LeadGuestPhone", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestFax", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("LeadGuestEmail", "String", 50, ValidationType.NotEmptyIsEmail))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in BookingComment
        If Not SQL.FKCheck("BookingComment", "BookingID", iTableID) Then
            Me.Warnings.Add("Booking cannot be deleted as it is in use")
        End If

        'check for records in BookingCreditCard
        If Not SQL.FKCheck("BookingCreditCard", "BookingID", iTableID) Then
            Me.Warnings.Add("Booking cannot be deleted as it is in use")
        End If

        'check for records in PropertyBooking
        If Not SQL.FKCheck("PropertyBooking", "BookingID", iTableID) Then
            Me.Warnings.Add("Booking cannot be deleted as it is in use")
        End If

        'delete child records from BookingComment
        SQL.Execute("Delete from BookingComment Where BookingID=" & iTableID.ToString)

        'delete child records from BookingCreditCard
        SQL.Execute("Delete from BookingCreditCard Where BookingID=" & iTableID.ToString)

        'delete child records from PropertyBooking
        SQL.Execute("Delete from PropertyBooking Where BookingID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
End Class
