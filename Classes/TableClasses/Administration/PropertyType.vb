Imports Intuitive
Public Class PropertyType
    Inherits TableBase

    Public Sub New()
        Me.Table = "PropertyType"
        With Me.Fields
            .Add(New Field("PropertyTypeID", "Integer"))
            .Add(New Field("PropertyType", "String", 25, ValidationType.NotEmptyNotDupe))
        End With
        Me.Clear()
    End Sub


    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean
        If SQL.FKCheck("Property", "PropertyTypeID", iTableID) Then
            Return True
        Else
            Me.Warnings.Add("The Property Type cannot be deleted as it is associated with one or more Properties")
            Return False
        End If
    End Function
End Class
