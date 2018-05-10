Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls.Grid


Public Class ContractCancellation

    Public Shared Function AddEntries(ByVal iContractID As Integer, _
        ByVal iContractCancellationOverrideID As Integer, ByVal aGridData As ArrayList) _
            As FunctionReturn

        Dim oReturn As New FunctionReturn

        'validate grid data
        ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            Dim sTable As String
            Dim sIDField As String
            Dim iID As Integer
            If iContractCancellationOverrideID < 1 Then
                sTable = "ContractCancellation"
                sIDField = "ContractID"
                iID = iContractID
            Else
                sTable = "ContractCancellationOverrideDef"
                sIDField = "ContractCancellationOverrideID"
                iID = iContractCancellationOverrideID
            End If


            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from {0} where " & _
                "{1}={2}", sTable, sIDField, iID)

            'add new entries
            Dim sInsert As String = "insert into {0} " & _
                "({1}, DaysPriorArrival, Penalty, Value, MinAmount) values ({2},{3},{4},{5},{6})"

            For Each oGridRow As GridRow In aGridData

                Dim iDaysPrior As Integer = SafeInt(oGridRow("Days Prior"))
                Dim sPenalty As String = SafeString(oGridRow("Penalty"))
                Dim nValue As Double = SafeNumeric(oGridRow("Value"))
                Dim nMinAmount As Double = SafeNumeric(oGridRow("Min Amount"))

                oSqlTrans.Add(sInsert, sTable, sIDField, iID, iDaysPrior, _
                    SQL.GetSqlValue(sPenalty, SQL.SqlValueType.String), _
                    SQL.GetSqlValue(nValue, SQL.SqlValueType.Numberic), _
                    SQL.GetSqlValue(nMinAmount, SQL.SqlValueType.Numberic))

            Next

            'execute - bosh
            oSqlTrans.Execute()

        End If

        Return oReturn

    End Function

    Public Shared Sub ValidateGridData(ByVal aGridData As ArrayList, _
        ByRef oReturn As FunctionReturn)

        Dim iRow As Integer = 1

        For Each oGridRow As GridRow In aGridData

            Dim iDaysPrior As Integer = SafeInt(oGridRow("Days Prior"))
            Dim sPenalty As String = SafeString(oGridRow("Penalty"))
            Dim nValue As Double = SafeNumeric(oGridRow("Value"))
            Dim nMinAmount As Double = SafeNumeric(oGridRow("Min Amount"))

            'any information
            If String.IsNullOrWhiteSpace(sPenalty) Then
                oReturn.AddWarning("The Penalty must be set on Row {0}", iRow)
            End If

            'not min amount only
            If nMinAmount <> 0 AndAlso String.IsNullOrWhiteSpace(sPenalty) Then
                oReturn.AddWarning("The Penalty must be set on Row {0}", iRow)
            ElseIf nMinAmount <> 0 AndAlso sPenalty = "No Penalty" Then
                oReturn.AddWarning("A Minimum Amount cannot be specified on Row {0} as No Penalty has been set", iRow)
            End If

            iRow += 1
        Next
    End Sub
End Class
