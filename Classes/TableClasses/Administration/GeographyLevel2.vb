Imports Intuitive
Public Class GeographyLevel2
    Inherits TableBase

    Public Sub New()

        Me.Table = "GeographyLevel2"
        With Me.Fields
            .Add(New Field("GeographyLevel2ID", "Integer"))
            .Add(New Field("GeographyLevel1ID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Code", "String", 5, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Name", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub


    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean
        If Not SQL.FKCheck("GeographyLevel3", "GeographyLevel2ID", iTableID) Then
            Me.Warnings.Add("Geography Level 2 cannot be deleted as it has Geography Level 3 data")
            Return False
        End If
        Return True
    End Function
End Class