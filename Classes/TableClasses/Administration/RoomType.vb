Imports Intuitive
Public Class RoomType
    Inherits TableBase

    Public Sub New()

        Me.Table = "RoomType"

        With Me.Fields
            .Add(New Field("RoomTypeID", "Integer"))
            .Add(New Field("RoomType", "String", 40, ValidationType.NotEmptyNotDupe))
            .Add(New Field("RoomTypeCode", "String", 10, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in PropertyRoomType
        If Not SQL.FKCheck("PropertyRoomType", "RoomTypeID", iTableID) Then
            Me.Warnings.Add("This room type cannot be deleted as it is associated with one or more properties")
        End If

        Return Me.Warnings.Count = 0
    End Function

    Public Shared Function GetRoomType(ByVal iID As Integer) As String

        If iID = 0 Then Return "All"

        'lil routine to get the roomtype for the param ID
        Return SQL.ExecuteSingleValue(String.Format("Select ''+RoomType " & _
               "From RoomType " & _
               "Where RoomTypeID={0}", iID), True).ToString
    End Function
End Class
