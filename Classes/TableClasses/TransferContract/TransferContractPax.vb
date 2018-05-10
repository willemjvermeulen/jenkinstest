Imports Intuitive
Imports Intuitive.Functions

Public Class TransferContractPax
    Inherits TableBase

    Public Sub New()

        Me.Table = "TransferContractPax"

        With Me.Fields
            .Add(New Field("TransferContractPaxID", "Integer"))
            .Add(New Field("TransferContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Vehicle", "String", ValidationType.NotEmpty))
            .Add(New Field("MinPax", "Integer"))
            .Add(New Field("MaxPax", "Integer"))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"
    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'if all flag is false then do a quick bit of validation on the min and max

        If SafeInt(Me("MaxPax")) < SafeInt(Me("MinPax")) Then
            Me.Warnings.Add("The Max Pax must be greater then the Min Pax")
            Me.Fields("MinPax").IsValid = False
            Me.Fields("MaxPax").IsValid = False
        End If

        'max pax can't be 0
        Me.AddFieldWarning("MaxPax", 0, "The Max Pax cannot be 0")

        'if everything's ok so far then check min/max don't overlap with existing min/max entries
        If Me.Warnings.Count = 0 Then

            Dim iMin As Integer = SafeInt(Me("MinPax"))
            Dim iMax As Integer = SafeInt(Me("MaxPax"))

            'set a flag to determine success or failure and get any existing datebands for this contract
            'also declare 2 more int variables to hold each of the int values we're checking against
            Dim bOK As Boolean = True
            Dim dtExistingEntries As DataTable = SQL.GetDatatable("Select MinPax, " & _
                "isnull(MaxPax,0) MaxPax from TransferContractPax Where TransferContractID={0} " & _
                "and TransferContractPaxID!={1}", SafeInt(Me("TransferContractID")), _
                Me.TableID)
            Dim iCheckMin As Integer
            Dim iCheckMax As Integer

            For Each drRow As DataRow In dtExistingEntries.Rows

                iCheckMin = SafeInt(drRow("MinPax"))
                iCheckMax = SafeInt(drRow("MaxPax"))

                'do 5 checks:
                '1 check min doesn't equal the existing min
                '2 check min doesn't lie within an existing duration range
                '3 check max doesn't equal the existing min
                '4 check max doesn't lie within an existing durationrange
                '5 check min and max date don't encompass an existing range
                If iMin = iCheckMin Then
                    bOK = False
                ElseIf iMin > iCheckMin AndAlso iMin <= iCheckMax Then
                    bOK = False
                ElseIf iMax > 0 AndAlso iMax = iCheckMin Then
                    bOK = False
                ElseIf iMax > 0 AndAlso iMax > iCheckMin AndAlso iMax <= iCheckMax Then
                    bOK = False
                ElseIf iMax > 0 AndAlso iMin < iCheckMin AndAlso iMax > iCheckMin Then
                    bOK = False
                End If

                'if it's not ok then add the warning set the fields invalid property to false and exit loop
                If Not bOK Then
                    Me.Warnings.Add("The Min/Max Pax entered overlaps with an existing Pax Type")
                    Me.Fields("MinPax").IsValid = False
                    Me.Fields("MaxPax").IsValid = False
                    Exit For
                End If
            Next
        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Check Delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'delete child records from TransferContractDef
            .Add("Delete from TransferContractDef Where TransferContractPaxID=" & iTableID.ToString)

            .Execute()

        End With

        TransferContract.SetLastModifiedData(, , iTableID)

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Set Default Rate"
    Public Shared Sub SetDefaultRate(ByVal iTransferContractID As Integer)

        Dim oSQLTRansaction As New SQLTransaction
        With oSQLTRansaction
            .Add("exec DeleteTransferContractRateDef {0}", iTransferContractID)
            .Add("exec SetDefaultTransferPaxBasis {0}", iTransferContractID)
            .Add("exec TransferContractRateInsertDefault '',0,{0}", iTransferContractID)
            .Execute()
        End With

    End Sub
#End Region

#Region "After Update"
    Private Sub TransferContractDate_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'insert empty rates
        SQL.Execute("exec TransferContractRateInsertDefault 'PaxBasis',{0}", iTableID)

        TransferContract.SetLastModifiedData(, , iTableID)
    End Sub
#End Region

End Class

