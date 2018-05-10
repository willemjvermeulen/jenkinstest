Imports Intuitive
Imports Intuitive.Functions

Public Class Erratum
    Inherits TableBase

    Public Sub New()

        Me.Table = "Erratum"

        With Me.Fields
            .Add(New Field("ErratumID", "Integer"))
            .Add(New Field("ParentType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("ParentID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("SystemUserID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("AddedDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("BookingStartDate", "Date", ValidationType.IsDate))
            .Add(New Field("BookingEndDate", "Date", ValidationType.IsDate))
            .Add(New Field("StayStartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("StayEndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("ErratumSubject", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("ErratumDescription", "Text", ValidationType.NotEmpty))
            .Add(New Field("Cancelled", "Boolean"))
            .Add(New Field("CancelledDateTime", "Datetime", ValidationType.IsDate))
            .Add(New Field("CancelledSystemUserID", "Integer"))
            .Add(New Field("PropertyRoomTypeID", "Integer"))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyBookingErratum
        If Not SQL.FKCheck("PropertyBookingErratum", "ErratumID", iTableID) Then
            Me.Warnings.Add("Erratum cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Cancel Erratum"
    Public Sub Cancel()
        Me.SetField("Cancelled", True)
        Me.SetField("CancelledDateTime", Now)
        Me.SetField("CancelledSystemUserID", UserSession.SystemUserID)
        Me.Update()
    End Sub
#End Region

    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()


        'if spec'd booking start date < end date
        If DateFunctions.IsDate(Me("BookingStartDate")) AndAlso DateFunctions.IsDate(Me("BookingEndDate")) Then
            If Not DateFunctions.SafeDate(Me("BookingStartDate")) < DateFunctions.SafeDate(Me("BookingEndDate")) Then
                Me.Warnings.Add("The Booking Start Date must be before the Booking End Date")
                Me.Fields("BookingStartDate").IsValid = False
                Me.Fields("BookingEndDate").IsValid = False
            End If
        End If

        'stay start date < stay end date
        If DateFunctions.IsDate(Me("StayStartDate")) AndAlso DateFunctions.IsDate(Me("StayEndDate")) Then
            If Not DateFunctions.SafeDate(Me("StayStartDate")) < DateFunctions.SafeDate(Me("StayEndDate")) Then
                Me.Warnings.Add("The Stay Start Date must be before the Stay End Date")
                Me.Fields("StayStartDate").IsValid = False
                Me.Fields("StayEndDate").IsValid = False
            End If
        End If

        'Room types
        If Me("ParentType") = "Property" AndAlso SafeInt(Me("PropertyRoomTypeID")) = 0 Then
            Me.Fields("PropertyRoomTypeID").IsValid = False
            Me.Warnings.Add("You must specify a Property Room Type, or choose 'All'")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Private Sub Erratum_BeforeUpdate(ByVal iTableID As Integer) Handles MyBase.BeforeUpdate

        'Encode any dodgy charecters (they upset SQL when passing in XML)
        Me.SetField("ErratumDescription", _
            Me("ErratumDescription").ToString.Replace("'", ""))
        Me.SetField("ErratumDescription", _
            HttpContext.Current.Server.HtmlEncode(Me("ErratumDescription")))

    End Sub

End Class
