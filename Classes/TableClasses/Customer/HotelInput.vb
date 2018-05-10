Imports Intuitive

Public Class HotelInput
    Inherits TableBase


    Public PropertyID As Integer
    Public ArrivalDate As Date
    Public Duration As Integer
    Public BookingNotes As String

    Public InterestIDs As ArrayList

    Public Sub New()

        Me.Table = "Customer"

        With Me.Fields
            .Add(New Field("CustomerID", "Integer"))
            .Add(New Field("Salutation", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Title", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Forename", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Surname", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Company", "String", 50))
            .Add(New Field("Address1", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Address2", "String", 50))
            .Add(New Field("TownCity", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("County", "String", 50))
            .Add(New Field("Postcode", "String", 50, ValidationType.NotEmpty))
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


#Region "checkupdate"

    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()

        If Me.PropertyID = 0 Then
            Me.Warnings.Add("The Property must be selected")
        End If
        If Me.ArrivalDate = DateFunctions.EmptyDate Then
            Me.Warnings.Add("A valid Arrival Date must be input")
        End If
        If Me.Duration = 0 Then
            Me.Warnings.Add("The Duration of the stay must be input")
        End If

        Return Me.Warnings.Count = 0
    End Function


#End Region


#Region "after add - save booking and interests"

    Private Sub HotelInput_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        'booking
        Dim oCustomerBooking As New CustomerBooking
        With oCustomerBooking
            .SetField("CustomerID", Me.TableID)
            .SetField("PropertyID", Me.PropertyID)
            .SetField("ArrivalDate", Me.ArrivalDate)
            .SetField("Duration", Me.Duration)
            .SetField("BookingNotes", Me.BookingNotes)
            .Update()
        End With

        'interests
        If Me.InterestIDs.Count > 0 Then
            Dim sInsert As String = "insert into CustomerInterest (CustomerID, InterestID) " & _
                "values ({0},{1})"
            Dim oSqlTrans As New SQLTransaction
            For Each iInterestID As Integer In Me.InterestIDs
                oSqlTrans.Add(sInsert, Me.TableID, iInterestID)
            Next
            oSqlTrans.Execute()
        End If

    End Sub

#End Region
End Class
