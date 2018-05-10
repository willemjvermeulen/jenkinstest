Imports Intuitive
Imports System.Text

Public Class Rates

#Region "properties"

    Private aDatebands As New ArrayList
    Private aHeaders As New ArrayList

    Public Property Datebands() As ArrayList
        Get
            Return aDatebands
        End Get
        Set(ByVal Value As ArrayList)
            aDatebands = Value
        End Set
    End Property

    Public Property Headers() As ArrayList
        Get
            Return aHeaders
        End Get
        Set(ByVal Value As ArrayList)
            aHeaders = Value
        End Set
    End Property


#End Region

#Region "GetRates"

    Public Function GetRates(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer) As DataTable

        Dim dt As DataTable
        Dim dr As DataRow

        'get datebands
        Me.Datebands.Clear()
        dt = SQL.GetDatatable("exec GetContractRateDate {0},{1}", iContractID, iContractRoomTypeID)
        For Each dr In dt.Rows
            Me.AddDateband(CType(dr("StartDate"), Date), CType(dr("EndDate"), Date), CType(dr(2), Integer))
        Next

        'get headers
        Me.PopulateHeadersCollection(iContractID, iContractRoomTypeID)

        'get rates 
        Dim dtRates As DataTable = SQL.GetDatatable("exec GetContractRate {0},{1}", _
            iContractID, iContractRoomTypeID)

        'set up the data table
        dt = New DataTable
        dt.Columns.Add("Start")
        dt.Columns.Add("End")

        Dim oHeader As Header
        Dim iColumn As Integer = 1
        For Each oHeader In Me.Headers
            dt.Columns.Add("col" & iColumn)
            iColumn += 1
        Next

        'scan through, for each date and for each column
        Dim oDateband As Dateband
        Dim nValue As Single = 0
        Dim drValueRow() As DataRow
        For Each oDateband In Me.Datebands

            dr = dt.NewRow
            dr(0) = oDateband.StartDate
            dr(1) = oDateband.EndDate

            iColumn = 2
            For Each oHeader In Me.Headers

                'filter the rates datatable
                drValueRow = dtRates.Select(String.Format("ContractRateDateID={0} and ContractRateHeaderID={1}", _
                    oDateband.ContractRateDateID, oHeader.ContractRateHeaderID))

                'set the column value
                If drValueRow.Length > 0 Then
                    dr(iColumn) = CType(drValueRow(0)(2), Single)
                    iColumn += 1
                End If
            Next

            dt.Rows.Add(dr)
        Next

        Return dt

    End Function

#End Region

#Region "SetRates"

    Public Sub SetRates(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer, _
        ByVal aGridData As ArrayList)

        'populate the headers collection
        Me.PopulateHeadersCollection(iContractID, iContractRoomTypeID)

        'set up stringbuilder and default sql
        Dim sb As New StringBuilder

        'set up holding variable for the contractratedate
        sb.Append("declare @iContractRateDateID int\n")

        'delete existing entries from the contractratedate and contractrate tables
        sb.AppendFormat("exec ClearDatesAndRates {0},{1}\n", iContractID, iContractRoomTypeID)

        'set default strings up
        Dim sSqlDates As String = "insert into ContractRateDate (" & _
            "ContractID, ContractRoomTypeID, StartDate, EndDate) values ({0},{1},{2},{3})\n" & _
            "select @iContractRateDateID=@@identity\n"
        Dim sSqlRates As String = "insert into ContractRate " & _
            " (ContractRateDateID, ContractRateHeaderID, Rate) values (@iContractRateDateID,{0},{1})\n"

        'scan through each row
        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData

            'add the date
            sb.AppendFormat(sSqlDates, iContractID, iContractRoomTypeID, _
                oGridRow("Start"), oGridRow("End"))

            'add each header
            Dim oHeader As Header
            For Each oHeader In Me.Headers
                sb.AppendFormat(sSqlRates, oHeader.ContractRateHeaderID, _
                    oGridRow(oHeader.ContractHeaderShortName))
            Next
        Next

        'execute the big query!
        SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))


    End Sub


#End Region

#Region "CopyRates"

    Public Sub CopyRates(ByVal iContractID As Integer, ByVal iFromContractRoomTypeiD As Integer, _
        ByVal iToContractRoomTypeID As Integer)

        SQL.Execute("exec CopyRates {0},{1},{2}", iContractID, iFromContractRoomTypeiD, _
            iToContractRoomTypeID)
    End Sub
#End Region

#Region "CopyHeaders"

    Public Shared Sub CopyHeaders(ByVal iContractID As Integer, _
        ByVal iFromContractRoomTypeID As Integer, ByVal iToContractRoomTypeID As Integer)

        SQL.Execute("exec CopyRateHeaders {0},{1},{2}", iContractID, _
            iFromContractRoomTypeID, iToContractRoomTypeID)

    End Sub

#End Region

#Region "CopyDates"

    Public Shared Sub CopyDates(ByVal iContractID As Integer, _
        ByVal iFromContractRoomTypeID As Integer, ByVal iToContractRoomTypeID As Integer)

        SQL.Execute("exec CopyRateDates {0}, {1}, {2}", iContractID, _
            iFromContractRoomTypeID, iToContractRoomTypeID)

    End Sub

#End Region


#Region "SetContractHeaders"

    Public Sub SetContractHeaders(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer, _
        ByVal aContractHeaderID As ArrayList)

        Dim sCSV As String = Intuitive.Functions.ArrayListToDelimitedString(aContractHeaderID)

        SQL.Execute("exec SetContractHeaders {0},{1},{2}", _
            iContractID, iContractRoomTypeID, SQL.GetSqlValue(sCSV, "String"))

    End Sub

#End Region

#Region "PopulateHeadersCollection"

    Private Sub PopulateHeadersCollection(ByVal iContractID As Integer, ByVal iContractRoomTypeID As Integer)
        Dim dt As DataTable
        Dim dr As DataRow

        Me.Headers.Clear()
        dt = SQL.GetDatatable("exec GetContractRateHeader {0},{1}", iContractID, iContractRoomTypeID)
        For Each dr In dt.Rows
            Me.AddHeader(dr(0).ToString, CType(dr(1), Integer), CType(dr(2), Integer))
        Next

    End Sub

#End Region

#Region "add to collections - dateband and header"

    'dateband
    Public Sub AddDateband(ByVal StartDate As Date, ByVal EndDate As Date, ByVal ContractRateDateID As Integer)
        Me.Datebands.Add(New Dateband(StartDate, EndDate, ContractRateDateID))
    End Sub

    'header
    Public Sub AddHeader(ByVal ContractHeaderShortname As String, ByVal ContractHeaderID As Integer, _
        ByVal ContractRateHeaderID As Integer)
        Me.Headers.Add(New Header(ContractHeaderShortname, ContractHeaderID, ContractRateHeaderID))
    End Sub

#End Region

#Region "dateband and header structure"

    'dateband
    Public Structure Dateband
        Public StartDate As Date
        Public EndDate As Date
        Public ContractRateDateID As Integer

        Public Sub New(ByVal StartDate As Date, ByVal EndDate As Date, ByVal ContractRateDateID As Integer)
            Me.StartDate = StartDate
            Me.EndDate = EndDate
            Me.ContractRateDateID = ContractRateDateID
        End Sub
    End Structure

    'header
    Public Structure Header
        Public ContractHeaderShortName As String
        Public ContractHeaderID As Integer
        Public ContractRateHeaderID As Integer

        Public Sub New(ByVal ContractHeaderShortName As String, ByVal ContractHeaderID As Integer, _
            ByVal ContractRateHeaderID As Integer)
            Me.ContractHeaderShortName = ContractHeaderShortName
            Me.ContractHeaderID = ContractHeaderID
            Me.ContractRateHeaderID = ContractRateHeaderID
        End Sub
    End Structure

#End Region

End Class
