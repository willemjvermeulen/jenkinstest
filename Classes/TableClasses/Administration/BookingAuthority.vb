Imports Intuitive
Imports Intuitive.Functions
Public Class BookingAuthority
    Inherits TableBase

    Public Sub New()

        Me.Table = "BookingAuthority"

        With Me.Fields
            .Add(New Field("BookingAuthorityID", "Integer"))
            .Add(New Field("BookingAuthority", "String", 30, ValidationType.NotEmptyNotDupe))
        End With

        Me.Clear()
    End Sub

End Class

