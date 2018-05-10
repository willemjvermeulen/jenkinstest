Imports Intuitive
Imports Intuitive.DateFunctions

Public Class ContractPaymentTermOverride

    'Add Dates
    Public Shared Function AddDates(ByVal iContractID As Integer, _
        ByVal sStartDate As String, ByVal sEndDate As String, ByVal sStayStartDate As String, _
        ByVal sStayEndDate As String) As FunctionReturn

        Dim oReturn As New FunctionReturn
        Dim dStartDate As Date
        Dim dEndDate As Date
        Dim dStayStartDate As Date
        Dim dStayEndDate As Date

        'validate the dates
        If Not IsDisplayDate(sStartDate) Then
            oReturn.AddWarning("A valid Start Date must be input")
        End If
        If Not IsDisplayDate(sEndDate) Then
            oReturn.AddWarning("A valid End Date must be input")
        End If

        'if valid dates do some more checking, end date after the start date
        'and that the dates do not conflict with other entries
        'and that are within the contract stay dates
        If oReturn.Success Then
            dStartDate = SafeDate(sStartDate)
            dEndDate = SafeDate(sEndDate)
            dStayStartDate = SafeDate(sStayStartDate)
            dStayEndDate = SafeDate(sStayEndDate)

            If dEndDate < dStartDate Then
                oReturn.AddWarning("The Start Date must be before the End Date")
            ElseIf Not CheckNoOverlappingDates(iContractID, dStartDate, dEndDate) Then
                oReturn.AddWarning("The dates input conflict with existing dates for this Contract")
            ElseIf dStartDate < dStayStartDate OrElse dEndDate > dStayEndDate Then
                oReturn.AddWarning("The Payment Term Dates must be within the Contract Stay Dates")
            End If
        End If

        'do the bizzo
        If oReturn.Success Then
            oReturn.Result = SQL.Execute("exec ContractPaymentTermOverrideAdd {0}, {1}, {2}", _
                iContractID, SQL.GetSqlValue(dStartDate, SQL.SqlValueType.Date), _
                SQL.GetSqlValue(dEndDate, SQL.SqlValueType.Date))
        End If

        Return oReturn
    End Function


    'Check No Overlapping Dates
    Public Shared Function CheckNoOverlappingDates(ByVal iContractID As Integer, _
        ByVal dStartDate As Date, ByVal dEndDate As Date) As Boolean


        Return SQL.ExecuteSingleValue("exec ContractPaymentTermOverrideCheckForOverlap {0},{1},{2}", _
            iContractID, SQL.GetSqlValue(dStartDate, SQL.SqlValueType.Date), _
            SQL.GetSqlValue(dEndDate, SQL.SqlValueType.Date)) = 0

    End Function

    Public Shared Sub Delete(ByVal iContractPaymentTermOverrideID As Integer)

        SQL.Execute("exec ContractPaymentTermDelete {0}", iContractPaymentTermOverrideID)

    End Sub

End Class
