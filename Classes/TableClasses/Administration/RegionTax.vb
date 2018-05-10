Imports Intuitive
Imports System.Text

Public Class RegionTax
    Inherits TableBase

#Region "Properties"

    Private sLocation As String
    Public Property Location() As String
        Get
            Return sLocation
        End Get
        Set(ByVal Value As String)
            sLocation = Value
        End Set
    End Property

#End Region

#Region "new"
    Public Sub New()

        Me.Table = "RegionTax"

        With Me.Fields
            .Add(New Field("RegionTaxID", "Integer"))
            .Add(New Field("TaxName", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("TaxType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Basis", "String", 50))
            .Add(New Field("CurrencyID", "Integer"))
            .Add(New Field("GeographyLevel1ID", "Integer"))
            .Add(New Field("GeographyLevel2ID", "Integer"))
            .Add(New Field("GeographyLevel3ID", "Integer"))
        End With

        Me.Clear()
    End Sub

#End Region

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in ContractRegionTax
        If Not SQL.FKCheck("ContractRegionTax", "RegionTaxID", iTableID) Then
            Me.Warnings.Add("Sory, the regional tax cannot be deleted as it is associated with one or more contracts")
        End If

        'delete child records from RegionTaxEffective
        SQL.Execute("Delete from RegionTaxEffective Where RegionTaxID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call default CheckUpdate()
        MyBase.CheckUpdate()

        'Check that neither the Basis nor Currency fields are blank
        If Me("TaxType") = "Value" Then
            If Me("Basis") = "" Then
                Me.Warnings.Add("The Basis must be specified")
                Me.Fields("Basis").IsValid = False
            End If
            If Me("CurrencyID") = "" Then
                Me.Warnings.Add("The Currency must be specified")
                Me.Fields("CurrencyID").IsValid = False
            End If
        End If

        'Check that a location has been selected
        If Not CType(Me("GeographyLevel1ID"), Integer) > 0 AndAlso _
                Not CType(Me("GeographyLevel2ID"), Integer) > 0 AndAlso _
                Not CType(Me("GeographyLevel3ID"), Integer) > 0 Then
            Me.Warnings.Add("The Location must be specified")
        End If

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "AddValues"

    Public Shared Sub AddValues(ByVal iRegionTaxID As Integer, ByVal aGridData As ArrayList)

        Dim sb As New StringBuilder

        'clear any existing values
        sb.AppendFormat("Delete from RegionTaxEffective where RegionTaxID={0}\n", iRegionTaxID)

        'and add the new ones
        Dim sInsert As String = "insert into RegionTaxEffective (RegionTaxID, StartDate, EndDate, Value) " & _
            " values ({0},{1},{2},{3})\n"

        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData

            sb.AppendFormat(sInsert, iRegionTaxID, oGridRow("Start Date"), _
                oGridRow("End Date"), oGridRow("Value"))
        Next

        SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))

    End Sub

#End Region

End Class
