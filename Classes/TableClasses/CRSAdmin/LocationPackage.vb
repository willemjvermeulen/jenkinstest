Imports Intuitive
Imports Intuitive.Functions

Public Class LocationPackage
    Inherits TableBase

#Region "Fields"
    Private oLocationBridgeEntries As New ArrayList
    Private oLocationFeatrueCosts As New ArrayList
#End Region

#Region "Properties"
    Public Property LocationFeatrueCosts() As ArrayList
        Get
            Return oLocationFeatrueCosts
        End Get
        Set(ByVal Value As ArrayList)
            oLocationFeatrueCosts = Value
        End Set
    End Property

    Public Property LocationBridgeEntries() As ArrayList
        Get
            Return oLocationBridgeEntries
        End Get
        Set(ByVal Value As ArrayList)
            oLocationBridgeEntries = Value
        End Set
    End Property
#End Region

    Public Sub New()

        Me.Table = "LocationPackage"

        Me.ClientValidation = True

        With Me.Fields
            .Add(New Field("LocationPackageID", "Integer"))
            .Add(New Field("PackageName", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("PayeeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("BookingStartDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("BookingEndDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("PassengerType", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("InvoicePeriod", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Description", "Text"))
            .Add(New Field("TermsAndConditionsOnline", "Text"))
            .Add(New Field("TermsAndConditionsCallCentre", "Text"))
            .Add(New Field("NetPackage", "Boolean"))
            .Add(New Field("CommissionOverride", "Boolean"))
            .Add(New Field("CommissionOverrideValue", "Double"))
            .Add(New Field("CurrentPackage", "Boolean"))
            .Add(New Field("Dated", "Boolean"))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from LocationPackageBridge
        SQL.Execute("Delete from LocationPackageBridge Where LocationPackageID=" & iTableID.ToString)

        'delete child records from LocationPackageCost
        SQL.Execute("Delete from LocationPackageCost Where LocationPackageID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Check update"
    Public Overrides Function CheckUpdate() As Boolean
        MyBase.CheckUpdate()


        Dim oFunctionReturn As FunctionReturn = DateFunctions.ValidateDates(DateFunctions.SafeDate(Me("BookingStartDate")), _
             DateFunctions.SafeDate(Me("BookingEndDate")))
        If Not oFunctionReturn.Success Then
            Me.Warnings.AddRange(oFunctionReturn.Warnings)
            Me.Fields("BookingStartDate").IsValid = False
            Me.Fields("BookingEndDate").IsValid = False
        End If

        'Location Package Bridge
        If Me.LocationBridgeEntries Is Nothing _
            OrElse Me.LocationBridgeEntries.Count <= 0 Then
            Me.Warnings.Add("You must select at least one Location for this Package")
        End If

        'check date validation
        Dim aDateWarnings As New ArrayList
        aDateWarnings = DateFunctions.ValidateDatebands(Me.LocationFeatrueCosts, _
            , , , , True)

        Me.Warnings.AddRange(aDateWarnings)

        If Not Me.LocationFeatrueCosts.Count = 0 Then
            For Each oGridRow As WebControls.Grid.GridRow In Me.LocationFeatrueCosts
                Select Case Me("PassengerType")
                    Case "All"
                        If oGridRow("Value") = "0" Then
                            Me.Warnings.Add("You must specify a Value Cost for each date range")
                        End If
                    Case "Adult Only"
                        If oGridRow("Adult") = "0" Then
                            Me.Warnings.Add("You must specify an Adult Cost for each date range")
                        End If
                    Case "Adult/Child"
                        If oGridRow("Adult") = "0" OrElse oGridRow("Child") = "0" Then
                            Me.Warnings.Add("You must specify an Adult and Child Cost for each date range")
                        End If
                    Case "Adult/Child/Youth"
                        If oGridRow("Adult") = "0" OrElse oGridRow("Child") = "" OrElse oGridRow("Youth") = "0" Then
                            Me.Warnings.Add("You must specify an Adult, Child and Youth Cost for each date range")
                        End If
                End Select
            Next
        Else
            Me.Warnings.Add("You must specify at least one cost for this package")
        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "After Update"
    Private Sub LocationPackage_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'Remove all LocationPackageBridge entries
        SQL.Execute("Delete From LocationPackageBridge Where LocationPackageID = " & iTableID)

        'Remove all LocationPackageCosts
        SQL.Execute("Delete From LocationPackageCost Where LocationPackageID = " & iTableID)

        'Add to LocationPackageBridge
        Dim sSql As String = _
            "Insert Into LocationPackageBridge (LocationpackageID, ParentID, ParentType) Values ({0}, {1}, {2})"
        Dim oTrans As New SQLTransaction
        For Each oLocationPackageBridge As LocationPackageBridge In oLocationBridgeEntries
            oTrans.Add(sSql, iTableID, _
                oLocationPackageBridge.LocationPackageBridgeParentID, _
                SQL.GetSqlValue(oLocationPackageBridge.LocationPackageBridgeParentType, SQL.SqlValueType.String) _
            )
        Next

        'Add to the LocationPackageCosts
        Dim nAll As Double = 0
        Dim nAdult As Double = 0
        Dim nChild As Double = 0
        Dim nYouth As Double = 0

        sSql = "Insert Into LocationPackageCost (LocationPackageID, StartDate, EndDate, Value, Adult, Child, Youth) Values ({0},{1},{2},{3},{4},{5},{6})"

        For Each oGridRow As WebControls.Grid.GridRow In Me.LocationFeatrueCosts
            Select Case Me("PassengerType").ToString
                Case "All"
                    nAll = SafeNumeric(oGridRow("Value"))
                Case "Adult Only"
                    nAdult = SafeNumeric(oGridRow("Adult"))
                Case "Adult/Child"
                    nAdult = SafeNumeric(oGridRow("Adult"))
                    nChild = SafeNumeric(oGridRow("Child"))
                Case "Adult/Child/Youth"
                    nAdult = SafeNumeric(oGridRow("Adult"))
                    nChild = SafeNumeric(oGridRow("Child"))
                    nYouth = SafeNumeric(oGridRow("Youth"))
            End Select

            oTrans.Add(sSql, iTableID, _
                SQL.GetSqlValue(oGridRow("Start"), SQL.SqlValueType.Date), _
                SQL.GetSqlValue(oGridRow("End"), SQL.SqlValueType.Date), _
                nAll, _
                nAdult, _
                nChild, _
                nYouth _
            )
        Next

        'Run it
        oTrans.Execute()


    End Sub
#End Region

#Region "Helper Structure"
    Public Structure LocationPackageBridge
        Public LocationPackageBridgeParentID As Integer
        Public LocationPackageBridgeParentType As String
    End Structure
#End Region

End Class
