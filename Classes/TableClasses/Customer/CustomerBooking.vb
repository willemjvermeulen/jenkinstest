Imports Intuitive

Public Class CustomerBooking
    Inherits TableBase

    Public Sub New()

        Me.Table = "CustomerBooking"

        With Me.Fields
            .Add(New Field("CustomerBookingID", "Integer"))
            .Add(New Field("CustomerID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("PropertyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ArrivalDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Duration", "Integer", ValidationType.NotEmpty))
            .Add(New Field("BookingNotes", "Text"))
            .Add(New Field("PropertyBookingID", "Integer"))
        End With

        Me.Clear()
    End Sub

End Class
