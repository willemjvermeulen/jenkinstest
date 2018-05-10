Imports Intuitive.WebControls
Imports System.Text
Imports Intuitive

Public Class DateSetter
    Inherits Control
    Implements IPostBackDataHandler

#Region "Properties"

    Private bShowStayDate As Boolean = True
    Private bShowRatesDate As Boolean = True
    Private bShowClearAll As Boolean = True

    Public Property ShowStayDate() As Boolean
        Get
            Return bShowStayDate
        End Get
        Set(ByVal Value As Boolean)
            bShowStayDate = Value
        End Set
    End Property
    Public Property ShowRatesDate() As Boolean
        Get
            Return bShowRatesDate
        End Get
        Set(ByVal Value As Boolean)
            bShowRatesDate = Value
        End Set
    End Property
    Public Property ShowClearAll() As Boolean
        Get
            Return bShowClearAll
        End Get
        Set(ByVal Value As Boolean)
            bShowClearAll = Value
        End Set
    End Property
#End Region

    Public Event SetDates(ByVal aDates As ArrayList)


#Region "render"

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        Dim sb As New StringBuilder

        'Set Date
        sb.Append("<label class=""DateSetterLabel"">Set Dates</label>\n")

        'drop down - select and blank option
        sb.AppendFormat("<select id=""{0}"" name=""{0}"" class=""dropdown"" " & _
            "onchange=""javascript:Postback('{0}','')"" >\n", Me.ID)
        sb.Append("<option></option>\n")

        'stay dates
        If Me.ShowStayDate Then
            sb.Append("<option>Stay Dates</option>\n")
        End If
        If Me.ShowRatesDate Then
            sb.Append("<option>Rate Dates</option>\n")
        End If
        If Me.ShowClearAll Then
            sb.Append("<option>------</option>\n")
            sb.Append("<option>Clear All</options>\n")
        End If

        'close select
        sb.Append("</select>\n")
        writer.Write(sb.ToString.Replace("\n", ControlChars.NewLine))

        MyBase.Render(writer)

    End Sub


#End Region

#Region "getdates"

    'stay dates
    Private Sub GetStayDates()

        'get the stay start and end dates off the navbar
        Dim oNav As ContractNavBar = Me.GetNavControl
        Dim aDates As New ArrayList
        aDates.Add(New Dateband(oNav.StayStartDate, oNav.StayEndDate))

        RaiseEvent SetDates(aDates)
    End Sub

    'rate dates
    Private Sub GetRateDates()

        'get the contractroomtypeid off the navbar and get
        'the dates from the db
        Dim oNav As ContractNavBar = Me.GetNavControl
        Dim iContractRoomTypeID As Integer = oNav.ContractRoomTypeID

        If iContractRoomTypeID > 0 Then
            Dim dt As DataTable = SQL.GetDatatable("exec GetContractRoomTypeDates {0}", _
                iContractRoomTypeID)
            Dim dr As DataRow
            Dim aDates As New ArrayList

            For Each dr In dt.Rows
                aDates.Add(New Dateband(CType(dr("StartDate"), Date), CType(dr("EndDate"), Date)))
            Next

            RaiseEvent SetDates(aDates)
        End If
    End Sub

    Private Function GetNavControl() As ContractNavBar
        Dim oControl As Control = Me.Page.FindControl("nav")
        If Not oControl Is Nothing And oControl.GetType Is GetType(ContractNavBar) Then
            Return CType(oControl, ContractNavBar)
        Else
            Throw New Exception("Could not find the Nav control")
        End If
    End Function
#End Region

#Region "postback handling"

    Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
        Return True
    End Function

    Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent

        'get the selection - either stay dates or rate dates
        Dim sSelection As String = Me.Page.Request(Me.ID)
        If sSelection = "Stay Dates" Then
            Me.GetStayDates()
        ElseIf sSelection = "Rate Dates" Then
            Me.GetRateDates()
        ElseIf sSelection = "Clear All" Then
            RaiseEvent SetDates(New ArrayList)
        End If
    End Sub

#End Region

#Region "dateband structure"
    Public Structure Dateband
        Dim StartDate As Date
        Dim EndDate As Date

        Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date)
            Me.StartDate = StartDate
            Me.EndDate = EndDate
        End Sub
    End Structure
#End Region

End Class
