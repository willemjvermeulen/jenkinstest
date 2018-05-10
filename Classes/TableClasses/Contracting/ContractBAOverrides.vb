Imports System.Text
Imports Intuitive

Public Class ContractBAOverrides
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractBAOverrides"

        With Me.Fields
            .Add(New Field("ContractBAOverridesID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Amount", "Real", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Shared Sub UpdateContract(ByVal iContractID As Integer, ByVal aMinimumDays As ArrayList)

        'delete existing items
        ContractBAOverrides.DeleteContract(iContractID)

        If aMinimumDays.Count > 0 Then

            'set up stringbuilder and default sql
            Dim sb As New StringBuilder

            'set default strings up
            Dim sSqlRow As String = "insert into ContractBAOverrides (" &
                "ContractID, StartDate, EndDate, Amount) values ({0},{1},{2},{3})\n"

            'scan through each row
            Dim oGridRow As Intuitive.WebControls.Grid.GridRow
            For Each oGridRow In aMinimumDays

                'add the date
                sb.AppendFormat(sSqlRow, iContractID, oGridRow("Start Date"),
                    oGridRow("End Date"), oGridRow("Amount"))

            Next

            'execute the query
            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

        End If

    End Sub

    Public Shared Sub DeleteContract(ByVal iContractID As Integer)

        SQL.Execute("delete from ContractBAOverrides where ContractID=" & iContractID)

    End Sub

End Class

