Imports Intuitive

Public Class GeographySet
    Inherits TableBase

    Public Sub New()

        Me.Table = "GeographySet"

        With Me.Fields
            .Add(New Field("GeographySetID", "Integer"))
            .Add(New Field("Name", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("Level1Name", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Level2Name", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("Level3Name", "String", 50, ValidationType.NotEmpty))
            .Add(New Field("DefaultSet", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in GeographyLevel1
        If Not SQL.FKCheck("GeographyLevel1", "GeographySetID", iTableID) Then
            Me.Warnings.Add("GeographySet cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        If Me.Warnings.Count = 0 Then
            If Me.GetField("DefaultSet") = "true" Then
                SQL.Execute("Update GeographySet Set DefaultSet=0 Where GeographySetID<>{0}", Me.TableID)
            End If
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
