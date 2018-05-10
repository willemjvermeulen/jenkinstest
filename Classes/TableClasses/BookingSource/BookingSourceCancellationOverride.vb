Imports Intuitive.DateFunctions
Imports Intuitive

Public Class BookingSourceCancellationOverride

    Public Shared Function AddDates(ByVal iBookingSourceID As Integer, _
        ByVal sStartDate As String, ByVal sEndDate As String) As FunctionReturn

        Dim oReturn As New FunctionReturn
        Dim dStartDate As Date
        Dim dEndDate As Date

        'validate the dates
        If Not IsDisplayDate(sStartDate) Then
            oReturn.AddWarning("A valid Start Date must be input")
        End If
        If Not IsDisplayDate(sEndDate) Then
            oReturn.AddWarning("A valid End Date must be input")
        End If

        'if valid dates do some more checking, end date after the start date
        'and that the dates do not conflict with other entries
        If oReturn.Success Then
            dStartDate = SafeDate(sStartDate)
            dEndDate = SafeDate(sEndDate)

            If dEndDate < dStartDate Then
                oReturn.AddWarning("The Start Date must be before the End Date")
            ElseIf Not CheckNoOverlappingDates(iBookingSourceID, dStartDate, dEndDate) Then
                oReturn.AddWarning("The dates input conflict with existing dates for this Booking Source")
            End If
        End If

        'do the bizzo
        If oReturn.Success Then

            oReturn.Result = SQL.Execute("exec BookingSourceCancellationOverrideAdd {0}, {1}, {2}", _
                iBookingSourceID, SQL.GetSqlValue(dStartDate, SQL.SqlValueType.Date), _
                SQL.GetSqlValue(dEndDate, SQL.SqlValueType.Date))
        End If

        Return oReturn
    End Function

    Public Shared Function CheckNoOverlappingDates(ByVal iBookingSourceID As Integer, _
        ByVal dStartDate As Date, ByVal dEndDate As Date) As Boolean


        Return SQL.ExecuteSingleValue("exec BookingSourceCancellationOverrideCheckForOverlap {0},{1},{2}", _
            iBookingSourceID, SQL.GetSqlValue(dStartDate, SQL.SqlValueType.Date), _
            SQL.GetSqlValue(dEndDate, SQL.SqlValueType.Date)) = 0

    End Function
End Class
