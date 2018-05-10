Imports Intuitive
Public Class MealBasis
    Inherits TableBase

    Public Sub New()

        Me.Table = "MealBasis"

        With Me.Fields
            .Add(New Field("MealBasisID", "Integer"))
            .Add(New Field("MealBasisCode", "String", 10, ValidationType.NotEmptyNotDupe))
            .Add(New Field("MealBasis", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in ContractRoomType
        If Not SQL.FKCheck("PropertyRoomType", "MealBasisID", iTableID) Then
            Me.Warnings.Add("The Meal Plan cannot be deleted as it is associated with one or more Property Room Types")
        End If

        If Not SQL.FKCheck("ContractRoomType", "MealBasisID", iTableID) Then
            Me.Warnings.Add("The Meal Plan cannot be deleted as it is associated with one or more Contracted Room Types")
        End If

        'check for records in ContractSupplement
        If Not SQL.FKCheck("ContractSupplement", "MealBasisID", iTableID) Then
            Me.Warnings.Add("The Meal Plan cannot be deleted as it is associated with one or more Contracted Supplements")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
