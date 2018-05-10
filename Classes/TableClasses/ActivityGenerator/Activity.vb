Imports Intuitive

Public Class Activity
    Inherits TableBase

    Public ActivitytradeContactGroup As ArrayList

    Public Sub New()

        Me.Table = "Activity"

        With Me.Fields
            .Add(New Field("ActivityID", "Integer"))
            .Add(New Field("Activity", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("ActivityType", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("ActivityDate", "Date", ValidationType.NotEmptyIsDate))
            .Add(New Field("Notes", "Text"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'Delete child records from CustomerNote
        SQL.Execute("Delete from ActivityCriterion Where ActivityID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

#Region "Flag Participants"
    Public Shared Function FlagParticipants(ByVal iActivityID As Integer, ByVal sSubject As String, ByVal sNote As String) As Intuitive.FunctionReturn
        Dim oReturn As New Intuitive.FunctionReturn

        If Not sSubject = "" Then
            SQL.Execute("ActivityFlagParticipant {0},{1},{2},{3}", _
                iActivityID, SQL.GetSqlValue(sSubject, SQL.SqlValueType.String), _
                SQL.GetSqlValue(sNote, SQL.SqlValueType.String), UserSession.SystemUserID)
            oReturn.Success = True
        Else
            oReturn.Warnings.Add("You must specify a Subject")
            oReturn.Success = False
        End If
        Return oReturn

    End Function
#End Region

#Region "Criterion Count"
    Public Function GetCriterionCount() As Integer
        Dim sSql As String

        sSql = String.Format("Select Count(*) " & _
                "From ActivityCriterion " & _
                "Where ActivityID ={0}", Me.TableID.ToString)

        Return SQL.ExecuteSingleValue(sSql)
    End Function
#End Region

#Region "After Update - Add any Trade Contact group stuff"
    Private Sub Activity_AfterUpdate(ByVal iTableID As Integer) Handles MyBase.AfterUpdate
        Dim oSqlTrans As New SQLTransaction
        oSqlTrans.Add("Delete from ActivityTradeContactGroup where ActivityID={0}", _
            Me.TableID)

        For Each i As Integer In Me.ActivitytradeContactGroup
            oSqlTrans.Add("Insert into ActivityTradeContactGroup (ActivityID, TradeContactGroupID) " & _
                "values ({0},{1})", Me.TableID, i)
        Next

        oSqlTrans.Execute()

    End Sub
#End Region
End Class
