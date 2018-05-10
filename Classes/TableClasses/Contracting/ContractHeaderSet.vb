Imports Intuitive
Imports System.Text

Public Class ContractHeaderSet
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractHeaderSet"

        With Me.Fields
            .Add(New Field("ContractHeaderSetID", "Integer"))
            .Add(New Field("ContractHeaderSet", "String", 25, ValidationType.NotEmptyNotDupe))
            .Add(New Field("ContractHeaderGroup", "String", 20, ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from ContractHeaderSetDef
        SQL.Execute("Delete from ContractHeaderSetDef Where ContractHeaderSetID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function


    Public Shared Sub AddHeaders(ByVal iContractHeaderSetID As Integer, ByVal aContractHeaderID As ArrayList)

        Dim sb As New StringBuilder
        sb.AppendFormat("Delete from ContractHeaderSetDef where ContractHeaderSetID={0}\n", _
            iContractHeaderSetID)

        Dim sSql As String = "exec AddContractHeaderSetDef {0},{1}\n"
        Dim iContractHeaderID As Integer
        For Each iContractHeaderID In aContractHeaderID
            sb.AppendFormat(sSql, iContractHeaderSetID, iContractHeaderID)
        Next

        SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

    End Sub
End Class
