Imports Intuitive
Imports Intuitive.Functions

Public Class ContractSpecialOffer
    Inherits TableBase

    Public Enum eBindMode
        Details
        Constraints
        Supplements
        Notes
    End Enum

#Region "Properties"

    Private dStayStartDate As Date
    Private dStayEndDate As Date
    Private dBookingStartDate As Date
    Private dBookingEndDate As Date
    Private oBindMode As eBindMode

    Public Property StayStartDate() As Date
        Get
            Return dStayStartDate
        End Get
        Set(ByVal Value As Date)
            dStayStartDate = Value
        End Set
    End Property

    Public Property StayEndDate() As Date
        Get
            Return dStayEndDate
        End Get
        Set(ByVal Value As Date)
            dStayEndDate = Value
        End Set
    End Property

    Public Property BookingStartDate() As Date
        Get
            Return dBookingStartDate
        End Get
        Set(ByVal Value As Date)
            dBookingStartDate = Value
        End Set
    End Property

    Public Property BookingEndDate() As Date
        Get
            Return dBookingEndDate
        End Get
        Set(ByVal Value As Date)
            dBookingEndDate = Value
        End Set
    End Property

    Public ExclusionCount As Integer = 0

#End Region

#Region "New"

    Public Sub New(Optional ByVal BindMode As eBindMode = eBindMode.Details)

        Me.Table = "ContractSpecialOffer"

        'Details
        If BindMode = eBindMode.Details Then

            With Me.Fields
                .Add(New Field("ContractSpecialOfferID", "Integer"))
                .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("ContractRoomTypeID", "Integer"))
                .Add(New Field("SpecialOfferTypeID", "Integer"))
                .Add(New Field("IsTypeDiscount", "Integer"))
                .Add(New Field("Category", "String", 15, ValidationType.NotEmpty))
                .Add(New Field("OfferName", "String", 60))
                .Add(New Field("BookingStartDate", "Date", ValidationType.NotEmptyIsDate))
                .Add(New Field("BookingEndDate", "Date", ValidationType.NotEmptyIsDate))
                .Add(New Field("StayDateType", "String", 20, ValidationType.NotEmpty))
                .Add(New Field("StayStartDate", "Date", ValidationType.IsDate))
                .Add(New Field("StayEndDate", "Date", ValidationType.IsDate))
                .Add(New Field("OfferType", "String", 30))
                .Add(New Field("DVPassengerType", "String", 20))
                .Add(New Field("DVRateCalculation", "String", 20))
                .Add(New Field("DVDurationCalculation", "String", 20))
                .Add(New Field("DPPassengerType", "String", 20))
                .Add(New Field("RUContractRoomTypeID", "Integer"))
                .Add(New Field("FNDuration", "Integer"))
                .Add(New Field("FNFreeNights", "Integer"))
                .Add(New Field("FNAppliesAt", "String", 20))
                .Add(New Field("FNDayType", "String", 20))
                .Add(New Field("FNMon", "Boolean"))
                .Add(New Field("FNTue", "Boolean"))
                .Add(New Field("FNWed", "Boolean"))
                .Add(New Field("FNThu", "Boolean"))
                .Add(New Field("FNFri", "Boolean"))
                .Add(New Field("FNSat", "Boolean"))
                .Add(New Field("FNSun", "Boolean"))
                .Add(New Field("VANotes", "String", 250))
                .Add(New Field("FromMealBasisID", "Integer"))
                .Add(New Field("ToMealBasisID", "Integer"))
                .Add(New Field("RoundTotal", "Boolean"))
                .Add(New Field("PartiallyApplicable", "Boolean"))
                .Add(New Field("ABAPD", "Boolean"))
                .Add(New Field("FreeKids", "Integer"))
                .Add(New Field("VoucherCode", "String", 10))
            End With
            oBindMode = eBindMode.Details
        End If

        'Constraints
        If BindMode = eBindMode.Constraints Then

            With Me.Fields
                .Add(New Field("ContractSpecialOfferID", "Integer"))
                .Add(New Field("MealBasisID", "Integer"))
                .Add(New Field("MinAdults", "Integer"))
                .Add(New Field("MaxAdults", "Integer"))
                .Add(New Field("MinChildren", "Integer"))
                .Add(New Field("MaxChildren", "Integer"))
                .Add(New Field("MinYouths", "Integer"))
                .Add(New Field("MaxYouths", "Integer"))
                .Add(New Field("MinHolidayLength", "Integer"))
                .Add(New Field("MaxHolidayLength", "Integer"))
                .Add(New Field("MinGuestAge", "Integer"))
                .Add(New Field("MaxGuestAge", "Integer"))
                .Add(New Field("PaymentTerms", "Text"))
                .Add(New Field("ReportByDate", "Date", ValidationType.IsDate))
                .Add(New Field("Combinable", "Boolean"))
            End With
            oBindMode = eBindMode.Constraints
        End If

        'Supplements
        If BindMode = eBindMode.Supplements Then

            With Me.Fields
                .Add(New Field("ContractSpecialOfferID", "Integer"))
                .Add(New Field("FNRateCalculation", "String", 20))
                .Add(New Field("FNDurationCalculation", "String", 20))
                .Add(New Field("FNAdult", "Numeric"))
                .Add(New Field("FNChild", "Numeric"))
                .Add(New Field("FNYouth", "Numeric"))
            End With
            oBindMode = eBindMode.Supplements
        End If

        'Notes
        If BindMode = eBindMode.Notes Then

            With Me.Fields
                .Add(New Field("ContractSpecialOfferID", "Integer"))
                .Add(New Field("Notes", "Text"))
                .Add(New Field("CustomerNote", "String", 150))
            End With
            oBindMode = eBindMode.Notes
        End If
        Me.Clear()

    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyRoomBookingSpecialOffer
        If Not SQL.FKCheck("PropertyRoomBookingSpecialOffer", "ContractSpecialOfferID", iTableID) Then
            Me.Warnings.Add("ContractSpecialOffer cannot be deleted as it is in use")
        End If

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'delete child records from ContractSpecialOfferExclusion
            .Add("Delete from ContractSpecialOfferExclusion Where ContractSpecialOfferID=" & iTableID.ToString)

            'delete child records from ContractSpecialOfferValue
            .Add("Delete from ContractSpecialOfferValue Where ContractSpecialOfferID=" & iTableID.ToString)

            .Execute()

        End With

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Update"

    'Details tab
    Public Overrides Function CheckUpdate() As Boolean

        'call the default check update
        MyBase.CheckUpdate()

        If oBindMode = eBindMode.Details AndAlso Me.Warnings.Count = 0 Then

            Fields("IsTypeDiscount").Value = If(Me.GetField("IsTypeDiscount") = "true", 1, 0)

            'check that a special offer type has been selected
            If SafeInt(Me.GetField("SpecialOfferTypeID")) = 0 Then
                Me.Warnings.Add("The Special Offer Type must be selected")
                Fields("SpecialOfferTypeID").IsValid = False
            End If

            'if Custom, check that the offer name and type have been specified
            If SafeInt(Me.GetField("SpecialOfferTypeID")) = -1 Then
                If Me.GetField("OfferName") = "" Then
                    Me.Warnings.Add("The Offer Name must be specified")
                    Fields("OfferName").IsValid = False
                End If
                If Me.GetField("OfferType") = "" Then
                    Me.Warnings.Add("The Offer Type must be specified")
                    Fields("OfferType").IsValid = False
                End If
            End If

            'Date checks

            'Booking Dates
            'Make sure the Booking Start Date is before the End Date
            Dim dSOBookingStart As Date = CType(Me.GetField("BookingStartDate"), Date)
            Dim dSOBookingEnd As Date = CType(Me.GetField("BookingEndDate"), Date)

            If dSOBookingStart > dSOBookingEnd Then
                Me.Warnings.Add("The Booking Start Date must be before the End Date")
                Fields("BookingStartDate").IsValid = False
                Fields("BookingEndDate").IsValid = False
            End If

            'Make sure booking dates are within Contract Booking Dates
            If dSOBookingStart < dBookingStartDate OrElse dSOBookingEnd > dBookingEndDate Then
                Me.Warnings.Add("The Special Offer Booking Dates must be within the Contract Booking Dates")
                Fields("BookingStartDate").IsValid = False
                Fields("BookingEndDate").IsValid = False
            End If

            'Stay Dates - only check if not Discount Value/Percentage types
            If Not InList(Me.GetField("OfferType"), "", "Discount Value", "Discount Percentage") Then

                'Make sure the Stay Start Date is before the End Date
                Dim dSOStayStart As Date = CType(Me.GetField("StayStartDate"), Date)
                Dim dSOStayEnd As Date = CType(Me.GetField("StayEndDate"), Date)

                If dSOStayStart > dSOStayEnd Then
                    Me.Warnings.Add("The Stay Start Date must be before the End Date")
                    Fields("StayStartDate").IsValid = False
                    Fields("StayEndDate").IsValid = False
                End If

                'Make sure Stay dates are within Contract Dates
                If dSOStayStart < dStayStartDate OrElse dSOStayEnd > dStayEndDate Then
                    Me.Warnings.Add("The Special Offer Stay Dates must be within the Contract Stay Dates")
                    Fields("StayStartDate").IsValid = False
                    Fields("StayEndDate").IsValid = False
                End If

            End If

            'Discount Value
            'Check that Pax Type, Rate Calc and Duration Calc are not blank
            If Me.GetField("OfferType") = "Discount Value" Then
                If Me.GetField("DVPassengerType") = "" Then
                    Me.Warnings.Add("The Passenger Type must be specified")
                    Fields("DVPassengerType").IsValid = False
                End If

                If Me.GetField("DVRateCalculation") = "" Then
                    Me.Warnings.Add("The Rate Calculation Type must be specified")
                    Fields("DVRateCalculation").IsValid = False
                End If

                If Me.GetField("DVDurationCalculation") = "" Then
                    Me.Warnings.Add("The Duration Calculation Type must be specified")
                    Fields("DVDurationCalculation").IsValid = False
                End If
            End If

            'Discount Percentage
            'Check that Pax Type is not blank
            If Me.GetField("OfferType") = "Discount Percentage" Then
                If Me.GetField("DPPassengerType") = "" Then
                    Me.Warnings.Add("The Passenger Type must be specified")
                    Fields("DPPassengerType").IsValid = False
                End If
            End If

            'Room Upgrade
            'Check that a Room Type has been selected
            If Me.GetField("OfferType") = "Room Upgrade" Then
                If Me.GetField("RUContractRoomTypeID") = "" Then
                    Me.Warnings.Add("The Upgraded Room Type must be specified")
                    Fields("RUContractRoomTypeID").IsValid = False
                End If
            End If

            'Meal Basis Upgrade
            'Check that a meal basis has been selected
            If Me.GetField("OfferType") = "Meal Basis Upgrade" Then
                If Me.GetField("FromMealBasisID") = "" OrElse Me.GetField("ToMealBasisID") = "" Then
                    Me.Warnings.Add("Both the Upgrade From Meal Basis and the Upgrade To Meal Basis must be specified")
                    Fields("FromMealBasisID").IsValid = False
                    Fields("ToMealBasisID").IsValid = False
                End If
            End If

            'Value Added
            'Check that some notes have been added
            If Me.GetField("OfferType") = "Value Added" Then
                If Me.GetField("VANotes") = "" Then
                    Me.Warnings.Add("The Value Added Description must be specified")
                    Fields("VANotes").IsValid = False
                End If
            End If

            'Free Nights
            'Check that Duration/Free Nights/Applies At/Arrival Days have been filled out
            If Me.GetField("OfferType") = "Free Nights" Then
                If Me.GetField("FNDuration") = "0" Then
                    Me.Warnings.Add("The Holiday Duration must be specified")
                    Fields("FNDuration").IsValid = False
                End If

                If Me.GetField("FNFreeNights") = "0" Then
                    Me.Warnings.Add("The number of Free Nights must be specified")
                    Fields("FNFreeNights").IsValid = False
                End If

                If Me.GetField("FNAppliesAt") = "" Then
                    Me.Warnings.Add("The Applies At type must be specified")
                    Fields("FNAppliesAt").IsValid = False
                End If

                If Me.GetField("FNDayType") = "" Then
                    Me.Warnings.Add("The Day Type must be specified")
                    Fields("FNDayType").IsValid = False
                End If

                'Check that, if Selected Days, at least one day has been selected
                If Me.GetField("FNDayType") = "Selected Days" Then
                    If Me.GetField("FNMon") = "false" AndAlso Me.GetField("FNTue") = "false" _
                            AndAlso Me.GetField("FNWed") = "false" AndAlso Me.GetField("FNThu") = "false" _
                            AndAlso Me.GetField("FNFri") = "false" AndAlso Me.GetField("FNSat") = "false" _
                            AndAlso Me.GetField("FNSun") = "false" Then
                        Me.Warnings.Add("At least one Arrival Day must be selected")
                    End If
                End If

            End If

            'check exclusions
            If SafeInt(Me("ContractRoomTypeID")) = 0 AndAlso Me.ExclusionCount > 0 Then

                If Me.ExclusionCount >= SQL.ExecuteSingleValue("select count(*) from ContractRoomType where ContractID={0}", Me("ContractID")) Then
                    Me.Warnings.Add("You cannot exclude all room types from a special offer")
                End If
            End If

        End If

        'Constraints tab
        If oBindMode = eBindMode.Constraints AndAlso Me.Warnings.Count = 0 Then

            'check that the Report By date is within the contract dates
            If Me("ReportByDate") <> "" Then

                Dim oContractSpecialOfferDetails As New ContractSpecialOffer(eBindMode.Details)
                oContractSpecialOfferDetails.Go(Me.TableID)

                Dim dSOBookingEndDate As Date = CType(oContractSpecialOfferDetails("BookingEndDate"), Date)
                Dim dReportByDate As Date = CType(Me("ReportByDate"), Date)

                If dReportByDate < dSOBookingEndDate Then
                    Me.Warnings.Add("The Report By Date cannot be earlier than the last Booking Date for the Special Offer")
                    Me.Fields("ReportByDate").IsValid = False
                End If
            End If
        End If

        'Supplements tab
        If oBindMode = eBindMode.Supplements Then

            'check that a Rate Calculation type is selected
            If Me.GetField("FNRateCalculation") = "" Then
                Me.Warnings.Add("The Rate Calculation type must be specified")
                Fields("FNRateCalculation").IsValid = False
            End If

            'check that a Duration Calculation type is selected
            If Me.GetField("FNDurationCalculation") = "" Then
                Me.Warnings.Add("The Duration Calculation type must be specified")
                Fields("FNDurationCalculation").IsValid = False
            End If

        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "After Add"

    Private Sub Cso_afterupdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate
        SQL.Execute("update ContractSpecialOffer set VoucherCode=NULL where VoucherCode=''")
    End Sub

    Private Sub ContractSpecialOffer_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        'set the MealBasisID
        SQL.Execute("update ContractSpecialOffer set MealBasisID=-1 where ContractSpecialOfferID={0}", iTableID)

    End Sub

#End Region


#Region "Save Exclusions"

    Public Shared Sub SaveExclusions(ByVal ContractSpecialOfferID As Integer, ByVal Exclusions As ArrayList)

        Dim oSQL As New SQLTransaction
        With oSQL
            .Add("delete from ContractSpecialOfferExclusion where ContractSpecialOfferID={0}", ContractSpecialOfferID)

            For Each iContractRoomTypeID As Integer In Exclusions
                .Add("insert into ContractSpecialOfferExclusion select {0},{1}", ContractSpecialOfferID, iContractRoomTypeID)
            Next

            .Execute()
        End With
    End Sub

#End Region

End Class

