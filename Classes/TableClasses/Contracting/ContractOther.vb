Imports Intuitive
Imports Intuitive.Functions

Public Class ContractOther
    Inherits TableBase

#Region "property"

    Private sMode As String
    Public ReadOnly Property Mode() As String
        Get
            Return sMode
        End Get
    End Property

    Public DataType As String

#End Region

    Public Sub New()

        Me.Table = "ContractOther"

        With Me.Fields
            .Add(New Field("ContractOtherID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("OtherTypeID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("CurrencyID", "Integer"))
            .Add(New Field("Value", "Integer"))
            .Add(New Field("Notes", "Text"))
        End With

        Me.Clear()
    End Sub

#Region "Check Update"

    Public Overrides Function CheckUpdate() As Boolean

        'Call the default CheckUpdate()
        MyBase.CheckUpdate()

        'text
        If Me.DataType = "Text" Then
            If Me("Notes") = "" Then
                Me.Warnings.Add("You must input some text before you can save")
                Me.Fields("Notes").IsValid = False
            End If
        End If

        'numeric and money
        If InList(Me.DataType, "Numeric", "Money") Then
            If SafeNumeric(Me("Value")) = 0 AndAlso Me("Notes") = "" Then
                Me.Warnings.Add("You must input either a Value or some text before you can save")
                Me.Fields("Value").IsValid = False
                Me.Fields("Notes").IsValid = False
            End If
        End If

        'if money, then make sure the currency has been specified
        If Me.DataType = "Money" AndAlso SafeInt(Me("CurrencyID")) = 0 Then
            Me.Warnings.Add("You must select a Currency before you can save")
            Me.Fields("CurrencyID").IsValid = False
        End If


        Return Me.Warnings.Count = 0

    End Function

#End Region

#Region "Setup"


    Public Function Setup(ByVal iContractID As Integer, ByVal iOtherTypeID As Integer, _
        ByVal iCurrencyID As Integer) As String

        'check to see if it already exists?
        Dim iContractOtherID As Integer = _
            SQL.ExecuteSingleValue("Select ContractOtherID from ContractOther " & _
                "Where ContractID={0} and OtherTypeID={1}", iContractID, iOtherTypeID)

        'if id>0 do a go - edit
        If iContractOtherID > 0 Then
            Me.Go(iContractOtherID)
            sMode = "Edit"
        End If

        'no id, add and set defaults
        If iContractOtherID < 1 Then
            Me.Clear()
            Me.SetField("CurrencyID", iCurrencyID)
            Me.SetField("OtherTypeID", iOtherTypeID)

            'get any default text
            Dim sDefaultText As String = SQL.GetValue("Select DefaultText from OtherType " & _
                "where OtherTypeID={0}", iOtherTypeID)
            Me.SetField("Notes", sDefaultText)

            sMode = "Add"
        End If

        'return the datatype
        Return SQL.GetValue("Select DataType from OtherType where OtherTypeID={0}", _
            iOtherTypeID)
    End Function

#End Region

End Class
