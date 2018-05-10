Imports Intuitive
Imports System.Text
Imports Intuitive.DateFunctions
Imports Intuitive.Functions

Public Class ContractNavBar
    Inherits ControlBase

    Private dtMenu As DataTable

#Region "ContractInformation Session Object"
    Private Property ContractInformation() As ContractInformationType
        Get
            If Not context.Session("ContractInformation") Is Nothing Then
                Return CType(context.Session("ContractInformation"), ContractInformationType)
            Else
                Throw New Exception("Could not find ContractInformation in session")
            End If
        End Get
        Set(ByVal Value As ContractInformationType)
            context.Session("ContractInformation") = Value
        End Set
    End Property

    Private Property OtherInformation() As ArrayList
        Get
            If Not context.Session("OtherInformation") Is Nothing Then
                Return CType(context.Session("OtherInformation"), ArrayList)
            Else
                Throw New Exception("Could not find other information in session")
            End If
        End Get
        Set(ByVal Value As ArrayList)
            context.Session("OtherInformation") = Value
        End Set
    End Property

    Public Property FromHomePage() As Boolean
        Get
            If Not context.Session("FromHomePage") Is Nothing Then
                Return CType(context.Session("FromHomePage"), Boolean)
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            context.Session("FromHomePage") = Value
        End Set
    End Property
#End Region

#Region "load - pick up the selected contract room type"
    Protected Overrides Sub oninit(ByVal e As System.EventArgs)
        If Not Page.IsPostBack AndAlso Not Me.Page.Request("ContractRoomTypeID") Is Nothing Then
            Me.ContractRoomTypeID = CType(Me.Page.Request("ContractRoomTypeID"), Integer)
        End If
    End Sub

#End Region

#Region "Properties"

    'propertyid
    Public ReadOnly Property PropertyID() As Integer
        Get
            Return Me.ContractInformation.PropertyID
        End Get
    End Property

    'property name
    Public ReadOnly Property PropertyName() As String
        Get
            Return Me.ContractInformation.PropertyName
        End Get
    End Property

    'property reference
    Public ReadOnly Property PropertyReference() As String
        Get
            Return Me.ContractInformation.PropertyReference
        End Get
    End Property

    'contractid
    Public ReadOnly Property ContractID() As Integer
        Get
            Return Me.ContractInformation.ContractID
        End Get
    End Property

    'contract type
    Public ReadOnly Property ContractType() As String
        Get
            Return Me.ContractInformation.ContractType
        End Get
    End Property

    'selectedroomtypeid
    Public Property ContractRoomTypeID() As Integer
        Get
            Return Me.ContractInformation.ContractRoomTypeID
        End Get
        Set(ByVal value As Integer)
            Me.ContractInformation.ContractRoomTypeID = value
        End Set
    End Property

    'selected room type
    Public ReadOnly Property RoomTypeName() As String
        Get
            Return Me.ContractInformation.GetRoomType(Me.ContractRoomTypeID)
        End Get
    End Property

    'stay start date
    Public ReadOnly Property StayStartDate() As Date
        Get
            Return Me.ContractInformation.StayStartDate
        End Get

    End Property

    'stay end date
    Public ReadOnly Property StayEndDate() As Date
        Get
            Return Me.ContractInformation.StayEndDate
        End Get
    End Property

    'default year
    Public ReadOnly Property DefaultYear() As Integer
        Get
            If IsDate(Me.StayStartDate.ToString) AndAlso IsDate(Me.StayEndDate.ToString) AndAlso _
                    Not IsEmptyDate(Me.StayStartDate) AndAlso Not IsEmptyDate(Me.StayEndDate) Then
                Return Me.StayStartDate.AddDays(DateDiff(DateInterval.Day, _
                    Me.StayStartDate, Me.StayEndDate) / 2).Year
            Else
                Return Now.Year
            End If

        End Get
    End Property

    'booking start Date
    Public ReadOnly Property BookingStartDate() As Date
        Get
            Return Me.ContractInformation.BookingStartDate
        End Get
    End Property

    'booking end date
    Public ReadOnly Property BookingEndDate() As Date
        Get
            Return Me.ContractInformation.BookingEndDate
        End Get
    End Property

    'currency
    Public ReadOnly Property CurrencyID() As Integer
        Get
            Return Me.ContractInformation.CurrencyID
        End Get
    End Property

    'status
    Public ReadOnly Property Status() As String
        Get
            Return Me.ContractInformation.Status
        End Get
    End Property

    'audittrailstatus
    Public ReadOnly Property AuditTrailStatus() As String
        Get
            Return Me.ContractInformation.AuditTrailStatus
        End Get
    End Property

    'current page
    Public ReadOnly Property CurrentPage() As String
        Get
            Dim aPageName() As String = Me.Page.ToString.Replace("ASP.", "").Replace("_aspx", "").Split("_"c)
            Return aPageName(aPageName.Length - 1)
        End Get
    End Property

    'HasRoomTypes
    Public ReadOnly Property HasRoomTypes() As Boolean
        Get
            Return Me.ContractInformation.HasRoomTypes
        End Get
    End Property

    'HasContractSetup
    Public ReadOnly Property HasContractSetup() As Boolean
        Get
            Return Not context.Session("ContractInformation") Is Nothing
        End Get
    End Property

    Public ReadOnly Property RatesSetup(ByVal ContractRoomTypeID As Integer) As Boolean
        Get

            dtMenu = CType(Context.Session("ContractMenuData"), DataTable)
            If dtMenu Is Nothing Then
                dtMenu = SQL.GetDatatable("Exec GetContractMenuData {0}", Me.ContractID.ToString)
                Context.Session.Add("ContractMenuData", dtMenu)
            End If

            Dim bRateSetup As Boolean = False
            For Each dr As DataRow In dtMenu.Rows
                If SafeInt(dr("ContractRoomTypeID")) = ContractRoomTypeID Then
                    bRateSetup = SafeInt(dr("RatesData")) > 0
                    Exit For
                End If
            Next

            Return bRateSetup
        End Get
    End Property

#End Region

#Region "set contract information and room types"

    'set contract information
    Public Sub SetContractInformation(ByVal iContractID As Integer, Optional ByVal sSignedBy As Integer = 0, Optional ByVal bGetRoomTypes As Boolean = True)

        Dim sAuditTrailStatus As String = _
            SQL.GetValue("exec GetAuditTrailStatus {0}", iContractID)

        'contract information
        Dim dr As DataRow = _
            SQL.GetDataRow("exec GetContractInformation {0}", iContractID)

        Dim signedBy As Integer
        Dim signature As Integer

        If dr("SignedBy") Is DBNull.Value Then

            signedBy = CType(dr("SignedBy"), Integer)
            signature = CType(dr("Signature"), Integer)

        End If

        Dim oContractInformation As New ContractInformationType( _
            CType(dr("PropertyID"), Integer), dr("PropertyName").ToString, dr("Reference").ToString, _
            CType(dr("ContractID"), Integer), dr("ContractType").ToString, _
            CType(dr("BookingStartDate"), Date).Date, CType(dr("BookingEndDate"), Date).Date, _
            CType(dr("StayStartDate"), Date).Date, CType(dr("StayEndDate"), Date).Date, _
            CType(dr("CurrencyID"), Integer), dr("Status").ToString, sAuditTrailStatus, _
            dr("ContractReference").ToString, signedBy, signature)

        If sSignedBy > 0 Then
            oContractInformation.SignedBy = sSignedBy
        End If

        Me.ContractInformation = oContractInformation

        'set room types if specified
        If bGetRoomTypes Then Me.SetRoomTypes(iContractID)

        'set the other information
        Me.SetOtherInformation(iContractID)

        'clear out any session stuff
        Me.Clear()

        'Set cookies to remember this contract has been viewed
        UserSession.AddLastViewedContract(iContractID)

    End Sub


    'set room types
    Public Sub SetRoomTypes(ByVal iContractID As Integer)

        Dim dt As DataTable = SQL.GetDatatable("exec GetContractRoomTypesNav {0}", iContractID)

        Me.ContractInformation.ClearRoomTypes()

        If dt.Rows.Count > 0 Then
            With Me.ContractInformation
                .AddRoomType(0, "All Room Types")

                Dim drRoomType As DataRow
                For Each drRoomType In dt.Rows
                    .AddRoomType(CType(drRoomType("ContractRoomTypeID"), Integer), _
                        drRoomType("RoomType").ToString)
                Next
            End With

        End If
    End Sub

    'reset audit trail status
    Public Sub ResetAuditTrailStatus()
        Me.ContractInformation.AuditTrailStatus = _
            SQL.GetValue("exec GetAuditTrailStatus {0}", Me.ContractID)
    End Sub

#End Region

#Region "set other information"

    Public Sub SetOtherInformation(ByVal iContractID As Integer)

        Dim dt As DataTable = SQL.GetDatatable("exec ListContractOther {0}", iContractID)

        'clear and add
        Me.OtherInformation = New ArrayList


        Dim dr As DataRow
        Dim oOtherInformation As OtherInformationType
        For Each dr In dt.Rows

            oOtherInformation = New OtherInformationType
            With oOtherInformation
                .OtherTypeID = Intuitive.Functions.SafeInt(dr(0))
                .OtherType = dr(1).ToString
                .Mandatory = CType(dr(2), Boolean)
                .HasData = CType(dr(3), Boolean)
            End With

            Me.OtherInformation.Add(oOtherInformation)
        Next


    End Sub

#End Region

#Region "clear"
    Public Sub Clear()
        'clear out any cached property values in session or viewstate
        'context.Session.Remove("NavMenuContractID")
        'context.Session.Remove("NavMenuPropertyID")
        'viewstate.Remove("NavMenuSelectedRoomTypeID")
        'context.Session.Remove("NavMenuContractType")
        Context.Session.Remove("ContractMenuData")
    End Sub

#End Region

#Region "SetSectionHasData"

    Public Sub SetSectionHasData(Optional ByVal bHasData As Boolean = True)

        Dim sDataField As String = Me.CurrentPage & "Data"

        'find the row and set the data flag
        Dim dr As DataRow
        Dim dtMenu As DataTable = CType(context.Session("ContractMenuData"), DataTable)
        If Not dtMenu Is Nothing Then
            For Each dr In dtMenu.Rows
                If CType(dr("ContractRoomTypeID"), Integer) = Me.ContractRoomTypeID Then
                    dr(sDataField) = IIf(bHasData, 1, -1)
                End If
            Next
        End If
    End Sub

#End Region

#Region "SetSectionHasNotes"

    Public Sub SetSectionHasNotes(ByVal sSection As String, Optional ByVal bHasNotes As Boolean = True)

        Dim sDataField As String = sSection & "Note"

        'find the row and set the data flag
        Dim dr As DataRow
        Dim dtMenu As DataTable = CType(context.Session("ContractMenuData"), DataTable)
        If Not dtMenu Is Nothing Then
            For Each dr In dtMenu.Rows
                If CType(dr("ContractRoomTypeID"), Integer) = Me.ContractRoomTypeID Then
                    dr(sDataField) = IIf(bHasNotes, 1, -1)
                End If
            Next
        End If

    End Sub

#End Region

#Region "Section Access"
    Private Function GetUserGroupContractSectionAccess(ByVal iUserGroupID As Integer) As ArrayList

        Dim sCacheName As String = "UserGroupContractSection" & iUserGroupID
        Dim oCache As Caching.Cache = HttpContext.Current.Cache

        'try and get from the cache 
        Dim aUserGroupContractSection As ArrayList = CType(oCache(sCacheName), ArrayList)

        If aUserGroupContractSection Is Nothing Then

            aUserGroupContractSection = New ArrayList

            Dim dt As DataTable = SQL.GetDatatable("exec GetUserGroupContractSection {0}", iUserGroupID)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    aUserGroupContractSection.Add(dr(0))
                Next
            End If

            'add to the cache
            AddToCache(sCacheName, aUserGroupContractSection, "UserGroupContractSection.txt")

        End If

        Return aUserGroupContractSection
    End Function
#End Region

#Region "drawmenu"

    Private Function DrawMenu() As String

        Dim sb As New StringBuilder

        Dim aSectionAccess As ArrayList = GetUserGroupContractSectionAccess(UserSession.UserGroupID)

        'set up sections and titles
        Dim sSections As String = "Contract,ContractRoomType,Rates,AllocationRelease,PaymentSchedule," &
            "Other,Margin,Supplement,SpecialOffers,Facility,Packages,FreeOffer,Tax,CloseOut,MinMaxStay,GroupDiscount"
        Dim aSections() As String = sSections.Split(","c)

        Dim sTitles As String = "Contract Details,Room Types,Rates,Allocation & Release,Payment Schedule," &
            "Other,Margins,Supplements,Special Offers,Facilities,Packages,Free Offers,Taxes,Close Outs,Min/Max Stay,Group Discounts"
        Dim aTitles() As String = sTitles.Split(","c)

        'get the list of open sections from the page
        Dim sOpenSections As String = SafeString(Me.Page.Request("OpenSections"))

        'set up the html templates
        Dim sNoteLink As String = "<a href=""Note.aspx?Section={0}"">" & _
            "<img src=""../../images/ContractNavBar/{1}.gif"" id=""imgNav{0}"" class=""notes"" alt=""{2} Notes"" /></a>\n"
        Dim sLink As String = "<a href=""Javascript:NavLink('{0}.aspx');"" class=""{2}"">{3}</a>\n"
        Dim sRoomTypeLink As String = "<a href=""Javascript:NavLink('{0}.aspx?ContractRoomTypeID={1}');"" class=""{2}"">{3}</a>\n"
        Dim sSectionLink As String = "<a id=""aNav{0}"" href=""{1}"" class=""{2}"">{3}</a>\n"
        Dim sTickImage As String = "<img id=""imgTicked{0}"" src=""../../images/ContractNavBar/sectionticked.gif"" style=""display:{1};"" class=""sectionticked""/>\n"
        Dim sSelected As String

        'Nav bar header
        sb.Append("<div id=""divNavHeader"" class=""section"">")
        sb.AppendFormat("<h3>{0}</h3>", Me.PropertyName)
        sb.AppendFormat("<h4>Reference: {0}</h4>", Me.ContractInformation.ContractReference)
        If Me.ContractID > 0 AndAlso UserSession.HasRight(UserSession.AccessRight.ViewPrintContract) Then
            sb.Append("<a id=""aNavBarPdfGray"" href=""javascript:Postback('ViewContractPDF', 'gray')"" title=""View Contract""></a>")
            sb.Append("<a id=""aNavBarPdfBlue"" href=""javascript:Postback('ViewContractPDF', 'blue')"" title=""View Contract""></a>")
            sb.Append("<a id=""aNavBarPdf"" href=""javascript:Postback('ViewContractPDF')"" title=""View Contract""></a>")
        End If
        If Not DateFunctions.IsEmptyDate(Me.StayStartDate) Then
            sb.AppendFormat("<h5>Date: {0}</h5>", DisplayDate(Me.StayStartDate) & " - " & DisplayDate(Me.StayEndDate))
        End If
        sb.AppendFormat("</div>")

        'div holder
        sb.Append("<div id=""navcontainer"">\n")

        'always show the contract stuff
        'sb.Append("<label>Contract Details</label>\n")

        If Me.ContractID > 0 Then
            sSelected = IIf(Me.CurrentPage = "Contract", " selected", "").ToString
            sb.AppendFormat(sLink, "Contract", Me.ContractID, "data" & sSelected, "Contract Details")
        Else
            sb.Append("<a href=""#"" class=""data selected"">Contract Details</a>\n")
        End If

        'if we have a contractid and a status<>'In Progress' then 
        'show the audit trail link
        If Me.ContractID > 0 And Me.Status <> "In Progress" Then

            'work out the class
            Dim sAuditTrailClass As String = _
                IIf(Me.AuditTrailStatus = "None", "nodata", _
                    IIf(Me.AuditTrailStatus = "Signed", "data", "nodatamandatory")).ToString

            If Me.CurrentPage = "AuditTrail" Then
                sAuditTrailClass += " selected"
            End If

            'sb.Append("<label>Audit Trail</label>\n")
            sb.AppendFormat(sLink, "AuditTrail", Me.ContractID, sAuditTrailClass, "View Audit Trail")
        End If

        'if we have a contractid then show the room types
        If Me.ContractID > 0 Then
            'sb.Append("<label>Room Types</label>\n")
            sSelected = IIf(Me.CurrentPage = "ContractRoomType", " selected", "").ToString
            sb.AppendFormat(sLink, "ContractRoomType", Me.ContractID, "data" & sSelected, "Edit Room Types")
        End If

        'get the data
        If Me.ContractID > 0 Then
            dtMenu = CType(context.Session("ContractMenuData"), DataTable)
            If dtMenu Is Nothing Then
                dtMenu = SQL.GetDatatable("Exec GetContractMenuData {0}", Me.ContractID.ToString)
                context.Session.Add("ContractMenuData", dtMenu)
            End If
        End If

        'for each section
        If Me.ContractID > 0 And Me.HasRoomTypes Then
            Dim sSection As String
            Dim sTitle As String
            Dim sNoteColumn As String
            Dim sDataColumn As String
            Dim sNoteImage As String
            Dim bHasData As Boolean
            Dim bShowAll As Boolean
            Dim bShowOnlyAll As Boolean
            Dim bSectionOpen As Boolean
            Dim iCountDataRow As Integer
            Dim iCountData As Integer

            Dim iRow As Integer
            Dim i As Integer
            For i = 2 To aSections.Length - 1
                'only show Payment Schedule if it's a Guaranteed contract
                If ArrayListContains(aSectionAccess, aTitles(i)) AndAlso _
                        (aTitles(i) <> "Payment Schedule" Or Me.ContractType <> "Allotted") AndAlso _
                        aTitles(i) <> "Other" Then

                    sSection = aSections(i)
                    sTitle = aTitles(i)
                    bSectionOpen = InStr(sOpenSections, sSection) > 0

                    'set up the names for the note and data fields
                    sNoteColumn = sSection & "Note"
                    sDataColumn = sSection & "Data"

                    'set up flag to determine if All 'room type' should be displayed
                    bShowAll = True
                    If sSection = "Rates" And Me.ContractType = "Allotted" Then bShowAll = False
                    '  If sSection = "AllocationRelease" Then bShowAll = False
                    bShowOnlyAll = sSection = "PaymentSchedule"

                    'icon for collapsed sections with data
                    If Not InList(sSection, "Contract", "ContractRoomType", "Rates", _
                        "AllocationRelease", "PaymentSchedule") Then

                        'work out if there's any data
                        iCountData = 0
                        For iCountDataRow = 0 To dtMenu.Rows.Count - 1
                            iCountData += SafeInt(dtMenu.Rows(iCountDataRow)(sDataColumn))
                        Next

                        'if so, add a tick; but hide it if the section's open
                        If iCountData > 0 Then
                            sb.AppendFormat(sTickImage, sSection, IIf(bSectionOpen, "none", "block"))
                        End If

                    End If

                    'note link - work out if the section has notes
                    If InList(sSection, "Rates", "AllocationRelease", "PaymentSchedule") OrElse bSectionOpen Then
                        sNoteImage = IIf(CType(dtMenu.Rows(0)(sNoteColumn), Integer) > 0, "text", "textnone").ToString
                    Else
                        sNoteImage = IIf(CType(dtMenu.Rows(0)(sNoteColumn), Integer) > 0, "fadedtext", "fadedtextnone").ToString
                    End If

                    'Hide notes icon for first two sections
                    If sSection.ToLower = "contract" OrElse sSection.ToLower = "contractroomtype" Then
                        sb.AppendFormat("", sSection, "", sTitle)
                    Else
                        sb.AppendFormat(sNoteLink, sSection, sNoteImage, sTitle)
                    End If

                    'add the section title (collapsible for lesser sections)
                    'and create a div for the room types
                    If InList(sSection, "Rates", "AllocationRelease", "PaymentSchedule") Then
                        sb.Append("<label>").Append(sTitle).Append("</label>\n")
                        sb.AppendFormat("<div id=""divNav{0}"">\n", sSection)
                    ElseIf bSectionOpen Then
                        sb.AppendFormat("<label id=""lblNav{0}"" class=""navsection"">", sSection)
                        sb.AppendFormat(sSectionLink, _
                            sSection, _
                            String.Format("javascript:ToggleContractNavSection('{0}')", sSection), _
                            "navsection", _
                            sTitle)
                        sb.Append("</label>\n")
                        sb.AppendFormat("<div id=""divNav{0}"" style=""display:block;"">\n", sSection)
                    Else
                        sb.AppendFormat("<label id=""lblNav{0}"" class=""navsectioncollapsed"">", sSection)
                        sb.AppendFormat(sSectionLink, _
                            sSection, _
                            String.Format("javascript:ToggleContractNavSection('{0}')", sSection), _
                            "navsectioncollapsed", _
                            sTitle)
                        sb.Append("</label>\n")
                        sb.AppendFormat("<div id=""divNav{0}"" style=""display:none;"">\n", sSection)
                    End If

                    'scan through the room types
                    For iRow = 0 To dtMenu.Rows.Count - 1

                        sSelected = IIf(Me.CurrentPage = sSection And _
                            CType(dtMenu.Rows(iRow)("ContractRoomTypeID"), Integer) = Me.ContractRoomTypeID, _
                            " selected", "").ToString

                        If (iRow > 0 OrElse bShowAll) AndAlso Not (bShowOnlyAll AndAlso iRow > 0) Then
                            bHasData = CType(dtMenu.Rows(iRow)(sDataColumn), Integer) > 0
                            If sSection.ToLower = "margin" Then
                                sb.AppendFormat("<a href=""Javascript:NavLink('Supplement.aspx?ContractRoomTypeID={1}&MarginModule=1');"" class=""{2}"">{3}</a>\n", sSection, dtMenu.Rows(iRow)("ContractRoomTypeID"),
                                IIf(bHasData, "data", "nodata").ToString & sSelected, dtMenu.Rows(iRow)("RoomTypeName"))
                            Else
                                sb.AppendFormat(sRoomTypeLink, sSection, dtMenu.Rows(iRow)("ContractRoomTypeID"),
                                IIf(bHasData, "data", "nodata").ToString & sSelected, dtMenu.Rows(iRow)("RoomTypeName"))
                            End If
                        End If
                    Next

                    sb.Append("</div>\n")

                ElseIf aTitles(i) = "Other" Then

                    'do the other sections
                    Dim sOtherLink As String = "<a href=""{0}.aspx?OtherTypeID={1}"" class=""{2}"">{3}</a>\n"
                    Dim oOtherInformation As OtherInformationType
                    If Me.OtherInformation.Count > 0 Then

                        'other label
                        sb.Append("<label>Other Information</label>\n")

                        'Try and get other type ID from request
                        Dim sOtherTypeId As String
                        Dim iOtherType As Integer = 0
                        If Not Page.Request("otherTypeId") Is Nothing Then
                            sOtherTypeId = Page.Request("otherTypeId")
                            iOtherType = Intuitive.Functions.SafeInt(sOtherTypeId)
                        End If

                        Dim sClass As String
                        For Each oOtherInformation In Me.OtherInformation

                            'work out the class
                            If oOtherInformation.HasData Then
                                sClass = "data"
                            ElseIf oOtherInformation.Mandatory = True And Not oOtherInformation.HasData Then
                                sClass = "nodatamandatory"
                            Else
                                sClass = "nodata"
                            End If
                            If oOtherInformation.OtherTypeID = iOtherType Then
                                sClass += " selected"
                            End If

                            sb.AppendFormat(sOtherLink, "OtherInformation", oOtherInformation.OtherTypeID, _
                                sClass, oOtherInformation.OtherType)
                        Next
                    End If
                End If

            Next

        End If

        'add the hidden input visibility tracker and close the holding div
        Dim oHidden As New Intuitive.WebControls.Hidden
        With oHidden
            .ID = "hidVisible"
            .Value = sOpenSections
        End With
        sb.Append(RenderControlToString(oHidden))


        sb.Append("</div>")

        Return sb.ToString.Replace("\n", ControlChars.NewLine)

    End Function

    'Private Function DrawMenu() As String

    '    Dim sb As New StringBuilder

    '    Dim aSectionAccess As ArrayList = GetUserGroupContractSectionAccess(UserSession.UserGroupID)

    '    'set up sections and titles
    '    Dim sSections As String = "Contract,ContractRoomType,Rates,AllocationRelease," & _
    '        "CloseOut,Tax,Supplement,Facility,MinMaxStay,SpecialOffers,Packages,PaymentSchedule,FreeOffer,GroupDiscount"
    '    Dim aSections() As String = sSections.Split(","c)
    '    Dim sTitles As String = "Contract,Room Types,Rates,Allocation & Release," & _
    '        "Close Outs,Taxes,Supplements,Facilities,Min/Max Stay,Special Offers,Packages,Payment Schedule,Free Offers,Group Discounts"
    '    Dim aTitles() As String = sTitles.Split(","c)

    '    'set up the html templates
    '    Dim sNoteLink As String = "<a href=""Note.aspx?Section={0}"">" & _
    '        "<img src=""../../images/ContractNavBar/{1}.gif"" class=""notes"" alt=""{2} Notes"" /></a>\n"
    '    Dim sLink As String = "<a href=""{0}.aspx"" class=""{2}"">{3}</a>\n"
    '    Dim sRoomTypeLink As String = "<a href=""{0}.aspx?ContractRoomTypeID={1}"" class=""{2}"">{3}</a>\n"
    '    Dim sSelected As String

    '    'Nav bar header
    '    sb.Append("<div id=""divNavHeader"" class=""section"">")
    '    sb.AppendFormat("<h3>{0}</h3>", Me.PropertyName)
    '    sb.AppendFormat("<h4>Reference: {0}</h4>", Me.ContractInformation.ContractReference)
    '    If Me.ContractID > 0 AndAlso UserSession.HasRight(UserSession.AccessRight.ViewPrintContract) Then
    '        sb.Append("<a id=""aNavBarPdf"" href=""javascript:Postback('ViewContractPDF')"" title=""View Contract""></a>")
    '    End If
    '    If Not DateFunctions.IsEmptyDate(Me.StayStartDate) Then
    '        sb.AppendFormat("<h5>Date: {0}</h5>", DisplayDate(Me.StayStartDate) & " - " & DisplayDate(Me.StayEndDate))
    '    End If
    '    sb.AppendFormat("</div>")

    '    'div holder
    '    sb.Append("<div id=""navcontainer"">\n")

    '    'always show the contract stuff
    '    sb.Append("<label>Contract Details</label>\n")

    '    If Me.ContractID > 0 Then
    '        sSelected = IIf(Me.CurrentPage = "Contract", " selected", "").ToString
    '        sb.AppendFormat(sLink, "Contract", Me.ContractID, "data" & sSelected, "Contract Details")
    '    Else
    '        sb.Append("<a href=""#"" class=""data selected"">Contract Details</a>\n")
    '    End If

    '    'if we have a contractid and a status<>'In Progress' then 
    '    'show the audit trail link
    '    If Me.ContractID > 0 And Me.Status <> "In Progress" Then

    '        'work out the class
    '        Dim sAuditTrailClass As String = _
    '            IIf(Me.AuditTrailStatus = "None", "nodata", _
    '                IIf(Me.AuditTrailStatus = "Signed", "data", "nodatamandatory")).ToString

    '        If Me.CurrentPage = "AuditTrail" Then
    '            sAuditTrailClass += " selected"
    '        End If

    '        sb.Append("<label>Audit Trail</label>\n")
    '        sb.AppendFormat(sLink, "AuditTrail", Me.ContractID, sAuditTrailClass, "View Audit Trail")
    '    End If


    '    'if we have a contractid then show the room types
    '    If Me.ContractID > 0 Then
    '        sb.Append("<label>Room Types</label>\n")
    '        sSelected = IIf(Me.CurrentPage = "ContractRoomType", " selected", "").ToString
    '        sb.AppendFormat(sLink, "ContractRoomType", Me.ContractID, "data" & sSelected, "Edit Room Types")
    '    End If

    '    'get the data
    '    If Me.ContractID > 0 Then
    '        dtMenu = CType(context.Session("ContractMenuData"), DataTable)
    '        If dtMenu Is Nothing Then
    '            dtMenu = SQL.GetDatatable("Exec GetContractMenuData {0}", Me.ContractID.ToString)
    '            context.Session.Add("ContractMenuData", dtMenu)
    '        End If
    '    End If

    '    'for each section
    '    If Me.ContractID > 0 And Me.HasRoomTypes Then
    '        Dim sSection As String
    '        Dim sTitle As String
    '        Dim sNoteColumn As String
    '        Dim sDataColumn As String
    '        Dim sNoteImage As String
    '        Dim bHasData As Boolean
    '        Dim bShowAll As Boolean
    '        Dim bShowOnlyAll As Boolean

    '        Dim iRow As Integer
    '        Dim i As Integer
    '        For i = 2 To aSections.Length - 1
    '            If ArrayListContains(aSectionAccess, aTitles(i)) Then
    '                sSection = aSections(i)
    '                sTitle = aTitles(i)

    '                'set up the names for the note and data fields
    '                sNoteColumn = sSection & "Note"
    '                sDataColumn = sSection & "Data"

    '                'set up flag to determine if All 'room type' should be displayed
    '                bShowAll = True
    '                If sSection = "Rates" And Me.ContractType = "Allotted" Then bShowAll = False
    '                'If sSection = "AllocationRelease" Then bShowAll = False
    '                bShowOnlyAll = sSection = "PaymentSchedule"

    '                'note link - work out if the section has notes
    '                sNoteImage = IIf(CType(dtMenu.Rows(0)(sNoteColumn), Integer) > 0, "text", "textnone").ToString

    '                'Hide notes icon for first two sections
    '                If sSection.ToLower = "contract" OrElse sSection.ToLower = "contractroomtype" Then
    '                    sb.AppendFormat("", sSection, "", sTitle)
    '                Else
    '                    sb.AppendFormat(sNoteLink, sSection, sNoteImage, sTitle)
    '                End If


    '                'add the section title
    '                sb.Append("<label>").Append(sTitle).Append("</label>\n")

    '                'scan through the room types
    '                For iRow = 0 To dtMenu.Rows.Count - 1

    '                    sSelected = IIf(Me.CurrentPage = sSection And _
    '                        CType(dtMenu.Rows(iRow)("ContractRoomTypeID"), Integer) = Me.ContractRoomTypeID, _
    '                        " selected", "").ToString

    '                    If (iRow > 0 OrElse bShowAll) AndAlso Not (bShowOnlyAll AndAlso iRow > 0) Then
    '                        bHasData = CType(dtMenu.Rows(iRow)(sDataColumn), Integer) > 0
    '                        sb.AppendFormat(sRoomTypeLink, sSection, dtMenu.Rows(iRow)("ContractRoomTypeID"), _
    '                            IIf(bHasData, "data", "nodata").ToString & sSelected, dtMenu.Rows(iRow)("RoomTypeName"))
    '                    End If
    '                Next
    '            End If
    '        Next

    '        'do the other sections
    '        Dim sOtherLink As String = "<a href=""{0}.aspx?OtherTypeID={1}"" class=""{2}"">{3}</a>\n"
    '        Dim oOtherInformation As OtherInformationType
    '        If Me.OtherInformation.Count > 0 Then

    '            'other label
    '            sb.Append("<label>Other Information</label>\n")

    '            'Try and get other type ID from request
    '            Dim sOtherTypeId As String
    '            Dim iOtherType As Integer = 0
    '            If Not Page.Request("otherTypeId") Is Nothing Then
    '                sOtherTypeId = Page.Request("otherTypeId")
    '                iOtherType = Intuitive.Functions.SafeInt(sOtherTypeId)
    '            End If

    '            Dim sClass As String
    '            For Each oOtherInformation In Me.OtherInformation

    '                'work out the class
    '                If oOtherInformation.HasData Then
    '                    sClass = "data"
    '                ElseIf oOtherInformation.Mandatory = True And Not oOtherInformation.HasData Then
    '                    sClass = "nodatamandatory"
    '                Else
    '                    sClass = "nodata"
    '                End If
    '                If oOtherInformation.OtherTypeID = iOtherType Then
    '                    sClass += " selected"
    '                End If

    '                sb.AppendFormat(sOtherLink, "OtherInformation", oOtherInformation.OtherTypeID, _
    '                    sClass, oOtherInformation.OtherType)
    '            Next
    '        End If
    '    End If


    '    'close the holding div
    '    sb.Append("</div>")

    '    Return sb.ToString.Replace("\n", ControlChars.NewLine)

    'End Function


#End Region

#Region "SetNewContract"

    Public Sub SetNewContract(ByVal iPropertyID As Integer)

        'Get the property name
        Dim dr As DataRow = SQL.GetDataRow("Select Name, Reference from Property where PropertyID={0}", iPropertyID)

        Dim oContractInformation As New ContractInformationType(iPropertyID, dr(0).ToString, dr(1).ToString, 0, _
             "", DateFunctions.EmptyDate, DateFunctions.EmptyDate, DateFunctions.EmptyDate, _
                DateFunctions.EmptyDate, 0, "", "None", "", 0, 0)

        Me.ContractInformation = oContractInformation
    End Sub
#End Region

#Region "render"

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write(Me.DrawMenu)
    End Sub

#End Region

#Region "ContractInformationType structure"

    Private Class ContractInformationType
        Public PropertyID As Integer
        Public PropertyName As String
        Public PropertyReference As String
        Public ContractID As Integer
        Public ContractRoomTypeID As Integer
        Public ContractType As String
        Public RoomTypes As New Hashtable
        Public BookingStartDate As Date
        Public BookingEndDate As Date
        Public StayStartDate As Date
        Public StayEndDate As Date
        Public CurrencyID As Integer
        Public Status As String
        Public AuditTrailStatus As String
        Public ContractReference As String
        Public SignedBy As Integer
        Public Signature As Integer

        Public ReadOnly Property HasRoomTypes() As Boolean
            Get
                Return Me.RoomTypes.Count > 0
            End Get
        End Property

        Public Sub ClearRoomTypes()
            Me.RoomTypes.Clear()
        End Sub

        Public Sub AddRoomType(ByVal iContractRoomTypeID As Integer, ByVal sRoomType As String)
            Me.RoomTypes.Add(iContractRoomTypeID, sRoomType)
        End Sub

        Public Function GetRoomType(ByVal iContractRoomTypeID As Integer) As String

            If Not Me.RoomTypes(iContractRoomTypeID) Is Nothing Then
                Return Me.RoomTypes(iContractRoomTypeID).ToString
            Else
                Throw New Exception("Could not find ContractRoomType " & iContractRoomTypeID)
            End If

        End Function

        Public Sub New(ByVal PropertyID As Integer, ByVal PropertyName As String, ByVal PropertyReference As String, _
            ByVal ContractID As Integer, ByVal ContractType As String, _
            ByVal BookingStartDate As Date, ByVal BookingEndDate As Date, _
            ByVal StayStartDate As Date, ByVal StayEndDate As Date, ByVal CurrencyID As Integer, _
            ByVal Status As String, ByVal AuditTrailStatus As String, ByVal ContractReference As String, ByVal SignedBy As Integer, ByVal Signature As Integer)

            Me.PropertyID = PropertyID
            Me.PropertyName = PropertyName
            Me.PropertyReference = PropertyReference
            Me.ContractID = ContractID
            Me.ContractType = ContractType
            Me.BookingStartDate = BookingStartDate
            Me.BookingEndDate = BookingEndDate
            Me.StayStartDate = StayStartDate
            Me.StayEndDate = StayEndDate
            Me.CurrencyID = CurrencyID
            Me.Status = Status
            Me.AuditTrailStatus = AuditTrailStatus
            Me.ContractReference = ContractReference
            Me.SignedBy = SignedBy
            Me.Signature = Signature
        End Sub

    End Class


    Private Structure OtherInformationType
        Public OtherTypeID As Integer
        Public OtherType As String
        Public Mandatory As Boolean
        Public HasData As Boolean
        Public Selected As Boolean

        Public Sub New(ByVal OtherTypeID As Integer, ByVal OtherType As String, _
            ByVal Mandatory As Boolean, ByVal HasData As Boolean, ByVal Selected As Boolean)

            Me.OtherTypeID = OtherTypeID
            Me.OtherType = OtherType
            Me.Mandatory = Mandatory
            Me.HasData = HasData
            Me.Selected = Selected
        End Sub
    End Structure
#End Region

#Region "AddAuditTrailEntry"

    Public Sub AddAuditTrailEntry()

        'audit trail
        If Me.Status <> "In Progress" Then

            'get the contract section id
            Dim iContractSectionID As Integer = ContractSection.GetContractSectionIDFromSection(Me.CurrentPage)

            'add the log entry
            Dim sSql As String = "exec AddAuditTrailEntry {0},{1},{2},{3}"
            SQL.Execute(sSql, Me.ContractID, Me.ContractRoomTypeID, iContractSectionID, UserSession.SystemUserID)

            'reset the status
            Me.ResetAuditTrailStatus()
        End If

        'set sync flag
        Contract.SetSyncRequired(Me.ContractID)

    End Sub
#End Region

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

        If Me.Command = "ViewContractPDF" Then
            'Dim sPDF As String = String.Format("Contract_{0}.pdf", _
            '    SQL.GetValue("Select replace(Reference,'/','-') from Contract Where ContractID={0}", Me.ContractID))

            'PDFGenerator.QuickPDFGenerator("XMLGetContracts", SQL.GetSqlValue(Me.ContractID, "string"), _
            '    "Contract.xsl", sPDF, True, CType(Me.Page, PageBase))
            Dim param As IntuitivePDF.PDFGenerator.XSLTParams = New IntuitivePDF.PDFGenerator.XSLTParams()
            param.AddParam("kind", SafeString(Me.Argument))
            Dim sDocumentName As String = String.Format("Contract_{0}_{1}.pdf", _
                    SQL.GetValue("Select replace(Reference,'/','-') from Contract Where ContractID={0}", Me.ContractID), _
                    Now.ToString("HHmmss")).Replace("*", "_")

            Dim sBase As String = GetBaseURL()
            Dim sPDFURL As String = String.Format(sBase & "PDFs/{0}", sDocumentName)

            'register script to do popup
            Dim sDocumentViewerPage As String = sBase & "DocumentViewer.aspx"
            Dim trades As ArrayList = Nothing
            If Me.Argument = "gray" Or Me.Argument = "blue" Then
                trades = New ArrayList(1)
                trades.Add(SafeInt(SQL.ExecuteSingleValue("exec GetMaxTradeMemberForContract {0}", Me.ContractID)))
                param.AddParam("forcePosition", 0)
                param.AddParam("voidDupe", 1)
            End If

            Contract.ViewContract(CType(Me.Page, PageBase), Me.ContractID, trades, sDocumentName, False, param)
            If CType(Me.Page, PageBase).Warnings.Count = 0 Then
                If Me.Argument = "gray" Then
                    'Me.Page.RegisterStartupScript("download", String.Format("<script>document.location='{0}';</script>", sPDFURL))
                    Me.Page.Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", sDocumentName))
                    Me.Page.Response.ContentType = "Application/pdf"
                    Me.Page.Response.TransmitFile(String.Format("{0}\\PDFs\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, sDocumentName))
                    Me.Page.Response.End()
                Else
                    Me.Page.RegisterStartupScript("popup", String.Format("<script>javascript:ShowPDF('{0}','{1}')</script>", sDocumentViewerPage, sPDFURL))
                End If
            End If
        End If

    End Sub

End Class
