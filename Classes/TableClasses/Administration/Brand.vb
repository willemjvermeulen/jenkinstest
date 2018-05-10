Imports Intuitive

Public Class Brand
    Inherits TableBase

    Public Sub New()

        Me.Table = "Brand"

        With Me.Fields
            .Add(New Field("BrandID", "Integer"))
            .Add(New Field("BrandCode", "String", 5, ValidationType.NotEmptyNotDupe))
            .Add(New Field("BrandName", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

End Class
