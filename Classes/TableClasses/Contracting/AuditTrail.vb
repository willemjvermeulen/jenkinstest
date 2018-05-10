Imports System.Text
Imports Intuitive
Imports Intuitive.Functions

Public Class AuditTrail
    Inherits TableBase

    Public Sub New()

        Me.Table = "AuditTrail"

        With Me.Fields
            .Add(New Field("AuditTrailID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("SignedSystemUserID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("SignedDateTime", "Datetime", ValidationType.NotEmptyIsDate))
            .Add(New Field("Notes", "Text"))
        End With

        Me.Clear()
    End Sub

#Region "signoff"
    Public Shared Function SignOff(ByVal iContractID As Integer, _
            ByVal iSystemUserID As Integer, ByVal sNotes As String, _
            ByVal aAuditTrailEntryID As ArrayList) As FunctionReturn

        Dim oReturn As New FunctionReturn

        'validate the data first
        If sNotes = "" Then
            oReturn.AddWarning("The Reason for the changes must be input")
        End If
        If aAuditTrailEntryID.Count = 0 Then
            oReturn.AddWarning("At least one Audit Trail entry must be selected")
        End If

        'if all ok, do the bizzo
        If oReturn.Success Then

            Dim sSql As String = "exec SignOffAuditTrail {0},{1},{2},{3},{4}"

            SQL.Execute(sSql, iContractID, iSystemUserID, _
                SQL.GetSqlValue(sNotes, SQL.SqlValueType.String), _
                SQL.GetSqlValue(ArrayListToDelimitedString(aAuditTrailEntryID), SQL.SqlValueType.String), _
                SQL.GetSqlValue(AuditTrail.GetXML(iContractID), SQL.SqlValueType.String))

            'sync settings
            Contract.SetSyncRequired(iContractID)

        End If

        Return oReturn
    End Function
#End Region

#Region "Status Change"
    Public Shared Sub StatusChange(ByVal iContractID As Integer, ByVal iSystemUserID As Integer, _
            ByVal sOldStatus As String, ByVal sStatus As String)

        Dim sSql As String = "exec SignOffAuditTrail {0}, {1}, {2}, '{3}', {4}"

        SQL.Execute(sSql, iContractID, iSystemUserID, _
            SQL.GetSqlValue("Status changed from " & sOldStatus & " to " & sStatus, SQL.SqlValueType.String), _
            "", SQL.GetSqlValue(AuditTrail.GetXML(iContractID), SQL.SqlValueType.String))

        'sync setting
        Contract.SetSyncRequired(iContractID)

    End Sub
#End Region

#Region "Get XML"
    Private Shared Function GetXML(ByVal iContractID As Integer) As String

        Dim dtXML As DataTable = SQL.GetDatatable("exec XMLGetContracts '{0}'", iContractID)
        Dim oSB As New StringBuilder
        For Each drRow As DataRow In dtXML.Rows
            oSB.Append(drRow(0).ToString)
        Next

        Return oSB.ToString
    End Function
#End Region

End Class

