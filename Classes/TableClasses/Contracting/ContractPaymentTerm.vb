Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls.Grid

Public Class ContractPaymentTerm

    Public Shared Function AddEntries(ByVal iContractID As Integer, _
    ByVal iContractPaymentTermOverrideID As Integer, ByVal aGridData As ArrayList) _
        As FunctionReturn

        Dim oReturn As New FunctionReturn

        'validate grid data
        ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            Dim sTable As String
            Dim sIDField As String
            Dim iID As Integer
            If iContractPaymentTermOverrideID < 1 Then
                sTable = "ContractPaymentTerm"
                sIDField = "ContractID"
                iID = iContractID
            Else
                sTable = "ContractPaymentTermOverrideDef"
                sIDField = "ContractPaymentTermOverrideID"
                iID = iContractPaymentTermOverrideID
            End If

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from {0} where " & _
                "{1}={2}", sTable, sIDField, iID)

            'add new entries
            Dim sInsert As String = "insert into {0} " & _
                "({1}, DateType, OffsetType, OffsetDays, Type, Amount, PayLocal) values ({2},{3},{4},{5},{6},{7},{8})"

            For Each oGridRow As GridRow In aGridData

                Dim iOffsetDays As Integer = SafeInt(oGridRow("Days"))
                Dim sOffsetType As String = SafeString(oGridRow("Offset Type"))
                Dim sDateType As String = SafeString(oGridRow("Date"))
                Dim sType As String = SafeString(oGridRow("Type"))
                Dim nAmount As Double = SafeNumeric(oGridRow("Amount"))
                Dim bPayLocal As Boolean = SafeBoolean(oGridRow("Pay Local"))

                oSqlTrans.Add(sInsert, sTable, sIDField, iID, _
                    SQL.GetSqlValue(sDateType, SQL.SqlValueType.String), _
                    SQL.GetSqlValue(sOffsetType, SQL.SqlValueType.String), iOffsetDays, _
                    SQL.GetSqlValue(sType, SQL.SqlValueType.String), _
                    SQL.GetSqlValue(nAmount, SQL.SqlValueType.Numberic), _
                    SQL.GetSqlValue(bPayLocal, SQL.SqlValueType.Boolean))

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

            Dim iOffsetDays As Integer = SafeInt(oGridRow("Days"))
            Dim sOffsetType As String = SafeString(oGridRow("Offset Type"))
            Dim sDateType As String = SafeString(oGridRow("Date"))
            Dim sType As String = SafeString(oGridRow("Type"))
            Dim nAmount As Double = SafeNumeric(oGridRow("Amount"))
            Dim bPayLocal As Boolean = SafeBoolean(oGridRow("Pay Local"))

            'any information
            If sDateType <> "" OrElse sOffsetType <> "" OrElse iOffsetDays > 0 OrElse sType <> "" _
                OrElse nAmount > 0 OrElse bPayLocal = True Then

                If iOffsetDays < 0 Then
                    oReturn.AddWarning("The Number of Offset Days must be set on Row {0}", iRow)
                End If

                If sOffsetType = "" Then
                    oReturn.AddWarning("The Offset Type must be set on Row {0}", iRow)
                End If

                If sDateType = "" Then
                    oReturn.AddWarning("The Date Type must be set on Row {0}", iRow)
                End If

                If sType = "" Then
                    oReturn.AddWarning("The Type must be set on Row {0}", iRow)
                End If

                If nAmount = 0 AndAlso InList(sType, "# Nights", "Percentage", "Amount") Then
                    oReturn.AddWarning("The Amount must be set on Row {0}", iRow)
                End If

            End If

            iRow += 1
        Next
    End Sub

End Class
