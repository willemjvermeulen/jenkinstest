Imports Intuitive
Imports System.Text
Public Class ContractGroupDiscount
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractGroupDiscount"

        With Me.Fields
            .Add(New Field("ContractGroupDiscountID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TravelStartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("TravelEndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("TotalPax", "Integer", ValidationType.NotEmpty))
            .Add(New Field("FreePax", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call the default CheckUpdate
        MyBase.CheckUpdate()

        'Date Checks
        If Me.Warnings.Count < 1 Then

            'Make sure that the Free Pax is not greater than Total Pax

            'Make sure that Free Pax is greater tha zero

        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "Grid Save"

    Public Shared Sub SetDiscounts(ByVal iContractId As Integer, ByVal iContractRoomTypeId As Integer, ByVal aGridValues As ArrayList)

        'clear existing records
        SQL.Execute("Delete from ContractGroupDiscount where ContractId={0} and ContractRoomTypeId={1}", _
            iContractId, iContractRoomTypeId)

        'set up stored procedure declare
        Dim sb As New StringBuilder
        Dim sSql As String = "exec AddContractGroupDiscount {0},{1},{2},{3},{4},{5} \n"

        'build up the sp calls
        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridValues
            sb.AppendFormat(sSql, iContractId, iContractRoomTypeId, _
                oGridRow("Start Date"), oGridRow("End Date"), oGridRow("Total Pax"), oGridRow("Free Pax"))
        Next

        'execute
        If sb.ToString <> "" Then
            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))
        End If

    End Sub

#End Region

End Class
