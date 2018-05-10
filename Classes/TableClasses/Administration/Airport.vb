Imports Intuitive

Public Class Airport
    Inherits TableBase

    Public Sub New()

        Me.Table = "Airport"

        With Me.Fields
            .Add(New Field("AirportID", "Integer"))
            .Add(New Field("GeographyLevel1ID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("IATACode", "String", 4, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Airport", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("CurrentAirport", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'delete child records from AirportGroupDef
        SQL.Execute("Delete from AirportGroupDef Where AirportID=" & iTableID.ToString)

        Return Me.Warnings.Count = 0
    End Function

    Public Overrides Sub Clear()

        MyBase.Clear()
        Me.SetField("CurrentAirport", True)
    End Sub
End Class
