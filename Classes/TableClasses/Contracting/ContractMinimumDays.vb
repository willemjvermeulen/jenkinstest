Imports Intuitive
Imports System.Text

Public Class ContractMinimumDays
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractMinimumDays"

        With Me.Fields
            .Add(New Field("ContractMinimumDaysID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("MinimumDays", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Shared Sub UpdateContractMinimumDays(ByVal iContractID As Integer, ByVal aMinimumDays As ArrayList)

        'delete existing items
        ContractMinimumDays.DeleteContractMinimumDays(iContractID)

        If aMinimumDays.Count > 0 Then

            'set up stringbuilder and default sql
            Dim sb As New StringBuilder

            'set default strings up
            Dim sSqlRow As String = "insert into ContractMinimumDays (" & _
                "ContractID, StartDate, EndDate, MinimumDays) values ({0},{1},{2},{3})\n"

            'scan through each row
            Dim oGridRow As Intuitive.WebControls.Grid.GridRow
            For Each oGridRow In aMinimumDays

                'add the date
                sb.AppendFormat(sSqlRow, iContractID, oGridRow("Start Date"), _
                    oGridRow("End Date"), oGridRow("Minimum Days"))

            Next

            'execute the query
            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

        End If

    End Sub

    Public Shared Sub DeleteContractMinimumDays(ByVal iContractID As Integer)

        SQL.Execute("delete from ContractMinimumDays where ContractID=" & iContractID)

    End Sub

End Class
