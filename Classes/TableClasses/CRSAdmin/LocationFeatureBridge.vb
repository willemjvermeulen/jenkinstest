Imports Intuitive
Imports Intuitive.Functions

Public Class LocationFeatureBridge
    Inherits TableBase

    Public Sub New()

        Me.Table = "LocationFeatureBridge"

        With Me.Fields
            .Add(New Field("LocationFeatureBridgeID", "Integer"))
            .Add(New Field("LocationFeatureID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ParentType", "String", 30, ValidationType.NotEmpty))
            .Add(New Field("ParentID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckUpdate() As Boolean

        Dim iCount As Integer
        Dim sSql As String = "Select count(*) From LocationFeatureBridge Where " & _
            "LocationFeatureID={0} and ParentType={1} and ParentID={2}"

        iCount = SafeInt(SQL.GetValue(sSql, Me("LocationFeatureID"), _
            SQL.GetSqlValue(Me("ParentType"), SQL.SqlValueType.String), Me("ParentID")))
        If iCount > 0 Then
            Me.Warnings.Add("This Location has already been selected")
        End If
        Return Me.Warnings.Count = 0
    End Function
End Class
