Imports Intuitive
Imports Intuitive.Functions

Public Class SearchParameter
    Inherits TableBase

    Public Sub New()

        Me.Table = "SearchParameter"

        With Me.Fields
            .Add(New Field("SearchParameterID", "Integer"))
            .Add(New Field("MinimumNumberHotels", "Integer"))
            .Add(New Field("MaximumHotelsPerPage", "Integer"))
            .Add(New Field("GeographyMiss", "Integer"))
            .Add(New Field("FeatureHitUplift", "Integer"))
            .Add(New Field("MaximumFeatureUplift", "Integer"))
            .Add(New Field("StarRatingMiss", "Integer"))
            .Add(New Field("PriceMiss", "Integer"))
            .Add(New Field("PriceVariance", "Integer"))
            .Add(New Field("FeatureMiss", "Integer"))
            .Add(New Field("SpecialOfferMiss", "Integer"))
            .Add(New Field("AdjoiningRoomsMiss", "Integer"))
            .Add(New Field("DisabledRoomsMiss", "Integer"))
            .Add(New Field("PropertyTypeMiss", "Integer"))
            .Add(New Field("NoInventoryMiss", "Integer"))
            .Add(New Field("Duration", "Integer"))
            .Add(New Field("ArrivalDate", "String", 20))
            .Add(New Field("DisplayNoAvailabilityMessage", "Boolean"))
            .Add(New Field("NoAvailabilityMessage", "String", 250))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckUpdate() As Boolean

        MyBase.CheckUpdate()

        If SafeBoolean(Me("DisplayNoAvailabilityMessage")) AndAlso Me("NoAvailabilityMessage") = "" Then
            Me.Warnings.Add("The Message to show when there is no availability must be specified")
            Me.Fields("NoAvailabilityMessage").IsValid = False
        End If

        Return Me.Warnings.Count = 0

    End Function

    Private Sub SearchParameter_BeforeUpdate(ByVal iTableID As Integer) Handles MyBase.BeforeUpdate

        If Not SafeBoolean(Me("DisplayNoAvailabilityMessage")) Then
            Me.SetField("NoAvailabilityMessage", "")
        End If

    End Sub

End Class
