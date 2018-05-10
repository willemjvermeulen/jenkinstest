Imports Intuitive
Public Class ContractCloseOut
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractCloseOut"

        With Me.Fields
            .Add(New Field("ContractCloseOutID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("CloseOutType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))

        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()

        If Me.Warnings.Count < 1 Then
            Dim dStart As Date = CType(Me.GetField("StartDate"), Date)
            Dim dEnd As Date = CType(Me.GetField("EndDate"), Date)

            'make sure the startdate is b4 the enddate
            If dStart > dEnd Then
                Me.Warnings.Add("The Start Date must be before the End Date")
            End If

            'make sure dates are within contract travel dates
            If Not InTravelDate(dStart, dEnd) Then
                Me.Warnings.Add("The date range must be within the contract Stay Dates")
            End If

            'make sure dates do not overlap
            If Me.RangeOverlaps(dStart, dEnd) Then
                Me.Warnings.Add("The dates entered overlap an existing close out period")
            End If

        End If

        Return Me.Warnings.Count < 1
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

    Private Function RangeOverlaps(ByVal dStart As Date, ByVal dEnd As Date) As Boolean
        Dim sSQL As String
        Dim iResult As Integer
        Dim iContractID As Integer = CType(GetField("ContractID"), Integer)
        Dim iRoomTypeID As Integer = CType(GetField("ContractRoomTypeID"), Integer)
        Dim iCloseOutID As Integer = CType(GetField("ContractCloseOutID"), Integer)

        'Get back closeoutIDs 4 same contract and roomtype that 
        'has overlapping dates with our start and end dates
        sSQL = "Select Count(ContractCloseOutID) " & _
            " From ContractCloseOut " & _
            " Where ContractID = {2} " & _
            " And ContractRoomTypeID={3} " & _
            " And ({0} Between StartDate And EndDate " & _
            " Or {1} Between StartDate And EndDate) " & _
            " And ContractCloseOutID<>{4}"

        'see how many recs we get back
        iResult = SQL.ExecuteSingleValue(sSQL, _
                    SQL.GetSqlValue(dStart, "Date"), _
                    SQL.GetSqlValue(dEnd, "Date"), iContractID, iRoomTypeID, iCloseOutID)

        If iResult > 0 Then
            Return True
        End If
    End Function

#End Region

End Class
