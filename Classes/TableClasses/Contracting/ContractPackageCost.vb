Imports Intuitive
Imports Intuitive.Functions

Public Class ContractPackageCost
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractPackageCost"

        With Me.Fields
            .Add(New Field("ContractPackageCostID", "Integer"))
            .Add(New Field("ContractPackageID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Value", "Numeric"))
            .Add(New Field("Adult", "Numeric"))
            .Add(New Field("Child", "Numeric"))
            .Add(New Field("Youth", "Numeric"))
        End With

        Me.Clear()
    End Sub

#Region "GridValidate"

    Public Function GridValidate() As ArrayList
        Dim aWarn As New ArrayList

        ''check fields
        If Not IsDate(GetField("StartDate")) Then
            aWarn.Add("The Start Date must be specified")
        End If

        If Not IsDate(GetField("EndDate")) Then
            aWarn.Add("The End Date must be specified")
        End If

        If aWarn.Count = 0 Then
            Dim dStart As Date = CType(GetField("StartDate"), Date)
            Dim dEnd As Date = CType(GetField("EndDate"), Date)

            'check start b4 end
            If dStart > dEnd Then
                aWarn.Add("Sorry, the start date must be before the end date")
            End If

            'make sure dates are within contract travel dates
            If Not inTravelDate(dStart, dEnd) Then
                aWarn.Add("Sorry, the date range must be within the contract Travel Dates")
            End If
        End If

        Return aWarn
    End Function

    Private Function inTravelDate(ByVal dStart As Date, ByVal dEnd As Date) As Boolean
        Dim iContractID As Integer = GetContractID(CType(GetField("ContractPackageID"), Integer))
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

    Private Function GetContractID(ByVal iPackageID As Integer) As Integer
        Return SQL.ExecuteSingleValue("Select ContractID " & _
                "From ContractPackage " & _
                "Where ContractPAckageID={0}", iPackageID)
    End Function

    Private Function GetRoomTypeID(ByVal iPackageID As Integer) As Integer
        Return SQL.ExecuteSingleValue("Select ContractRoomTypeID " & _
                "From ContractPackage " & _
                "Where ContractPackageID={0}", iPackageID)
    End Function
#End Region

    'Public Shared Function GetContractPackageCostIDByDate(ByVal iContractPackageID As Integer, _
    '    ByVal dArrivalDate As Date) As Integer
    '    Dim dr As DataRow = SQL.GetDataRow("GetContractPackageCostsByDate {0}, {1}", _
    '            iContractPackageID, _
    '            SQL.GetSqlValue(dArrivalDate, SQL.SqlValueType.Date))

    '    Return SafeInt(dr("ContractPackageCostID"))
    'End Function

End Class
