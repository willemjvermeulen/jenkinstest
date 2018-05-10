Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls.Grid

Public Interface BookingCancellation
    Function AddEntries(ByVal iID As Integer, ByVal aGridData As ArrayList) As FunctionReturn
End Interface


'booking source cancellation
Public Class BookingSourceCancellation
    Implements BookingCancellation

    Public Function AddEntries(ByVal iID As Integer, ByVal aGridData As System.Collections.ArrayList) As Intuitive.FunctionReturn Implements BookingCancellation.AddEntries

        Dim oReturn As New FunctionReturn

        'validate grid data
        BookingSourceCancellation.ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from BookingSourceCancellation where " & _
                "BookingSourceID={0}", iID)

            'add new entries
            Dim sInsert As String = "insert into BookingSourceCancellation " & _
                "(BookingSourceID, DaysPriorArrival, Penalty, Value, MinAmount) values ({0},{1},{2},{3},{4})"

            For Each oGridRow As GridRow In aGridData

                Dim iDaysPrior As Integer = SafeInt(oGridRow("Days Prior"))
                Dim sPenalty As String = SafeString(oGridRow("Penalty"))
                Dim nValue As Double = SafeNumeric(oGridRow("Value"))
                Dim nMinAmount As Double = SafeNumeric(oGridRow("Min Amount"))

                oSqlTrans.Add(sInsert, iID, iDaysPrior, _
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
            Dim nMinAmount As Double = SafeNumeric(oGridRow("Min Amount"))

            'any information
            If iDaysPrior > 0 OrElse sPenalty <> "" Then
                If iDaysPrior = 0 Then
                    oReturn.AddWarning("The Days Prior must be set on Row {0}", iRow)
                End If
                If sPenalty = "" Then
                    oReturn.AddWarning("The Penalty must be set on Row {0}", iRow)
                End If
            End If

            'not min amount only
            If nMinAmount <> 0 AndAlso sPenalty = "" Then
                oReturn.AddWarning("The Penalty must be set on Row {0}", iRow)
            ElseIf nMinAmount <> 0 AndAlso sPenalty = "No Penalty" Then
                oReturn.AddWarning("A Minimum Amount cannot be specified on Row {0} as No Penalty has been set", iRow)
            End If

            iRow += 1
        Next
    End Sub

End Class


'booking source cancellation overridedef
Public Class BookingSourceCancellationOverrideDef
    Implements BookingCancellation

    Public Function AddEntries(ByVal iID As Integer, ByVal aGridData As System.Collections.ArrayList) As Intuitive.FunctionReturn Implements BookingCancellation.AddEntries


        Dim oReturn As New FunctionReturn

        'validate grid data
        BookingSourceCancellation.ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from BookingSourceCancellationOverrideDef where " & _
                "BookingSourceCancellationOverrideDefID={0}", iID)

            'add new entries
            Dim sInsert As String = "insert into BookingSourceCancellationOverrideDef " & _
                "(BookingSourceCancellationOverrideID, DaysPriorArrival, Penalty, Value, MinAmount) values ({0},{1},{2},{3},{4)"

            For Each oGridRow As GridRow In aGridData

                Dim iDaysPrior As Integer = SafeInt(oGridRow("Days Prior"))
                Dim sPenalty As String = SafeString(oGridRow("Penalty"))
                Dim nValue As Double = SafeNumeric(oGridRow("Value"))
                Dim nMinAmount As Double = SafeNumeric(oGridRow("Min Amount"))

                oSqlTrans.Add(sInsert, iID, iDaysPrior, _
                    SQL.GetSqlValue(sPenalty, SQL.SqlValueType.String), _
                    SQL.GetSqlValue(nValue, SQL.SqlValueType.Numberic), _
                    SQL.GetSqlValue(nMinAmount, SQL.SqlValueType.Numberic))

            Next


            'execute - bosh
            oSqlTrans.Execute()

        End If

        Return oReturn

    End Function
End Class

