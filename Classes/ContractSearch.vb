Imports Intuitive

Public Class ContractSearch
    Private sResultsSQL As String
    Private sText As String
    Private iGeographyLevel3ID As Integer
    Private bCurrent As Boolean = True
    Private aWarnings As New ArrayList

#Region "Properties"
    Public ReadOnly Property ResultsSQL() As String
        Get
            Return sResultsSQL
        End Get
    End Property

    Public Property Text() As String
        Get
            Return sText
        End Get
        Set(ByVal Value As String)
            sText = Value
        End Set
    End Property

    Public Property GeographyLevel3ID() As Integer
        Get
            Return iGeographyLevel3ID
        End Get
        Set(ByVal Value As Integer)
            iGeographyLevel3ID = Value
        End Set
    End Property

    Public Property Current() As Boolean
        Get
            Return bCurrent
        End Get
        Set(ByVal Value As Boolean)
            bCurrent = Value
        End Set
    End Property


    Public Property Warnings() As ArrayList
        Get
            Return aWarnings
        End Get
        Set(ByVal Value As ArrayList)
            aWarnings = Value
        End Set
    End Property
#End Region

    'Public Function Search() As Boolean
    '    Dim oWhere As New SqlWhere
    '    Dim sBaseSql As String
    '    Dim sCountSql As String
    '    Dim iResults As Integer

    '    'build our where filter

    '    'text
    '    If sText <> "" Then
    '        oWhere.Add("isnull(Contract.Reference,'')+isnull(Contract.Category,'')+isnull(Contract.Type,'')+" & _
    '        "isnull(Contract.Status,'')+isnull(Contract.SpecialOfferCode,'')+isnull(Property.Name,'')+" & _
    '        "isnull(Property.ShortName,'')+isnull(Property.Reference,'')+isnull(GeographyLevel1.Name,'')+" & _
    '        "isnull(GeographyLevel2.Name,'')+isnull(GeographyLevel3.Name,'')+isnull(PropertyGroup.PropertyGroup,'')+" & _
    '        "isnull(PropertyType.PropertyType,'')+isnull(Property.ContactName,'')+isnull(Property.Address1,'')+" & _
    '        "isnull(Property.Address2,'')+isnull(Property.TownCity,'')+isnull(Property.County,'')+" & _
    '        "isnull(Property.Country,'')+isnull(SupplementaryAddress.Address1,'')+isnull(SupplementaryAddress.Address2,'')+" & _
    '        "isnull(SupplementaryAddress.TownCity,'')+isnull(SupplementaryAddress.County,'')+isnull(SupplementaryAddress.Country,'')+" & _
    '        "isnull(SupplementaryAddress.ContactName,'')" & _
    '        " like '%" & sText.Trim & "%' ")
    '    End If

    '    'geography level 3
    '    If iGeographyLevel3ID > 0 Then
    '        oWhere.Add("Property.GeographyLevel3ID=" & iGeographyLevel3ID.ToString)
    '    End If

    '    'Current
    '    If bCurrent = True Then
    '        oWhere.Add("Contract.Status in ('Live', 'In Progress')")
    '    End If

    '    'make sure we have at least one criteria
    '    If Not oWhere.HasCriteria Then
    '        Me.Warnings.Add("At least one search criterion must be specified. Please revise yor search and try again.")
    '        Return False
    '    End If

    '    'set up base sql with subtable (property group)
    '    sBaseSql = "From Contract " & _
    '               "Inner Join Property On Contract.PropertyID=Property.PropertyID " & _
    '               "Inner Join PropertyGroup On Property.PropertyGroupID=PropertyGroup.PropertyGroupID " & _
    '               "Inner Join PropertyType On Property.PropertyTypeID=PropertyType.PropertyTypeID " & _
    '               "Inner Join GeographyLevel3 On Property.GeographyLevel3ID=GeographyLevel3.GeographyLevel3ID " & _
    '               "Inner Join GeographyLevel2 On GeographyLevel3.GeographyLevel2ID=GeographyLevel2.GeographyLevel2ID " & _
    '               "Inner Join GeographyLevel1 On GeographyLevel2.GeographyLevel1ID=GeographyLevel1.GeographyLevel1ID " & _
    '               "Inner Join SupplementaryAddress On Property.PropertyID=SupplementaryAddress.PropertyID"

    '    'add on the where clause
    '    sBaseSql += oWhere.Where

    '    'do our count
    '    sCountSql = "Select count(*) " & sBaseSql
    '    iResults = SQL.ExecuteSingleValue(sCountSql)

    '    If iResults > 0 Then
    '        sResultsSQL = "Select " & _
    '        "   left(Contract.Reference,24) + ' ...' , " & _
    '        "   Property.Name, " & _
    '        "	dbo.DateRange(StayStartDate, StayEndDate) Stay, Type, Status,ContractID " & _
    '        sBaseSql
    '        Return True
    '    Else
    '        Me.Warnings.Add("No contracts were found for the input criteria. Please revise your search and try again.")
    '        Return False
    '    End If
    'End Function
End Class
