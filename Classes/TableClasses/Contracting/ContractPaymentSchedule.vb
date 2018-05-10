Imports Intuitive
Imports System.Text

Public Class ContractPaymentSchedule


    Public Shared Function SetPaymentSchedule(ByVal iContractID As Integer, _
        ByVal aGridValues As ArrayList, ByRef iAdded As Integer) As ArrayList

        Dim aWarnings As New ArrayList
        iAdded = 0

        'validate the grid values first
        'make sure there is a payment date, currencyid and value
        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        Dim iRowCount As Integer = 1
        For Each oGridRow In aGridValues

            If oGridRow("Date") = "null" AndAlso Not oGridRow("Value") = "0" Then
                aWarnings.Add("There is not a valid date on Row " & iRowCount)
            End If
            If oGridRow("Currency") = "0" Then
                aWarnings.Add("There is not a Currency selected for Row " & iRowCount)
            End If
            If oGridRow("Value") = "0" AndAlso Not oGridRow("Date") = "null" Then
                aWarnings.Add("No Value has been input on Row " & iRowCount)
            End If
            iRowCount += 1
        Next

        'if all is well then do the update
        If aWarnings.Count = 0 Then
            Dim sb As New StringBuilder

            'delete any existing rows
            sb.AppendFormat("Delete from ContractPaymentSchedule where ContractID={0}\n", iContractID)

            Dim sSql As String = "exec AddContractPaymentSchedule {0},{1},{2},{3}\n"

            For Each oGridRow In aGridValues
                If Not oGridRow("Date") = "null" AndAlso Not oGridRow("Value") = "0" Then
                    sb.AppendFormat(sSql, iContractID, oGridRow("Date"), _
                        oGridRow("Currency"), oGridRow("Value"))
                    iAdded += 1
                End If
            Next

            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))
        End If

        Return aWarnings

    End Function



End Class
