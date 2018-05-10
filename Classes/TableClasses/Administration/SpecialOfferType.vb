Imports Intuitive

Public Class SpecialOfferType
    Inherits TableBase

    Public Sub New()

        Me.Table = "SpecialOfferType"

        With Me.Fields
            .Add(New Field("SpecialOfferTypeID", "Integer"))
            .Add(New Field("SpecialOfferType", "String", 40, ValidationType.NotEmptyNotDupe))
            .Add(New Field("OfferType", "String", 40, ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

    Public Shared Function GetOfferType(ByVal iSpecialOfferTypeID As Integer) As String

        Return SQL.GetValue("Select OfferType from SpecialOfferType where SpecialOfferTypeID={0}", _
            iSpecialOfferTypeID)
    End Function

End Class