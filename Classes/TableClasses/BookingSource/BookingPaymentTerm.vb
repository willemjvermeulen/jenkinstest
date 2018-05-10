Imports Intuitive
Imports Intuitive.Functions
Imports Intuitive.WebControls.Grid

Public Interface BookingPaymentTerm
    Function AddEntries(ByVal iID As Integer, ByVal aGridData As ArrayList) As FunctionReturn
End Interface

'Booking source Payment Terms
Public Class BookingSourcePaymentTerm
    Implements BookingPaymentTerm

#Region "Booking Source Payment Term - Add Entries"

    Public Function AddEntries(ByVal iID As Integer, ByVal aGridData As System.Collections.ArrayList) As Intuitive.FunctionReturn Implements BookingPaymentTerm.AddEntries

        Dim oReturn As New FunctionReturn

        'validate grid data
        BookingSourcePaymentTerm.ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from BookingSourcePaymentTerm where " & _
                "BookingSourceID={0}", iID)

            'add new entries
            Dim sInsert As String = "insert into BookingSourcePaymentTerm (BookingSourceID, DateType, " & _
                "OffsetType, OffsetDays, Type, Amount, PayLocal) values ({0},{1},{2},{3},{4},{5},{6})"

            For Each oGridRow As GridRow In aGridData

                Dim iOffsetDays As Integer = SafeInt(oGridRow("Days"))
                Dim sOffsetType As String = SafeString(oGridRow("Offset Type"))
                Dim sDateType As String = SafeString(oGridRow("Date"))
                Dim sType As String = SafeString(oGridRow("Type"))
                Dim nAmount As Double = SafeNumeric(oGridRow("Amount"))
                Dim bPayLocal As Boolean = SafeBoolean(oGridRow("Pay Local"))

                oSqlTrans.Add(sInsert, iID, SQL.GetSqlValue(sDateType, SQL.SqlValueType.String), _
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

#End Region

#Region "Validate Grid"

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

                If sDateType = "" Then
                    oReturn.AddWarning("The Date Type must be set on Row {0}", iRow)
                End If

                If sOffsetType = "" Then
                    oReturn.AddWarning("The Offset Type must be set on Row {0}", iRow)
                End If

                If iOffsetDays < 0 Then
                    oReturn.AddWarning("The Number of Offset Days must be set on Row {0}", iRow)
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

#End Region

End Class

Public Class BookingSourcePaymentTermOverrideDef
    Implements BookingPaymentTerm

#Region "Booking Source Payment Term Override Def - Add Entries"

    Public Function AddEntries(ByVal iID As Integer, ByVal aGridData As System.Collections.ArrayList) As Intuitive.FunctionReturn Implements BookingPaymentTerm.AddEntries

        Dim oReturn As New FunctionReturn

        'validate grid data
        BookingSourcePaymentTerm.ValidateGridData(aGridData, oReturn)

        'do the bizzo
        If oReturn.Success Then

            'clear existing records
            Dim oSqlTrans As New SQLTransaction
            oSqlTrans.Add("delete from BookingSourcePaymentTermOverrideDef where " & _
                "BookingSourcePaymentTermOverrideDefID={0}", iID)

            'add new entries
            Dim sInsert As String = "insert into BookingSourcePaymentTermOverrideDef (BookingSourceID, " & _
            "DateType, OffsetType, OffsetDays, Type, Amount, PayLocal) values ({0},{1},{2},{3},{4},{5},{6})"

            For Each oGridRow As GridRow In aGridData

                Dim iOffsetDays As Integer = SafeInt(oGridRow("Days"))
                Dim sOffsetType As String = SafeString(oGridRow("Offset Type"))
                Dim sDateType As String = SafeString(oGridRow("Date"))
                Dim sType As String = SafeString(oGridRow("Type"))
                Dim nAmount As Double = SafeNumeric(oGridRow("Amount"))
                Dim bPayLocal As Boolean = SafeBoolean(oGridRow("Pay Local"))

                oSqlTrans.Add(sInsert, iID, SQL.GetSqlValue(sDateType, SQL.SqlValueType.String), _
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

#End Region

End Class

