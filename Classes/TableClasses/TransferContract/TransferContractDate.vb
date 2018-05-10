Imports Intuitive
Imports Intuitive.DateFunctions
Imports Intuitive.Functions

Public Class TransferContractDate
    Inherits TableBase

    Public Sub New()

        Me.Table = "TransferContractDate"

        With Me.Fields
            .Add(New Field("TransferContractDateID", "Integer"))
            .Add(New Field("TransferContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("StartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("EndDate", "Date", ValidationType.NotEmptyIsDate))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'delete child records from TransferContractDef
            .Add("Delete from TransferContractDef Where TransferContractDateID=" & iTableID.ToString)

            .Execute()

        End With

        TransferContract.SetLastModifiedData(, iTableID)

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Check Update"
    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        Dim dStartDate As Date = SafeDate(Me("StartDate"))
        Dim dEndDate As Date = SafeDate(Me("EndDate"))

        'if there are no warnings then do some more date checking
        If Me.Warnings.Count = 0 AndAlso dEndDate < dStartDate Then
            Me.Warnings.Add("The End Date cannot be before the Start Date")
            Me.Fields("StartDate").IsValid = False
            Me.Fields("EndDate").IsValid = False
        End If

        'if we still have no warnings then need to check the new dates don't overlap with any exiting ones
        If Me.Warnings.Count = 0 Then

            'set a flag to determine success or failure and get any existing datebands for this contract
            'also declare 2 more date variables to hold each of the date values we're checking against
            Dim bOK As Boolean = True
            Dim dtExistingDates As DataTable = SQL.GetDatatable("Select StartDate, EndDate from " & _
                "TransferContractDate Where TransferContractID={0} and TransferContractDateID!={1}", _
                SafeInt(Me("TransferContractID")), Me.TableID)
            Dim dCheckStartDate As Date
            Dim dCheckEndDate As Date

            For Each drRow As DataRow In dtExistingDates.Rows

                dCheckStartDate = SafeDate(drRow("StartDate"))
                dCheckEndDate = SafeDate(drRow("EndDate"))

                'do 3 checks:
                '1 check Start Date doesn't lie within an existing date range
                '2 check End Date doesn't lie within an existing range
                '3 check start and end date don't encompass an existing range
                If dStartDate >= dCheckStartDate AndAlso dStartDate <= dCheckEndDate Then
                    bOK = False
                ElseIf dEndDate >= dCheckStartDate AndAlso dEndDate <= dCheckEndDate Then
                    bOK = False
                ElseIf dStartDate < dCheckStartDate AndAlso dEndDate > dCheckEndDate Then
                    bOK = False
                End If

                'if it's not ok then add the warning set the fields invalid property to false and exit loop
                If Not bOK Then
                    Me.Warnings.Add("The Dateband entered clashes with an existing dateband")
                    Me.Fields("StartDate").IsValid = False
                    Me.Fields("EndDate").IsValid = False
                    Exit For
                End If
            Next
        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "After Update"
    Private Sub TransferContractDate_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'insert empty rates
        SQL.Execute("exec TransferContractRateInsertDefault 'Dateband',{0}", iTableID)

        TransferContract.SetLastModifiedData(, iTableID)
    End Sub
#End Region

End Class
