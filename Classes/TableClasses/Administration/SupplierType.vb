Imports Intuitive

Public Class SupplierType
    Inherits TableBase

    Public Sub New()

        Me.Table = "SupplierType"

        With Me.Fields
            .Add(New Field("SupplierTypeID", "Integer"))
            .Add(New Field("SupplierType", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("SystemSupplier", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in SupplierTypeDef
        If Not SQL.FKCheck("SupplierTypeDef", "SupplierTypeID", iTableID) Then
            Me.Warnings.Add("SupplierType cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function


    Public Shared Function GetSupplierType(ByVal iSupplierTypeID As Integer) As String

        Dim dr As DataRow = SQL.GetDataRow("Select SupplierType from SupplierType " & _
            "where SupplierTypeID={0}", iSupplierTypeID)
        If Not dr Is Nothing Then
            Return dr("SupplierType").ToString
        Else
            Throw New Exception("Could not established SupplierType for ID " & iSupplierTypeID)
        End If
    End Function

End Class

