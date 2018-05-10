Imports Intuitive

Public Class ContractTrade
    Inherits TableBase

    Public Sub New()

        Me.Table = "ContractTrade"

        With Me.Fields
            .Add(New Field("ContractTradeID", "Integer"))
            .Add(New Field("ContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("TradeID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Shared Sub UpdateContractTrade(ByVal iContractID As Integer, ByVal aTradeIDs As ArrayList)

        Dim oSqlTrans As New SQLTransaction

        'delete existing items
        oSqlTrans.Add("Delete from ContractTrade where ContractID={0}", iContractID)

        'add the new ones
        Dim sInsert As String = "Insert into ContractTrade (ContractID, TradeID) values ({0}, {1})"

        'add em
        For Each iTradeID As Integer In aTradeIDs
            oSqlTrans.Add(sInsert, iContractID, iTradeID)
        Next

        'execute
        oSqlTrans.Execute()

    End Sub

    Public Shared Sub DeleteContractTrade(ByVal iContractID As Integer)

        SQL.Execute("delete from ContractTrade where ContractID=" & iContractID)

    End Sub

End Class
