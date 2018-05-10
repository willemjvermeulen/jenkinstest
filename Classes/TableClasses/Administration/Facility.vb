Imports Intuitive
Public Class Facility
    Inherits TableBase

    Public Sub New()

        Me.Table = "Facility"

        With Me.Fields
            .Add(New Field("FacilityID", "Integer"))
            .Add(New Field("Facility", "String", 50, ValidationType.NotEmptyNotDupe))
            .Add(New Field("ShowInSearch", "Boolean"))
        End With

        Me.Clear()
    End Sub

    Public Overrides Function CheckDelete(ByVal iTableID As Integer) As Boolean

        'check for records in ContractFacility
        If Not SQL.FKCheck("ContractFacility", "FacilityID", iTableID) Then
            Me.Warnings.Add("Sorry, this Facility is assigned to a Contract")
        End If

        Return Me.Warnings.Count = 0
    End Function
End Class
