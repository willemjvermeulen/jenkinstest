Imports Intuitive
Imports Intuitive.Functions
Imports System.Text

Public Class BookingCommentType
    Inherits TableBase

    Public Sub New()

        Me.Table = "BookingCommentType"

        With Me.Fields
            .Add(New Field("BookingCommentTypeID", "Integer"))
            .Add(New Field("BookingCommentType", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("HotelFlag", "Boolean"))
            .Add(New Field("CustomerFlag", "Boolean"))
            .Add(New Field("WarnFlag", "Boolean"))
            .Add(New Field("RelevantToPayment", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Shared Function GetSystemBookingCommentTypeID() As Integer

        Dim iBookingCommentTypeID As Integer = _
            SQL.ExecuteSingleValue("select BookingCommentTypeID from BookingCommentType " & _
                "where System=1")

        If iBookingCommentTypeID > 0 Then
            Return iBookingCommentTypeID
        Else

            Throw New Exception("Could not find system Booking Comment Type")
        End If

    End Function

    Public Shared Function AsDropdownOptions() As String
        Dim sOptions As String = ""
        Dim sOptionTemplate As String = "{0}|{1}_{2}_{3}#"

        Dim tbl As DataTable = CType(HttpContext.Current.Cache("ListBookingCommentTypeOptions"), DataTable)

        If tbl Is Nothing _
            OrElse tbl.Rows.Count > 0 Then

            tbl = SQL.GetDatatable("ListBookingCommentType 1")
            Dim sCacheDependencyFile As String = System.Configuration.ConfigurationManager.AppSettings.Get("CacheDependencyFolder")
            sCacheDependencyFile += "BookingCommentType.txt"
            HttpContext.Current.Cache.Insert("ListBookingCommentTypeOptions", tbl, _
                New Caching.CacheDependency(sCacheDependencyFile))
        End If

        For Each dr As DataRow In tbl.Rows

            sOptions += String.Format(sOptionTemplate, dr("BookingCommentType").ToString, _
                    dr("BookingCommentTypeID").ToString, _
                    dr("WarnFlag").ToString, dr("BookingCommentType").ToString)

        Next

        Return Chop(sOptions)

    End Function

End Class
