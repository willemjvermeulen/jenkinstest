Imports Intuitive
Public Class SupplierTypeDef

#Region "Delete Supplier Types"
    Public Shared Sub DeleteSupplierTypes(ByVal iSupplierID As Integer)

        SQL.Execute("Delete from SupplierTypeDef Where SupplierID={0}", iSupplierID)
    End Sub
#End Region

#Region "Add Supplier Types"
    Public Shared Sub AddSupplierTypes(ByVal iSupplierID As Integer, ByVal aSupplierTypeIDs As ArrayList)

        Dim oSQLTransaction As New SQLTransaction
        With oSQLTransaction

            'delete existing stuff first
            .Add("Delete from SupplierTypeDef Where SupplierID={0}", iSupplierID)

            'add a new line for everything else
            For Each iSupplierTypeID As Integer In aSupplierTypeIDs
                .Add("Insert into SupplierTypeDef (SupplierID, SupplierTypeID) Values ({0},{1})", _
                    iSupplierID, iSupplierTypeID)
            Next

            'execute
            .Execute()

        End With

    End Sub
#End Region

End Class
