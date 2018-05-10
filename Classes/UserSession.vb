Imports System.Web.Security
Imports System.Text
Imports Intuitive
Imports Intuitive.Functions
Imports System.Configuration.ConfigurationSettings

Public Class UserSession
    Inherits UserSessionBase

    Private sWarning As String
    Private sRedirectURL As String = ConfigurationSettings.AppSettings.Get("MainMenuURL")
    Private sLoginURL As String = ConfigurationSettings.AppSettings.Get("LoginURL")

#Region "Live Properties"

    Public Shared ReadOnly Property SystemUserID() As Integer
        Get
            Dim sReturn As String = GetCookieUserData(0)

            If IsNumeric(sReturn) Then
                Return CType(sReturn, Integer)
            Else
                Return 0
            End If
        End Get
    End Property

    Public Shared ReadOnly Property Username() As String
        Get
            Return GetCookieUserData(1).Trim
        End Get
    End Property

    Public Shared ReadOnly Property UserGroupID() As Integer
        Get
            Dim sReturn As String = GetCookieUserData(2)
            If IsNumeric(sReturn) Then
                Return CType(sReturn, Integer)
            Else
                Return 0
            End If
        End Get
    End Property

    Public Shared ReadOnly Property PasswordExpires() As Date
        Get
            Dim oSystemUser As New SystemUser
            oSystemUser.Go(UserSession.SystemUserID)
            Return CType(oSystemUser("PasswordExpires"), Date)
        End Get
    End Property

    Public Shared ReadOnly Property SuperUserAccess() As Boolean
        Get
            Return SafeBoolean(GetCookieUserData(3))
        End Get
    End Property

    Public Shared ReadOnly Property TimezoneMinutesOffset() As Integer
        Get
            Return SafeInt(GetCookieUserData(4))
        End Get
    End Property

    Public Shared ReadOnly Property Email() As string
        Get
            Return SafeString(GetCookieUserData(5))
        End Get
    End Property

    Public Shared ReadOnly Property BookingAuthorityID() As Integer
        Get
            Return safeint(GetCookieUserData(6))
        End Get
    End Property

    Public Shared ReadOnly Property CompanyName() As String
        Get
            Return SafeString(AppSettings("CompanyName"))
        End Get
    End Property

    Public Shared ReadOnly Property AllowTransferBookings() As Boolean
        Get
            Return SafeBoolean(AppSettings("AllowTransfers"))
        End Get
    End Property

#End Region

#Region "Login/Out"

#Region "rubbish"
    Public Overloads Overrides Function Login(ByVal s As String, ByVal s2 As String) As Boolean

    End Function
#End Region

    Public Overloads Function Login(ByVal sLogin As String, ByVal sPassword As String, _
                ByVal iTimezoneMinutesOffset As Integer) As Boolean

        Dim sSql As String = ""
        Dim oDataTable As DataTable
        Dim bFail As Boolean = False

        If sLogin = "" Then
            Me.Warning = "Please input your username"
            Return False
        End If

        If sPassword = "" Then
            Me.Warning = "Please input your password"
            Return False
        End If

        'get the user data
        oDataTable = SQL.GetDatatable("GetUserLogin {0}", SQL.GetSqlValue(sLogin, "String"))

        'make sure we have at least one row and then check passwordhash 
        If oDataTable.Rows.Count < 1 OrElse _
                oDataTable.Rows(0)("Password").ToString <> UserSession.PasswordHash(sPassword) Then
            Me.Warning = "Sorry, the username and/or password input is not valid"
            bFail = True
        End If

        'if all ok, build our userdata string and set authenticated
        If Not bFail Then
            Dim oSB As New StringBuilder

            'Is it US date format?
            If SafeBoolean(oDataTable.Rows(0)("USDateFormat")) = True Then
                HttpContext.Current.Session.Add("USDateFormat", True)
            End If

            'Get the name etc
            oSB.Append(oDataTable.Rows(0)("SystemUserID").ToString).Append(",")
            oSB.Append(oDataTable.Rows(0)("Username").ToString.Trim).Append(",")
            oSB.Append(oDataTable.Rows(0)("UserGroupID").ToString.Trim).Append(",")
            oSB.Append(oDataTable.Rows(0)("SuperUserAccess").ToString.Trim).Append(",")
            oSB.Append(iTimezoneMinutesOffset).Append(",")
            oSB.Append(oDataTable.Rows(0)("Email").ToString.Trim).Append(",")
            oSB.Append(oDataTable.Rows(0)("BookingAuthorityID").ToString.Trim).Append(",")

            'set authentication ticket
            UserSession.SetAuthenticated(oSB.ToString)

            'default, then set up redirect
            If Not oDataTable.Rows(0).Item("LoginRedirect").ToString.Trim.Equals("") Then
                sRedirectURL = oDataTable.Rows(0).Item("LoginRedirect").ToString
            End If

            'check to see if there is a return url
            Dim sReturnURL As String = HttpContext.Current.Request("ReturnURL")
            If Not sReturnURL Is Nothing Then
                sRedirectURL = sReturnURL
            End If

            'If password has expired then override redirect - overrides other stuff
            Dim oTimeSpan As TimeSpan = UserSession.PasswordExpires.Subtract(Now)
            Dim iDaysToExpire As Integer = oTimeSpan.Days
            If iDaysToExpire < 0 Then
                sRedirectURL = "PasswordUpdate.aspx"
            End If

            'Save login
            UserSession.SetLastLogin(sLogin)

            'redirect the user
            UserSession.Redirect(sRedirectURL)

        End If
    End Function

#Region "redirect"
    Public Shared Sub Redirect(ByVal sURL As String)
        With HttpContext.Current.Response
            .Buffer = True
            .Clear()
            .Status = "301 Moved"
            .AddHeader("Location", sURL)
            .End()
        End With
    End Sub
#End Region
#End Region

#Region "access rights"

    Public Enum AccessRight
        AddContract
        EditContract
        SetContractLive
        CancelContract
        AddProperty
        EditProperty
        EditBankDetails
        OverrideLockedContract
        Contracts
        ContractHeaderSets
        RegionalTax
        Properties
        PropertyGroups
        PropertyTypes
        ContractViewer
        Geography
        AccessKeys
        AddressTypes
        Brands
        Currencies
        ExtraBedTypes
        Facilities
        FreeOfferTypes
        MealPlans
        OtherInformationTypes
        RoomTypes
        RoomViews
        SpecialOfferTypes
        UserManagement
        AddOption
        ViewPrintContract
    End Enum

    Public Shared Function HasRight(ByVal eAccessRight As AccessRight) As Boolean

        Dim aAccessRights As ArrayList = UserSession.GetUserGroupAccessRights(UserSession.UserGroupID)

        Return ArrayListContains(aAccessRights, eAccessRight.ToString)

    End Function

    Private Shared Function GetUserGroupAccessRights(ByVal iUserGroupID As Integer) As ArrayList
        Dim sCacheName As String = "UserGroupAccessRight" & iUserGroupID
        Dim oCache As Caching.Cache = HttpContext.Current.Cache

        'try and get from the cache 
        Dim aUserGroupAccessRights As ArrayList = CType(oCache(sCacheName), ArrayList)

        If aUserGroupAccessRights Is Nothing Then

            aUserGroupAccessRights = New ArrayList

            Dim dt As DataTable = SQL.GetDatatable("exec UserGroupGetAccessRights {0}", iUserGroupID)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    aUserGroupAccessRights.Add(dr(0))
                Next
            End If

            'add to the cache
            AddToCache(sCacheName, aUserGroupAccessRights, "UserGroupAccessRight.txt")

        End If

        Return aUserGroupAccessRights
    End Function

#End Region

#Region "Access keys"
    Public Shared Sub RefreshAccessKeys()
        'Get Access Keys
        Dim tblAccessKeys As DataTable = _
            SQL.GetDatatable("GetAccessKeysByUser {0}", UserSession.SystemUserID)
        Dim oAccessKeys As New AccessKeys
        For Each dr As DataRow In tblAccessKeys.Rows
            Dim oAccessKey As New AccessKey
            oAccessKey.Key = SafeInt(dr("AccessKey"))
            oAccessKey.URL = dr("AccessRightURL").ToString
            oAccessKeys.Add(oAccessKey)
        Next
        'Add to session
        System.Web.HttpContext.Current.Session("AccessKeys") = oAccessKeys
    End Sub
#End Region

#Region "Last Viewed Contracts"
    'Public Shared Function GetLastViewedContracts() As ArrayList
    '    Dim aContractIDs As New ArrayList
    '    Dim oCookie As HttpCookie = HttpContext.Current.Request.Cookies("RecentContracts")
    '    If Not oCookie Is Nothing Then
    '        aContractIDs = Intuitive.Functions.DelimitedStringToArrayList(oCookie.Value)
    '    End If
    '    Return aContractIDs
    'End Function

    Public Shared Sub AddLastViewedContract(ByVal iContractID As Integer)

        SQL.Execute("exec ContractRecentAdd {0},{1}", _
            UserSession.SystemUserID, iContractID)

        ''Get Existing cookie
        'Dim oCookie As HttpCookie = HttpContext.Current.Request.Cookies("RecentContracts")

        ''Convert to arraylist
        'Dim aExistingContractIds As New ArrayList
        'If Not oCookie Is Nothing Then
        '    aExistingContractIds = _
        '        Intuitive.Functions.DelimitedStringToArrayList(oCookie.Value)
        'End If

        ''Add new contract to new list
        'Dim aNewContractIDs As New ArrayList
        'aNewContractIDs.Add(iContractID)
        'For i As Integer = 0 To aExistingContractIds.Count - 1
        '    If Not aNewContractIDs.Count > 4 AndAlso SafeInt(aExistingContractIds(i)) <> iContractID Then
        '        aNewContractIDs.Add(SafeInt(aExistingContractIds(i)))
        '    End If
        'Next

        ''Create and add new cookie
        'oCookie = New HttpCookie("RecentContracts")
        'oCookie.Value = Intuitive.Functions.ArrayListToDelimitedString(aNewContractIDs)
        'oCookie.Expires = Now.AddDays(7) 'remember for a week
        'HttpContext.Current.Response.Cookies.Add(oCookie)

    End Sub
#End Region

End Class
