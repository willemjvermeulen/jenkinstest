Imports Intuitive
Imports Intuitive.Functions
Public Class Supplier
    Inherits TableBase

#Region "Pseudo Fields"
    Public CarHire As Boolean = False
    Public Accommodation As Boolean = False
    Public Flights As Boolean = False
    Public NumberOfSupplierTypes As Integer = 0
#End Region

    Public Sub New()

        Me.Table = "Supplier"

        Me.ClientValidation = True
        With Me.Fields
            .Add(New Field("SupplierID", "Integer"))
            .Add(New Field("SupplierCode", "String", 5, ValidationType.NotEmpty))
            .Add(New Field("SupplierName", "String", 40, ValidationType.NotEmptyNotDupe))
            .Add(New Field("CurrencyID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("CurrentSupplier", "Boolean"))
            .Add(New Field("DurationHeaderSetID", "Integer"))
        End With

        Me.Clear()
    End Sub

#Region "Check Delete"
    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in CarHireContract
        If Not SQL.FKCheck("CarHireContract", "SupplierID", iTableID) Then
            Me.Warnings.Add("Supplier cannot be deleted as it is in use")
        End If

        'delete child records from SupplierCarType
        SQL.Execute("Delete from SupplierCarType Where SupplierID=" & iTableID.ToString)

        'delete child records from SupplierTypeDef
        SQL.Execute("Delete from SupplierTypeDef Where SupplierID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Check Update"
    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        'check we have at least one supplier type selected
        If Me.NumberOfSupplierTypes = 0 Then
            Me.Warnings.Add("You must select at least one Supplier Type")
        End If

        Return Me.Warnings.Count = 0
    End Function
#End Region

#Region "Set System Supplier Types"
    Public Sub SetSystemSupplierTypes(ByVal iSupplierID As Integer)

        Dim dtSystemTypes As DataTable = SQL.GetDatatable("exec GetSystemSupplierTypes {0}", iSupplierID)
        Dim sSystemSupplierTypes As String = ""
        For Each drRow As DataRow In dtSystemTypes.Rows
            sSystemSupplierTypes += SafeString(drRow("SupplierType").ToString)
        Next

        Me.CarHire = sSystemSupplierTypes.IndexOf("Car Hire") >= 0
        Me.Flights = sSystemSupplierTypes.IndexOf("Flights") >= 0
        Me.Accommodation = sSystemSupplierTypes.IndexOf("Accommodation") >= 0

    End Sub
#End Region

#Region "After Go"
    Private Sub Supplier_AfterGo(ByVal iTableID As Integer) Handles MyBase.AfterGo
        Me.SetSystemSupplierTypes(iTableID)
    End Sub
#End Region

#Region "Delete Car Hire"
    Public Shared Sub DeleteCarHire(ByVal iSupplierID As Integer)

        'if we're not car hire any more than delete any car type entries 
        'and delete the duration header set ID
        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction
            .Add("Update Supplier Set DurationHeaderSetID=0 Where SupplierID={0}", iSupplierID)
            .Add("Delete from SupplierCarType Where SupplierID={0}", iSupplierID)
            .Execute()
        End With

    End Sub
#End Region

#Region "Get Default Currency ID"
    Public Shared Function GetDefaultCurrencyID(ByVal iSupplierID As Integer) As Integer

        Return SafeInt(SQL.ExecuteSingleValue("Select CurrencyID from Supplier Where SupplierID={0}", iSupplierID))
    End Function
#End Region

End Class

