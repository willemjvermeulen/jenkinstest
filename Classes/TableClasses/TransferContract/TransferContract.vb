Imports Intuitive
Imports Intuitive.Functions

Public Class TransferContract
    Inherits TableBase
    Public LastPaxBasis As String = "CockPissPartridge"

#Region "Pseudo Fields"
    Public SetupComplete As Boolean = False
    Public ChangeStatusOK As Boolean = True
    Public CancellationReasonOK As Boolean = True
#End Region

    Public Sub New(Optional ByVal bTermsAndConditionsOnly As Boolean = False)

        Me.Table = "TransferContract"

        If Not bTermsAndConditionsOnly Then

            With Me.Fields
                .Add(New Field("TransferContractID", "Integer"))
                .Add(New Field("PayeeID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("Reference", "String", 30, ValidationType.NotEmptyNotDupe))
                .Add(New Field("CurrencyID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("Status", "String", 20))
                .Add(New Field("CalculationBasis", "String", 20, ValidationType.NotEmpty))
                .Add(New Field("RateBasis", "String", 20, ValidationType.NotEmpty))
                .Add(New Field("AllowOneWay", "Boolean"))
                .Add(New Field("OneWayPercentage", "Numeric"))
                .Add(New Field("PaxBasis", "String", 20))
                .Add(New Field("ChildValueType", "String", 20))
                .Add(New Field("MinChildAge", "Integer"))
                .Add(New Field("MaxChildAge", "Integer"))
                .Add(New Field("DepartureCountryID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("ArrivalCountryID", "Integer", ValidationType.NotEmpty))
                .Add(New Field("TaxPercentage", "Numeric"))
                .Add(New Field("NetContract", "Boolean"))
                .Add(New Field("CommissionOverride", "Boolean"))
                .Add(New Field("CommissionOverrideValue", "Numeric"))
                .Add(New Field("SystemUserID", "Integer"))
                .Add(New Field("DateAdded", "Date", ValidationType.IsDate))
                .Add(New Field("SignedSystemUserID", "Integer"))
                .Add(New Field("DateSigned", "Date", ValidationType.IsDate))
                .Add(New Field("CancellationSystemUserID", "Integer"))
                .Add(New Field("CancellationDate", "Date", ValidationType.IsDate))
                .Add(New Field("CancellationReason", "Text"))
                .Add(New Field("LastModifiedDate", "DateTime"))
                .Add(New Field("LastModifiedUser", "Integer"))
            End With

        End If

        If bTermsAndConditionsOnly Then
            Me.Fields.Add(New Field("TransferContractID", "Integer"))
            Me.Fields.Add(New Field("TermsAndConditions", "Text"))
        End If

        Me.Clear()
    End Sub

#Region "Check Update"
    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'if Calculation Basis is set to per person then Pax Basis is mandatory, if it aint
        'then clear the field
        If Me("CalculationBasis") = "Per Person" Then
            Me.AddFieldWarning("PaxBasis", "", "The Pax Basis must be specified")
        Else
            Me.SetField("PaxBasis", "")
        End If

        'if pax basis is set to Adult/Child then child value type is mandatory
        If Me("PaxBasis") = "Adult/Child" Then
            Me.AddFieldWarning("ChildValueType", "", "The Child Value Type must be specified")
        Else
            Me.SetField("ChildValueType", "")
        End If

        'Wipe out oneway the oneway option isnt checked
        If SafeBoolean(Me("AllowOneWay")) = False Then
            Me.SetField("OneWayPercentage", 0)
        End If

        'if pax basis is set to Adult/Child then a couple of checks to
        'do on the min max ages
        If Me("PaxBasis") = "Adult/Child" Then

            'first check max >= min
            If SafeInt(Me("MinChildAge")) > SafeInt(Me("MaxChildAge")) Then

                Me.Warnings.Add("The Min Child Age cannot be greater than the Max Child Age")
                Me.Fields("MinChildAge").IsValid = False
                Me.Fields("MaxChildAge").IsValid = False
            End If

            'check max isn't=0
            Me.AddFieldWarning("MaxChildAge", 0, "The Max Child Age cannot be 0")
        Else

            Me.SetField("MinChildAge", "")
            Me.SetField("MaxChildAge", "")
        End If

        'if we're adding then set the user, status and date added fields
        If Me.TableID = 0 Then

            Me.SetField("SystemUserID", UserSession.SystemUserID)
            Me.SetField("DateAdded", Now.Date)
            Me.SetField("Status", "Estimate")
        End If


        'Make sure the commission is cleared if required
        If SafeBoolean(Me("NetContract")) Then
            Me.SetField("CommissionOverrideValue", 0)
        End If

        Return Me.Warnings.Count = 0

    End Function
#End Region

#Region "After Go"
    Private Sub TransferContract_AfterGo(ByVal iTableID As Integer) Handles MyBase.AfterGo

        'store the Pax Basis
        Me.LastPaxBasis = Me("PaxBasis")

        'set the Setup complerte flag
        Me.SetupComplete = SQL.GetValue("exec TransferContractCheckSetupComplete {0}", iTableID) = "Ready"
    End Sub
#End Region

#Region "After Update"
    Private Sub TransferContract_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'if we're adding and the pax basis is set to adult/child, or if we've changed the pax basis to adult/child
        'subsequently then set up default pax basis
        'If Me.LastPaxBasis <> Me("PaxBasis") AndAlso Me("PaxBasis") = "Adult/Child" Then
        '    TransferContractPax.SetDefaultRate(iTableID)
        'End If

        'set the Setup complerte flag
        Me.SetupComplete = SQL.GetValue("exec TransferContractCheckSetupComplete {0}", iTableID) = "Ready"

        TransferContract.SetLastModifiedData(iTableID)

    End Sub
#End Region

#Region "After Edit"
    Private Sub TransferContract_AfterEdit(ByVal iTableID As Integer) Handles MyBase.AfterEdit

        'if we've just changed the pax basis to All then delete existing rates etc.
        If Me.LastPaxBasis <> Me("PaxBasis") AndAlso Me("PaxBasis") = "All" Then

            Dim oSQLTransaction As New SQLTransaction
            oSQLTransaction.Add("exec DeleteTransferContractRateDef {0}", iTableID)
            oSQLTransaction.Add("exec TransferContractRateInsertDefault '',0,{0}", iTableID)
            oSQLTransaction.Execute()
        End If
    End Sub
#End Region

#Region "Change Status"

    Public Function ChangeStatus(ByVal bSetupComplete As Boolean, ByVal sStatus As String, _
                ByVal sCancellationReason As String) As Boolean

        'check we have a status
        If sStatus = "" Then
            Me.Warnings.Add("You must select a Status")
            Me.ChangeStatusOK = False
        End If

        'check the setup is complete if trying to sign
        If sStatus = "Signed" AndAlso Not bSetupComplete Then
            Me.Warnings.Add("The Rates Setup must be completed before the Contract can be Signed")
        End If

        'if the status is cancelled check we have a reason
        If sStatus = "Cancelled" AndAlso sCancellationReason = "" Then
            Me.Warnings.Add("The Cancellation Reason must be Specified")
            Me.CancellationReasonOK = False
        End If

        'if it's all ok then do the update
        If Me.Warnings.Count = 0 Then
            Me.SetField("Status", sStatus)

            Select Case sStatus
                Case "Signed"
                    Me.SetField("SignedSystemUserID", UserSession.SystemUserID)
                    Me.SetField("DateSigned", Now.Date)
                Case "Cancelled"
                    Me.SetField("CancellationSystemUserID", UserSession.SystemUserID)
                    Me.SetField("CancellationDate", Now.Date)
                    Me.SetField("CancellationReason", sCancellationReason)
            End Select

            'do the update
            Me.Update()

            TransferContract.SetLastModifiedData(Me.TableID)

        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Set Last Modified Data"
    Public Shared Sub SetLastModifiedData(Optional ByVal iTransferContractID As Integer = 0, _
            Optional ByVal iTransferContractDateID As Integer = 0, _
            Optional ByVal iTransferContractPaxID As Integer = 0)

        SQL.Execute("exec UpdateTransferContractLastModifiedDate {0},{1},{2},{3}", _
            UserSession.SystemUserID, iTransferContractID, iTransferContractDateID, iTransferContractPaxID)
    End Sub

    Public Sub SetLastModifiedFields()

        Dim drRow As DataRow
        drRow = SQL.GetDataRow("Select LastModifiedDate, LastModifiedUser from TransferContract Where " & _
            "TransferContractID={0}", Me.TableID)

        Me.SetField("LastModifiedDate", drRow("LastModifiedDate"))
        Me.SetField("LastModifiedUser", drRow("LastModifiedUser"))
    End Sub
#End Region

#Region "IsPerVehicle"

    Public Shared Function IsPerVehicle(ByVal iTransferContractID As Integer) As Boolean

        Dim oTransferContract As New TransferContract
        oTransferContract.Go(iTransferContractID)

        Return oTransferContract("CalculationBasis") = "Per Vehicle"
    End Function

#End Region

End Class