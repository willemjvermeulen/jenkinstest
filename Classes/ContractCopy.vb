Imports System
Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.DateFunctions

Public Class ContractCopy

#Region "Properties"

    Private iContractID As Integer = 0
    Private bTacticalOffer As Boolean = False
    Private sAppliesTo As String = ""
    Private iAppliesToID As Integer = 0
    Private sContractIdentifier As String
    Private sTacticalCode As String = ""
    Private iSystemUserID As Integer = 0
    Private sContractType As String = ""
    Private sStatus As String
    Private iYearOffset As Integer = 0
    Private iDayOffset As Integer = 0
    Private dBookingStartDate As Date = EmptyDate()
    Private dBookingEndDate As Date = EmptyDate()
    Private dStayStartDate As Date = EmptyDate()
    Private dStayEndDate As Date = EmptyDate()
    Private nRateIncrease As Single = 0
    Private nSupplementIncrease As Single = 0
    Private sRoundTo As String = ""
    Private sRounding As String = ""
    Private sAdjustmentAppliesTo As String = ""
    Private aAdjustmentContractRoomTypeID As New ArrayList
    Private aContractRoomType As New ArrayList
    Private aContractSection As New ArrayList
    Private dBaseBookingStartDate As Date = EmptyDate()
    Private dBaseBookingEndDate As Date = EmptyDate()
    Private dBaseStayStartDate As Date = EmptyDate()
    Private dBaseStayEndDate As Date = EmptyDate()

    Private aWarnings As New ArrayList
    Private iNewContractID As Integer

    Public Property ContractID() As Integer
        Get
            Return iContractID
        End Get
        Set(ByVal Value As Integer)
            iContractID = Value
        End Set
    End Property
    Public Property AppliesTo() As String
        Get
            Return sAppliesTo
        End Get
        Set(ByVal Value As String)
            sAppliesTo = Value
        End Set
    End Property
    Public Property AppliesToID() As Integer
        Get
            Return iAppliesToID
        End Get
        Set(ByVal Value As Integer)
            iAppliesToID = Value
        End Set
    End Property
    Public Property ContractIdentifier() As String
        Get
            Return sContractIdentifier
        End Get
        Set(ByVal Value As String)
            sContractIdentifier = Value
        End Set
    End Property
    Public Property TacticalOffer() As Boolean
        Get
            Return bTacticalOffer
        End Get
        Set(ByVal Value As Boolean)
            bTacticalOffer = Value
        End Set
    End Property
    Public Property TacticalCode() As String
        Get
            Return sTacticalCode
        End Get
        Set(ByVal Value As String)
            sTacticalCode = Value
        End Set
    End Property
    Public Property SystemUserID() As Integer
        Get
            Return iSystemUserID
        End Get
        Set(ByVal Value As Integer)
            iSystemUserID = Value
        End Set
    End Property
    Public Property ContractType() As String
        Get
            Return sContractType
        End Get
        Set(ByVal Value As String)
            sContractType = Value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return sStatus
        End Get
        Set(ByVal Value As String)
            sStatus = Value
        End Set
    End Property
    Public Property YearOffset() As Integer
        Get
            Return iYearOffset
        End Get
        Set(ByVal Value As Integer)
            iYearOffset = Value
        End Set
    End Property
    Public Property DayOffset() As Integer
        Get
            Return iDayOffset
        End Get
        Set(ByVal Value As Integer)
            iDayOffset = Value
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
    Public Property RateIncrease() As Single
        Get
            Return nRateIncrease
        End Get
        Set(ByVal Value As Single)
            nRateIncrease = Value
        End Set
    End Property
    Public Property SupplementIncrease() As Single
        Get
            Return nSupplementIncrease
        End Get
        Set(ByVal Value As Single)
            nSupplementIncrease = Value
        End Set
    End Property
    Public Property RoundTo() As String
        Get
            Return sRoundTo
        End Get
        Set(ByVal Value As String)
            sRoundTo = Value
        End Set
    End Property
    Public Property Rounding() As String
        Get
            Return sRounding
        End Get
        Set(ByVal Value As String)
            sRounding = Value
        End Set
    End Property
    Public Property AdjustmentAppliesTo() As String
        Get
            Return sAdjustmentAppliesTo
        End Get
        Set(ByVal Value As String)
            sAdjustmentAppliesTo = Value
        End Set
    End Property
    Public Property AdjustmentContractRoomTypeID() As ArrayList
        Get
            Return aAdjustmentContractRoomTypeID
        End Get
        Set(ByVal Value As ArrayList)
            aAdjustmentContractRoomTypeID = Value
        End Set
    End Property
    Public Property ContractRoomType() As ArrayList
        Get
            Return aContractRoomType
        End Get
        Set(ByVal Value As ArrayList)
            aContractRoomType = Value
        End Set
    End Property
    Public Property ContractSection() As ArrayList
        Get
            Return aContractSection
        End Get
        Set(ByVal Value As ArrayList)
            aContractSection = Value
        End Set
    End Property
    Public Property Warnings() As ArrayList
        Get
            Return aWarnings
        End Get
        Set(ByVal Value As ArrayList)
            aWarnings = Value
        End Set
    End Property
    Public Property NewContractID() As Integer
        Get
            Return iNewContractID
        End Get
        Set(ByVal Value As Integer)
            iNewContractID = Value
        End Set
    End Property



#End Region

#Region "Copy"

    Public Function Copy() As Boolean

        'call the validate routine
        If Not Me.Validate Then
            Return False
        End If

        Dim sSP As String = "exec CopyContract {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, " & _
            "{11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}"

        iNewContractID = SQL.ExecuteSingleValue(sSP, _
            Me.ContractID, _
            SQL.GetSqlValue(Me.ContractIdentifier, SQL.SqlValueType.String), _
            Me.SystemUserID, _
            SQL.GetSqlValue(Me.AppliesTo, SQL.SqlValueType.String), _
            Me.AppliesToID, _
            SQL.GetSqlValue(Me.ContractType, "String"), _
            SQL.GetSqlValue(Me.Status, "String"), _
            Me.YearOffset, _
            Me.DayOffset, _
            SQL.GetSqlValue(Me.sTacticalCode, "String"), _
            SQL.GetSqlValue(Me.BookingStartDate, "DateTime"), _
            SQL.GetSqlValue(Me.BookingEndDate, "DateTime"), _
            SQL.GetSqlValue(Me.StayStartDate, "DateTime"), _
            SQL.GetSqlValue(Me.StayEndDate, "DateTime"), _
            SQL.GetSqlValue(Me.RateIncrease, "Numeric"), _
            SQL.GetSqlValue(Me.SupplementIncrease, "Numeric"), _
            SQL.GetSqlValue(Me.RoundTo, "String"), _
            SQL.GetSqlValue(Me.Rounding, "String"), _
            SQL.GetSqlValue(Me.AdjustmentAppliesTo, "String"), _
            SQL.GetSqlValue(ArrayListToDelimitedString(Me.AdjustmentContractRoomTypeID), "String"), _
            SQL.GetSqlValue(ArrayListToDelimitedString(Me.ContractRoomType), "String"), _
            SQL.GetSqlValue(ArrayListToDelimitedString(Me.ContractSection), "String"), _
            SQL.GetSqlValue(Config.Installation))

        'update the sync settings
        'sync setttings
        Select Case Config.Installation
            Case "Server"
                SQL.Execute("update Contract set SyncGUID=newid(), SyncStatus='In', SyncSystemUserID=0, " & _
                    "SyncRequired=0, EditStatus='Full' where ContractID={0}", iNewContractID)

                'add to audit trail 
                ContractSyncAuditTrail.AddAuditTrailEntry("Contract", iNewContractID, UserSession.SystemUserID, _
                    ContractSyncAuditTrail.EventType.AddedOnServer)
            Case "Client"
                SQL.Execute("update Contract set SyncGUID=newid(), SyncStatus='Out', SyncSystemUserID={1}, " & _
                    "SyncRequired=1, EditStatus='Full' where ContractID={0}", iNewContractID, UserSession.SystemUserID)
            Case Else
                Throw New Exception("Unknown Installtion Mode " & Config.Installation)
        End Select

        Return True

    End Function

#End Region

#Region "validate"

    Private Function Validate() As Boolean

        'make sure a user has been set
        If Me.SystemUserID = 0 Then
            Me.Warnings.Add("The user must be specified")
        End If

        'Check for Contract Identifier
        If Me.ContractIdentifier = "" Then
            Me.Warnings.Add("You must specify a Contract Identifier")
        End If
        If Me.ContractIdentifier.Length > 10 Then
            Me.Warnings.Add("The Contract Identifier cannot be more than 10 characters long")
        End If

        'check not duping contract identifier
        Dim iPropertyID As Integer = SQL.ExecuteSingleValue("select PropertyID from Contract where ContractID={0}", _
            Me.ContractID)
        Dim sAppliesTo As String
        Dim iAppliesToID As Integer

        If Me.ContractType = "New Base Contract" Then
            sAppliesTo = SQL.GetValue("select AppliesTo from Contract where ContractID={0}", Me.ContractID)
            iAppliesToID = SQL.ExecuteSingleValue("select AppliesToID from Contract where ContractID={0}", Me.ContractID)
        Else
            sAppliesTo = Me.AppliesTo
            iAppliesToID = Me.AppliesToID
        End If

        If Not Contract.CheckContractReferenceNotDupe(iPropertyID, sAppliesTo, iAppliesToID, _
               Me.ContractIdentifier, Me.StayStartDate.AddYears(Me.YearOffset).AddDays(Me.DayOffset)) Then
            Me.Warnings.Add("This Contract Identifier would result in a duplicate Contract Reference")
        End If

        'sub-contracts, make sure applies to stuff selected
        If Me.ContractType = "Sub-Contract" Then

            If Me.AppliesTo = "" Then
                Me.Warnings.Add("The Applies To Level must be specified")
            ElseIf Me.AppliesToID = 0 Then
                Me.Warnings.Add("The " & Me.AppliesTo & " must be specified")
            End If

        End If

        'if tactical offer, make sure a code has been entered
        If Me.TacticalOffer Then

            'set the base date
            Dim oContract As New Contract
            With oContract
                .Go(iContractID)
                dBaseBookingStartDate = SafeDate(oContract("BookingStartDate")).Date
                dBaseBookingEndDate = SafeDate(oContract("BookingEndDate")).Date
                dBaseStayStartDate = SafeDate(oContract("StayStartDate")).Date
                dBaseStayEndDate = SafeDate(oContract("StayEndDate")).Date
            End With

            If Me.TacticalCode = "" Then
                Me.Warnings.Add("The Tactical Code must be specified")
            End If

            'booking date range
            If IsEmptyDate(Me.BookingStartDate) Then
                Me.Warnings.Add("The Booking Start Date must be a valid date")
            ElseIf IsEmptyDate(Me.BookingEndDate) Then
                Me.Warnings.Add("The Booking End Date must be a valid date")
            ElseIf Me.BookingStartDate < dBaseBookingStartDate Then
                Me.Warnings.Add("The earliest Booking Start Date allowed is " & _
                    DisplayDate(dBaseBookingStartDate))
            ElseIf Me.BookingEndDate > dBaseBookingEndDate Then
                Me.Warnings.Add("The latest Booking End Date allowed is " & _
                    DisplayDate(dBaseBookingEndDate))
            ElseIf Me.BookingStartDate > Me.BookingEndDate Then
                Me.Warnings.Add("The Booking Start Date must be before the Booking End Date")
            End If

            'stay date range
            If IsEmptyDate(Me.StayStartDate) Then
                Me.Warnings.Add("The Stay Start Date must be a valid date")
            ElseIf IsEmptyDate(Me.StayEndDate) Then
                Me.Warnings.Add("The Stay End Date must be a valid date")
            ElseIf Me.StayStartDate < dBaseStayStartDate Then
                Me.Warnings.Add("The earliest Stay Start Date allowed is " & _
                    DisplayDate(dBaseStayStartDate))
            ElseIf Me.StayEndDate > dBaseStayEndDate Then
                Me.Warnings.Add("The latest Stay End Date allowed is " & _
                    DisplayDate(dBaseStayEndDate))
            ElseIf Me.StayStartDate > Me.StayEndDate Then
                Me.Warnings.Add("The Stay Start Date must be before the Stay End Date")
            End If
        End If

        'if we have any increases make sure we have rounding rules
        'and either Selected Room Types or 1 or more room types selected
        If Me.RateIncrease > 0 OrElse Me.SupplementIncrease > 0 Then

            If Me.RoundTo = "" Then
                Me.Warnings.Add("The Round To must have a value")
            End If

            If Me.Rounding = "" Then
                Me.Warnings.Add("The Rounding must have a value")
            End If

            If Me.AdjustmentAppliesTo = "Selected Room Types" AndAlso _
                Me.AdjustmentContractRoomTypeID.Count < 1 Then
                Me.Warnings.Add("At least one Adjustment Room Type must be selected")
            End If
        End If

        'at least one room type must be selected
        If Me.ContractRoomType.Count < 1 Then
            Me.Warnings.Add("At least one Room Type must be selected")
        End If

        'at least one section must be selected
        If Me.ContractSection.Count < 1 Then
            Me.Warnings.Add("At least one Contract Section must be selected")
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Get Base Info"
    Public Function GetBaseInfo(ByVal iContractID As Integer) As DataTable

        Return SQL.GetDatatable("exec GetContractBaseInfo {0}", iContractID)
    End Function
#End Region

#Region "Get Tactical Data"

    Public Function GetTacticalData(ByVal iContractID As Integer) As DataRow

        Return SQL.GetDataRow("select ContractIdentifier, SpecialOffer, SpecialOfferCode, " & _
            "BookingStartDate, BookingEndDate, StayStartDate, StayEndDate from Contract where ContractID={0}", _
            iContractID)

    End Function

#End Region

End Class
