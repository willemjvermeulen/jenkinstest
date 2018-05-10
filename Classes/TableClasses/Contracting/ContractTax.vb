Imports Intuitive
Imports System.Text
Public Class ContractTax
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractTax"

        With Me.Fields
            .Add(New Field("ContractTaxID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ContractRoomTypeID", "Integer"))
            .Add(New Field("TaxName", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Type", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("Basis", "String", 30))
            .Add(New Field("CurrencyID", "Integer"))
            .Add(New Field("Inclusive", "String", 10, ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from ContractSupplementTax
        SQL.Execute("Delete from ContractSupplementTax Where ContractTaxID=" & iTableID.ToString)

        'delete child records from ContractTaxValue
        SQL.Execute("Delete from ContractTaxValue Where ContractTaxID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

#End Region

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call default CheckUpdate()
        MyBase.CheckUpdate()

        'Check that neither the Basis nor Currency fields are blank
        'if Value tax type
        If Me("Type") = "Value" Then
            If Me("Basis") = "" Then
                Me.Warnings.Add("The Basis must be specified")
                Me.Fields("Basis").IsValid = False
            End If
            If Me("CurrencyID") = "" Then
                Me.Warnings.Add("The Currency must be specified")
                Me.Fields("CurrencyID").IsValid = False
            End If
        End If

        'set the validations
        'If GetField("Type") = "Value" Then
        '    'basis,currency and value required.
        '    Fields("Basis").ValidationType = ValidationType.NotEmpty
        '    Fields("CurrencyID").ValidationType = ValidationType.NotEmpty
        '    Fields("TaxValue").ValidationType = ValidationType.NotEmpty
        '    Fields("TaxPercent").ValidationType = ValidationType.None

        '    'set to int - cow boy fix - cos numeric allows zeros 
        '    If GetField("Taxvalue") = "0" Then
        '        Fields("TaxValue").FieldType = "Integer"
        '    End If
        'Else
        '    Fields("Basis").ValidationType = ValidationType.None
        '    Fields("TaxValue").ValidationType = ValidationType.None

        '    'percent only required            
        '    Fields("TaxPercent").ValidationType = ValidationType.NotEmpty
        'End If

        ''quick check
        'MyBase.CheckUpdate()

        ''extra checks
        'If Me.Warnings.Count < 1 Then
        '    Dim dStart As Date = CType(Me.GetField("StartDate"), Date)
        '    Dim dEnd As Date = CType(Me.GetField("EndDate"), Date)

        '    'make sure the startdate is b4 the enddate
        '    If dStart > dEnd Then
        '        Me.Warnings.Add("Sorry, start date must be before the end date")
        '    End If

        '    'make sure dates are within contract travel dates
        '    'If Not inTravelDate(dStart, dEnd) Then
        '    '    Me.Warnings.Add("Sorry, the date range must be within the Contract Travel Dates")
        '    'End If

        '    ''make sure dates do not overlap
        '    'If RangeOverlaps(dStart, dEnd) Then
        '    '    Me.Warnings.Add("Sorry, the date range cannot overlap.")
        '    'End If

        '    'make sure this name doesnt exist for this roomtype already
        'End If

        'If Me.Warnings.Count < 1 Then
        '    'do the check again
        '    MyBase.CheckUpdate()
        'End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

    Public Shared Sub SetValues(ByVal iContractTaxID As Integer, ByVal aGridData As ArrayList)

        'clear existing records
        SQL.Execute("Delete from ContractTaxValue where ContractTaxId=" & iContractTaxID)

        'set up stored procedure declare
        Dim sb As New StringBuilder
        Dim sSql As String = "exec AddContractTaxValue {0},{1},{2},{3} \n"

        'build up the sp calls
        Dim oGridRow As Intuitive.WebControls.Grid.GridRow
        For Each oGridRow In aGridData
            sb.AppendFormat(sSql, iContractTaxID, _
                oGridRow("Start Date"), oGridRow("End Date"), oGridRow("Value"))
        Next

        'execute
        If sb.ToString <> "" Then
            SQL.Execute(sb.ToString.Replace("\n", ControlChars.NewLine))
        End If


    End Sub
End Class
