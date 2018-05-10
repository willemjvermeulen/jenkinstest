Imports Intuitive

Public Class LocationFeature
    Inherits TableBase

    Public Sub New()

        Me.Table = "LocationFeature"

        With Me.Fields
            .Add(New Field("LocationFeatureID", "Integer"))
            .Add(New Field("LocationFeature", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from LocationFeatureBridge
        SQL.Execute("Delete from LocationFeatureBridge Where LocationFeatureID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function
End Class
