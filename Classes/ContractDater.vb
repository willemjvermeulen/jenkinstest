Imports Intuitive
Imports Intuitive.WebControls

Public Class ContractDater
    Inherits ControlBase
    Implements IPostBackDataHandler

#Region "Generated Properties"
    Private bContractDate As Boolean = True
    Private bRateDate As Boolean = False
    Private bRateDateAll As Boolean = False
    Private sValue As String = ""
    Private iContractID As Integer = -1
    Private iRoomTypeID As Integer = -1
    Private sStartDate As String = ""
    Private sEndDate As String = ""

    Public Property ShowContractDate() As Boolean
        Get
            Return bContractDate
        End Get
        Set(ByVal Value As Boolean)
            bContractDate = Value
        End Set
    End Property

    Public Property ShowRateDate() As Boolean
        Get
            Return bRateDate
        End Get
        Set(ByVal Value As Boolean)
            bRateDate = Value
        End Set
    End Property
    Public Property ShowRateDateAll() As Boolean
        Get
            Return bRateDateAll
        End Get
        Set(ByVal Value As Boolean)
            bRateDateAll = Value
        End Set
    End Property
    Public Property ContractID() As Integer
        Get
            Return iContractID
        End Get
        Set(ByVal Value As Integer)
            iContractID = Value
        End Set
    End Property

    Public Property RoomTypeD() As Integer
        Get
            Return iRoomTypeID
        End Get
        Set(ByVal Value As Integer)
            iRoomTypeID = Value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return sValue
        End Get
        Set(ByVal Value As String)
            sValue = Value

            'set the start n end date proeprties
            Dim aValues() As String
            'split date range and set the start an end date fields
            aValues = Split(sValue, "~")

            If aValues.Length = 2 Then
                sStartDate = aValues(0)
                sEndDate = aValues(1)
            End If
        End Set
    End Property

    Public ReadOnly Property StartDate() As String
        Get
            Return sStartDate
        End Get
    End Property

    Public ReadOnly Property EndDate() As String
        Get
            Return sEndDate
        End Get
    End Property

#End Region

#Region "New/onLoad/onPreRender/Render"
    Public Sub New()
        MyBase.New()
        Me.Clear()
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write(Populate)
    End Sub
#End Region

#Region "Methods"
    Public Sub Clear()
        'clear any cached data
    End Sub

    Private Function Populate() As String

        Dim oSB As New System.Text.StringBuilder

        oSB.AppendFormat("<select id=""{0}"" name=""{0}"" class=""dropdown"">\n", Me.ID)

        'add the contract dates
        oSB.Append(GetContractDates)

        'add the rate dates
        oSB.Append(GetRateDates)

        oSB.Append("</select>\n")

        Return oSB.ToString.Replace("\n", ControlChars.CrLf)

    End Function

    Private Function GetContractDates() As String
        Dim sSQL As String
        Dim oDt As DataTable
        Dim oRow As DataRow
        Dim oSB As New System.Text.StringBuilder
        Dim sReturn As String = ""

        If Me.ShowContractDate Then
            'fill the date list with contract dates
            sSQL = "Select StayStartDate, StayEndDate " & _
                    "From Contract " & _
                    "Where ContractID={0}"
            oDt = SQL.GetDatatable(sSQL, Me.ContractID)

            If Not oDt Is Nothing AndAlso oDt.Rows.Count > 0 Then
                'get the data row
                oRow = oDt.Rows(0)

                'get the value
                Dim dStart As Date = CType(oRow("StayStartDate"), Date)
                Dim dEnd As Date = CType(oRow("StayEndDate"), Date)
                Dim sValue As String = DateFunctions.DisplayDate(dStart) & "~" & DateFunctions.DisplayDate(dEnd)

                'add the option
                oSB.AppendFormat("<option value=""{0}"" class=""dropdowngroup"">Contract</option>\n", sValue)
            End If

            sReturn = oSB.ToString.Replace("\n", ControlChars.CrLf)
        End If

        Return sReturn
    End Function

    Private Function GetRateDates() As String
        Dim sSQL As String
        Dim oDt As DataTable
        Dim oRow As DataRow
        Dim oSB As New System.Text.StringBuilder
        Dim sReturn As String = ""

        If Me.ShowRateDate Then

            oSB.Append("<option value=""Rates"">Rates</option>\n")

            'add 'All Rates' option?
            If Me.ShowRateDateAll Then
                oSB.Append("<option value=""AllRates"">All</option>\n")
            End If

            'fill the date list with rate dates
            sSQL = "Select StartDate, EndDate " & _
                    "From ContractRateDate " & _
                    "Where ContractID={0} " & _
                    "And ContractRoomTypeID={1}"
            oDt = SQL.GetDatatable(sSQL, Me.ContractID, Me.RoomTypeD)

            If Not oDt Is Nothing AndAlso oDt.Rows.Count > 0 Then
                'get the data row
                oRow = oDt.Rows(0)

                'get the value
                Dim dStart As Date = CType(oRow("StartDate"), Date)
                Dim dEnd As Date = CType(oRow("EndDate"), Date)
                Dim sValue As String = DateFunctions.DisplayDate(dStart) & "~" & DateFunctions.DisplayDate(dEnd)

                'add the option
                'Dim oDate As DateFunctions
                oSB.AppendFormat("<option value=""{0}"" class=""dropdowngroup"">{1}</option>\n", sValue, _
                                (DateFunctions.DisplayDate(dStart) & " to " & DateFunctions.DisplayDate(dEnd)))
            End If

            sReturn = oSB.ToString.Replace("\n", ControlChars.CrLf)
        End If

        Return sReturn
    End Function

#End Region

#Region "Postback Event"
    Public Event ItemSelected(ByVal Value As String)

    Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
        'set the value
        Me.Value = postCollection(postDataKey)

        'return true if this control has posted back
        Return Me.Command = Me.ID
    End Function

    Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
        RaiseEvent ItemSelected(sValue)
    End Sub
#End Region


End Class
