Imports Intuitive
Imports System.Text

Public Class ContractSupplementValue
    Inherits TableBase

    Private iRoomTypeID As Integer = 0

    Public Sub New()

        Me.Table = "ContractSupplementValue"

        With Me.Fields
            .Add(New Field("ContractSupplementValueID", "Integer"))
            .Add(New Field("ContractSupplementID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Value", "Numeric"))
            .Add(New Field("Adult", "Numeric"))
            .Add(New Field("Child", "Numeric"))
            .Add(New Field("Child1st", "Numeric"))
            .Add(New Field("Child2nd", "Numeric"))
            .Add(New Field("Youth", "Numeric"))
            .Add(New Field("Rate", "Numeric"))
        End With

        Me.Clear()
    End Sub

    Public Shared Sub SetValues(ByVal iContractSupplementID As Integer, _
         ByVal sPaxType As String, ByVal aGridData As ArrayList, _
        ByVal sChildValueType As String, ByVal sYouthValueType As String)

        'clear existing records
        SQL.Execute("Delete from ContractSupplementValue where ContractSupplementID=" & _
            iContractSupplementID)

        Dim sb As New StringBuilder
        Dim sSql As String = "exec AddContractSupplementValue {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n"

        Dim sChildSuffix As String = IIf(sChildValueType = "Percentage", " %", "").ToString
        Dim sYouthSuffix As String = IIf(sYouthValueType = "Percentage", " %", "").ToString

        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData

            Dim oValues() As Object = {0, 0, 0, 0, 0, 0, 1}

            Select Case sPaxType
                Case "All"
                    oValues(0) = oGridRow("Value")
                    Try
                        oValues(6) = oGridRow("Rate")
                    Catch ex As Exception

                    End Try
                Case "Adult Only"
                    oValues(1) = oGridRow("Adult")

                Case "Adult/Child"
                    oValues(1) = oGridRow("Adult")
                    oValues(2) = oGridRow("Child" & sChildSuffix)

                Case "Adult/Child/Youth"
                    oValues(1) = oGridRow("Adult")
                    oValues(2) = oGridRow("Child" & sChildSuffix)
                    oValues(5) = oGridRow("Youth" & sYouthSuffix)

                Case "Adult/1st Child/2nd Child"
                    oValues(1) = oGridRow("Adult")
                    oValues(3) = oGridRow("1st Child" & sChildSuffix)
                    oValues(4) = oGridRow("2nd Child" & sChildSuffix)

                Case "Adult/1st Child/2nd Child/Youth"
                    oValues(1) = oGridRow("Adult")
                    oValues(3) = oGridRow("1st Child" & sChildSuffix)
                    oValues(4) = oGridRow("2nd Child" & sChildSuffix)
                    oValues(5) = oGridRow("Youth" & sYouthSuffix)

                Case Else
                    Throw New Exception("Could not find Pax Type " & sPaxType)
            End Select

            sb.AppendFormat(sSql, iContractSupplementID,
                oGridRow("Start"), oGridRow("End"), oValues(0), oValues(1),
                oValues(2), oValues(3), oValues(4), oValues(5), oValues(6))

        Next

        'execute
        SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

    End Sub

#Region "GridValidate"

    'Public Function GridValidate() As ArrayList
    '    Dim aWarn As New ArrayList

    '    ''check fields
    '    If Not IsDate(GetField("StartDate")) Then
    '        aWarn.Add("The Start Date must be specified")
    '    End If

    '    If Not IsDate(GetField("EndDate")) Then
    '        aWarn.Add("The End Date must be specified")
    '    End If

    '    If aWarn.Count = 0 Then
    '        Dim dStart As Date = CType(GetField("StartDate"), Date)
    '        Dim dEnd As Date = CType(GetField("EndDate"), Date)

    '        'check start b4 end
    '        If dStart > dEnd Then
    '            aWarn.Add("Sorry, start date must be before the end date")
    '        End If

    '        'make sure dates are within contract travel dates
    '        If Not inTravelDate(dStart, dEnd) Then
    '            aWarn.Add("Sorry, the date range must be within the Contract Travel Dates")
    '        End If
    '    End If

    '    Return aWarn
    'End Function

    'Private Function GetDupeCheckString() As String
    '    Dim sStart As String = SQL.GetSqlValue(CType(GetField("StartDate"), Date), "Date")
    '    Dim sEnd As String = SQL.GetSqlValue(CType(GetField("EndDate"), Date), "Date")
    '    Dim sReturn As String

    '    Return String.Format("And (ContractSupplementID<>{0} " & _
    '              "And StartDate<>{1} " & _
    '              "And EndDate<>{2} " & _
    '              "And Adult<>{3} " & _
    '              "And Child<>{4} " & _
    '              "And Child1st<>{5} " & _
    '              "And Child2nd<>{6})", GetField("ContractSupplementID"), _
    '              sStart, sEnd, _
    '              GetField("Adult"), GetField("Adult"), _
    '              GetField("Adult"), GetField("Adult"))
    'End Function

    'Private Function inTravelDate(ByVal dStart As Date, ByVal dEnd As Date) As Boolean
    '    Dim iContractID As Integer = GetContractID(CType(GetField("ContractSupplementID"), Integer))
    '    Dim iResult As Integer
    '    Dim sSql As String = "" & _
    '        "Select count(ContractID) " & _
    '        "From Contract  " & _
    '        "Where ContractID={2} " & _
    '        "And ({0} Between StayStartDate And StayEndDate  " & _
    '        "And {1} Between StayStartDate And StayEndDate)"

    '    'check our date range is between our contract stay date range
    '    iResult = SQL.ExecuteSingleValue(sSql, _
    '                        SQL.GetSqlValue(dStart, "Date"), _
    '                        SQL.GetSqlValue(dEnd, "Date"), iContractID)

    '    Return (iResult > 0)
    'End Function

    'Private Function GetContractID(ByVal iSupplementID As Integer) As Integer
    '    Return SQL.ExecuteSingleValue("Select ContractID " & _
    '            "From ContractSupplement " & _
    '            "Where ContractSupplementID={0}", GetField("ContractSupplementID"))
    'End Function

    'Private Function GetRoomTypeID(ByVal iSupplementID As Integer) As Integer
    '    Return SQL.ExecuteSingleValue("Select ContractRoomTypeID " & _
    '            "From ContractSupplement " & _
    '            "Where ContractSupplementID={0}", GetField("ContractSupplementID"))
    'End Function
#End Region

End Class
