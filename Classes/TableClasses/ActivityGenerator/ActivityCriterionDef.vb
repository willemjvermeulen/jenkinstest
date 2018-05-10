Imports Intuitive

Public Class ActivityCriterionDef
    Inherits TableBase

    Public Sub New()

        Me.Table = "ActivityCriterionDef"

        With Me.Fields
            .Add(New Field("ActivityCriterionDefID", "Integer"))
            .Add(New Field("ActivityCriterionDef", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("CriterionType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("StoredProcedure", "Text"))
        End With

        Me.Clear()
    End Sub


End Class
