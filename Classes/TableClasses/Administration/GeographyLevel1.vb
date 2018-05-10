Imports Intuitive
Public Class GeographyLevel1
    Inherits TableBase

    Public Sub New()

        Me.Table = "GeographyLevel1"
        With Me.Fields
            .Add(New Field("GeographyLevel1ID", "Integer"))
            .Add(New Field("GeographySetID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Code", "String", 5, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Name", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("CurrencyID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Fields("Code").ParentField = "GeographySetID"
        Me.Fields("Name").ParentField = "GeographySetID"

        Me.Clear()
    End Sub


    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        If Not SQL.FKCheck("GeographyLevel2", "GeographyLevel1ID", iTableID) Then
            Me.Warnings.Add("Geography Level 1 cannot be deleted as it has Level 2 data")
        End If

        If Not SQL.FKCheck("Airport", "GeographyLevel1ID", iTableID) Then
            Me.Warnings.Add("The Geography Level cannot be deleted as it is associated with one or more Airports")
        End If

        Return Me.Warnings.Count = 0

    End Function
End Class
