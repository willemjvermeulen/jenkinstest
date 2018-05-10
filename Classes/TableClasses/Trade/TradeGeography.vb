Imports Intuitive

Public Class TradeGeography

    Public Shared Sub AddGeographyLevel(ByVal iTradeID As Integer, _
        ByVal sParentType As String, ByVal iParentID As Integer)
        SQL.Execute("exec TradeAddGeography {0},{1},{2} ", _
            iTradeID, SQL.GetSqlValue(sParentType, SQL.SqlValueType.String), _
                iParentID)
    End Sub

End Class
