Imports Intuitive
Imports Intuitive.Functions

Public Class Currency
    Inherits TableBase

    Public Sub New()
        Me.Table = "Currency"

        With Me.Fields
            .Add(New Field("CurrencyID", "Integer"))
            .Add(New Field("CurrencyCode", "String", 10, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Currency", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Symbol", "String", 10, ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Contract
        If Not SQL.FKCheck("Contract", "CurrencyID", iTableID) Then
            Me.Warnings.Add("Currency cannot be deleted as it is in use")
        End If


        Return Me.Warnings.Count = 0
    End Function


#Region "Get Currency Symbol"

    Public Shared Function GetCurrencySymbol(ByVal CurrencyID As Integer) As String

        Dim oCurrency As Hashtable = CType(HttpContext.Current.Cache("CurrencySymbols"), Hashtable)
        If oCurrency Is Nothing Then

            oCurrency = New Hashtable
            Dim dt As DataTable = SQL.GetDatatable("select CurrencyID, Symbol from Currency")
            For Each dr As DataRow In dt.Rows
                oCurrency.Add(safeint(dr("CurrencyID")), dr("Symbol").ToString)
            Next

            AddToCache("CurrencySymbols", oCurrency, 1440)
        End If

        Return oCurrency(CurrencyID).ToString
    End Function

#End Region

End Class
