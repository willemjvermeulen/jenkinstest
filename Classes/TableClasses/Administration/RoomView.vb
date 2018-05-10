Imports Intuitive

Public Class RoomView
    Inherits TableBase

    Public Sub New()

        Me.Table = "RoomView"

        With Me.Fields
            .Add(New Field("RoomViewID", "Integer"))
            .Add(New Field("RoomView", "String", 30, ValidationType.NotEmptyNotDupe))
            .Add(New Field("RoomViewCode", "String", 10, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyRoomType
        If Not SQL.FKCheck("PropertyRoomType", "RoomViewID", iTableID) Then
            Me.Warnings.Add("RoomView cannot be deleted as it is in use")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
