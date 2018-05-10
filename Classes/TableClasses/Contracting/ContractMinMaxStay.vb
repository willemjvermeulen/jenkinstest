Imports Intuitive
Public Class ContractMinMaxStay
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractMinMaxStay"

        With Me.Fields
            .Add(New Field("ContractMinMaxStayID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("MinStay", "Integer"))
            .Add(New Field("MaxStay", "Integer"))
            .Add(New Field("DayType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("DayMon", "Boolean"))
            .Add(New Field("DayTue", "Boolean"))
            .Add(New Field("DayWed", "Boolean"))
            .Add(New Field("DayThu", "Boolean"))
            .Add(New Field("DayFri", "Boolean"))
            .Add(New Field("DaySat", "Boolean"))
            .Add(New Field("DaySun", "Boolean"))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call the default CheckUpdate
        MyBase.CheckUpdate()

        'if no errors
        If Me.Warnings.Count = 0 Then

            Dim dStart As Date = CType(Me.GetField("StartDate"), Date)
            Dim dEnd As Date = CType(Me.GetField("EndDate"), Date)
            Dim iMinStay As Integer = CType(Me.GetField("MinStay"), Integer)
            Dim iMaxStay As Integer = CType(Me.GetField("MaxStay"), Integer)

            'make sure the startdate is b4 the enddate
            If dStart > dEnd Then
                Me.Warnings.Add("The Start Date must be before the End Date")
            End If

            'make sure dates are within contract travel dates
            If Not InTravelDate(dStart, dEnd) Then
                Me.Warnings.Add("The date range must be within the contract Stay Dates")
            End If

            'check min OR max stay is specified
            If iMinStay = 0 AndAlso iMaxStay = 0 Then
                Warnings.Add("You must specify either a Minimum or Maximum Stay")
            End If

            'Check that Min Stay is not greater than the Max Stay (if Max Stay > 0)
            If iMaxStay > 0 AndAlso iMinStay > iMaxStay Then
                Warnings.Add("The Minimum Stay cannot be greater than the Maximum Stay")
            End If

            'check if 'Day Type' is 'Specific Days', that we have at least one day selected
            If GetField("DayType") = "Specific Days" Then
                'if none of these are true - warn
                If Not (GetField("DayMon").ToLower = "true" Or GetField("DayTue").ToLower = "true" Or _
                    GetField("DayWed").ToLower = "true" Or GetField("DayThu").ToLower = "true" Or _
                    GetField("DayFri").ToLower = "true" Or GetField("DaySat").ToLower = "true" Or _
                    GetField("DaySun").ToLower = "true") Then

                    Me.Warnings.Add("You must select at least one specific day")
                End If
            End If
        End If

        Return (Me.Warnings.Count = 0)
    End Function

    Private Function InTravelDate(ByVal dStart As Date, ByVal dEnd As Date) As Boolean
        Dim iContractID As Integer = CType(GetField("ContractID"), Integer)
        Dim iResult As Integer
        Dim sSql As String = "" & _
            "Select count(ContractID) " & _
            "From Contract  " & _
            "Where ContractID={2} " & _
            "And ({0} Between StayStartDate And StayEndDate  " & _
            "And {1} Between StayStartDate And StayEndDate)"

        'check our date range is between our contract stay date range
        iResult = SQL.ExecuteSingleValue(sSql, _
                            SQL.GetSqlValue(dStart, "Date"), _
                            SQL.GetSqlValue(dEnd, "Date"), iContractID)

        Return (iResult > 0)
    End Function

#End Region

End Class
