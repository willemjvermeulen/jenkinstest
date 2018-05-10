Imports Intuitive
Public Class GeographyLevel3
    Inherits TableBase

    Public Sub New()

        Me.Table = "GeographyLevel3"
        With Me.Fields
            .Add(New Field("GeographyLevel3ID", "Integer"))
            .Add(New Field("GeographyLevel2ID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("Code", "String", 5, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Name", "String", 50, ValidationType.NotEmptyNotDupe))
        End With

        Me.Fields("Code").ParentField = "GeographyLevel3ID"
        Me.Fields("Name").ParentField = "GeographyLevel3ID"

        Me.Clear()
    End Sub

#Region "Check Delete"

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in Property
        If Not SQL.FKCheck("Property", "GeographyLevel3ID", iTableID) Then
            Me.Warnings.Add("The Location cannot be deleted as it is associated with one or more Properties")
        End If

        'check for records in RegionTax
        If Not SQL.FKCheck("RegionTax", "GeographyLevel3ID", iTableID) Then
            Me.Warnings.Add("The Location cannot be deleted as it is associated with one or more Taxes")
        End If

        'delete child records from UserGroupLocation
        If Me.Warnings.Count = 0 Then
            SQL.Execute("Delete from UserGroupLocation Where GeographyLevel3ID=" & iTableID.ToString)
        End If

        Return Me.Warnings.Count = 0

    End Function

#End Region

End Class
