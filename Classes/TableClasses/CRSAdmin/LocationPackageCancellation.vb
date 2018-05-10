Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls.Grid


Public Class LocationPackageCancellation

    Public Shared Function AddEntries(ByVal iLocationPackageID As Integer, _
        ByVal iLocationPackageCancellationOverrideID As Integer, ByVal aGridData As ArrayList) _
            As FunctionReturn

        Dim oReturn As New FunctionReturn

        'validate grid data
        ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            Dim sTable As String
            Dim sIDField As String
            Dim iID As Integer
            If iLocationPackageCancellationOverrideID < 1 Then
                sTable = "LocationPackageCancellation"
                sIDField = "LocationPackageID"
                iID = iLocationPackageID
            Else
                sTable = "LocationPackageCancellationOverrideDef"
                sIDField = "LocationPackageCancellationOverrideID"
                iID = iLocationPackageCancellationOverrideID
            End If

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from {0} where " & _
                "{1}={2}", sTable, sIDField, iID)

            'add new entries
            Dim sInsert As String = "insert into {0} " & _
                "({1}, DaysPriorArrival, Penalty, Value) values ({2},{3},{4},{5})"

            For Each oGridRow As GridRow In aGridData

                Dim iDaysPrior As Integer = SafeInt(oGridRow("Days Prior"))
                Dim sPenalty As String = SafeString(oGridRow("Penalty"))
                Dim nValue As Double = SafeNumeric(oGridRow("Value"))

                oSqlTrans.Add(sInsert, sTable, sIDField, iID, iDaysPrior, _
                    SQL.GetSqlValue(sPenalty, SQL.SqlValueType.String), _
                    SQL.GetSqlValue(nValue, SQL.SqlValueType.Numberic))

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

            'any information
            If iDaysPrior > 0 OrElse sPenalty <> "" Then
                If iDaysPrior = 0 Then
                    oReturn.AddWarning("The Days Prior must be set on Row {0}", iRow)
                End If
                If sPenalty = "" Then
                    oReturn.AddWarning("The Penalty must be set on Row {0}", iRow)
                End If
            End If

            iRow += 1
        Next
    End Sub
End Class
