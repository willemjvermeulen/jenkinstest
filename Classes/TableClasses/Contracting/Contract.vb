Imports Intuitive
Imports Intuitive.DateFunctions
Imports Intuitive.WebControls
Imports Intuitive.Functions
Imports System.Text

Public Class Contract
    Inherits TableBase

#Region "Properties"

    Private sSystemUser As String
    Private iTradesSelected As Integer = 0

    Public Property SystemUser() As String
        Get
            Return sSystemUser
        End Get
        Set(ByVal Value As String)
            sSystemUser = Value
        End Set
    End Property
#End Region

#Region "New"

    Public Sub New()

        Me.Table = "Contract"

        'Details
        With Me.Fields
            .Add(New Field("ContractID", "Integer"))
            .Add(New Field("ContractParentID", "Integer"))
            .Add(New Field("PropertyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("SystemUserID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Reference", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Category", "String", 15, ValidationType.NotEmpty))
            '.Add(New Field("ContractName", "String", 50))
            .Add(New Field("SpecialOffer", "Boolean"))
            .Add(New Field("SpecialOfferCode", "String", 40))
            .Add(New Field("BookingSourceID", "Integer"))
            '.Add(New Field("BookingSourceMemberSelection", "String", 20))
            .Add(New Field("AppliesTo", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("AppliesToID", "Integer"))
            .Add(New Field("BookingAuthorityID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractIdentifier", "String", 10, ValidationType.NotEmpty))
            .Add(New Field("Status", "String", 20))
            .Add(New Field("ContractDate", "Date", ValidationType.IsDate))
            .Add(New Field("Type", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("BookingStartDate", "DateTime", ValidationType.NotEmptyIsDate))
            .Add(New Field("BookingEndDate", "DateTime", ValidationType.NotEmptyIsDate))
            .Add(New Field("StayStartDate", "DateTime", ValidationType.NotEmptyIsDate))
            .Add(New Field("StayEndDate", "DateTime", ValidationType.NotEmptyIsDate))
            .Add(New Field("CurrencyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("BookingCurrency", "String", ValidationType.NotEmpty))
            .Add(New Field("NetContract", "Boolean"))
            .Add(New Field("CommissionOverride", "Boolean"))
            .Add(New Field("CommissionOverrideValue", "Numeric"))
            .Add(New Field("PriceOnArrival", "Boolean"))
            .Add(New Field("MaximumRooms", "Integer"))
            .Add(New Field("CompleteStayOnly", "Boolean"))
            .Add(New Field("PaymentTermID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("CancellationSystemUserID", "Integer"))
            .Add(New Field("CancellationDate", "Date", ValidationType.IsDate))
            .Add(New Field("CancellationReason", "Text"))
            .Add(New Field("Deposit", "Numeric"))
            .Add(New Field("TermsAndConditionsOnline", "Text"))
            .Add(New Field("TermsAndConditionsCallCentre", "Text"))
            .Add(New Field("ContractSigned", "Boolean"))
            .Add(New Field("Notes", "Text"))
            .Add(New Field("InvoicePeriod", "Integer"))
            .Add(New Field("SignedBy", "Integer"))
            .Add(New Field("Signature", "Boolean"))
            .Add(New Field("XenonCommPercent", "Integer"))

            .Field("Notes").NonBinding = True
            .Field("TermsAndConditionsOnline").NonBinding = True
            .Field("TermsAndConditionsCallCentre").NonBinding = True
            .Field("AppliesTo").DisplayFieldName = "Applies To Type"
            .Field("BookingSourceID").NonBinding = True
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in AuditTrail
        If Not SQL.FKCheck("AuditTrail", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractAllocationRelease
        If Not SQL.FKCheck("ContractAllocationRelease", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractBrand
        If Not SQL.FKCheck("ContractBrand", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractCloseOut
        If Not SQL.FKCheck("ContractCloseOut", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractFacility
        If Not SQL.FKCheck("ContractFacility", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractFreeOffer
        If Not SQL.FKCheck("ContractFreeOffer", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractGroupDiscount
        If Not SQL.FKCheck("ContractGroupDiscount", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractGuarantee
        If Not SQL.FKCheck("ContractGuarantee", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractMinMaxStay
        If Not SQL.FKCheck("ContractMinMaxStay", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractNote
        If Not SQL.FKCheck("ContractNote", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractOther
        If Not SQL.FKCheck("ContractOther", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractPackage
        If Not SQL.FKCheck("ContractPackage", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractPaymentSchedule
        If Not SQL.FKCheck("ContractPaymentSchedule", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractRateDate
        If Not SQL.FKCheck("ContractRateDate", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractRateHeader
        If Not SQL.FKCheck("ContractRateHeader", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractRegionTax
        If Not SQL.FKCheck("ContractRegionTax", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractRoomType
        If Not SQL.FKCheck("ContractRoomType", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractSpecialOffer
        If Not SQL.FKCheck("ContractSpecialOffer", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractSupplement
        If Not SQL.FKCheck("ContractSupplement", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        'check for records in ContractTax
        If Not SQL.FKCheck("ContractTax", "ContractID", iTableID) Then
            Me.Warnings.Add("Contract cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Is Base?"

    Private Function isBase(ByVal iID As Integer) As Boolean
        'Check if the contract is a Base or not
        Return Convert.ToBoolean(SQL.ExecuteSingleValue( _
                String.Format( _
                "Select Case Category " & _
                "When 'Base' Then 1 Else 0 End " & _
                "From contract Where ContractID={0}", iID)))
    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'applies to id
        If SafeString(Me("AppliesTo")) <> "" AndAlso SafeInt(Me("AppliesToID")) = 0 Then
            Me.Warnings.Add("The " & Me("AppliesTo") & " must be specified")
            Me.Fields("AppliesToID").IsValid = False
        End If

        If Me.Warnings.Count = 0 Then

            'Booking and Stay dates
            Dim dBookingStartDate As Date = DisplayDateToDate(Me("BookingStartDate"))
            Dim dBookingEndDate As Date = DisplayDateToDate(Me("BookingEndDate"))
            Dim dStayStartDate As Date = DisplayDateToDate(Me("StayStartDate"))
            Dim dStayEndDate As Date = DisplayDateToDate(Me("StayEndDate"))

            If dBookingStartDate > dBookingEndDate Then
                Me.Warnings.Add("The Booking Start Date must be before the Booking End Date")
            End If
            If dStayStartDate > dStayEndDate Then
                Me.Warnings.Add("The Stay Start Date must be before the Stay End Date")
            End If
            If dBookingStartDate > dStayStartDate Then
                Me.Warnings.Add("The Booking Start Date should be before or equal to the Stay Start Date")
            End If
            If dBookingEndDate > dStayEndDate Then
                Me.Warnings.Add("The Booking End Date can not be after the Stay End Date")
            End If

            'If sub-contract, check that the Contract Name is specified
            'If Me.GetField("Category") = "Sub-Contract" AndAlso Me.GetField("ContractName") = "" Then
            '    Me.Warnings.Add("The Contract Name must be specified")
            '    Fields("ContractName").IsValid = False
            'End If

            'Make sure the commission is cleared if required
            If SafeBoolean(Me("CommissionOverride")) = False Then
                Me.SetField("CommissionOverrideValue", 0)
            End If

            If SafeInt(Me("XenonCommPercent")) < 0 Then
                Me.SetField("XenonCommPercent", 0)
            End If

            If SafeInt(Me("XenonCommPercent")) > 100 Then
                Me.SetField("XenonCommPercent", 100)
            End If

        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Setup New Contract"
    Public Sub SetupNewContract(ByVal iPropertyId As Integer)
        Me.Clear()
        Me.SetField("PropertyID", iPropertyId)
        Me.SetField("Category", "Base")
        Me.SetField("BookingAuthorityID", UserSession.BookingAuthorityID)
        Me.SetField("Status", "In Progress")
        Me.SetField("ContractDate", Date.Now.ToString)
        Me.SetField("Type", "Allotted")
        Me.SetField("NetContract", True)
        Me.SetField("PriceOnArrival", "0")
        Me.SetField("XenonCommPercent", 0)
        Me.SetField("SystemUserID", UserSession.SystemUserID)

        'Get Default Currency
        Dim iCurrencyId As Integer _
            = Intuitive.Functions.SafeInt(SQL.GetValue("GetDefualtCurrencyForPropertyId {0}", iPropertyId))
        Me.SetField("CurrencyId", 1)
        Me.SetField("BookingCurrency", "USD")


        'Get a few bits on info from the property table
        Dim oProperty As New PropertyTable
        oProperty.Go(iPropertyId)
        Me.SetField("MaximumRooms", oProperty("MaximumRooms"))
        'Me.SetField("InvoicePeriod", oProperty("InvoicePeriod"))
        Me.SetField("InvoicePeriod", "30")
    End Sub


#End Region

#Region "before add - set default status"
    Private Sub Contract_BeforeAdd() Handles MyBase.BeforeAdd
        Me.SetField("Status", "In Progress")
        'add 23:59:00 to the last day 
        Dim dBookingEndDate As Date
        If Date.TryParse(Me.GetField("BookingEndDate"), dBookingEndDate) Then
            dBookingEndDate = dBookingEndDate.AddMinutes(59)
            dBookingEndDate = dBookingEndDate.AddHours(23)
            Me.SetField("BookingEndDate", dBookingEndDate)
        End If
    End Sub

#End Region

#Region "after add"

    Private Sub Contract_AfterAdd(ByVal iTableID As Integer) Handles MyBase.AfterAdd

        Dim sStayStartDate As String = SQL.GetSqlValue(Me("StayStartDate"), SQL.SqlValueType.Date)
        Dim sStayEndDate As String = SQL.GetSqlValue(Me("StayEndDate"), SQL.SqlValueType.Date)

        Dim oSQL As New SQLTransaction
        With oSQL

            'set reference
            Dim sNewReference As String = SQL.GetValue("exec GenerateContractReference {0}", iTableID)

            'sync setttings
            Select Case Config.Installation
                Case "Server"
                    oSQL.Add("update Contract set SyncGUID=newid(), SyncStatus='In', SyncSystemUserID=0, " & _
                        "SyncRequired=0, EditStatus='Full' where ContractID={0}", iTableID)

                    'add to audit trail 
                    ContractSyncAuditTrail.AddAuditTrailEntry("Contract", iTableID, UserSession.SystemUserID, _
                        ContractSyncAuditTrail.EventType.AddedOnServer)
                Case "Client"
                    oSQL.Add("update Contract set SyncGUID=newid(), SyncStatus='Out', SyncSystemUserID={1}, " & _
                        "SyncRequired=1, EditStatus='Full' where ContractID={0}", iTableID, UserSession.SystemUserID)
                Case Else
                    Throw New Exception("Unknown Installtion Mode " & Config.Installation)
            End Select

            'Set the booking source
            .Add("exec SetContractBookingSourceID {0}", iTableID)

            'set default terms and conditions
            .Add("exec SetContractDefaultTerms {0}", iTableID)

            'set default cancellation
            .Add("exec SetContractDefaultCancellation {0},{1},{2}", _
                 iTableID, sStayStartDate, sStayEndDate)

            'set default minimum days
            .Add("exec SetContractDefaultMinimumDays {0},{1},{2},{3}", _
                iTableID, sStayStartDate, sStayEndDate, 2)

            'set default facilities
            .Add("exec SetDefaultContractFacilties {0}", iTableID)

            .Execute()
        End With

        Me.Refresh()

    End Sub

#End Region

#Region "after go - get the username"

    Private Sub Contract_AfterGo(ByVal iTableID As Integer) Handles MyBase.AfterGo

        Dim dr As DataRow = SQL.GetDataRow("exec GetContractSystemUser {0}", iTableID)
        If Not dr Is Nothing Then
            Me.SystemUser = dr(0).ToString
        Else
            Me.SystemUser = "Unknown"
        End If
    End Sub

#End Region

#Region "after update"

    Private Sub Contract_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate

        'Set the booking source
        SQL.Execute("exec SetContractBookingSourceID {0}", iTableID)

        SQL.Execute("UPDATE Contract SET ContractCategoryID=(SELECT ContractCategoryID FROM ContractCategory WHERE ContractCategory=Contract.Category) WHERE ContractID = {0}", iTableID)

        'delete bk source members if selection=all
        If Me("BookingSourceMemberSelection") = "All" Then
            ContractTrade.DeleteContractTrade(iTableID)
        End If

        'clear the special offer code if not required
        If Not SafeBoolean(Me("SpecialOffer")) Then
            SQL.Execute("update Contract set SpecialOfferCode='' where ContractID={0}", iTableID)
        End If

        'sync settings
        Contract.SetSyncRequired(iTableID)

    End Sub

#End Region

#Region "Set Sync Required"

    Public Shared Sub SetSyncRequired(ByVal iContractID As Integer)

        'if in client mode, set the sync required flag
        If Config.Installation = "Client" Then
            SQL.Execute("update Contract set SyncRequired=1 where ContractID={0}", iContractID)
        ElseIf Config.Installation = "Server" Then
            SQL.Execute("update Contract set SyncRequired=1 where ContractID={0} and SyncStatus='In' " & _
                "and SyncSystemUserID>0", iContractID)
        Else
            Throw New Exception("Unknown Installation Mode " & Config.Installation)
        End If

    End Sub

#End Region

#Region "Contract Reference - Build"

    Public Function BuildContractReference() As String

        Return SQL.GetValue("ContractReferenceInfo {0},{1},{2},{3},{4}", _
            Me("PropertyID"), _
            SQL.GetSqlValue(Me("AppliesTo"), SQL.SqlValueType.String), _
            SafeInt(Me("AppliesToID")), _
            SQL.GetSqlValue(Me("ContractIdentifier"), SQL.SqlValueType.String), _
            SQL.GetSqlValue(Me("StayStartDate"), SQL.SqlValueType.Date))

    End Function

    Public Shared Function CheckContractReferenceNotDupe(ByVal iPropertyID As Integer, _
        ByVal sAppliesTo As String, ByVal iAppliesToID As Integer, _
        ByVal sContractIdentifier As String, ByVal dStayStartDate As Date) As Boolean

        Dim sReference As String = SQL.GetValue("ContractReferenceInfo {0},{1},{2},{3},{4}", _
            iPropertyID, _
            SQL.GetSqlValue(sAppliesTo), _
            iAppliesToID, _
            SQL.GetSqlValue(sContractIdentifier), _
            SQL.GetSqlValue(dStayStartDate, SQL.SqlValueType.Date))

        Return SQL.ExecuteSingleValue("select count(*) from Contract where Reference={0}", _
            SQL.GetSqlValue(sReference)) = 0

    End Function

#End Region

#Region "GetChangeStatuses"

    Public Shared Function GetChangeStatuses(ByVal sStatus As String, _
        ByVal bCanSetToLive As Boolean, ByVal bCanCancel As Boolean) As String


        Select Case sStatus
            Case "In Progress", "Estimate", "Holding", "Closed"

                If Not (bCanSetToLive Or bCanCancel) Then
                    Return ""
                ElseIf bCanSetToLive And Not bCanCancel Then
                    Return "Live"
                ElseIf Not bCanSetToLive And bCanCancel Then
                    Return "Cancelled"
                Else
                    Return "Live#Cancelled"
                End If
                Return "Live#Cancelled"
            Case "Live"
                If bCanCancel Then
                    Return "In Progress#Cancelled"
                Else
                    Return "In Progress"
                End If
            Case "Closed"
                If Not bCanSetToLive Then
                    Return "In Progress"
                Else
                    Return "In Progress#Live"
                End If
             Case "Cancelled"
                Return ""
            Case Else
                Throw New Exception("Unexpected status " & sStatus)
        End Select

    End Function
#End Region

#Region "SetContractAllocation"

    'set 
    Public Shared Sub SetContractAllocation(ByVal iContractID As Integer)

        SQL.Execute("exec CalcSetContractAllocation {0}", iContractID)

    End Sub

    Public Shared Sub ModPDF(ByVal iContractID As Integer)

        Dim sDocumentName As String = String.Format("Contract_{0}_{1}.pdf",
                    SQL.GetValue("Select replace(Reference,'/','-') from Contract Where ContractID={0}", iContractID),
                    Now.ToString("yyyyMMddHHmmss")).Replace("*", "_")
        'Contract.ViewContract(CType(Me.Page, PageBase), Me.nav.ContractID, Nothing, sDocumentName, True, New IntuitivePDF.PDFGenerator.XSLTParams())
        PDFGenerator.QuickPDFGenerator("XMLGetContracts", iContractID.ToString, "Contract.xsl", sDocumentName, False)
        SQL.Execute("Update Contract set LastPDFMod = {0} where contractid = {1}", SQL.GetSqlValue(sDocumentName), SQL.GetSqlValue(iContractID, SQL.SqlValueType.Integer))

    End Sub


#End Region

#Region "CancelContractAllocation"

    Public Shared Sub CancelContractAllocation(ByVal iContractID As Integer)

        SQL.Execute("exec CalcCancelContractAllocation {0}", iContractID)

    End Sub
#End Region

#Region "status change procedures - live and cancelled"

    'live
    Public Shared Function SetContractLive(ByVal iContractID As Integer, ByVal sSignedOff As Integer) As ArrayList

        Dim aWarnings As New ArrayList

        'make sure we have at least one roomtype
        Dim iRoomTypeCount As Integer = _
            SQL.ExecuteSingleValue("Select count(*) from ContractRoomType where ContractID=" & iContractID)
        If iRoomTypeCount < 1 Then
            aWarnings.Add("At least one Room Type must be set up on the Contract")
        End If

        'make sure the mandatory other information sections have been completed
        Dim iMissingOtherInformationTypes As Integer = _
            SQL.ExecuteSingleValue("exec GetIncompleteMandatoryInformation " & iContractID)
        If iMissingOtherInformationTypes > 0 Then
            aWarnings.Add("All of the Mandatory Other Information types must be completed on the Contract")
        End If

        If sSignedOff <= 0 Then
            aWarnings.Add("You should provide the 'Signed off By' information and then check the 'Signature' checkbox.")
        End If
        

        If aWarnings.Count = 0 Then

            SQL.Execute("Update Contract Set Status='Live', SignedBy = {0}, Signature = 1 where ContractID={1}", sSignedOff, iContractID)
            Contract.SetContractAllocation(iContractID)
            AuditTrail.StatusChange(iContractID, UserSession.SystemUserID, "In Progress", "Live")
        End If

        Return aWarnings

    End Function

    'cancelled
    Public Shared Function SetContractCancelled(ByVal iContractID As Integer, _
        ByVal iSystemUserID As Integer, ByVal sCancellationReason As String) As ArrayList

        Dim aWarnings As New ArrayList

        'make sure we have a cancellation reason
        If sCancellationReason = "" Then
            aWarnings.Add("The Cancel Reason must be input before the State can be changed")
        End If


        'do the update
        If aWarnings.Count = 0 Then
            Dim sOldStatus As String = SQL.GetValue("Select Status from Contract Where ContractID={0}", _
                iContractID)
            SQL.Execute("Update Contract Set Status='Cancelled', " & _
                "CancellationSystemUserID={0}, CancellationDate=getdate(), CancellationReason={1} " & _
                "where ContractID={2}", _
                    iSystemUserID, SQL.GetSqlValue(sCancellationReason, SQL.SqlValueType.String), iContractID)
            Contract.CancelContractAllocation(iContractID)
            AuditTrail.StatusChange(iContractID, UserSession.SystemUserID, sOldStatus, "Cancelled")
        End If

        Return aWarnings


    End Function
#End Region

#Region "Get CurrencyID"
    Public Shared Function GetCurrencyIDFromContractID(ByVal iContractID As Integer) As Integer
        Return SafeInt(SQL.GetValue("Select CurrencyID From Contract Where COntractID = " & iContractID))
    End Function
#End Region

#Region "View Contract"

    Public Shared Sub ViewContract(ByVal oPage As Serenity.PageBase, ByVal iContractID As Integer, _
        Optional ByVal aTradeIDs As ArrayList = Nothing, Optional ByVal sPDF As String = "", Optional ByVal bPreview As Boolean = True, _
         Optional ByVal oXSLTParameters As IntuitivePDF.PDFGenerator.XSLTParams = Nothing)

        'work out the PDF name first if we don't already have one
        If sPDF = "" Then
            sPDF = String.Format("Contract_{0}_{1}.pdf", _
                SQL.GetValue("Select replace(Reference,'/','-') from Contract Where ContractID={0}", iContractID), _
                Now.ToString("HHmmss"))
        End If

        'make sure there are no asterisks
        sPDF = sPDF.Replace("*", "_")

        If Not aTradeIDs Is Nothing Then

            'print contract for selected trade members
            PDFGenerator.QuickPDFGenerator("XMLGetContracts",
                iContractID & ", " & SQL.GetSqlValue(ArrayListToDelimitedString(aTradeIDs), SQL.SqlValueType.String),
                "Contract.xsl", sPDF, bPreview, oPage, oXSLTParameters:=oXSLTParameters)

        Else

            'work out if it's a public contract
            Dim sOperateAs As String = SQL.GetValue("select OperateAs from Contract inner join BookingSource " & _
                "on Contract.BookingSourceID=BookingSource.BookingSourceID where ContractID={0}", _
                iContractID)

            If sOperateAs = "Public" OrElse bPreview Then

                'print contract
                PDFGenerator.QuickPDFGenerator("XMLGetContracts", iContractID.ToString, _
                    "Contract.xsl", sPDF, bPreview, oPage, oXSLTParameters:=oXSLTParameters)

            Else

                'count the number of trade members
                Dim iTradeCount As Integer
                iTradeCount = SafeInt(SQL.ExecuteSingleValue("exec CountTradeMembersForContract {0}", _
                    iContractID))

                If iTradeCount = 1 Then

                    'print contract
                    Dim iTradeID As Integer = SQL.ExecuteSingleValue("exec GetMaxTradeMemberForContract {0}", _
                      iContractID)

                    'auto view
                    Dim bAutoView As Boolean = bPreview
                    If oPage.ToString = "ASP.Viewer_aspx" Then bAutoView = False

                    PDFGenerator.QuickPDFGenerator("XMLGetContracts", SQL.GetSqlValue(iContractID, "string") & _
                        ", " & SQL.GetSqlValue(iTradeID, SQL.SqlValueType.String), _
                        "Contract.xsl", sPDF, bAutoView, oPage, oXSLTParameters:=oXSLTParameters)

                ElseIf iTradeCount > 1 Then

                    'redirect to trade selection page
                    HttpContext.Current.Response.Redirect( _
                        oPage.ResolveURL(String.Format("~/Secure/ContractViewer/TradeSelection.aspx?" & _
                            "ContractID={0}&ReferringPage={1}", iContractID, _
                            HttpContext.Current.Server.UrlEncode(oPage.Request.RawUrl.Replace("/Serenity", "")))))

                Else
                    'Throw New Exception("Cound not establish any Trade Members for Contract " & iContractID)
                    oPage.Warnings.Add("No Trade Members could be established for the Contract")
                End If

            End If

        End If

    End Sub

#End Region

#Region "Get T&C"

    Public Function GetTermsAndConditions(ByVal Type As String) As String

        Return SQL.GetValue("select TermsAndConditions{0} from Contract where ContractID={1}", _
            Type.Replace(" ", ""), Me.TableID)
    End Function
#End Region

#Region "Is Public Contract"

    Public ReadOnly Property IsPublicContract() As Boolean
        Get

            Return Me("AppliesTo") = "Booking Source" AndAlso BookingSource.IsPublic(SafeInt(Me("AppliesToID")))
        End Get
    End Property

#End Region


#Region "delete Payment Term Ovveride"

    Public Shared Sub DeletePaymentTermOverride(ByVal ContractTradePaymentTermID As Integer)

        SQL.Execute("delete from ContractTradePaymentTerm where ContractTradePaymentTermID={0}", ContractTradePaymentTermID)
    End Sub

#End Region

#Region "Clear All Payment Terms"

    Public Shared Sub ClearAllPaymentTerms(ByVal iContractID As Integer)

        SQL.Execute("delete from ContractTradePaymentTerm where ContractID={0}", iContractID)
    End Sub

#End Region

#Region "Save Payment Term Overrides"

    Public Shared Sub SavePaymentTermOverrides(ByVal ContractID As Integer, ByVal PaymentTermID As Integer, ByVal aTradeIDs As ArrayList)

        SQL.Execute("exec Contract_SavePaymentTermOverride {0},{1},{2}", ContractID, PaymentTermID, SQL.GetSqlValue(ArrayListToDelimitedString(aTradeIDs)))
    End Sub

#End Region

End Class