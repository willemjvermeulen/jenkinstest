Imports Intuitive

Public Class PropertySearch
    Private sResultsSQL As String
    Private sText As String
    Private iPropertyGroupID As Integer = 0
    Private iPropertyTypeID As Integer = 0
    Private iGeographyLevel As Integer
    Private iGeographyLevelID As Integer
    Private bCurrentProperty As Boolean = True
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

    Public Property GeographyLevel() As Integer
        Get
            Return iGeographyLevel
        End Get
        Set(ByVal Value As Integer)
            iGeographyLevel = Value
        End Set
    End Property

    Public Property GeographyLevelID() As Integer
        Get
            Return iGeographyLevelID
        End Get
        Set(ByVal Value As Integer)
            iGeographyLevelID = Value
        End Set
    End Property


    Public Property PropertyGroupID() As Integer
        Get
            Return iPropertyGroupID
        End Get
        Set(ByVal Value As Integer)
            iPropertyGroupID = Value
        End Set
    End Property

    Public Property PropertyTypeID() As Integer
        Get
            Return iPropertyTypeID
        End Get
        Set(ByVal Value As Integer)
            iPropertyTypeID = Value
        End Set
    End Property

    Public Property CurrentProperty() As Boolean
        Get
            Return bCurrentProperty
        End Get
        Set(ByVal Value As Boolean)
            bCurrentProperty = Value
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

    Public Function Search() As Boolean

        'check the input params first
        If Not Me.Validate() Then
            Return False
        End If

        Dim oWhere As New SqlWhere
        Dim sBaseSql As String
        Dim sCountSql As String
        Dim iResults As Integer


        'build our where filter

        'text
        If sText <> "" Then

            sText = SQL.GetSqlValue(sText, SQL.SqlValueType.String)
            sText = "'%" & sText.Substring(1, sText.Length - 2) & "%'"

            'reference, name, short name, geoglevel1, geoglevel2, geoglevel3, 
            'group, type, contact name, 
            'address1, address2, towncity, county, country, 
            'supp address1, supp address2, supp towncity, supp county, supp country, supp contact name
            oWhere.Add("isnull(Property.Reference,'')+isnull(Property.Name,'')+isnull(Property.ShortName,'')+" & _
            "isnull(GeographyLevel1.Name,'')+isnull(GeographyLevel2.Name,'')+isnull(GeographyLevel3.Name,'')+" & _
            "isnull(PropertyGroup.PropertyGroup,'')+isnull(PropertyType.PropertyType,'')+" & _
            "isnull(Property.ContactName,'')+isnull(Property.Address1,'')+isnull(Property.Address2,'')+" & _
            "isnull(Property.TownCity,'')+isnull(Property.County,'')+isnull(Property.Country,'')+" & _
            "isnull(SupplementaryAddress.Address1,'')+isnull(SupplementaryAddress.Address2,'')+" & _
            "isnull(SupplementaryAddress.TownCity,'')+isnull(SupplementaryAddress.County,'')+" & _
            "isnull(SupplementaryAddress.Country,'')+isnull(SupplementaryAddress.ContactName,'') " & _
            "like " & sText.Trim)
            oWhere.Add("UserGroupPropertyGroup.User", UserSession.UserGroupID, SqlMatchType.Equal)
        End If

        'geography
        If iGeographyLevel > 0 Then
            Dim sGeography As String = _
                String.Format("GeographyLevel{0}.GeographyLevel{0}ID={1}", _
                    iGeographyLevel, iGeographyLevelID)
            oWhere.Add(sGeography)
        End If

        'Property Group
        If iPropertyGroupID > 0 Then
            oWhere.Add("Property.PropertyGroupID=" & iPropertyGroupID.ToString)
        End If

        'Property Type
        If iPropertyTypeID > 0 Then
            oWhere.Add("Property.PropertyTypeID=" & iPropertyTypeID.ToString)
        End If

        'Current
        If bCurrentProperty = True Then
            oWhere.Add("Property.CurrentProperty=1")
        End If


        'set up base sql with subtable
        sBaseSql = "From Property "
        sBaseSql += " inner join PropertyGroup on " & _
                   "Property.PropertyGroupID=PropertyGroup.PropertyGroupID "

        sBaseSql += " inner join PropertyType on " & _
                   "Property.PropertyTypeID=PropertyType.PropertyTypeID "

        sBaseSql += " " & _
            "inner Join GeographyLevel3 " & _
            "On GeographyLevel3.GeographyLevel3ID=Property.GeographyLevel3ID " & _
            "inner Join GeographyLevel2 " & _
            "On GeographyLevel2.GeographyLevel2ID=GeographyLevel3.GeographyLevel2ID " & _
            "inner Join GeographyLevel1 " & _
            "On GeographyLevel1.GeographyLevel1ID=GeographyLevel2.GeographyLevel1ID "

        sBaseSql += " left join SupplementaryAddress on " & _
                   "Property.PropertyID=SupplementaryAddress.PropertyID "

        'add on the where clause
        sBaseSql += oWhere.Where

        'do our count
        sCountSql = "Select count(*) " & sBaseSql
        iResults = SQL.ExecuteSingleValue(sCountSql)

        If iResults > 100 Then
            Me.Warnings.Add("More than 100 properties were returned.  Please refine your search and try again")
        ElseIf iResults = 0 Then
            Me.Warnings.Add("No properties were found for the input criteria. Please revise your search and try again.")
        Else

            sResultsSQL = "Select distinct GeographyLevel1.Name, GeographyLevel3.Name, Property.Reference, Property.Name, Rating, Property.PropertyID " & _
                sBaseSql & " Order by GeographyLevel1.Name, GeographyLevel3.Name, Property.Name"
        End If

        Return Me.Warnings.Count = 0

    End Function

    Private Function Validate() As Boolean

        If Me.Text = "" AndAlso Me.GeographyLevel = 0 AndAlso Me.PropertyTypeID = 0 _
            AndAlso Me.PropertyGroupID = 0 Then
            Me.Warnings.Add("At least one criterion must be set before you can search")
            Return False
        else
            Return True

        End If

    End Function
End Class
