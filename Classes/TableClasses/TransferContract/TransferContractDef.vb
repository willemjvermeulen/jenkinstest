Imports Intuitive
Imports Intuitive.Functions
Public Class TransferContractDef

    Public Shared Sub AddRates(ByVal iTransferContractDateID As Integer, ByVal dtRates As DataTable)

        Dim oSQLTransaction As New SQLTransaction
        Dim sSQL As String = "Insert into transferContractDef (TransferContractDateID, " & _
            "TransferContractJourneyID, TransferContractPaxID, Value, Adult, Child) Values ({0},{1},{2},{3},{4},{5})"

        With oSQLTransaction

            'delete the old stuff first
            .Add("Delete from TransferContractDef Where TransferContractDateID={0}", iTransferContractDateID)

            'add the rest
            For Each drRow As DataRow In dtRates.Rows
                .Add(sSQL, iTransferContractDateID, drRow("TransferContractJourneyID"), _
                    drRow("TransferContractPaxID"), drRow("Value"), drRow("Adult"), drRow("Child"))
            Next

            'and execute
            .Execute()

        End With

        TransferContract.SetLastModifiedData(, iTransferContractDateID)

    End Sub
End Class
