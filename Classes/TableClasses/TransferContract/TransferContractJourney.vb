Imports Intuitive

Public Class TransferContractJourney
    Inherits TableBase

    Public Sub New()

        Me.Table = "TransferContractJourney"

        With Me.Fields
            .Add(New Field("TransferContractJourneyID", "Integer"))
            .Add(New Field("TransferContractID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("DepartureParentType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("DepartureParentID", "Integer", ValidationType.NotEmpty))
            .Add(New Field("ArrivalParentType", "String", 20, ValidationType.NotEmpty))
            .Add(New Field("ArrivalParentID", "Integer", ValidationType.NotEmpty))
        End With

        Me.Clear()
    End Sub

End Class
