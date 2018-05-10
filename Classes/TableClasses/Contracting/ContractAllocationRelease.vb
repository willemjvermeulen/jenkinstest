Imports Intuitive
Imports System.Text

Public Class ContractAllocationRelease

    Public Shared Sub SetValues(ByVal iContractID As Integer, _
        ByVal iContractRoomTypeID As Integer, ByVal bSplit As Boolean, ByVal aGridData As ArrayList)

        'clear existing records
        If iContractRoomTypeID = 0 Then
            SQL.Execute("Delete from ContractAllocationRelease where ContractID={0} and ContractRoomTypeID=0", iContractID)
        Else
            SQL.Execute("Delete from ContractAllocationRelease where ContractRoomTypeID=" & iContractRoomTypeID)
        End If

        'If at all level, remove release data from roomtypes
        If iContractRoomTypeID = 0 Then
            SQL.Execute("Update ContractAllocationRelease Set Release = null, ReleaseSplit = null," & _
                "AllocationSplit = null Where ContractRoomTypeID <> 0 And ContractID = {0}", iContractID)
        End If

        'get count of contract level entries
        Dim iAllLevelDataCount As Integer = _
            SQL.ExecuteSingleValue("Select count(*) From ContractAllocationRelease " & _
                "Where ContractID={0} and ContractRoomTypeID=0", iContractID)


        'work out if we have split allocations - we'll have six columns
        'set up stored procedure delcare
        Dim sSql As String = "exec AddAllocationRelease {0},{1},{2},{3},{4},{5},{6},{7}"

        Dim oGridRow As New Intuitive.WebControls.Grid.GridRow

        Dim iAllocation As Integer = 0
        Dim iRelease As Integer = 0
        Dim iAllocationSplit As Integer = 0
        Dim iReleaseSplit As Integer = 0
        Dim oSQLTrans As New SQLTransaction

        'scan through, build up sql
        If aGridData.Count > 0 Then
            For Each oGridRow In aGridData

                If bSplit Then
                    iAllocationSplit = Intuitive.Functions.SafeInt(oGridRow("All (split)"))
                    iReleaseSplit = Intuitive.Functions.SafeInt(oGridRow("Rel (split)"))
                End If

                'if contract level, then don't get allocation
                If Not iContractRoomTypeID = 0 Then
                    iAllocation = Intuitive.Functions.SafeInt(oGridRow("Allocation"))
                End If

                'if we have contract info then don't get release
                If iContractRoomTypeID = 0 OrElse iAllLevelDataCount = 0 Then
                    iRelease = Intuitive.Functions.SafeInt(oGridRow("Release"))
                End If

                oSQLTrans.Add(sSql, _
                    iContractID, iContractRoomTypeID, _
                    oGridRow("Start"), oGridRow("End"), iAllocation, iRelease, _
                    iAllocationSplit, iReleaseSplit)

            Next

            osqltrans.Execute()
        End If

    End Sub

End Class
