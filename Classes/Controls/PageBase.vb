Imports Intuitive
Imports Intuitive.Webcontrols
Imports System.Data.SqlClient
Imports System.Text

Public Class PageBase
    Inherits Intuitive.WebControls.Page

    Private sPageName As String = ""
    Private sPageImage As String = ""
    Private sEditMode As String = ""
    Private bHeaderVisible As Boolean = True
    Private bShowMainNav As Boolean = True
    Private bShowLogOutNav As Boolean = False
    Protected bForceSecure As Boolean = False

    Public Enum AppNavType
        ContractPage
        PropertyPage
        SupplementPage
    End Enum

#Region "Generated Properties"
    Public Property PageImage() As String
        Get
            Return sPageImage
        End Get
        Set(ByVal Value As String)
            sPageImage = Value
        End Set
    End Property
    Public Property EditMode() As String
        Get
            Return GetViewState("EditMode")
        End Get
        Set(ByVal Value As String)
            SetViewState("EditMode", Value)
        End Set
    End Property
    Public Property HeaderVisible() As Boolean
        Get
            Return bHeaderVisible
        End Get
        Set(ByVal Value As Boolean)
            bHeaderVisible = Value
        End Set
    End Property

    Public Property ShowMainNav() As Boolean
        Get
            Return bShowMainNav
        End Get
        Set(ByVal Value As Boolean)
            bShowMainNav = Value
        End Set
    End Property

    Public Property ShowLogOutNav() As Boolean
        Get
            Return bShowLogOutNav
        End Get
        Set(ByVal Value As Boolean)
            bShowLogOutNav = Value
        End Set
    End Property

    Public ReadOnly Property QueryStringCommand() As String
        Get
            If Not IsNothing(Page.Request.QueryString("Command")) Then
                Return Page.Request.QueryString("Command").ToString
            ElseIf Not IsNothing(Page.Request.Form("Command")) Then
                Return Page.Request.Form("Command").ToString
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property QueryStringArgument() As String
        Get
            If Not IsNothing(Page.Request.QueryString("Argument")) Then
                Return Page.Request.QueryString("Argument").ToString
            ElseIf Not IsNothing(Page.Request.Form("Argument")) Then
                Return Page.Request.Form("Argument").ToString
            Else
                Return ""
            End If

        End Get
    End Property

    Private Property PageAccessKeys() As AccessKeys
        Get
            Return CType(Me.Session("AccessKeys"), AccessKeys)
        End Get
        Set(ByVal Value As AccessKeys)
            Me.Session("AccessKeys") = Value
        End Set
    End Property

#End Region


#Region "Set app nav"
    Public Sub SetAppNav(ByVal Type As AppNavType, _
                         ByVal iID As Integer, _
                         Optional ByVal sUpdate As String = "")

        'no id
        If iID < 0 Then Return

        'this needs error handling - some! any! just think about it!!!!

        'add app navigation links
        Select Case Type
            Case AppNavType.ContractPage


            Case AppNavType.PropertyPage
                'add link back to property
                Dim dtInfo As DataTable
                Dim oSB As New StringBuilder
                Dim sPropertyBase As String

                'build up query and get data
                oSB.Append("select PropertyID,ShortName ")
                oSB.Append("	from Property")
                oSB.Append("	Where PropertyID=" & iID.ToString)
                dtInfo = SQL.GetDatatable(oSB.ToString)

                'property
                sPropertyBase = ResolveURL("~/secure/Property/Property.aspx?Command=Edit&Argument={0}&Update={1}")
                Me.AddAppNav("Property (" & dtInfo.Rows(0)("ShortName").ToString & ")", _
                            String.Format(sPropertyBase, dtInfo.Rows(0)("PropertyID"), sUpdate))

            Case AppNavType.SupplementPage
                'add link back to Supplement
                Dim dtInfo As DataTable
                Dim oSB As New StringBuilder
                Dim sBase As String

                'build up query and get data
                oSB.Append("select ContractSupplementID, ContractRoomTypeID,ContractID, SupplementName ")
                oSB.Append("	from ContractSupplement")
                oSB.Append("	Where ContractSupplementID=" & iID.ToString)
                dtInfo = SQL.GetDatatable(oSB.ToString)

                'add property nav 1st 
                SetAppNav(AppNavType.PropertyPage, _
                    GetPropertyIDByContract(CType(dtInfo.Rows(0)("ContractID"), Integer)))

                'now our Supplement
                sBase = ResolveURL("~/secure/ContractSetup/Supplement.aspx?ID={0}&Update={1}")
                Me.AddAppNav("contract supplement (" & dtInfo.Rows(0)("SupplementName").ToString & ")", _
                            String.Format(sBase, dtInfo.Rows(0)("ContractRoomTypeID"), sUpdate))
        End Select

    End Sub

    Private Function GetPropertyIDByContract(ByVal iContractID As Integer) As Integer
        Return SQL.ExecuteSingleValue("Select PropertyID From Contract Where ContractID={0}", iContractID)
    End Function
#End Region

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        If Page.IsPostBack AndAlso Not Me.Command Is Nothing Then
            If Me.Command = "Logout" Then

                'signOut - Remove the ol' Authentication Ticket
                System.Web.Security.FormsAuthentication.SignOut()

                'remove auth ticket manually cos signout never works (thx MS)
                Response.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName).Expires = Date.Now.AddDays(-1)

                'abandon the baby
                'Session.Abandon()

                'redirect to Login
                Response.Redirect(Intuitive.Functions.GetBaseURL & "Login.aspx")
            End If
        End If

        MyBase.OnLoad(e)

        If Not Request.IsSecureConnection AndAlso Config.SecuritySet AndAlso (HttpContext.Current.Request.Url.AbsoluteUri.Contains("/Secure/Administration/") Or bForceSecure) Then
            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"))
        End If

        If Request.IsSecureConnection AndAlso Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("/Secure/Administration/") AndAlso Not bForceSecure Then
            Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https:", "http:"))
        End If

    End Sub

#Region "render"
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        If Me.ShowLinkToMainMenu Then
            If Me.PageAccessKeys Is Nothing Then
                UserSession.RefreshAccessKeys()
            End If

            Me.PageAccessKeys.DrawAccessKeys(Me)
        End If

        MyBase.Render(writer)
    End Sub

#End Region

    Public Sub New()
        MyBase.New()
        Me.ShowHelp = True
    End Sub
End Class