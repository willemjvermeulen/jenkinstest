Imports Intuitive
Imports System.Text

Public Class ContractSpecialOfferValue
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractSpecialOfferValue"

        With Me.Fields
            .Add(New Field("ContractSpecialOfferValueID", "Integer"))
            .Add(New Field("ContractSpecialOfferID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Value", "Numeric"))
            .Add(New Field("Adult", "Numeric"))
            .Add(New Field("Child", "Numeric"))
            .Add(New Field("Youth", "Numeric"))
        End With

        Me.Clear()
    End Sub

    Public Shared Sub SetValues(ByVal iContractSpecialOfferID As Integer, _
     ByVal sPaxType As String, ByVal aGridData As ArrayList, ByVal sSuffix As String)

        'clear existing records
        SQL.Execute("Delete from ContractSpecialOfferValue where ContractSpecialOfferID=" & _
            iContractSpecialOfferID)

        Dim sb As New StringBuilder
        Dim sSql As String = "exec AddContractSpecialOfferValue {0},{1},{2},{3},{4},{5},{6}\n"

        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData

            Dim oValues() As Object = {0, 0, 0, 0}

            Select Case sPaxType
                Case "All"
                    oValues(0) = oGridRow("Value" & sSuffix)

                Case "Adult Only"
                    oValues(1) = oGridRow("Adult" & sSuffix)

                Case "Adult/Child"
                    oValues(1) = oGridRow("Adult" & sSuffix)
                    oValues(2) = oGridRow("Child" & sSuffix)

                Case "Child Only"
                    oValues(2) = oGridRow("Child" & sSuffix)

                Case "Adult/Child/Youth"
                    oValues(1) = oGridRow("Adult" & sSuffix)
                    oValues(2) = oGridRow("Child" & sSuffix)
                    oValues(3) = oGridRow("Youth" & sSuffix)

                Case "Youth Only"
                    oValues(3) = oGridRow("Youth" & sSuffix)

                Case Else
                    Throw New Exception("Could not find Pax Type " & sPaxType)
            End Select

            sb.AppendFormat(sSql, iContractSpecialOfferID, _
                oGridRow("Start"), oGridRow("End"), oValues(0), oValues(1), oValues(2), oValues(3))

        Next

        'execute
        SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

    End Sub

End Class